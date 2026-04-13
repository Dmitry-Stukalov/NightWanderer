using System;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Rendering.HighDefinition;

public class Sun : MonoBehaviour, ICanTakeDamage
{
	[SerializeField] private GameObject Moon;
	[SerializeField] private GameObject FakeSun;
	[SerializeField] private GameObject Player;
	[field: NonSerialized] public DefenseSystem Health { get; set; }
	[field: SerializeField] public float MinDamage { get; set; }
	[field: SerializeField] public float MaxDamage { get; set; }
	[field: NonSerialized] public float Damage { get; set; }
	[SerializeField] private float TakeDamagePause;
	[field: SerializeField] public bool IsFireDamage { get; set; }
	[SerializeField] private float AllDayLength;
	[SerializeField] private float TransitionDayLength;
	public Timer AllDayTimer { get; private set; }
	private Timer TransitionDayTimer;
	private Timer TransitionNightTimer;
	private Timer TakeDamageTimer;
	private Ray SunRay;
	private RaycastHit[] SunRayCast;
	private int _day = 1;

	public event Action OnDayStart;
	public event Action OnNightStart;
	public event Action OnTransitionDayEnd;
	public event Action OnTransitionNightEnd;

	public void Initializing()
	{
		Health = Player.GetComponent<ShipMovement>().GetPlayerDefenseSystem();

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
		};
		AllDayTimer.OnTimerHalf += () =>
		{
			Damage = MinDamage;
			_day++;
			TransitionDayTimer.Continue();
			OnDayStart?.Invoke();
		};

		TakeDamageTimer = new Timer(TakeDamagePause);
		TakeDamageTimer.OnTimerEnd += ResetTakeDamage;
		TakeDamageTimer.SetPause();

		Damage = MinDamage;

		ResetDayTimer();
	}

	private void ResetDayTimer()
	{
		AllDayTimer.ResetTimer(false);
	}

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
		TakeDamage(IsFireDamage, Damage);
		TakeDamageTimer.ResetTimer(true);
	}

	public void TakeDamage(bool isFireDamage, float damage)
	{
		if (isFireDamage) Health.GetFireDamage(damage);
		else Health.GetDamage(damage);
	}

	public int GetDayCount() => _day;

	private void FixedUpdate()
	{
		AllDayTimer.Tick(Time.deltaTime);
		TransitionDayTimer.Tick(Time.deltaTime);
		TransitionNightTimer.Tick(Time.deltaTime);

		transform.rotation = Quaternion.Euler(-360 / (AllDayTimer.MaxTime / AllDayTimer.CurrentTime), 0, 0);
		Moon.transform.rotation = Quaternion.Euler(360 / (AllDayTimer.MaxTime / AllDayTimer.CurrentTime), 0, 0);

		TakeDamageTimer.Tick(Time.deltaTime);

		//if (AllDayTimer.CurrentTime < TransitionDayLength) Damage = MinDamage;
		//else if (AllDayTimer.CurrentTime >= TransitionDayLength && AllDayTimer.CurrentTime <= AllDayLength / 2) Damage = MaxDamage;
		//else if (AllDayTimer.CurrentTime > AllDayLength / 2 && AllDayTimer.CurrentTime < AllDayLength / 2 + TransitionDayLength) Damage = MinDamage;
		//else if (AllDayTimer.CurrentTime >= AllDayLength / 2 + TransitionDayLength) Damage = 0;

		//if (AllDayTimer.CurrentTime >= 0 && AllDayTimer.CurrentTime <= AllDayLength / 2) Damage = 0;
		//else  Damage = MaxDamage;

		if (Damage > 0)
		{
			SunRay = new Ray(FakeSun.transform.position, Player.transform.position - FakeSun.transform.position);

			SunRayCast = Physics.RaycastAll(SunRay, 10000f);

			System.Array.Sort(SunRayCast, (a, b) => a.distance.CompareTo(b.distance));

			if (SunRayCast.Length > 0 && SunRayCast[0].transform.CompareTag("Player")) TakeDamageTimer.Continue();
			else TakeDamageTimer.ResetTimer(true);
		}
	}
}
 