using System;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.VFX;

public class WeatherManager : MonoBehaviour
{
	[SerializeField] private Sun _Sun;
	[SerializeField] private VisualEffect Rain2;
	[SerializeField] private VisualEffect Sandstorm2;
	[SerializeField] private LocalVolumetricFog Fog;
	private bool IsWeatherActive = false;
	private Timer RandomWeatherPauseTimer;
	private Timer RandomWeatherTimer;
	private string _activeWeatherName = "";

	public event Action OnWeatherChange;

	public void Initializing()
	{
		RandomWeatherPauseTimer = new Timer(UnityEngine.Random.Range(5, 10));
		RandomWeatherPauseTimer.OnTimerEnd += StartWeather;

		_Sun.OnDayStart += FogOff;
		_Sun.OnNightStart += FogOn;

		Sandstorm2.Stop();
		Rain2.Stop();

		if (!_Sun.IsDayNow()) FogOn();
	}

	private void StartWeather()
	{
		RandomWeatherTimer = new Timer(UnityEngine.Random.Range(15, 17));
		RandomWeatherTimer.OnTimerEnd += EndWeather;
		IsWeatherActive = true;

		if (_Sun.IsDayNow()) Sandstorm2.Play();
		else Rain2.Play();

		OnWeatherChange?.Invoke();
	}

	private void EndWeather()
	{
		RandomWeatherTimer.OnTimerEnd -= EndWeather;

		Sandstorm2.Stop();
		Rain2.Stop();

		IsWeatherActive = false;
		RandomWeatherPauseTimer.ResetTimer(false);

		OnWeatherChange?.Invoke();
	}

	private void FogOn()
	{
		Fog.enabled = true;

		OnWeatherChange?.Invoke();
	}

	private void FogOff()
	{
		Fog.enabled = false;

		OnWeatherChange?.Invoke();
	}

	public string GetActiveWeather()
	{
		if (_Sun.IsDayNow())
		{
			if (IsWeatherActive) _activeWeatherName = "ясно, песчана€ бур€";
			else _activeWeatherName = "ясно";
		}
		else
		{
			if (IsWeatherActive) _activeWeatherName = "“уманно, дождь";
			else _activeWeatherName = "“уманно";
		}

		return _activeWeatherName;
	}

	private void Update()
	{
		RandomWeatherPauseTimer?.Tick(Time.deltaTime);
		RandomWeatherTimer?.Tick(Time.deltaTime);
	}
}
