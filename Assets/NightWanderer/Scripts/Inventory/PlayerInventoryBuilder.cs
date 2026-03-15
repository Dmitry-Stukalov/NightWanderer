using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;


//Отвечает за инвентарь игрока, добавление и удаление из него ресурсов
public class PlayerInventoryBuilder : MonoBehaviour
{
	[SerializeField] private UIDocument UI;
	[SerializeField] private VisualTreeAsset InventoryCell;
	[SerializeField] private GameObject InventoryBackground;
	[SerializeField] private GameObject Inventory;
	[SerializeField] private GameObject CellObject;
	[SerializeField] private int InventoryCellCount;
	private VisualElement Cell;
	private VisualElement Inventory2;
	private Inventory _PlayerInventory;
	private List<ResourceBase> ResourceQueue = new List<ResourceBase>();
	private List<CanvasGroup> ResourceCanvasGroups = new List<CanvasGroup>();
	private bool IsProcessing = false;

	public void Initializing()
	{
		Cell = UI.rootVisualElement.Q<VisualElement>("InventoryCell");
		Inventory2 = UI.rootVisualElement.Q<VisualElement>("Inventory");

		GameObject newResource;

		_PlayerInventory = new Inventory(InventoryCellCount);

		for (int i = 0; i < InventoryCellCount; i++)
		{
			var newCell = InventoryCell.Instantiate();

			newCell.userData = new ResourceCellObject(newCell.Q<VisualElement>("CellResource"), newCell.Q<Label>("CellResourceCount"));

			Inventory2.Add(newCell);

			_PlayerInventory.InitializeArray((ResourceCellObject)newCell.userData, i);

			//newResource = Instantiate(CellObject, Inventory.transform);
			//ResourceCanvasGroups.Add(newResource.GetComponentInChildren<CanvasGroup>());
			//newResource.GetComponentInChildren<ResourceCellObject>().Initializing();
			//newResource.GetComponentInChildren<ResourceCellObject>()._PlayerInventory = this;
			//newResource.GetComponentInChildren<ResourceCellObject>().InventoryBackground = InventoryBackground;
			//newResource.GetComponentInChildren<ResourceCellObject>().InventoryObject = Inventory;
			//_PlayerInventory.InitializeArray(newResource.GetComponentInChildren<ResourceCellObject>(), i);
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
