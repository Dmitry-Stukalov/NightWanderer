using UnityEngine;
using UnityEngine.UIElements;

public class HealthRecovery
{
	private Health _health;
	private VisualElement _healthForeground;

	public HealthRecovery(Health health, VisualElement healthForeground)
	{
		_health = health;
		_health.OnHealthChange += UpdateData;

		_healthForeground = healthForeground;
	}

	public void RecoverHealth(float value)
	{
		_health.Healing(value);

		UpdateData();
	}

	public float NeedToRefueling() => _health.NeedToHealing();

	private void UpdateData()
	{
		var fuelPercent = _health.GetCurrentHealth() / _health.GetMaxHealth() * 100;

		_healthForeground.style.width = new Length(Mathf.Clamp(fuelPercent, 0, 100), LengthUnit.Percent);
	}
}
