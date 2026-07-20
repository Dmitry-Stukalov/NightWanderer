using System;
using UnityEngine;

public class Sun : MonoBehaviour, ICanTakeDamage
{
	[SerializeField] private GameObject Moon;
	[SerializeField] private GameObject FakeSun;
	private GameObject Player;
	[field: NonSerialized] public DefenseSystem Health { get; set; }
	[field: SerializeField] public float MinDamage { get; set; }
	[field: SerializeField] public float MaxDamage { get; set; }
	[field: NonSerialized] public float Damage { get; set; }
	[SerializeField] private float TakeDamagePause;
	[field: NonSerialized] public bool IsFireDamage { get; set; } = true;
	[SerializeField] private float AllDayLength;
	[SerializeField] private float TransitionDayLength;
	public Timer AllDayTimer { get; private set; }
	private Timer TransitionDayTimer;
	private Timer TransitionNightTimer;
	private Timer TakeDamageTimer;
	private Ray SunRay;
	private RaycastHit[] SunRayCast;
	private int _day = 1;
	private int _layerMask;
	public bool IsTimeSkip { get; set; } = false;
	private bool IsGameStart = false;

	public event Action OnDayStart;
	public event Action OnDayUpdate;
	public event Action OnNightStart;
	public event Action OnTransitionDayEnd;
	public event Action OnTransitionNightEnd;

	public void Initializing()
	{
		Player = GameObject.FindGameObjectWithTag("Player");
		Health = Player.GetComponent<ShipMovement>().GetPlayerDefenseSystem();

		_layerMask = ~LayerMask.GetMask("DeadZoneAndTutorial");

		TransitionDayTimer = new Timer(TransitionDayLength);
		TransitionDayTimer.OnTimerEnd += DayStart;
		TransitionDayTimer.SetPause();

		TransitionNightTimer = new Timer(TransitionDayLength);
		TransitionNightTimer.OnTimerEnd += NightStart;
		TransitionNightTimer.SetPause();

		AllDayTimer = new Timer(AllDayLength);
		AllDayTimer.OnTimerEnd += ResetDayTimer;
		AllDayTimer.OnTimerStart += () =>
		{
			Damage = MinDamage;
			TransitionNightTimer.Continue();

			OnNightStart?.Invoke();
			GameEvents.OnNightStart?.Invoke();
		};
		AllDayTimer.OnTimerHalf += () =>
		{
			Damage = MinDamage;
			_day++;
			TransitionDayTimer.Continue();

			OnDayStart?.Invoke();
			OnDayUpdate?.Invoke();
			GameEvents.OnDayStart?.Invoke();
		};

		TakeDamageTimer = new Timer(TakeDamagePause);
		TakeDamageTimer.OnTimerEnd += ResetTakeDamage;
		TakeDamageTimer.SetPause();

		Damage = MinDamage;

		GameEvents.OnSkipTimeStart += () => IsTimeSkip = true;
		GameEvents.OnSkipTimeEnd += () => IsTimeSkip = false;

		GameEvents.OnCurrentTimeLoad += LoadData;
		GameEvents.OnCurrentDayLoad += LoadData;
		GameEvents.OnSave += SaveData;

		GameEvents.OnGameStart += () => IsGameStart = true;

		ResetDayTimer();
	}

	private void ResetDayTimer() => AllDayTimer.ResetTimer(false);

	public bool IsDayNow()
	{
		if (AllDayTimer.CurrentTime < AllDayTimer.MaxTime / 2) return false;
		else return true;
	}

	private void NightStart()
	{
		TransitionNightTimer.ResetTimer(true);

		Damage = 0;

		OnTransitionNightEnd?.Invoke();
	}

	private void DayStart()
	{
		TransitionDayTimer.ResetTimer(true);

		Damage = MaxDamage;

		OnTransitionDayEnd?.Invoke();
	}

	private void ResetTakeDamage()
	{
		GiveDamage(Damage, IsFireDamage);
		TakeDamageTimer.ResetTimer(true);
	}

	public void GiveDamage(float damage, bool isFireDamage)
	{
		if (Health == null) Health = Player.GetComponent<ShipMovement>().GetPlayerDefenseSystem();

		if (isFireDamage) Health?.GetFireDamage(damage);
		else Health?.GetDamage(damage);
	}

	public int GetDayCount() => _day;

	private void Update()
	{
		if (!IsGameStart) return;

		if (IsTimeSkip)
		{
			AllDayTimer?.Tick(Time.deltaTime * 15);
			TransitionDayTimer?.Tick(Time.deltaTime * 15);
			TransitionNightTimer?.Tick(Time.deltaTime * 15);
		}
        else
        {
			AllDayTimer?.Tick(Time.deltaTime);
			TransitionDayTimer?.Tick(Time.deltaTime);
			TransitionNightTimer?.Tick(Time.deltaTime);
		}

		transform.rotation = Quaternion.Euler(-360 / (AllDayTimer.MaxTime / AllDayTimer.CurrentTime), 0, 0);
		Moon.transform.rotation = Quaternion.Euler(360 / (AllDayTimer.MaxTime / AllDayTimer.CurrentTime), 0, 0);

		TakeDamageTimer?.Tick(Time.deltaTime);

		if (Damage > 0)
		{
			SunRay = new Ray(FakeSun.transform.position, Player.transform.position - FakeSun.transform.position);

			SunRayCast = Physics.RaycastAll(SunRay, 10000f, _layerMask, QueryTriggerInteraction.Collide);

			System.Array.Sort(SunRayCast, (a, b) => a.distance.CompareTo(b.distance));

			if (SunRayCast.Length > 0 && SunRayCast[0].transform.CompareTag("Player"))
			{
				TakeDamageTimer.Continue();
			}
			else TakeDamageTimer.ResetTimer(true);
		}
	}

	private void LoadData(float currentTime)
	{
		AllDayTimer.SetCurrentTime(currentTime);

		if (IsDayNow()) Damage = MaxDamage;
		else Damage = 0;
	}


	private void LoadData(int currentDay)
	{
		_day = currentDay;
		OnDayUpdate?.Invoke();
	}

	private void SaveData()
	{
		GameEvents.OnCurrentTimeSave?.Invoke(AllDayTimer.CurrentTime);
		GameEvents.OnCurrentDaySave?.Invoke(_day);
	}

	private void OnDisable()
	{
		GameEvents.OnSkipTimeStart -= () => IsTimeSkip = true;
		GameEvents.OnSkipTimeEnd -= () => IsTimeSkip = false;


		GameEvents.OnCurrentTimeLoad -= LoadData;
		GameEvents.OnSave -= SaveData;

		GameEvents.OnGameStart -= () => IsGameStart = true;
	}
}
 