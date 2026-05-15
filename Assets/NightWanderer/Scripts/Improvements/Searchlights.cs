using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Searchlights : MonoBehaviour, IImprovementBase
{
	[SerializeField] private GameObject[] _searchlights;
	public string Name { get; set; }
	public Dictionary<int, int> NeedResources { get; set; }
	public ImprovementConfig Config { get; set; }
	public int CurrentLevel { get; set; }
	public event Action OnUpgrade;

	private ImprovementSearchlightConfig _config;

	private void Start()
	{
		for (int i = 0;  i < _searchlights.Length; i++) _searchlights[i].SetActive(false);
	}

	public void AddConfig(ImprovementConfig config)
	{
		Config = config;
		_config = (ImprovementSearchlightConfig)config;
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

	public int GetActiveSearchlights()
	{
		int t = 1;

		for (int i = 0; i < _searchlights.Length; i++)
			if (_searchlights[i].activeSelf) t++;

		return t;
	}

	public void Upgrade()
	{
		_searchlights[CurrentLevel].SetActive(true);

		if (CurrentLevel == 0) GameEvents.OnMissionComplete?.Invoke(1);

		CurrentLevel++;

		OnUpgrade?.Invoke();
	}
}
