using System.ComponentModel;
using UnityEngine;
using UnityEngine.Audio;

public class ShipSoundsManager : MonoBehaviour
{
	[SerializeField] private AudioMixerSnapshot _sand;
	[SerializeField] private AudioMixerSnapshot _base;
	[SerializeField] private AudioMixerSnapshot _settings;
	[SerializeField] private AudioSource _sandDaySandstormSound;
	[SerializeField] private AudioSource _sandNightSandstromSound;
	[SerializeField] private AudioSource _rainNightSound;
	[SerializeField] private AudioSource[] _sandBackgroundMusic;
	[SerializeField] private AudioSource[] _sandBackgroundSounds;
	[SerializeField] private AudioSource[] _enginesSounds;
	private Sun _sun;
	private Timer _backgroundMusicPause;
	private Timer _backgroundMusicDuration;
	private Timer _backgroundSoundPause;
	private Timer _backgroundSoundDuration;
	private int _lastBackgroundMusic = 0;

	private void Start()
	{
		_sand.TransitionTo(65);

		_backgroundMusicPause = new Timer(Random.Range(60, 121));
		_backgroundMusicPause.OnTimerEnd += SetRandomBackgroundMusic;

		_backgroundSoundPause = new Timer(Random.Range(20, 41));
		_backgroundSoundPause.OnTimerEnd += SetRandomSandSound;

		_backgroundMusicDuration = new Timer(0);
		_backgroundMusicDuration.OnTimerEnd += StartMusicPause;

		_backgroundSoundDuration = new Timer(0);
		_backgroundSoundDuration.OnTimerEnd += StartSoundPause;

		GameEvents.OnDayStart += DayStart;
		GameEvents.OnNightStart += NightStart;
		GameEvents.OnInBase += () => _base.TransitionTo(1);
		GameEvents.OnOutBase += () => _sand.TransitionTo(1);

		GameEvents.OnMovementChange += SetEnginesSounds;

		GameEvents.OnSandstormStart += SandstormStart;
		GameEvents.OnSandstormEnd += SandstormEnd;
		GameEvents.OnRainStart += RainStart;
		GameEvents.OnRainEnd += RainEnd;

		GameEvents.OnSettingsOpen += () => _settings.TransitionTo(1);
		GameEvents.OnSettingsClose += () => _sand.TransitionTo(1);

		SetRandomSandSound();
	}

	public void Initializing(Sun sun)
	{
		_sun = sun;
		_sun.OnDayStart += DayStart;
		_sun.OnNightStart += NightStart;
	}

	private void DayStart()
	{

	}

	private void NightStart()
	{

	}

	private void SetEnginesSounds(int id)
	{
		/*if (id == 0)
		{
			for (int i = 0; i < _enginesSounds.Length; i++) _enginesSounds[i].Stop();
		}
		else _enginesSounds[0].Play();*/
	}

	private void StartMusicPause()
	{
		_backgroundMusicPause.SetMaxTimeAndReset(Random.Range(60, 121));
	}

	private void SetRandomBackgroundMusic()
	{
		int newMusic;

		do
		{
			newMusic = Random.Range(0, _sandBackgroundMusic.Length);
		} while (newMusic == _lastBackgroundMusic);

		_backgroundMusicDuration.SetMaxTimeAndReset(_sandBackgroundMusic[newMusic].clip.length);
		_sandBackgroundMusic[newMusic].Play();
	}

	private void StartSoundPause()
	{
		_backgroundSoundPause.SetMaxTimeAndReset(Random.Range(20, 41));
	}

	private void SetRandomSandSound()
	{
		int randomNumber = Random.Range(0, 11);

		if (_sun == null || _sun.IsDayNow())
		{
			if (randomNumber <= 6) _sandBackgroundSounds[0].Play();
			else _sandBackgroundSounds[1].Play();
		}
		else
		{
			if (randomNumber <= 5) _sandBackgroundSounds[0].Play();
			if (randomNumber > 5 && randomNumber <= 8) _sandBackgroundSounds[1].Play();
			if (randomNumber > 8) _sandBackgroundSounds[2].Play();
		}

		for (int i = 0; i < _sandBackgroundSounds.Length; i++)
		{
			if (_sandBackgroundSounds[i].isPlaying)
			{
				_backgroundSoundDuration.SetMaxTimeAndReset(_sandBackgroundSounds[i].clip.length);
				return;
			}
		}
	}

	private void SandstormStart()
	{
		for (int i = 0; i < _sandBackgroundSounds.Length; i++) _sandBackgroundSounds[i].Stop();
		_backgroundSoundDuration.SetPause();

		if (_sun.IsDayNow()) _sandDaySandstormSound.Play();
		else _sandNightSandstromSound.Play();
	}

	private void SandstormEnd()
	{
		_sandDaySandstormSound.Stop();

		SetRandomSandSound();
		_backgroundSoundDuration.Continue();
	}

	private void RainStart()
	{
		for (int i = 0; i < _sandBackgroundSounds.Length; i++) _sandBackgroundSounds[i].Stop();
		_backgroundSoundDuration.SetPause();

		_rainNightSound.Play();
	}

	private void RainEnd()
	{
		_rainNightSound.Stop();

		SetRandomSandSound();
		_backgroundSoundDuration.Continue();
	}

	private void Update()
	{
		_backgroundMusicPause.Tick(Time.deltaTime);
		_backgroundMusicDuration.Tick(Time.deltaTime);
		_backgroundSoundDuration.Tick(Time.deltaTime);
	}

	private void OnDisable()
	{
		if (_sun != null)
		{
			_sun.OnDayStart -= DayStart;
			_sun.OnNightStart -= NightStart;
		}

		_backgroundMusicPause.OnTimerEnd -= SetRandomBackgroundMusic;
		_backgroundMusicDuration.OnTimerEnd -= StartMusicPause;
		_backgroundSoundDuration.OnTimerEnd -= SetRandomSandSound;

		GameEvents.OnDayStart -= DayStart;
		GameEvents.OnNightStart -= NightStart;
		GameEvents.OnInBase -= () => _base.TransitionTo(5);
		GameEvents.OnOutBase -= () => _sand.TransitionTo(5);

		GameEvents.OnMovementChange -= SetEnginesSounds;

		GameEvents.OnSandstormStart -= SandstormStart;
		GameEvents.OnSandstormEnd -= SandstormEnd;
		GameEvents.OnRainStart -= RainStart;
		GameEvents.OnRainEnd -= RainEnd;

		GameEvents.OnSettingsOpen -= () => _settings.TransitionTo(1);
		GameEvents.OnSettingsClose -= () => _sand.TransitionTo(1);
	}
}
