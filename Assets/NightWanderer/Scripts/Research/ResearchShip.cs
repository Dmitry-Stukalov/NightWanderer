using Mono.Cecil.Cil;
using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ResearchShip : MonoBehaviour
{
	[field: SerializeField] public GameObject DockingPlace { get; private set; }
	[SerializeField] private ResearchConfig config;
	private bool IsEmpty = false;

	public void Search()
	{
		if (IsEmpty) return;

		for (int i = 0; i < config.ImprovementName.Length; i++) GameEvents.OnImprovementOpen?.Invoke(config.ImprovementName[i]);

		for (int i = 0; i < config.CraftName.Length; i++) GameEvents.OnCraftOpen?.Invoke(config.CraftName[i]);

		for (int i = 0; i < config.StoryName.Length; i++) GameEvents.OnStoryOpen?.Invoke(config.StoryName[i]);

		IsEmpty = true;
	}

	public bool TakeResources(Inventory inventory)
	{

		Dictionary<int, int> substractedResources = new Dictionary<int, int>();

		for (int i = 0; i < config.NeedResourceID.Length; i++) substractedResources[config.NeedResourceID[i]] = config.NeedResourceCount[i];

		//Dictionary<int, int> inventoryResources = new Dictionary<int, int>();
		//Dictionary<Vector2Int, ResourceCellObject> inventoryCells = new Dictionary<Vector2Int, ResourceCellObject>(inventory.GetCells());
		//int t = 0;

		if (CheckInventoryResources.CheckResources(new Inventory[] { inventory }, substractedResources)) return true;
		else
		{
			GameEvents.OnResearchMessage?.Invoke("Недостаточно ресурсов");

			return false;
		}

					//foreach (var key in inventoryCells.Keys)
					//{
					//	for (int i = 0; i < config.NeedResourceID.Length; i++)
					//	{
					//		if (config.NeedResourceID[i] == inventoryCells[key].GetId() && config.NeedResourceCount[i] <= inventoryCells[key].GetResourceCount()) t++;
					//	}
					//}

					//if (t == config.NeedResourceID.Length)
					//{
					//	Dictionary<int, int> substractedResources = new Dictionary<int, int>();

					//	for (int i = 0; i < config.NeedResourceID.Length; i++) substractedResources[config.NeedResourceID[i]] = config.NeedResourceCount[i];

					//	CheckInventoryResources.SubtractResources
					//}
	}

	public bool GiveResources(Inventory inventory)
	{
		if (inventory.GetEmptyCellsCount() < config.GiveResourceID.Length)
		{
			GameEvents.OnResearchMessage?.Invoke("Недостаточно места в инвентаре");
			return false;
		}

		for (int i = 0; i < config.GiveResourceID.Length; i++)
			GameEvents.OnResourceAdd?.Invoke(config.GiveResourceID[i], config.GiveResourceCount[i]);

		return true;
	}

	public ResearchConfig GetResearchConfig() => config;
	public bool IsDataUpload() => IsEmpty;
	public void LoadData(bool isEmpty) => IsEmpty = isEmpty;
}
