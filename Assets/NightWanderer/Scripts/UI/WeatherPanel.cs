using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;

public class WeatherPanel : MonoBehaviour
{
	[SerializeField] private UIDocument PlayerUI;
	[SerializeField] private UIDocument BaseUI;
	private List<Label> _statusTexts = new List<Label>();
	private List<Label> _weatherTexts = new List<Label>();
	private List<Label> _dayTexts = new List<Label>();
	private List<Label> _timeTexts = new List<Label>();

	public void Initializing(/*Health health*/HealthFireDefense health, WeatherManager weather, Sun sun)
	{
		_statusTexts.Add(PlayerUI.rootVisualElement.Q<Label>("StatusText"));
		_statusTexts.Add(BaseUI.rootVisualElement.Q<Label>("StatusText"));

		_weatherTexts = PlayerUI.rootVisualElement.Query<Label>("WeatherText").ToList();
		_weatherTexts.Add(BaseUI.rootVisualElement.Q<Label>("WeatherText"));

		_dayTexts = PlayerUI.rootVisualElement.Query<Label>("DayText").ToList();
		_dayTexts.Add(BaseUI.rootVisualElement.Q<Label>("DayText"));

		_timeTexts = PlayerUI.rootVisualElement.Query<Label>("TimeText").ToList();
		_timeTexts.Add(BaseUI.rootVisualElement.Q<Label>("TimeText"));

		for (int i = 0; i < _statusTexts.Count; i++) _statusTexts[i].dataSource = new StatusTextLabel(_statusTexts[i], health);
		for (int i = 0; i < _weatherTexts.Count; i++) _weatherTexts[i].dataSource = new WeatherTextLabel(_weatherTexts[i], weather);
		for (int i = 0; i < _dayTexts.Count; i++) _dayTexts[i].dataSource = new DayTextLabel(_dayTexts[i], sun);
		for (int i = 0; i < _timeTexts.Count; i++) _timeTexts[i].dataSource = new TimeTextLabel(_timeTexts[i], sun);
	}
}
