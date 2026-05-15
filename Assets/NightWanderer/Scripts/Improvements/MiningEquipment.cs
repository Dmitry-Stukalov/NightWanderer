using System;
using System.Collections.Generic;

public class MiningEquipment : IImprovementBase
{
	public string Name { get; set; }
	public Dictionary<int, int> NeedResources { get; set; } = new Dictionary<int, int>();
	public ImprovementConfig Config { get; set; }
	public int CurrentLevel { get; set; }
	public event Action OnUpgrade;

	private ImprovementMiningConfig _config;

	private Fuel _fuel;


	public MiningEquipment(ImprovementConfig config, Fuel fuel)
	{
		Config = config;
		_config = (ImprovementMiningConfig)config;
		CurrentLevel = 0;

		_fuel = fuel;
	}

	public void MiningRest() => _fuel.Consumption(_config.Levels[CurrentLevel].ConsumptionAtRest);
	public void MiningLaser() => _fuel.Consumption(_config.Levels[CurrentLevel].ConsumptionAtLaser);
	public void MiningSound() => _fuel.Consumption(_config.Levels[CurrentLevel].ConsumptionAtSound);

	public Dictionary<int, int> GetNeedResources()
	{
		NeedResources?.Clear();

		for (int i = 0; i < _config.Levels[CurrentLevel].Resource.Count; i++)
		{
			NeedResources[_config.Levels[CurrentLevel].Resource[i]] = _config.Levels[CurrentLevel].Count[i];
		}

		return NeedResources;
	}
}
