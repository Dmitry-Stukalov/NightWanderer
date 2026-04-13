using System;
using System.Collections.Generic;
using UnityEngine;

//Общий класс для здоровья, защиты и термической защиты
public class HealthFireDefense : IImprovementBase
{
	public string Name { get; set; }
	public Dictionary<int, int> _needResources { get; set; } = new Dictionary<int, int>();
	public ImprovementConfig Config { get; set; }
	public int CurrentLevel { get; set; }

	private ImprovementHealthConfig _config;

	private float _minHealth;
	private float _currentHealth;

	public event Action OnHealthChange;

	public HealthFireDefense(ImprovementConfig config)
	{
		Config = config;
		_config = (ImprovementHealthConfig)config;
		CurrentLevel = 0;

		_currentHealth = _config.Levels[CurrentLevel].MaxHealth;
	}	

	public void GetDamage(float value)
	{
		_currentHealth -= value;

		if (_currentHealth <= 0) _currentHealth = _minHealth;

		OnHealthChange?.Invoke();
	}

	public void Healing(float healValue)
	{
		_currentHealth += healValue;

		if (_currentHealth > _config.Levels[CurrentLevel].MaxHealth) _currentHealth = _config.Levels[CurrentLevel].MaxHealth;

		OnHealthChange?.Invoke();
	}

	public float GetCurrentHealth() => _currentHealth;
	public float GetMaxHealth() => _config.Levels[CurrentLevel].MaxHealth;
	public float NeedToHealing() => _config.Levels[CurrentLevel].MaxHealth - _currentHealth;

	public Dictionary<int, int> GetNeedResources()
	{
		_needResources?.Clear();

		for (int i = 0; i < _config.Levels[CurrentLevel].Resource.Count; i++)
		{
			_needResources[_config.Levels[CurrentLevel].Resource[i]] = _config.Levels[CurrentLevel].Count[i];
		}

		return _needResources;
	}
}
