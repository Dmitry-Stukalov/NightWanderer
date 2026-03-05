using Unity.VisualScripting;
using UnityEngine;

public class WeatherManager : MonoBehaviour
{
	[SerializeField] private Sun _Sun;
	[SerializeField] private ParticleSystem Rain;
	[SerializeField] private ParticleSystem Sandstorm;
	private bool IsWeatherActive = false;
	private Timer RandomWeatherPauseTimer;
	private Timer RandomWeatherTimer;

	private void Start()
	{
		RandomWeatherPauseTimer = new Timer(Random.Range(5, 10));
		RandomWeatherPauseTimer.OnTimerEnd += StartWeather;
	}

	private void StartWeather()
	{
		RandomWeatherTimer = new Timer(Random.Range(5, 7));
		RandomWeatherTimer.OnTimerEnd += EndWeather;
		IsWeatherActive = true;

		if (_Sun.IsDayNow()) Sandstorm.Play();
		else Rain.Play();
	}

	private void EndWeather()
	{
		RandomWeatherTimer.OnTimerEnd -= EndWeather;

		if (Sandstorm.isPlaying) Sandstorm.Stop();
		if (Rain.isPlaying) Rain.Stop();

		IsWeatherActive = false;
		RandomWeatherPauseTimer.ResetTimer(false);
	}

	private void Update()
	{
		RandomWeatherPauseTimer.Tick(Time.deltaTime);
		if (RandomWeatherTimer != null) RandomWeatherTimer.Tick(Time.deltaTime);
	}
}
