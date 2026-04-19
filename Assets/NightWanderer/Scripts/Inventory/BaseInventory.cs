using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BaseInventory : MonoBehaviour
{
	[SerializeField] private UIDocument BaseUI;
	[SerializeField] private VisualTreeAsset InventoryCell;
	[SerializeField] private string InventoryElementName;
	[SerializeField] private int InventoryCellCount;
	private VisualElement Inventory;
	private Inventory _baseInventory;
	private List<ResourceBase> ResourceQueue = new List<ResourceBase>();
	private bool IsProcessing = false;

	public void Initializing()
	{
		Inventory = BaseUI.rootVisualElement.Q<VisualElement>(InventoryElementName);

		_baseInventory = new Inventory(InventoryCellCount + 1);

		for (int i = 0; i < InventoryCellCount + 1; i++)
		{
			var newCell = InventoryCell.Instantiate();
			newCell.hierarchy.ElementAt(0).dataSource = new CellObject(false);

			newCell.Q<VisualElement>("CellResource").dataSource = new ResourceCellObject();
			newCell.Q<VisualElement>("CellResource").AddManipulator(new DraggableManipulator(newCell.Q<VisualElement>("CellResource"), false));
			newCell.hierarchy.ElementAt(0).AddToClassList("BorderCell");

			Inventory.Add(newCell);

			_baseInventory.InitializeArray((ResourceCellObject)newCell.Q<VisualElement>("CellResource").dataSource, i);

			if (i == InventoryCellCount) newCell.transform.position = new Vector2(0, 10000);
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
			_baseInventory.AddResource(ResourceQueue[0]);
			ResourceQueue.RemoveAt(0);

			yield return null;
		}

		IsProcessing = false;
	}

	public Inventory GetBaseInventory() => _baseInventory;

}
