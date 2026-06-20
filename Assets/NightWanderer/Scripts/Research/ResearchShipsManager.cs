using System.Collections.Generic;
using UnityEngine;

public class ResearchShipsManager : MonoBehaviour
{
	private Dictionary<int, bool> _researchShipsData = new Dictionary<int, bool>();
	private List<ResearchShip> _researchShips = new List<ResearchShip>();

	private void Awake()
	{
		foreach (Transform child in transform) _researchShips.Add(child.GetComponent<ResearchShip>());

		GameEvents.OnResearchShipsLoad += LoadData;
		GameEvents.OnSave += SaveData;
	}

	public void LoadData(SaveDataClass.ResearchShipData researchShips)
	{
		for (int i = 0; i < researchShips.ResearchShipID.Count; i++)
		{
			_researchShips[i].LoadData(researchShips.ResearchShipEmpty[i]);
		}
	}

	public void SaveData()
	{
		for (int i = 0; i < _researchShips.Count; i++) _researchShipsData[i] = _researchShips[i].IsDataUpload();

		GameEvents.OnResearchShipsSave?.Invoke(_researchShipsData);
	}

	private void OnDisable()
	{
		GameEvents.OnSave -= SaveData;
		GameEvents.OnResearchShipsLoad -= LoadData;
	}
}
