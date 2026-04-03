using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.VFX;

public class WeatherManager : MonoBehaviour
{
	[SerializeField] private Sun _Sun;
	[SerializeField] private VisualEffect Rain2;
	[SerializeField] private VisualEffect Sandstorm2;
	[SerializeField] private float _rainSpawnRate;
	[SerializeField] private float _sandstormSpawnRate;
	[SerializeField] private LocalVolumetricFog Fog;
	[SerializeField] private Material _fogMaterial;
	private bool IsSandstormActive = false;
	private bool IsRainActive = false;
	private bool IsWeatherActive = false;
	private Timer RandomWeatherPauseTimer;
	private Timer RandomWeatherTimer;
	private string _activeWeatherName = "";

	public event Action OnWeatherChange;

	public void Initializing()
	{
		RandomWeatherPauseTimer = new Timer(UnityEngine.Random.Range(300, 700));
		RandomWeatherPauseTimer.OnTimerEnd += StartWeather;

		_Sun.OnDayStart += EndWeather;
		_Sun.OnNightStart += EndWeather;
		_Sun.OnTransitionDayEnd += FogOff;
		_Sun.OnTransitionNightEnd += FogOn;

		Sandstorm2.Stop();
		Rain2.Stop();

		_fogMaterial.SetFloat("_FogDistance", 100);
	}

	private void StartWeather()
	{
		RandomWeatherTimer = new Timer(UnityEngine.Random.Range(80, 120));
		RandomWeatherTimer.OnTimerEnd += EndWeather;
		IsWeatherActive = true;

		if (_Sun.IsDayNow())
		{
			Sandstorm2.SetFloat("SpawnRate", 0);

			Sandstorm2.Play();

			DOTween.To(() => Sandstorm2.GetFloat("SpawnRate"), x => Sandstorm2.SetFloat("SpawnRate", x), _rainSpawnRate, 20f).SetEase(Ease.Linear);

			IsSandstormActive = true;
		}
		else
		{
			Rain2.SetFloat("SpawnRate", 0);

			Rain2.Play();

			DOTween.To(() => Rain2.GetFloat("SpawnRate"), x => Rain2.SetFloat("SpawnRate", x), _rainSpawnRate, 20f).SetEase(Ease.Linear);

			IsRainActive = true;
		}

		OnWeatherChange?.Invoke();
	}

	private void EndWeather()
	{
		if (RandomWeatherTimer != null)	RandomWeatherTimer.OnTimerEnd -= EndWeather;
		
		if (IsSandstormActive)
		{
			DOTween.To(() => Sandstorm2.GetFloat("SpawnRate"), x => Sandstorm2.SetFloat("SpawnRate", x), 0, 20).SetEase(Ease.Linear).OnComplete(() =>
			{
				Sandstorm2.Stop();
			});

			IsSandstormActive = false;
		}

		if (IsRainActive)
		{
			DOTween.To(() => Rain2.GetFloat("SpawnRate"), x => Rain2.SetFloat("SpawnRate", x), 0, 20).SetEase(Ease.Linear).OnComplete(() =>
			{
				Rain2.Stop();
			});

			IsRainActive = false;
		}


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
