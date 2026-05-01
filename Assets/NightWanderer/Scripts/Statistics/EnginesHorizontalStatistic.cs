using UnityEngine;
using UnityEngine.UIElements;

public class EnginesHorizontalStatistic
{
	private JetEngines _engines;
	private Label _text;

	public EnginesHorizontalStatistic(Label text, JetEngines engines)
	{
		_text = text;
		_engines = engines;
		_engines.OnUpgrade += UpdateData;
		UpdateData();
	}

	private void UpdateData()
	{
		_text.text = $"Горизонт. скорость: {_engines.GetWalkSpeed() * 100} км/ч";
	}
}
