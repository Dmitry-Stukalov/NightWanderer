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

	public void Initializing()
	{
		Inventory = BaseUI.rootVisualElement.Q<VisualElement>(InventoryElementName);

		_baseInventory = new Inventory(InventoryCellCount + 1);

		for (int i = 0; i < InventoryCellCount + 1; i++)
		{
			var newCell = InventoryCell.Instantiate();

			newCell.Q<VisualElement>("CellResource").dataSource = new ResourceCellObject(newCell.Q<VisualElement>("CellResource"), newCell.Q<Label>("CellResourceCount"));
			newCell.Q<VisualElement>("CellResource").AddManipulator(new DraggableManipulator(newCell, false));

			Inventory.Add(newCell);

			_baseInventory.InitializeArray((ResourceCellObject)newCell.Q<VisualElement>("CellResource").dataSource, i);

			if (i == InventoryCellCount) newCell.transform.position = new Vector2(0, 10000);
		}
	}

	public Inventory GetBaseInventory() => _baseInventory;

}
