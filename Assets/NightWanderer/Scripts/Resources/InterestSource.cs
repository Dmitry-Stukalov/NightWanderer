using UnityEngine;
using System.Collections.Generic;

public class InterestSource : MonoBehaviour
{
	[SerializeField] private Material _dayMaterial;
	[SerializeField] private Material _nightMaterial;
	[SerializeField] private int _showCrystalsCount;
	private List<GameObject> _crystals = new List<GameObject>();
	private List<MeshRenderer> _oreMaterial = new List<MeshRenderer>();
	private Sun _sun;
	private Timer _recoveryTimer;
	private int _currentCrystalCount;

	private void Start()
	{
		foreach (Transform obj in transform) _crystals.Add(obj.gameObject);
		_crystals.RemoveAt(0);
		_crystals.RemoveAt(0);

		for (int i = 0; i < _crystals.Count; i++) _oreMaterial.Add(_crystals[i].GetComponent<MeshRenderer>());

		_crystals.RemoveAt(_crystals.Count - 1);

		_currentCrystalCount = _crystals.Count;

		_recoveryTimer = new Timer(120);
		_recoveryTimer.OnTimerEnd += RecoveryCrystal;

		_sun = FindAnyObjectByType<Sun>();
		_sun.OnDayStart += () =>
		{
			for (int i = 0; i < _oreMaterial.Count; i++) _oreMaterial[i].material = _dayMaterial;
		};

		_sun.OnNightStart += () =>
		{
			for (int i = 0; i < _oreMaterial.Count; i++) _oreMaterial[i].material = _nightMaterial;
		};

		for (int i = 0; i < _oreMaterial.Count; i++) _oreMaterial[i].material = _nightMaterial;

		HideRandomCrystals();
	}

	private void HideRandomCrystals()
	{
		int t = _crystals.Count - _showCrystalsCount;
		int randomNumber = 0;

		while (t > 0)
		{
			randomNumber = Random.Range(0, _crystals.Count);

			if (_crystals[randomNumber].activeSelf)
			{
				_crystals[randomNumber].SetActive(false);
				_currentCrystalCount--;
				t--;
			}
		}
	}

	private void RecoveryCrystal()
	{
		int randomNumber = 0;

		if (_currentCrystalCount < _showCrystalsCount)
		{
			while(true)
			{
				randomNumber = Random.Range(0, _crystals.Count);

				if (!_crystals[randomNumber].activeSelf)
				{
					_crystals[randomNumber].SetActive(true);
					_currentCrystalCount++;
					break;
				}
			}
		}

		_recoveryTimer.ResetTimer(false);
	}

	private void Update()
	{
		if (_sun.IsTimeSkip) _recoveryTimer?.Tick(Time.deltaTime * 15);
		else _recoveryTimer?.Tick(Time.deltaTime);
	}

	private void OnDisable()
	{
		_sun.OnDayStart -= () =>
		{
			for (int i = 0; i < _oreMaterial.Count; i++) _oreMaterial[i].material = _dayMaterial;
		};

		_sun.OnNightStart -= () =>
		{
			for (int i = 0; i < _oreMaterial.Count; i++) _oreMaterial[i].material = _nightMaterial;
		};
	}
}
