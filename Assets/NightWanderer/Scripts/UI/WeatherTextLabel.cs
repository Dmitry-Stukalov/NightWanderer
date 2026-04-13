using UnityEngine;
using UnityEngine.UIElements;

public class WeatherTextLabel
{
	private Label _text;
	private WeatherManager _weather;

	public WeatherTextLabel(Label text, WeatherManager weather)
	{
		_text = text;

		_weather = weather;
		_weather.OnWeatherChange += UpdateData;
	}

	private void UpdateData() => _text.text = _weather.GetActiveWeather();
}
