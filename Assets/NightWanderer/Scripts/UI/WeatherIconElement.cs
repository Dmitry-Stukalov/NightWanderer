using UnityEngine;
using UnityEngine.UIElements;

public class WeatherIconElement
{
	private VisualElement _icon;
	private WeatherManager _weatherManager;
	private Sprite[] _weatherSprites;

	public WeatherIconElement(VisualElement icon, WeatherManager weatherManager, Sprite[] weatherSprites)
	{
		_icon = icon;

		_weatherManager = weatherManager;
		_weatherManager.OnWeatherChange += UpdateData;

		_weatherSprites = weatherSprites;
	}

	private void UpdateData()
	{
		switch (_weatherManager.GetActiveWeather())
		{
			case "“уманно":
				_icon.style.backgroundImage = new StyleBackground(_weatherSprites[0]);
			break;

			case "“уманно, дождь":
				_icon.style.backgroundImage = new StyleBackground(_weatherSprites[1]);
				break;

			case "ясно":
				_icon.style.backgroundImage = new StyleBackground(_weatherSprites[2]);
				break;

			case "ясно, песчана€ бур€":
				_icon.style.backgroundImage = new StyleBackground(_weatherSprites[3]);
				break;
		}
	}
}
