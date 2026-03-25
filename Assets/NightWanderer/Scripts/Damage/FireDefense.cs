using System;
using System.Collections.Generic;
using UnityEngine;

public class FireDefense : IImprovementBase
{
	public string Name { get; set; }
	public Dictionary<int, int> _needResources { get; set; } = new Dictionary<int, int>();
	public ImprovementConfig Config { get; set; }
	public int CurrentLevel { get; set; }

	private ImprovementHealthConfig _config;

	private float _minFireDefense;
	private float _currentFireDefense;

	public event Action OnFireDefenseChange;

	public FireDefense(ImprovementConfig config)
	{
		Config = config;
		_config = (ImprovementHealthConfig)config;
		CurrentLevel = 0;

		_currentFireDefense = _config.Levels[CurrentLevel].MaxHealth;
	}

	public void GetFireDamage(float value)
	{
		_currentFireDefense -= value;

		if (_currentFireDefense <= 0)
		{
			_currentFireDefense = _minFireDefense;

			Debug.Log("«ащита от огн€ разрушена");
		}

		OnFireDefenseChange?.Invoke();
	}

		//¬осстановление термальной защиты на указанное значение
	public void RestoreFireDefense(float restoreValue)
	{
		_currentFireDefense += restoreValue;

		if (_currentFireDefense > _config.Levels[CurrentLevel].MaxHealth) _currentFireDefense = _config.Levels[CurrentLevel].MaxHealth;

		OnFireDefenseChange?.Invoke();
	}

	public float GetCurrentFireDefense() => _currentFireDefense;
	public float GetMaxFireDefense() => _config.Levels[CurrentLevel].MaxHealth;
	public float NeedToRecover() => _config.Levels[CurrentLevel].MaxHealth - _currentFireDefense;

	public Dictionary<int, int> GetNeedResources()
	{
		return null;
	}
}
