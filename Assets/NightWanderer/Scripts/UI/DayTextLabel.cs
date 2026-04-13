using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class DayTextLabel
{
	private Label _text;
	private Sun _sun;

	public DayTextLabel(Label text, Sun sun)
	{
		_text = text;

		_sun = sun;
		_sun.OnDayStart += UpdateData;

		UpdateData();
	}

	private void UpdateData() => _text.text = $"Δενό {_sun.GetDayCount()}";
}
