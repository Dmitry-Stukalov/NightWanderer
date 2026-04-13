using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;


//Отвечает за инвентарь игрока, добавление и удаление из него ресурсов
public class PlayerInventoryBuilder : MonoBehaviour
{
	[SerializeField] private UIDocument PlayerUI;
	[SerializeField] private UIDocument BaseUI;
	[SerializeField] private VisualTreeAsset InventoryCell;
	[SerializeField] private string InventoryElementName;
	[SerializeField] private string InventoryElementName2;
	[SerializeField] private int InventoryCellCount;
	private VisualElement Inventory;
	private VisualElement Inventory2;
	private Inventory _PlayerInventory;
	private List<ResourceBase> ResourceQueue = new List<ResourceBase>();
	private bool IsProcessing = false;

	public void Initializing()
	{
		Inventory = PlayerUI.rootVisualElement.Q<VisualElement>(InventoryElementName);
		Inventory2 = BaseUI.rootVisualElement.Q<VisualElement>(InventoryElementName2);

		_PlayerInventory = new Inventory(InventoryCellCount + 1);

		for (int i = 0; i < InventoryCellCount + 1; i++)
		{
			var newCell = InventoryCell.Instantiate();
			var newCell2 = InventoryCell.Instantiate();

			newCell.Q<VisualElement>("CellResource").dataSource = new ResourceCellObject();
			newCell.Q<VisualElement>("CellResource").AddManipulator(new DraggableManipulator(newCell.Q<VisualElement>("CellResource"), false));

			newCell.hierarchy.ElementAt(0).dataSource = new CellObject(false);
			newCell.hierarchy.ElementAt(0).AddToClassList("BorderCell");

			newCell2.Q<VisualElement>("CellResource").dataSource = newCell.Q<VisualElement>("CellResource").dataSource;
			newCell2.Q<VisualElement>("CellResource").AddManipulator(new DraggableManipulator(newCell2.Q<VisualElement>("CellResource"), true));
			newCell2.hierarchy.ElementAt(0).dataSource = new CellObject(false);
			newCell2.hierarchy.ElementAt(0).AddToClassList("BorderCell");

			Inventory.Add(newCell);
			Inventory2.Add(newCell2);

			_PlayerInventory.InitializeArray((ResourceCellObject)newCell.Q<VisualElement>("CellResource").dataSource, i);

			if (i == InventoryCellCount)
			{
				newCell.transform.position = new Vector2(0, 10000);
				newCell2.transform.position = new Vector2(0, 10000);
			}
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

	public Inventory GetPlayerInventory() => _PlayerInventory;
}
