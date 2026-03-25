using System;
using UnityEngine;
using UnityEngine.UIElements;

public class TimeTextLabel
{
	private Label _text;
	private Sun _sun;

	public TimeTextLabel(Label text, Sun sun)
	{
		_text = text;

		_sun = sun;
		_sun.AllDayTimer.OnTick += UpdateData;

		UpdateData();
	}

	private void UpdateData() => _text.text = $"{ConvertTime(_sun.AllDayTimer.CurrentTime, false)}:{ConvertTime(_sun.AllDayTimer.CurrentTime, true)}";

	private string ConvertTime(float time, bool isMinutes)
	{
		float newTime = Mathf.Round(time);

		if (isMinutes) newTime %= 60;
		else newTime /= 60;

		if (newTime == 0) return "00";
		else if (newTime > 0 && time < 10) return $"0{Convert.ToInt32(newTime)}";
		else return Convert.ToInt32(newTime).ToString();
	}
}
