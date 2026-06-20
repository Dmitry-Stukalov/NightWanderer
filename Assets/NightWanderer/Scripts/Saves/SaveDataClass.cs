using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering;
using Unity.Properties;

[Serializable]
public class SaveDataClass
{
	[SerializeField] private List<float> _stats = new List<float>();
	[field: SerializeField] public InventoryData Inventory { get; private set; } = new InventoryData();
	[field: SerializeField] public InventoryData BaseInventory { get; private set; } = new InventoryData();
	[field: SerializeField] public ImprovementData Improvements { get; private set; } = new ImprovementData();
	[field: SerializeField] public ImprovementUnlockData ImprovementsUnlock { get; private set; } = new ImprovementUnlockData();
	[field: SerializeField] public ResourceSourceData[] ResourceSources { get; private set; } = new ResourceSourceData[4];
	[field: SerializeField] public ResearchShipData ResearchShips { get; private set; } = new ResearchShipData();
	[field: SerializeField] public Vector3 _position { get; private set; } = Vector3.zero;
	[field: SerializeField] public ShipTransform _shipTransform { get; private set; } = new ShipTransform();
	[field: SerializeField] public Base _currentBase { get; private set; } = null;
	[field: SerializeField] public string _currentSceneName { get; private set; } = "";
	[field: SerializeField] public int _currentDay { get; private set; } = 0;
	[field: SerializeField] public float _currentTime { get; private set; } = 0;
	[field: SerializeField] public int _currentMission { get; private set; } = 0;
	[field: SerializeField] public int _currentDialogue { get; private set; } = 0;

	[SerializeField] public IReadOnlyList<float> Stats => _stats;

	public void Initializing()
	{
		GameEvents.OnInventorySave += SetInventory;
		GameEvents.OnBaseInventorySave += SetBaseInventory;
		GameEvents.OnImprovementsSave += SetImprovements;
		GameEvents.OnImprovementPanelsSave += SetImprovementsUnlock;
		GameEvents.OnStatsSave += SetStats;
		GameEvents.OnResourceSourcesSave += SetResourceSources;
		GameEvents.OnResearchShipsSave += SetResearchShips;
		GameEvents.OnTransformSave += SetTransform;
		GameEvents.OnBaseSave += SetCurrentBase;
		GameEvents.OnSceneSave += SetSceneName;
		GameEvents.OnCurrentDaySave += SetCurrentDay;
		GameEvents.OnCurrentTimeSave += SetCurrentTime;
		GameEvents.OnCurrentMissionSave += SetCurrentMission;
		GameEvents.OnCurrentDialogueSave += SetCurrentDialogue;
	}

	public void SetInventory(Inventory inventory)
	{
		Inventory = new InventoryData(inventory);
	}

	public void SetBaseInventory(Inventory inventory)
	{
		BaseInventory = new InventoryData(inventory);
	}

	public void SetImprovements(Dictionary<string, IImprovementBase> improvements)
	{
		Improvements = new ImprovementData(improvements);
	}

	public void SetImprovementsUnlock(Dictionary<string, bool> improvements)
	{
		ImprovementsUnlock = new ImprovementUnlockData(improvements);
	}

	public void SetStats(IReadOnlyList<float> stats)
	{
		_stats = new List<float>(stats.Count);

		foreach (var stat in stats) _stats.Add(stat);
	}

	public void SetResourceSources(int id, Dictionary<int, int> resourceSources)
	{
		ResourceSources[id] = new ResourceSourceData(resourceSources);
	}

	public void SetResearchShips(Dictionary<int, bool> researchShips)
	{
		ResearchShips = new ResearchShipData(researchShips);
	}

