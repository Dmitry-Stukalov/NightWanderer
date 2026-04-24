using UnityEngine;
using UnityEngine.UIElements;

public class HealthStatistic
{
	private HealthFireDefense _health;
	private Label _text;

	public HealthStatistic(Label text, HealthFireDefense health)
	{
		_text = text;
		_health = health;
		_health.OnUpgrade += UpdateData;
		UpdateData();
	}

	private void UpdateData()
	{
		_text.text = $"Çהמנמגüו: {_health.GetMaxHealth()} וה.";
	}
}
