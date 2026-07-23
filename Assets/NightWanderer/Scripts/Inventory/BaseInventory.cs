using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class BaseInventory : MonoBehaviour
{
	[SerializeField] private UIDocument BaseUI;
	[SerializeField] private VisualTreeAsset InventoryCell;
	[SerializeField] private string InventoryElementName;
	[SerializeField] private int InventoryXCount;
	[SerializeField] private int InventoryYCount;
	private VisualElement Inventory;
	private VisualElement _invisibleInventory;
	private Inventory _baseInventory;
	private ResourceLibrary _library;
	private List<ResourceBase> ResourceQueue = new List<ResourceBase>();
	private int _inventoryCellSize;
	private bool IsProcessing = false;

	public void Initializing()
	{
		Inventory = BaseUI.rootVisualElement.Q<VisualElement>(InventoryElementName);
		_invisibleInventory = BaseUI.rootVisualElement.Q<VisualElement>("InvisibleInventory");

		_baseInventory = new Inventory();
		_baseInventory.InitializeArray(InventoryXCount, InventoryYCount);

		_inventoryCellSize = (int)(Inventory.resolvedStyle.height / 11);

		CreateNewCell(Inventory, new Vector2Int(99, 99));
		CreateNewCell(_invisibleInventory, new Vector2Int(99, 99));

		for (int y = 0; y < InventoryYCount; y++)
			for (int x = 0; x < InventoryXCount; x++)
				CreateNewCell(Inventory, new Vector2Int(x, y));

		GameEvents.OnSave += SaveData;
		GameEvents.OnResourceActivate += ActivateCells;
		GameEvents.OnResourceDeactivate += DeactivateCells;
	}

	public void InitializeInventoryLibrary(ResourceLibrary library) => _library = library;

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
			_baseInventory.AddResource(ResourceQueue[0], false);
			ResourceQueue.RemoveAt(0);

			yield return null;
		}

		IsProcessing = false;
	}

	private VisualElement CreateNewCell(VisualElement inventory, Vector2Int index)
	{
		VisualElement newCell, lastCell;

		if (index != new Vector2Int(99, 99))
		{
			lastCell = Inventory.Children().ElementAt(Inventory.childCount - 1);

			lastCell.style.width = _inventoryCellSize;
			lastCell.style.flexBasis = _inventoryCellSize;
			lastCell.style.height = _inventoryCellSize;

			lastCell.Q<VisualElement>("CellResource").dataSource = _baseInventory.GetResourceData(index);
			lastCell.Q<VisualElement>("CellResource").AddManipulator(new DraggableManipulator(lastCell.Q<VisualElement>("CellResource"), true));
			lastCell.dataSource = new CellObject(false);

			lastCell.style.left = index.x * _inventoryCellSize;
			lastCell.style.top = index.y * _inventoryCellSize;
		}

		newCell = InventoryCell.Instantiate().hierarchy.ElementAt(0);

		newCell.style.width = _inventoryCellSize;
		newCell.style.flexBasis = _inventoryCellSize;
		newCell.style.height = _inventoryCellSize;

		newCell.Q<VisualElement>("CellResource").dataSource = _baseInventory.GetResourceData(new Vector2Int(99, 99));
		newCell.Q<VisualElement>("CellResource").AddManipulator(new DraggableManipulator(newCell.Q<VisualElement>("CellResource"), true));
		newCell.dataSource = new CellObject(false);

		newCell.style.left = 99 * _inventoryCellSize;
		newCell.style.top = 99 * _inventoryCellSize;

		inventory.Add(newCell);

		return newCell;
	}

	//Включает возможность перетаскивать ресурсы по инвентарю
	private void ActivateCells()
	{
		foreach (var cell in Inventory.Children())
		{
			VisualElement cellResource = cell.Q<VisualElement>("CellResource");

			if (cellResource != null) cellResource.pickingMode = PickingMode.Position;
		}
	}

	//Выключает возможность перетаскивать ресурсы по инвентарю
	private void DeactivateCells()
	{
		foreach (var cell in Inventory.Children())
		{
			VisualElement cellResource = cell.Q<VisualElement>("CellResource");

			if (cellResource != null) cellResource.pickingMode = PickingMode.Ignore;
		}
	}

	public Inventory GetBaseInventory() => _baseInventory;


	private void SaveData() => GameEvents.OnBaseInventorySave?.Invoke(_baseInventory);

	private void Update()
	{
		_baseInventory?.UpdateInventory(_library, Time.deltaTime);
	}

	private void OnDisable()
	{
		GameEvents.OnSave -= SaveData;
		GameEvents.OnResourceActivate -= ActivateCells;
		GameEvents.OnResourceDeactivate -= DeactivateCells;
	}
}
