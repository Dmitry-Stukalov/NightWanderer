using UnityEngine;
using UnityEngine.UIElements;

public class EnginesVerticalStatistic
{
	private JetEngines _engines;
	private Label _text;

	public EnginesVerticalStatistic(Label text, JetEngines engines)
	{
		_text = text;
		_engines = engines;
		_engines.OnUpgrade += UpdateData;
		UpdateData();
	}

	private void UpdateData()
	{
		_text.text = $"Вертикал. скорость: {_engines.GetWalkSpeedUp() * 100} км/ч";
	}
}
