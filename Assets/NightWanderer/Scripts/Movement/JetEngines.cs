using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class JetEngines : IImprovementBase
{
	public string Name { get; set; }
	public Dictionary<int, int> _needResources { get; set; } = new Dictionary<int, int>();
	public ImprovementConfig Config { get; set; }
	public int CurrentLevel { get; set; }

	private ImprovementEnginesConfig _config;

	private Fuel _fuel;
	public event Action OnUpgrade;

	public JetEngines(ImprovementConfig config, Fuel fuel)
	{
		Config = config;
		_config = (ImprovementEnginesConfig)config;
		_fuel = fuel;

		CurrentLevel = 0;
	}

	public Dictionary<int, int> GetNeedResources()
	{
		_needResources?.Clear();

		for (int i = 0; i < _config.Levels[CurrentLevel].Resource.Count; i++)
		{
			_needResources[_config.Levels[CurrentLevel].Resource[i]] = _config.Levels[CurrentLevel].Count[i];
		}

		return _needResources;
	}


	public void EnginesRunning(int state)
	{
		switch (state)
		{
			case 0:
				_fuel.Consumption(_config.Levels[CurrentLevel].ConsumptionIdle);
			break;

			case 1:
				_fuel.Consumption(_config.Levels[CurrentLevel].ConsumptionWalk);
			break;

			case 2:
				_fuel.Consumption(_config.Levels[CurrentLevel].ConsumptionRun);
			break;
		}
	}

	public void Upgrade()
	{
		CurrentLevel++;

		OnUpgrade?.Invoke();
	}

	public float GetWalkSpeed() => _config.Levels[CurrentLevel].WalkSpeed;
	public float GetWalkSpeedUp() => _config.Levels[CurrentLevel].WalkUpSpeed;
	public float GetRunSpeed() => _config.Levels[CurrentLevel].RunSpeed;
	public float GetRunSpeedUp() => _config.Levels[CurrentLevel].RunUpSpeed;
}
