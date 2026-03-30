using UnityEngine;
using System.Collections.Generic;
using Unity.Mathematics;

//Создает ресурс после окончания добычи
public class ResourceSource : MonoBehaviour
{
	[field: SerializeField] public int ExtractionID { get; private set; }
	[SerializeField] private int _resourceID;
	[SerializeField] private GameObject SpawnResourceZone;
 	[SerializeField] private int _resourceCountMin;
	[SerializeField] private int _resourceCountMax;
	[SerializeField] private int MinResourceCapacity;
	[SerializeField] private int MaxResourceCapacity;
	[SerializeField] private Material[] _sourceMaterial;
	[SerializeField] private Material _dayMaterial;
	[SerializeField] private Material _nightMaterial;
	[SerializeField] private bool ISRemoveFirst;
	private ResourceLibrary Library;
	private List<GameObject> _crystals = new List<GameObject>();
	private List<MeshRenderer> _oreMaterial = new List<MeshRenderer>();
	private Sun _sun;
	private int _currentResourceCount;
	private float _countInCrystals;

	private void Start()
	{
		Library = GameObject.FindGameObjectWithTag("ResourceLibrary").GetComponent<ResourceLibrary>();

		transform.GetChild(0).GetComponent<MeshRenderer>().material = _sourceMaterial[UnityEngine.Random.Range(0, _sourceMaterial.Length)];

		foreach (Transform crystal in transform) _crystals.Add(crystal.gameObject);

		_crystals.RemoveAt(_crystals.Count - 1);

		if (ISRemoveFirst) _crystals.RemoveAt(0);

		_currentResourceCount = UnityEngine.Random.Range(_resourceCountMin, _resourceCountMax);
		_countInCrystals = _crystals.Count / _currentResourceCount;

		foreach (MeshRenderer renderer in GetComponentsInChildren<MeshRenderer>()) _oreMaterial.Add(renderer);

		_oreMaterial.RemoveAt(_oreMaterial.Count - 1);

		if (ISRemoveFirst) _oreMaterial.RemoveAt(0);

		_sun = FindAnyObjectByType<Sun>();
		_sun.OnDayStart += () =>
		{
			for (int i = 0; i < _oreMaterial.Count; i++) _oreMaterial[i].material = _dayMaterial;
		};

		_sun.OnNightStart += () =>
		{
			for (int i = 0; i < _oreMaterial.Count; i++) _oreMaterial[i].material = _nightMaterial;
		};
	}

	public void ResourceExtracted()
	{
		if (_currentResourceCount == 0) return;

		int randomCapacity = UnityEngine.Random.Range(MinResourceCapacity, MaxResourceCapacity + 1);

		GameObject resource = Library.GetResource(_resourceID);
		resource.transform.SetParent(gameObject.transform, true);
		resource.GetComponent<ResourceOnLand>().SetResourceCount(randomCapacity);
		resource.transform.position = new Vector3(SpawnResourceZone.transform.position.x + UnityEngine.Random.Range(-1, 1), SpawnResourceZone.transform.position.y, SpawnResourceZone.transform.position.z + UnityEngine.Random.Range(1, 2));

		_currentResourceCount--;
		HideCrystals();
	}

	private void HideCrystals()
	{
		for (int i = 0; i < _crystals.Count - math.round(_currentResourceCount * _countInCrystals); i++) _crystals[i].SetActive(false);
	}
}
