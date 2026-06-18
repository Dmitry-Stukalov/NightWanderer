using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class ResourceSourceManager : MonoBehaviour
{
	private List<ResourceSource> _resourceSources = new List<ResourceSource>();

	private void Start()
	{
		foreach (Transform child in transform) _resourceSources.Add(child.GetComponent<ResourceSource>());

		GameEvents.OnSave += SaveData;
	}

	public void SaveData()
	{
		//GameEvents.OnResourceSourcesSave?.Invoke(_resourceSources);
	}

	private void OnDisable()
	{
		GameEvents.OnSave -= SaveData;
	}
}
