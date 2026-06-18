using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering;

[Serializable]
public class SaveDataClass
{
	[SerializeField] private List<Inventory> _inventories = new List<Inventory>();
	[SerializeField] private List<int> _improvements = new List<int>();
	[SerializeField] private List<int> _stats = new List<int>();
	[SerializeField] private List<int> _resourceSources = new List<int>();
	[SerializeField] private List<int> _researchShips = new List<int>();
	[field: SerializeField] public Vector3 _position { get; private set; } = Vector3.zero;
	[field: SerializeField] public ShipTransform _shipTransform { get; private set; } = new ShipTransform();
	[field: SerializeField] public Base _currentBase { get; private set; } = null;
	[field: SerializeField] public string _currentSceneName { get; private set; } = "";
	[field: SerializeField] public int _currentDay { get; private set; } = 0;
	[field: SerializeField] public float _currentTime { get; private set; } = 0;
	[field: SerializeField] public int _currentMission { get; private set; } = 0;
	[field: SerializeField] public int _currentDialogue { get; private set; } = 0;

	[SerializeField] public IReadOnlyList<Inventory> Inventories => _inventories;
	[SerializeField] public IReadOnlyList<int> Improvements => _improvements;
	[SerializeField] public IReadOnlyList<int> Stats => _stats;
	[SerializeField] public IReadOnlyList<int> ResourceSources => _resourceSources;
	[SerializeField] public IReadOnlyList<int> ResearchShips => _researchShips;

	public void Initializing()
	{
		GameEvents.OnInventorySave += SetInventories;
		GameEvents.OnImprovementsSave += SetImprovements;
		GameEvents.OnStatsSave += SetStats;
		GameEvents.OnResourceSourcesSave += SetResourceSources;
		GameEvents.OnResearchShipsSave += SetResearchShips;
		GameEvents.OnPositionSave += SetPosition;
		GameEvents.OnTransformSave += SetTransform;
		GameEvents.OnBaseSave += SetCurrentBase;
		GameEvents.OnSceneSave += SetSceneName;
		GameEvents.OnCurrentDaySave += SetCurrentDay;
		GameEvents.OnCurrentTimeSave += SetCurrentTime;
		GameEvents.OnCurrentMissionSave += SetCurrentMission;
		GameEvents.OnCurrentDialogueSave += SetCurrentDialogue;
	}

	public void SetInventories(IReadOnlyList<Inventory> inventories)
	{
		_inventories = new List<Inventory>(inventories.Count);

		foreach (var inventory in inventories) _inventories.Add(inventory);
	}

	public void SetImprovements(IReadOnlyList<int> improvements)
	{
		_improvements = new List<int>(improvements.Count);

		foreach (var improvement in improvements) _improvements.Add(improvement);
	}

	public void SetStats(IReadOnlyList<int> stats)
	{
		_stats = new List<int>(stats.Count);

		foreach (var stat in stats) _stats.Add(stat);
	}

	public void SetResourceSources(IReadOnlyList<int> resourceSources)
	{
		_resourceSources = new List<int>(resourceSources.Count);

		foreach (var resourceSource in resourceSources) _resourceSources.Add(resourceSource);
	}

	public void SetResearchShips(IReadOnlyList<int> researchShips)
	{
		_researchShips = new List<int>(researchShips.Count);

		foreach (var researchShip in researchShips) _researchShips.Add(researchShip);
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
		GameEvents.OnInventorySave -= SetInventories;
		GameEvents.OnImprovementsSave -= SetImprovements;
		GameEvents.OnStatsSave -= SetStats;
		GameEvents.OnResourceSourcesSave -= SetResourceSources;
		GameEvents.OnResearchShipsSave -= SetResearchShips;
		GameEvents.OnPositionSave -= SetPosition;
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
}
