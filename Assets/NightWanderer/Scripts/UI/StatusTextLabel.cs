using UnityEngine;
using UnityEngine.UIElements;

public class StatusTextLabel
{
	private Label _text;
	//private Health _health;
	private HealthFireDefense _health;

	public StatusTextLabel(Label text, /*Health health*/HealthFireDefense health)
	{
		_text = text;
		_health = health;
		_health.OnHealthChange += UpdateData;
	}

	private void UpdateData()
	{
		switch(_health.GetCurrentHealth())
		{
			case float h when h <= _health.GetMaxHealth() / 4:
				_text.text = "Критический";
			break;

			case float h when (h > _health.GetMaxHealth() / 4) && (h <= _health.GetMaxHealth() / 2):
				_text.text = "Аварийный";
			break;

			case float h when (h > _health.GetMaxHealth() / 2) && (h < _health.GetMaxHealth() / 4 * 3):
				_text.text = "Поврежденный";
			break;

			case float h when (h >= _health.GetMaxHealth() / 4 * 3):
				_text.text = "Стабильный";
			break;
		}
	}
}
