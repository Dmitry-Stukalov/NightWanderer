using UnityEngine;

public class Defense : IImprovementBase
{
	public ImprovementConfig Config { get; set; }
	public int CurrentLevel { get; set; }

	private ImprovementHealthConfig _config;

	private float _minDefense;
	private float _maxDefense;
	private float _currentDefense;


	public Defense(ImprovementConfig config)
	{
		_config = (ImprovementHealthConfig)config;
		CurrentLevel = 0;

		_maxDefense = _config.Levels[CurrentLevel].MaxHealth;

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

		if (_currentDefense > _maxDefense) _currentDefense = _maxDefense;
	}

	public float GetCurrentDefense() => _currentDefense;

	public void Upgrade()
	{
		CurrentLevel++;

		_maxDefense = _config.Levels[CurrentLevel].MaxHealth;
	}
}
