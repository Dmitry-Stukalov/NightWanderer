using System.Collections.Generic;
using UnityEngine;

public class Searchlights : MonoBehaviour, IImprovementBase
{
	[SerializeField] private GameObject[] _searchlights;
	public string Name { get; set; }
	public Dictionary<int, int> _needResources { get; set; }
	public ImprovementConfig Config { get; set; }
	public int CurrentLevel { get; set; }
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

		_needResources = new Dictionary<int, int>();
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

	public void Upgrade()
	{
		_searchlights[CurrentLevel].SetActive(true);

		CurrentLevel++;
	}
}
