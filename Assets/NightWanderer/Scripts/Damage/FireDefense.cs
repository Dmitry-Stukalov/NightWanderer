using UnityEngine;

public class FireDefense : IImprovementBase
{
	public ImprovementConfig Config { get; set; }
	public int CurrentLevel { get; set; }

	private ImprovementHealthConfig _config;

	private float _minFireDefense;
	private float _maxFireDefense;
	private float _currentFireDefense;

	public FireDefense(ImprovementConfig config)
	{
		_config = (ImprovementHealthConfig)config;
		CurrentLevel = 0;

		_maxFireDefense = _config.Levels[CurrentLevel].MaxHealth;

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
	}

		//¬осстановление термальной защиты на указанное значение
	public void RestoreFireDefense(float restoreValue)
	{
		_currentFireDefense += restoreValue;

		if (_currentFireDefense > _maxFireDefense) _currentFireDefense = _maxFireDefense;
	}

	public float GetCurrentFireDefense() => _currentFireDefense;

	public void Upgrade()
	{
		CurrentLevel++;

		_maxFireDefense = _config.Levels[CurrentLevel].MaxHealth;
	}
}