	public void SetPosition(Vector3 position) => _position = position;
	public void SetTransform(Transform transform) => _shipTransform = new ShipTransform(transform.position.x, transform.position.y, transform.position.z, transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
	public void SetCurrentBase(Base currentBase) => _currentBase = currentBase;
	public void SetSceneName(string currentScene) => _currentSceneName = currentScene;
	public void SetCurrentDay(int currentDay) => _currentDay = currentDay;
	public void SetCurrentTime(float currentTime) => _currentTime = currentTime;
	public void SetCurrentMission(int currentMission) => _currentMission = currentMission;
	public void SetCurrentDialogue(int currentDialogue) => _currentDialogue = currentDialogue;

	public void OnDisable()
	{
		GameEvents.OnInventorySave -= SetInventory;
		GameEvents.OnBaseInventorySave -= SetBaseInventory;
		GameEvents.OnImprovementsSave -= SetImprovements;
		GameEvents.OnImprovementPanelsSave -= SetImprovementsUnlock;
		GameEvents.OnStatsSave -= SetStats;
		GameEvents.OnResourceSourcesSave -= SetResourceSources;
		GameEvents.OnResearchShipsSave -= SetResearchShips;
		GameEvents.OnTransformSave -= SetTransform;
		GameEvents.OnBaseSave -= SetCurrentBase;
		GameEvents.OnSceneSave -= SetSceneName;
		GameEvents.OnCurrentDaySave -= SetCurrentDay;
		GameEvents.OnCurrentTimeSave -= SetCurrentTime;
		GameEvents.OnCurrentMissionSave -= SetCurrentMission;
		GameEvents.OnCurrentDialogueSave -= SetCurrentDialogue;
	}





	[Serializable]
	public struct ShipTransform
	{
		public float X;
		public float Y;
		public float Z;

		public float RX;
		public float RY;
		public float RZ;

		public ShipTransform(float x, float y, float z, float rx, float ry, float rz)
		{
			X = x;
			Y = y;
			Z = z;
			RX = rx;
			RY = ry;
			RZ = rz;
		}
	}

	[Serializable]
	public struct InventoryData
	{
		public List<int> ResourceID;
		public List<int> ResourceCount;

		public InventoryData(Inventory inventory)
		{
			ResourceID = new List<int>();
			ResourceCount = new List<int>();

			for (int i = 0; i < inventory.GetCellCount(); i++)
			{
				ResourceID.Add(inventory.GetResourceData(i).GetResource().ID);
				ResourceCount.Add(inventory.GetResourceData(i).GetResource().CurrentCount);
			}
		}
	}

	[Serializable]
	public struct ImprovementData
	{
		public List<string> ImprovementName;
		public List<int> ImprovementLevel;

		public ImprovementData(Dictionary<string, IImprovementBase> improvements)
		{
			ImprovementName = new List<string>();
			ImprovementLevel = new List<int>();

			foreach (var key in improvements.Keys)
			{
				ImprovementName.Add(key);
				ImprovementLevel.Add(improvements[key].CurrentLevel);
			}
		}
	}

	[Serializable]
	public struct ImprovementUnlockData
	{
		public List<string> ImprovementName;
		public List<bool> ImprovementUnlock;

		public ImprovementUnlockData(Dictionary<string, bool> improvements)
		{
			ImprovementName = new List<string>();
			ImprovementUnlock = new List<bool>();

			foreach (var key in improvements.Keys)
			{
				ImprovementName.Add(key);
				ImprovementUnlock.Add(improvements[key]);
			}
		}
	}


	[Serializable]
	public struct ResourceSourceData
	{
		public List<int> ResourceSourceID;
		public List<int> ResourceSourceCount;

		public ResourceSourceData(Dictionary<int, int> resourceSources)
		{
			ResourceSourceID = new List<int>();
			ResourceSourceCount = new List<int>();

			foreach (var key in resourceSources.Keys)
			{
				ResourceSourceID.Add(key);
				ResourceSourceCount.Add(resourceSources[key]);
			}
		}
	}

	[Serializable]
	public struct ResearchShipData
	{
		public List<int> ResearchShipID;
		public List<bool> ResearchShipEmpty;

		public ResearchShipData(Dictionary<int, bool> researchShips)
		{
			ResearchShipID = new List<int>();
			ResearchShipEmpty = new List<bool>();

			foreach (var key in researchShips.Keys)
			{
				ResearchShipID.Add(key);
				ResearchShipEmpty.Add(researchShips[key]);
			}
		}
	}
}
