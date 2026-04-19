using DG.Tweening;
using System;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering;
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
	[SerializeField] private float _sandstormDamage;
	private bool IsSandstormActive = false;
	private bool IsRainActive = false;
	private bool IsWeatherActive = false;
	private Timer RandomWeatherPauseTimer;
	private Timer RandomWeatherTimer;
	private Timer PlayerGetDamageTimer;
	private string _activeWeatherName = "";

	public event Action OnWeatherChange;

	public void Initializing()
	{
		Fog = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<LocalVolumetricFog>();

		RandomWeatherPauseTimer = new Timer(UnityEngine.Random.Range(300, 700));
		RandomWeatherPauseTimer.OnTimerEnd += StartWeather;

		PlayerGetDamageTimer = new Timer(2f);
		PlayerGetDamageTimer.OnTimerEnd += PlayerGetDamage;
		PlayerGetDamageTimer.SetPause();

		_Sun.OnDayStart += EndWeather;
		_Sun.OnNightStart += EndWeather;
		_Sun.OnTransitionDayEnd += FogOff;
		_Sun.OnTransitionNightEnd += FogOn;

		Sandstorm2.Stop();
		Rain2.Stop();

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

			PlayerGetDamageTimer.Continue();
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

			PlayerGetDamageTimer.ResetTimer(true);
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

	private void PlayerGetDamage()
	{
		_Sun.TakeDamage(false, _sandstormDamage);
	}

	private void FogOn()
	{
		DOTween.To(() => Fog.parameters.distanceFadeEnd, x => Fog.parameters.distanceFadeEnd = x, 100f, 60f).SetEase(Ease.Linear);

		OnWeatherChange?.Invoke();
	}

	private void FogOff()
	{
		DOTween.To(() => Fog.parameters.distanceFadeEnd, x => Fog.parameters.distanceFadeEnd = x, 0f, 5f).SetEase(Ease.Linear);

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
		PlayerGetDamageTimer?.Tick(Time.deltaTime);
	}
}
