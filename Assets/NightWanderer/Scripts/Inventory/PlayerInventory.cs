using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Отвечает за инвентарь игрока, добавление и удаление из него ресурсов
public class PlayerInventory : MonoBehaviour
{	[field: SerializeField] private GameObject InventoryBackground;
	[field: SerializeField] private GameObject Inventory;
	[field: SerializeField] private GameObject CellObject;
	[field: SerializeField] private int InventoryCellCount;
	private Inventory _PlayerInventory;
	private List<ResourceBase> ResourceQueue = new List<ResourceBase>();
	private List<CanvasGroup> ResourceCanvasGroups = new List<CanvasGroup>();
	private bool IsProcessing = false;

	public void Initializing()
	{
		GameObject newResource;

		_PlayerInventory = new Inventory(InventoryCellCount);

		for (int i = 0; i < InventoryCellCount; i++)
		{
			newResource = Instantiate(CellObject, Inventory.transform);
			ResourceCanvasGroups.Add(newResource.GetComponentInChildren<CanvasGroup>());
			newResource.GetComponentInChildren<ResourceCellObject>().Initializing();
			newResource.GetComponentInChildren<ResourceCellObject>()._PlayerInventory = this;
			newResource.GetComponentInChildren<ResourceCellObject>().InventoryBackground = InventoryBackground;
			newResource.GetComponentInChildren<ResourceCellObject>().InventoryObject = Inventory;
			_PlayerInventory.InitializeArray(newResource.GetComponentInChildren<ResourceCellObject>(), i);
		}
	}

	public void AddResource(ResourceBase newResource)
	{
		if (ResourceQueue.Count > 0 && newResource.CurrentCount > 0)
		{
			foreach (var resource in ResourceQueue)
			{
				if (resource.ID == newResource.ID)
				{
					resource.CurrentCount += newResource.CurrentCount;
					newResource.CurrentCount = 0;
					break;
				}
			}
		}

		if (newResource.CurrentCount != 0) ResourceQueue.Add(newResource);

		if (!IsProcessing) StartCoroutine(ProcessResourceQueue());
	}

	public void CanvasGroupBlock(bool flag)
	{
		foreach (var group in ResourceCanvasGroups) group.blocksRaycasts = flag;
	}

	private IEnumerator ProcessResourceQueue()
	{
		IsProcessing = true;

		while (ResourceQueue.Count > 0)
		{
			_PlayerInventory.AddResource(ResourceQueue[0]);
			ResourceQueue.RemoveAt(0);
			yield return null;
		}

		IsProcessing = false;
	}
}
