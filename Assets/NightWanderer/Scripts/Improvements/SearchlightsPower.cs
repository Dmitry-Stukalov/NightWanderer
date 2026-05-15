using System;
using System.Collections.Generic;
using UnityEngine;

public class SearchlightsPower : MonoBehaviour, IImprovementBase
{
	[SerializeField] private Light[] _searchlights;
	public string Name { get; set; }
	public Dictionary<int, int> NeedResources { get; set; }
	public ImprovementConfig Config { get; set; }
	public int CurrentLevel { get; set; }
	public event Action OnUpgrade;

	private ImprovementSearchlightPowerConfig _config;

	public void AddConfig(ImprovementConfig config)
	{
		Config = config;
		_config = (ImprovementSearchlightPowerConfig)config;
		CurrentLevel = 0;

		NeedResources = new Dictionary<int, int>();
	}

	public Dictionary<int, int> GetNeedResources()
	{
		NeedResources?.Clear();

		for (int i = 0; i < _config.Levels[CurrentLevel].Resource.Count; i++)
		{
			NeedResources[_config.Levels[CurrentLevel].Resource[i]] = _config.Levels[CurrentLevel].Count[i];
		}

		return NeedResources;
	}

	public void Upgrade()
	{
		for (int i = 0; i < _searchlights.Length; i++)
		{
			_searchlights[i].spotAngle = _config.Levels[CurrentLevel].SearchlightSpotAngle;
			_searchlights[i].range = _config.Levels[CurrentLevel].SearchlightRange;
			_searchlights[i].intensity = _config.Levels[CurrentLevel].SearchlightPower;
		}

		CurrentLevel++;

		OnUpgrade?.Invoke();
	}
}
