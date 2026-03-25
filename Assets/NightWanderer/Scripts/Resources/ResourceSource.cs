using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using Unity.Mathematics;

//Создает ресурс после окончания добычи
public class ResourceSource : MonoBehaviour
{
	[field: SerializeField] public int ExtractionID { get; private set; }
	[SerializeField] private ResourceLibrary Library;
	[SerializeField] private GameObject[] Resources;
	[SerializeField] private GameObject SpawnResourceZone;
	[SerializeField] private int _resourceCount;
	[SerializeField] private int MinResourceCapacity;
	[SerializeField] private int MaxResourceCapacity;
	private List<GameObject> _crystals = new List<GameObject>();
	private int _currentResourceCount;
	private float _countInCrystals;

	private void Start()
	{
		foreach (Transform crystal in transform) _crystals.Add(crystal.gameObject);

		_currentResourceCount = _resourceCount;
		_countInCrystals = _crystals.Count / _resourceCount;
	}

	public void ResourceExtracted()
	{
		if (_currentResourceCount == 0) return;

		int id = UnityEngine.Random.Range(0, Resources.Length);
		int randomCapacity = UnityEngine.Random.Range(MinResourceCapacity, MaxResourceCapacity + 1);

		GameObject resource = Library.GetResource(id);
		resource.transform.SetParent(gameObject.transform, true);
		resource.GetComponent<ResourceOnLand>().SetResourceCount(randomCapacity);
		resource.transform.position = new Vector3(SpawnResourceZone.transform.position.x + UnityEngine.Random.Range(-1, 1), SpawnResourceZone.transform.position.y, SpawnResourceZone.transform.position.z + UnityEngine.Random.Range(1, 2));

		Debug.Log($"ResourceOnLandCount = {resource.GetComponent<ResourceOnLand>().GetResource().CurrentCount}");
		_currentResourceCount--;
		HideCrystals();
	}

	private void HideCrystals()
	{
		for (int i = 0; i < _crystals.Count - math.round(_currentResourceCount * _countInCrystals); i++)
		{
			_crystals[i].SetActive(false);
			Debug.Log(i);
		}
	}
}
