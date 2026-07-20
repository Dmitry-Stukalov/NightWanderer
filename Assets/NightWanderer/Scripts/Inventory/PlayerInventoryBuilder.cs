using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using static Unity.VisualScripting.Metadata;


//ќтвечает за инвентарь игрока, добавление и удаление из него ресурсов
public class PlayerInventoryBuilder : MonoBehaviour
{
	[SerializeField] private UIDocument PlayerUI;
	[SerializeField] private UIDocument BaseUI;
	[SerializeField] private VisualTreeAsset InventoryCell;
	[SerializeField] private VisualTreeAsset CreateInventoryCell;
	[SerializeField] private VisualTreeAsset DeleteInventoryCell;
	[SerializeField] private string InventoryElementName;
	[SerializeField] private string InventoryElementName2;
	[SerializeField] private int InventoryCellCount;
	private VisualElement Inventory;
	private VisualElement Inventory2;
	private List<VisualElement> _createdCells = new List<VisualElement>();
	private List<VisualElement> _deletedCells = new List<VisualElement>();
	private VisualElement _lastCell;
	private Inventory _playerInventory;
	private ResourceLibrary _library;
	private List<ResourceBase> ResourceQueue = new List<ResourceBase>();
	private Dictionary<Vector2Int, bool> _createdInventory = new Dictionary<Vector2Int, bool>();
	private int _inventoryCellSize, _baseInventoryCellSize;
	private float _inventoryHalfWidth, _inventoryHalfHeight, _baseInventoryHalfWidth, _baseInventoryHalfHeight;
	private bool IsProcessing = false;

	public void Initializing()
	{
		Inventory = PlayerUI.rootVisualElement.Q<VisualElement>(InventoryElementName);
		Inventory2 = BaseUI.rootVisualElement.Q<VisualElement>(InventoryElementName2);

		_playerInventory = new Inventory();
		_playerInventory.InitializeArray(2, 2);

		_inventoryHalfWidth = Inventory.resolvedStyle.width / 2;
		_inventoryHalfHeight = Inventory.resolvedStyle.height / 2;
		_inventoryCellSize = (int)(Inventory.resolvedStyle.height / 5);

		_baseInventoryHalfWidth = Inventory2.resolvedStyle.width / 2;
		_baseInventoryHalfHeight = Inventory2.resolvedStyle.height / 2;
		_baseInventoryCellSize = (int)(Inventory2.resolvedStyle.height / 7);

		Vector2Int[] startCells = new Vector2Int[] { new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(0, 1), new Vector2Int(1, 1) };

		CreateNewCell(Inventory, new Vector2Int(99, 99));

		for (int i = 0 ; i < startCells.Length; i++)
		{
			var newCell = CreateNewCell(Inventory, startCells[i]);
			var newCell2 = InventoryCell.Instantiate().hierarchy.ElementAt(0);

			newCell2.Q<VisualElement>("CellResource").dataSource = newCell.Q<VisualElement>("CellResource").dataSource;
			newCell2.Q<VisualElement>("CellResource").AddManipulator(new DraggableManipulator(newCell2.Q<VisualElement>("CellResource"), true));
			newCell2.dataSource = new CellObject(false);
			newCell2.AddToClassList("BorderCell");

			newCell2.style.width = _baseInventoryCellSize;
			newCell2.style.flexBasis = _baseInventoryCellSize;
			newCell2.style.height = _baseInventoryCellSize;

			newCell2.style.left = _baseInventoryHalfWidth - _baseInventoryCellSize + startCells[i].x * _baseInventoryCellSize;
			newCell2.style.top = _baseInventoryHalfHeight - _baseInventoryCellSize + startCells[i].y * _baseInventoryCellSize;

			Inventory2.Add(newCell2);
		}

		GameEvents.OnSave += SaveData;
		GameEvents.OnOpenCreateCells += ShowCreatedCells;
		GameEvents.OnOpenCreateCells += ShowDeletedCells;
		GameEvents.OnCloseCreateCells += HideCreatedCells;
		GameEvents.OnCloseCreateCells2 += HideCreatedCells;
}

