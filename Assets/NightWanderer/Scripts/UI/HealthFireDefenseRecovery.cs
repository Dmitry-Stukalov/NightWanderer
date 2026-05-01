using UnityEngine;
using UnityEngine.UIElements;

public class HealthFireDefenseRecovery
{
	private HealthFireDefense _health;
	private VisualElement _healthForeground;

	public HealthFireDefenseRecovery(HealthFireDefense health, VisualElement healthForeground)
	{
		_health = health;
		_health.OnHealthChange += UpdateData;

		_healthForeground = healthForeground;
		UpdateData();
	}

	public void RecoverHealth(float value)
	{
		_health.Healing(value);

		UpdateData();
	}

	public float NeedToHealing() => _health.NeedToHealing();

	private void UpdateData()
	{
		float healthPercent;

		if (_health.GetMaxHealth() != 0)
		{
			healthPercent = _health.GetCurrentHealth() / _health.GetMaxHealth() * 100;
			_healthForeground.style.width = new Length(Mathf.Clamp(healthPercent, 0, 100), LengthUnit.Percent);
		}
		else
		{
			healthPercent = 0;
			_healthForeground.style.width = new Length(0, LengthUnit.Percent);
		}
	}
}
