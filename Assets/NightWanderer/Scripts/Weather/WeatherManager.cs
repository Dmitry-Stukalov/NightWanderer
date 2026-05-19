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
	[SerializeField] private VisualEffect _rain;
	[SerializeField] private VisualEffect _sandstorm;
	[SerializeField] private float _rainSpawnRate;
	[SerializeField] private float _sandstormSpawnRate;
	[SerializeField] private LocalVolumetricFog _fog;
	[SerializeField] private float _sandstormDamage;
	private Timer _randomWeatherPauseTimer;
	private Timer _randomWeatherTimer;
	private Timer _playerGetDamageTimer;
	private string _activeWeatherName = "";
	private float _rainCurrentSpawnRate;
	private float _sandstormCurrentSpawnRate;
	private bool IsSandstormActive = false;
	private bool IsRainActive = false;
	private bool IsWeatherActive = false;

	public event Action OnWeatherChange;

	public void Initializing()
	{
		_fog = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<LocalVolumetricFog>();

		_randomWeatherPauseTimer = new Timer(UnityEngine.Random.Range(300, 800));
		_randomWeatherPauseTimer.OnTimerEnd += StartWeather;

		_playerGetDamageTimer = new Timer(2f);
		_playerGetDamageTimer.OnTimerEnd += PlayerGetDamage;
		_playerGetDamageTimer.SetPause();

		_Sun.OnDayStart += EndWeather;
		_Sun.OnNightStart += EndWeather;
		_Sun.OnTransitionDayEnd += FogOff;
		_Sun.OnTransitionNightEnd += FogOn;

		_sandstorm.Stop();
		_rain.Stop();

		_rainCurrentSpawnRate = 0;
		_sandstormCurrentSpawnRate = 0;

		GameEvents.OnInBase += HideParticles;
		GameEvents.OnOutBase += ShowParticles;
	}

	private void StartWeather()
	{
		_randomWeatherTimer = new Timer(UnityEngine.Random.Range(60, 91));
		_randomWeatherTimer.OnTimerEnd += EndWeather;
		IsWeatherActive = true;

		DOTween.Kill(_sandstorm);
		DOTween.Kill(_rain);

		if (_Sun.IsDayNow())
		{
			_sandstorm.SetFloat("SpawnRate", 0);

			_sandstorm.Play();

			DOTween.To(() => _sandstorm.GetFloat("SpawnRate"), x => _sandstorm.SetFloat("SpawnRate", x), _sandstormCurrentSpawnRate, 10f).SetEase(Ease.Linear);

			IsSandstormActive = true;

			_playerGetDamageTimer.Continue();
		}
		else
		{
			_rain.SetFloat("SpawnRate", 0);

			_rain.Play();

			DOTween.To(() => _rain.GetFloat("SpawnRate"), x => _rain.SetFloat("SpawnRate", x), _rainCurrentSpawnRate, 10f).SetEase(Ease.Linear);

			IsRainActive = true;
		}

		_randomWeatherPauseTimer.ResetTimer(false);

		OnWeatherChange?.Invoke();
	}

	private void EndWeather()
	{
		if (_randomWeatherTimer != null) _randomWeatherTimer.OnTimerEnd -= EndWeather;
		
		if (IsSandstormActive)
		{
			DOTween.To(() => _sandstorm.GetFloat("SpawnRate"), x => _sandstorm.SetFloat("SpawnRate", x), 0, 3).SetEase(Ease.Linear).OnComplete(() =>
			{
				_sandstorm.Stop();
			});

			IsSandstormActive = false;

			_playerGetDamageTimer.ResetTimer(true);
		}

		if (IsRainActive)
		{
			DOTween.To(() => _rain.GetFloat("SpawnRate"), x => _rain.SetFloat("SpawnRate", x), 0, 3).SetEase(Ease.Linear).OnComplete(() =>
			{
				_rain.Stop();
			});

			IsRainActive = false;
		}

		IsWeatherActive = false;

		OnWeatherChange?.Invoke();
	}

	private void PlayerGetDamage()
	{
		_Sun.TakeDamage(false, _sandstormDamage);
	}

	private void FogOn()
	{
		DOTween.Kill(_fog);

		DOTween.To(() => _fog.parameters.distanceFadeEnd, x => _fog.parameters.distanceFadeEnd = x, 100f, 20f).SetEase(Ease.Linear);

		OnWeatherChange?.Invoke();
	}

	private void FogOff()
	{
		DOTween.Kill(_fog);

		DOTween.To(() => _fog.parameters.distanceFadeEnd, x => _fog.parameters.distanceFadeEnd = x, 0f, 3f).SetEase(Ease.Linear);

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

	private void HideParticles()
	{
		DOTween.Kill(_rain);
		DOTween.Kill(_sandstorm);

		_rainCurrentSpawnRate = 0;
		_sandstormCurrentSpawnRate = 0;

		DOTween.To(() => _sandstorm.GetFloat("SpawnRate"), x => _sandstorm.SetFloat("SpawnRate", x), _sandstormCurrentSpawnRate, 1f).SetEase(Ease.Linear);
		DOTween.To(() => _rain.GetFloat("SpawnRate"), x => _rain.SetFloat("SpawnRate", x), _rainCurrentSpawnRate, 1f).SetEase(Ease.Linear);
	}	

	private void ShowParticles()
	{
		DOTween.Kill(_rain);
		DOTween.Kill(_sandstorm);

		_rainCurrentSpawnRate = _rainSpawnRate;
		_sandstormCurrentSpawnRate = _sandstormSpawnRate;

		DOTween.To(() => _sandstorm.GetFloat("SpawnRate"), x => _sandstorm.SetFloat("SpawnRate", x), _sandstormCurrentSpawnRate, 1f).SetEase(Ease.Linear);
		DOTween.To(() => _rain.GetFloat("SpawnRate"), x => _rain.SetFloat("SpawnRate", x), _rainCurrentSpawnRate, 1f).SetEase(Ease.Linear);
	}

	private void Update()
	{
		if (_Sun != null && _Sun.IsTimeSkip)
		{
			if (!IsWeatherActive) _randomWeatherPauseTimer?.Tick(Time.deltaTime * 15);
			_randomWeatherTimer?.Tick(Time.deltaTime * 15);
			_playerGetDamageTimer?.Tick(Time.deltaTime * 15);
		}
		else
		{
			if (!IsWeatherActive) _randomWeatherPauseTimer?.Tick(Time.deltaTime);
			_randomWeatherTimer?.Tick(Time.deltaTime);
			_playerGetDamageTimer?.Tick(Time.deltaTime);
		}
	}

	private void OnDisable()
	{
		GameEvents.OnInBase -= HideParticles;
		GameEvents.OnOutBase -= ShowParticles;
	}
}
