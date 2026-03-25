using System;
using System.Collections.Generic;
using UnityEngine;

public class Defense : IImprovementBase
{
	public string Name { get; set; }
	public Dictionary<int, int> _needResources { get; set; } = new Dictionary<int, int>();
	public ImprovementConfig Config { get; set; }
	public int CurrentLevel { get; set; }

	private ImprovementHealthConfig _config;

	private float _minDefense;
	private float _currentDefense;

	public event Action OnDefenseChange;


	public Defense(ImprovementConfig config)
	{
		Config = config;
		_config = (ImprovementHealthConfig)config;
		CurrentLevel = 0;

		_currentDefense = _config.Levels[CurrentLevel].MaxHealth;
	}

	public void GetDamage(float value)
	{
		_currentDefense -= value;

		if (_currentDefense <= 0) _currentDefense = _minDefense;
	}

	//Восстановление физической защиты на указанное значение
	public void RestoreDefense(float restoreValue)
	{
		_currentDefense += restoreValue;

		if (_currentDefense > _config.Levels[CurrentLevel].MaxHealth) _currentDefense = _config.Levels[CurrentLevel].MaxHealth;
	}

	public float GetCurrentDefense() => _currentDefense;
	public float GetMaxDefense() => _config.Levels[CurrentLevel].MaxHealth;
	public float NeedToRecover() => _config.Levels[CurrentLevel].MaxHealth - _currentDefense;

	public Dictionary<int, int> GetNeedResources()
	{
		return null;
	}
}
