using UnityEngine;

public class DeadZone : MonoBehaviour
{
	[SerializeField] private float _damageTick;
	[SerializeField] private float _damage;
	private Timer _damageTimer;
	private DefenseSystem _playerHealth;

	private void Start()
	{
		_damageTimer = new Timer(_damageTick);
		_damageTimer.OnTimerEnd += DealDamage;
		_damageTimer.SetPause();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			_damageTimer.Continue();
			_playerHealth = other.GetComponent<ShipMovement>().GetPlayerDefenseSystem();
			//GameEvents.OnDialogueWarningStart?.Invoke(0);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			_damageTimer.ResetTimer(true);
			_playerHealth = null;
		}
	}

	private void DealDamage()
	{
		_playerHealth.GetDamage(_damage);
		_damageTimer.ResetTimer(false);
	}

	private void Update()
	{
		_damageTimer.Tick(Time.deltaTime);
	}
}
