using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.VFX;

public class WeatherManager : MonoBehaviour
{
	[SerializeField] private Sun _Sun;
	[SerializeField] private ParticleSystem Rain;
	[SerializeField] private ParticleSystem Sandstorm;
	[SerializeField] private VisualEffect Sandstorm2;
	[SerializeField] private LocalVolumetricFog Fog;
	private bool IsWeatherActive = false;
	private Timer RandomWeatherPauseTimer;
	private Timer RandomWeatherTimer;

	private void Start()
	{
		RandomWeatherPauseTimer = new Timer(Random.Range(5, 10));
		RandomWeatherPauseTimer.OnTimerEnd += StartWeather;

		_Sun.OnDayStart += FogOff;
		_Sun.OnNightStart += FogOn;
	}

	private void StartWeather()
	{
		RandomWeatherTimer = new Timer(Random.Range(5, 7));
		RandomWeatherTimer.OnTimerEnd += EndWeather;
		IsWeatherActive = true;

		if (_Sun.IsDayNow()) Sandstorm2.Play();
		else Rain.Play();
	}

	private void EndWeather()
	{
		RandomWeatherTimer.OnTimerEnd -= EndWeather;

		/*if (Sandstorm.isPlaying)*/ Sandstorm2.Stop();
		if (Rain.isPlaying) Rain.Stop();

		IsWeatherActive = false;
		RandomWeatherPauseTimer.ResetTimer(false);
	}

	private void FogOn()
	{
		Fog.enabled = true;
	}

	private void FogOff()
	{
		Fog.enabled = false;
	}

	private void Update()
	{
		RandomWeatherPauseTimer.Tick(Time.deltaTime);
		if (RandomWeatherTimer != null) RandomWeatherTimer.Tick(Time.deltaTime);
	}
}
