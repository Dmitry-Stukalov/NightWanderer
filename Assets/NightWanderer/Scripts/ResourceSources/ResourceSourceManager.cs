using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class ResourceSourceManager : MonoBehaviour
{
	[SerializeField] private int ID;
	private Dictionary<int, int> _resourceSourcesData = new Dictionary<int, int>();
	private List<ResourceSource> _resourceSources = new List<ResourceSource>();

	private void Awake()
	{
		foreach (Transform child in transform) _resourceSources.Add(child.GetComponent<ResourceSource>());

		GameEvents.OnResourceSourcesLoad += LoadData;
		GameEvents.OnSave += SaveData;
	}

	public void LoadData(int id, SaveDataClass.ResourceSourceData resourceSources)
	{
		if (ID != id) return;

		for (int i = 0; i < resourceSources.ResourceSourceID.Count; i++)
		{
			_resourceSources[i].LoadData(resourceSources.ResourceSourceCount[i]);
		}
	}

	public void SaveData()
	{
		for (int i = 0; i < _resourceSources.Count; i++) _resourceSourcesData[i] = _resourceSources[i].GetCurrentResourceCount();

		GameEvents.OnResourceSourcesSave?.Invoke(ID, _resourceSourcesData);
	}

	private void OnDisable()
	{
		GameEvents.OnSave -= SaveData;
		GameEvents.OnResourceSourcesLoad -= LoadData;
	}
}
