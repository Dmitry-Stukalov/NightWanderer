using System;
using UnityEditor.VersionControl;
using UnityEngine;

public class Fuel : IImprovementBase
{
	public ImprovementConfig Config { get; set; }
	public int CurrentLevel { get; set; }

	private ImprovementFuelConfig _config;

	private float _minFuel;
	private float _maxFuel;
	private float _consumptionIdle;
	private float _consumptionWalk;
	private float _consumptionRun;
	private float _currentFuel;
	public bool IsFuelEmpty { get; set; } = false;

	public event Action OnFuelEmpty;
	public event Action OnFuelMax;

	public Fuel(ImprovementConfig config)
	{
		_config = (ImprovementFuelConfig)config;
		CurrentLevel = 0;

		_minFuel = _config.Levels[CurrentLevel].MinFuel;
		_maxFuel = _config.Levels[CurrentLevel].MaxFuel;
		_consumptionIdle = _config.Levels[CurrentLevel].ConsumptionWalk;
		_consumptionWalk = _config.Levels[CurrentLevel].ConsumptionIdle;
		_consumptionRun = _config.Levels[CurrentLevel].ConsumptionRun;

		_currentFuel = _maxFuel;
	}

	public void EnginesRunning(int state)
	{
		switch (state)
		{
			case 0:
				Consumption(_consumptionIdle);
			break;

			case 1:
				Consumption(_consumptionWalk);
			break;

			case 2:
				Consumption(_consumptionRun);
			break;
		}
	}

	private void Consumption(float fuel)
	{
		_currentFuel -= fuel;

		if (_currentFuel <= _minFuel)
		{
			_currentFuel = _minFuel;

			IsFuelEmpty = true;

			OnFuelEmpty?.Invoke();
		}
	}

	public void Refueling(float fuel)
	{
		_currentFuel += fuel;

		IsFuelEmpty = false;

		if (_currentFuel >= _maxFuel)
		{
			_currentFuel = _maxFuel;

			OnFuelMax?.Invoke();
		}
	}

	public void Upgrade()
	{
		CurrentLevel++;

		_minFuel = _config.Levels[CurrentLevel].MinFuel;
		_maxFuel = _config.Levels[CurrentLevel].MaxFuel;
		_consumptionIdle = _config.Levels[CurrentLevel].ConsumptionWalk;
		_consumptionWalk = _config.Levels[CurrentLevel].ConsumptionIdle;
		_consumptionRun = _config.Levels[CurrentLevel].ConsumptionRun;
	}
}
