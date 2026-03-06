using System;
using System.Threading;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Rendering.HighDefinition;

public class Sun : MonoBehaviour, ICanTakeDamage
{
	[SerializeField] private GameObject Moon;
	[SerializeField] private GameObject FakeSun;
	[SerializeField] private GameObject Player;
	[SerializeField] public Health PlayerHealth { get; set; }
	[SerializeField] public float MinDamage { get; set; }
	[SerializeField] public float MaxDamage { get; set; }
	[field: NonSerialized] public float Damage { get; set; }
	[SerializeField] private float TakeDamagePause;
	[SerializeField] public bool IsFireDamage { get; set; }
	[SerializeField] private float AllDayLength;
	[SerializeField] private float TransitionDayLength;
	private Timer AllDayTimer;
	private Timer TakeDamageTimer;
	private Ray SunRay;
	private RaycastHit[] SunRayCast;

	public event Action OnDayStart;
	public event Action OnNightStart;

	public void Initializing()
	{
		AllDayTimer = new Timer(AllDayLength);
		AllDayTimer.OnTimerEnd += ResetDayTimer;
		AllDayTimer.OnTimerStart += () => OnDayStart?.Invoke();
		AllDayTimer.OnTimerHalf += () => OnNightStart?.Invoke();

		TakeDamageTimer = new Timer(TakeDamagePause);
		TakeDamageTimer.OnTimerEnd += ResetTakeDamage;
		TakeDamageTimer.SetPause();

		Damage = MinDamage;
	}

	private void ResetDayTimer()
	{
		AllDayTimer.ResetTimer(false);
	}

	public bool IsDayNow()
	{
		if (AllDayTimer.CurrentTime < AllDayTimer.MaxTime / 2) return true;
		else return false;
	}

	private void ResetTakeDamage()
	{
		TakeDamage();
		TakeDamageTimer.ResetTimer(true);
	}

	public void TakeDamage()
	{
		if (IsFireDamage) PlayerHealth.GetFireDamage(Damage);
		else PlayerHealth.GetDamage(Damage);
	}

	private void FixedUpdate()
	{
		AllDayTimer.Tick(Time.deltaTime);
		transform.rotation = Quaternion.Euler(360 / (AllDayTimer.MaxTime / AllDayTimer.CurrentTime), 0, 0);
		Moon.transform.rotation = Quaternion.Euler(-360 / (AllDayTimer.MaxTime / AllDayTimer.CurrentTime), 0, 0);

		if (AllDayTimer.CurrentTime >= AllDayTimer.MaxTime / 2) OnNightStart?.Invoke();

		TakeDamageTimer.Tick(Time.deltaTime);

		if (AllDayTimer.CurrentTime < TransitionDayLength) Damage = MinDamage;
		else if (AllDayTimer.CurrentTime >= TransitionDayLength && AllDayTimer.CurrentTime <= AllDayLength / 2) Damage = MaxDamage;
		else if (AllDayTimer.CurrentTime > AllDayLength / 2 && AllDayTimer.CurrentTime < AllDayLength / 2 + TransitionDayLength) Damage = MinDamage;
		else if (AllDayTimer.CurrentTime >= AllDayLength / 2 + TransitionDayLength) Damage = 0;

		if (Damage > 0)
		{
			SunRay = new Ray(FakeSun.transform.position, Player.transform.position - FakeSun.transform.position);

			SunRayCast = Physics.RaycastAll(SunRay, 1000f);

			System.Array.Sort(SunRayCast, (a, b) => a.distance.CompareTo(b.distance));

			if (SunRayCast.Length > 0 && SunRayCast[0].transform.CompareTag("Player"))
			{
				TakeDamageTimer.Continue();
			}
			else
			{
				TakeDamageTimer.ResetTimer(true);
			}
		}
	}
}
 