	public void InitializeInventoryLibrary(ResourceLibrary library) => _library = library;

	public void AddResource(ResourceBase newResource, bool randomAdd)
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

		if (newResource.CurrentCount != 0) 
			ResourceQueue.Add(newResource);

		if (!IsProcessing) StartCoroutine(ProcessResourceQueue(randomAdd));
	}

	//ќчередь добавлени€ ресурса в инвентарь
	private IEnumerator ProcessResourceQueue(bool randomAdd)
	{
		IsProcessing = true;

		while (ResourceQueue.Count > 0)
		{
			_playerInventory.AddResource(ResourceQueue[0], randomAdd);
			ResourceQueue.RemoveAt(0);

			yield return null;
		}

		IsProcessing = false;
	}

	//—оздает €чейки, которые показывают где можно создать новые €чейки инвентар€
	public void ShowCreatedCells(ClickEvent evt)
	{
		Vector2Int[] directions = new Vector2Int[] { Vector2Int.left, Vector2Int.right, Vector2Int.up, Vector2Int.down };
		Vector2Int index;

		DeactivateCells();

		foreach (var cell in _playerInventory.GetCells().Keys)
		{
			if (cell == new Vector2Int(99, 99)) continue;

			foreach (var direction in directions)
			{
				index = cell + direction;

				if (!_playerInventory.CheckCell(index) && !CheckCreatedCell(index))
				{
					Vector2Int finalIndex = index;

					var createdCell = CreateInventoryCell.Instantiate().hierarchy.ElementAt(0);

					Inventory.Add(createdCell);

					createdCell.style.width = _inventoryCellSize;
					createdCell.style.flexBasis = _inventoryCellSize;
					createdCell.style.height = _inventoryCellSize;

					createdCell.userData = finalIndex;
					createdCell.RegisterCallback<ClickEvent>(CreateCell);

					createdCell.style.left = _inventoryHalfWidth - _inventoryCellSize + finalIndex.x * _inventoryCellSize;
					createdCell.style.top = _inventoryHalfHeight - _inventoryCellSize + finalIndex.y * _inventoryCellSize;

					_createdInventory[finalIndex] = true;
					_createdCells.Add(createdCell);
				}
			}
		}
	}

	//ѕр€чет €чейки, которые показывают куда можно добавить новые €чейки и какие можно убрать
	public void HideCreatedCells(ClickEvent evt)
	{
		_createdInventory.Clear();
		_createdCells.Clear();

		Inventory.Query(className: "CreatedCell").ForEach(cell =>
		{
			Inventory.Remove(cell);
		});

		ActivateCells();
	}


	public void HideCreatedCells()
	{
		_createdInventory.Clear();
		_createdCells.Clear();

		Inventory.Query(className: "CreatedCell").ForEach(cell =>
		{
			Inventory.Remove(cell);
		});

		ActivateCells();
	}


	//ѕоказывает €чейки, которые можно удалить
	public void ShowDeletedCells(ClickEvent evt)
	{
		Vector2Int[] directions = new Vector2Int[] { Vector2Int.left, Vector2Int.right, Vector2Int.up, Vector2Int.down };
		Vector2Int index;

		foreach (var cell in _playerInventory.GetCells().Keys)
		{
			if (cell == new Vector2Int(99, 99)) continue;

			foreach (var direction in directions)
			{
				index = cell + direction;

				if (_playerInventory.CheckCell(index))
				{
					Vector2Int finalIndex = index;

					var deletedCell = DeleteInventoryCell.Instantiate().hierarchy.ElementAt(0);

					Inventory.Add(deletedCell);

					deletedCell.style.width = _inventoryCellSize;
					deletedCell.style.flexBasis = _inventoryCellSize;
					deletedCell.style.height = _inventoryCellSize;

					deletedCell.userData = finalIndex;
					deletedCell.RegisterCallback<ClickEvent>(DeleteCell);

					deletedCell.style.left = _inventoryHalfWidth - _inventoryCellSize + finalIndex.x * _inventoryCellSize;
					deletedCell.style.top = _inventoryHalfHeight - _inventoryCellSize + finalIndex.y * _inventoryCellSize;

					_deletedCells.Add(deletedCell);
				}
			}
		}
	}

	//¬ключает возможность перетаскивать ресурсы по инвентарю
	private void ActivateCells()
	{
		foreach (var cell in Inventory.Children())
		{
			cell.Q<VisualElement>("CellResource").pickingMode = PickingMode.Position;
		}
	}

	//¬ыключает возможность перетаскивать ресурсы по инвентарю
	private void DeactivateCells()
	{
		foreach (var cell in Inventory.Children())
		{
			cell.Q<VisualElement>("CellResource").pickingMode = PickingMode.Ignore;
		}
	}

	//¬ызывает метод, который создает новую €чейку
	private void CreateCell(ClickEvent evt)
	{
		ResourceBase craftCell = _library.GetCraftResourceBase(0);
		craftCell.CurrentCount = 1;

		if (!_playerInventory.CheckResource(craftCell))
		{
			Debug.Log("Ќе хватает ресурсов");
			return;
		}

		if (evt.currentTarget is VisualElement cellObject && cellObject.userData is Vector2Int index)
		{
			HideCreatedCells(evt);

			_playerInventory.CreateCell(index);
			_playerInventory.DeleteResource(craftCell);

			var newCell2 = InventoryCell.Instantiate().hierarchy.ElementAt(0);

			newCell2.style.width = _baseInventoryCellSize;
			newCell2.style.flexBasis = _baseInventoryCellSize;
			newCell2.style.height = _baseInventoryCellSize;

			newCell2.Q<VisualElement>("CellResource").dataSource = CreateNewCell(Inventory, index).Q<VisualElement>("CellResource").dataSource;
			newCell2.Q<VisualElement>("CellResource").AddManipulator(new DraggableManipulator(newCell2.Q<VisualElement>("CellResource"), true));
			newCell2.dataSource = new CellObject(false);
			newCell2.AddToClassList("BorderCell");
			newCell2.style.left = _baseInventoryHalfWidth - _baseInventoryCellSize + index.x * _baseInventoryCellSize;
			newCell2.style.top = _baseInventoryHalfHeight - _baseInventoryCellSize + index.y * _baseInventoryCellSize;

			Inventory2.Add(newCell2);

			ShowCreatedCells(evt);
			ShowDeletedCells(evt);
		}
	}

	//¬ызывает метод, который удал€ет €чейку из инвентар€
	private void DeleteCell(ClickEvent evt)
	{
		if (evt.currentTarget is VisualElement cellObject && cellObject.userData is Vector2Int index && _playerInventory.GetResourceData(index).GetId() == -1)
		{
			HideCreatedCells(evt);

			DeleteThisCell(Inventory, index);

			_playerInventory.DeleteCell(index);

			ResourceBase craftCell = _library.GetCraftResourceBase(0);
			craftCell.CurrentCount = 1;

			_playerInventory.AddResource(craftCell, true);

			ShowCreatedCells(evt);

			ShowDeletedCells(evt);
		}
	}

	//ѕровер€ет существует ли уже €чейка, котора€ показывает где можно добавить еще одну €чейку инвентар€
	public bool CheckCreatedCell(Vector2Int cell)
	{
		if (_createdInventory.TryGetValue(cell, out bool cellObject))
		{
			return true;
		}
		return false;
	}

	public Vector2Int GetResourceNearbyIndex(Vector2Int index)
	{
		Vector2Int[] directions = new Vector2Int[] { Vector2Int.left, Vector2Int.right, Vector2Int.up, Vector2Int.down };
		List<Vector2Int> nearbyCells = new List<Vector2Int>();
		Vector2Int nearbyIndex;

		foreach (var direction in directions)
		{
			nearbyIndex = index + direction;

			if (_playerInventory.CheckCell(nearbyIndex)) nearbyCells.Add(nearbyIndex);
		}

		if (nearbyCells.Count > 0)
		{
			return nearbyCells[UnityEngine.Random.Range(0, nearbyCells.Count)];
		}
		else return new Vector2Int(-1, -1);
	}

	public void EatResource(Vector2Int index)
	{
		if (!_playerInventory.CheckCell(index))
		{
			Debug.Log($"ячейки с индексом: {index} не существует");
			return;
		}

		ResourceBase resource = new ResourceBase(_playerInventory.GetResourceData(index).GetResource().View, _playerInventory.GetResourceData(index).GetResource().Name, _playerInventory.GetResourceData(index).GetResource().ID, _playerInventory.GetResourceData(index).GetResource().MaxCount, 5);

		_playerInventory.GetResourceData(index).DeleteResource(resource);
	}

	//—оздает новую €чейку в инвентаре
	private VisualElement CreateNewCell(VisualElement inventory, Vector2Int index)
	{
		VisualElement newCell;

		if (index != new Vector2Int(99, 99))
		{
			_lastCell = Inventory.Children().ElementAt(Inventory.childCount - 1);

			_lastCell.style.width = _inventoryCellSize;
			_lastCell.style.flexBasis = _inventoryCellSize;
			_lastCell.style.height = _inventoryCellSize;

			_lastCell.Q<VisualElement>("CellResource").dataSource = _playerInventory.GetResourceData(index);
			_lastCell.Q<VisualElement>("CellResource").AddManipulator(new DraggableManipulator(_lastCell.Q<VisualElement>("CellResource"), false));
			_lastCell.dataSource = new CellObject(false);

			_lastCell.style.left = _inventoryHalfWidth - _inventoryCellSize + index.x * _inventoryCellSize;
			_lastCell.style.top = _inventoryHalfHeight - _inventoryCellSize + index.y * _inventoryCellSize;
		}

		newCell = InventoryCell.Instantiate().hierarchy.ElementAt(0);

		newCell.style.width = _inventoryCellSize;
		newCell.style.flexBasis = _inventoryCellSize;
		newCell.style.height = _inventoryCellSize;

		newCell.Q<VisualElement>("CellResource").dataSource = _playerInventory.GetResourceData(new Vector2Int(99, 99));
		newCell.Q<VisualElement>("CellResource").AddManipulator(new DraggableManipulator(newCell.Q<VisualElement>("CellResource"), false));
		newCell.dataSource = new CellObject(false);

		newCell.style.left = 99 * _inventoryCellSize;
		newCell.style.top = 99 * _inventoryCellSize;

		inventory.Add(newCell);

		return _lastCell;
	}

	//”дал€ет €чейку из инвентар€
	public void DeleteThisCell(VisualElement inventory, Vector2Int index)
	{
		foreach (var cell in inventory.Children())
		{
			if (((ResourceCellObject)cell.Q<VisualElement>("CellResource").dataSource).GetCellID() == index)
			{
				cell.RemoveFromHierarchy();
				return;
			}
		}
	}

	public Inventory GetPlayerInventory() => _playerInventory;

	private void SaveData() => GameEvents.OnInventorySave?.Invoke(_playerInventory);

	private void Update()
	{
		_playerInventory?.UpdateInventory(_library, Time.deltaTime);
	}

	private void OnDisable()
	{
		GameEvents.OnSave -= SaveData;
		GameEvents.OnOpenCreateCells -= ShowCreatedCells;
		GameEvents.OnOpenCreateCells -= ShowDeletedCells;
		GameEvents.OnCloseCreateCells -= HideCreatedCells;
		GameEvents.OnCloseCreateCells2 -= HideCreatedCells;
	}
}
