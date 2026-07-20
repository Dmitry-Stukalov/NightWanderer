using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;
using TMPro;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine.Experimental.AI;


//»нвентарь игрока
public class Inventory : ICellsCreator
{
	private Dictionary<Vector2Int, ResourceCellObject> _inventory = new Dictionary<Vector2Int, ResourceCellObject>();
	private List<Vector2Int> emptyCells = new List<Vector2Int>();
	private Vector2Int j = new Vector2Int(-1, -1);

	public Inventory() { }

	public void InitializeArray(int maxX, int maxY)
	{
		_inventory.Clear();

		for (int y = 0; y < maxY; y++)
		{
			for (int x = 0; x < maxX; x++)
			{
				_inventory[new Vector2Int(x, y)] = new ResourceCellObject(new Vector2Int(x, y));
			}
		}

		_inventory[new Vector2Int(99, 99)] = new ResourceCellObject(new Vector2Int(99, 99));
	}

	public void AddResource(ResourceBase resource, bool randomAdd)
	{
		//ѕередали пустой ресурс
		if (resource.CurrentCount == 0 || resource.ID == -1) return;

		j = new Vector2Int(-1, -1);
		emptyCells.Clear();

		foreach (var key in _inventory.Keys)
		{
			//–есурс есть, но он другой
			if (_inventory[key].GetId() != -1 && _inventory[key].GetId() != resource.ID) continue;

			//–есурса нет (€чейка пуста€)
			if (_inventory[key].GetId() == -1)
			{
				if (j == new Vector2Int(-1, -1)) j = key;

				emptyCells.Add(key);

				continue;
			}

			//–есурс есть, он тот же, и места в €чейке хватает дл€ получени€ / не хватает дл€ получени€
			if (_inventory[key].GetId() == resource.ID && _inventory[key].GetEmptyResourceCount() >= resource.CurrentCount)
			{
				_inventory[key].AddResource(resource);

				j = new Vector2Int(-1, -1);

				return;
			}
			else
			{
				if (_inventory[key].GetEmptyResourceCount() != 0)
				{
					int countDifference = resource.CurrentCount - _inventory[key].GetEmptyResourceCount();
					_inventory[key].AddResource(resource);
					resource.CurrentCount = countDifference;

					continue;
				}
				else continue;
			}
		}

		if (randomAdd) _inventory[emptyCells[Random.Range(0, emptyCells.Count)]].AddResource(resource);
		else _inventory[j].AddResource(resource);
	}

	public void AddResource(IResourceFactory factory, int resource, Vector2Int index, int count)
	{
		if (_inventory[index].GetId() == -1 || _inventory[index].GetId() == resource)
		{
			ResourceBase newResource = factory.GetResourceBase(resource);
			newResource.CurrentCount = count;

			_inventory[index].AddResource(newResource);
		}
	}

	public void DeleteResource(Vector2Int index, ResourceBase resource)
	{
		if (_inventory[index].GetId() != resource.ID) return;

		_inventory[index].DeleteResource(resource);
	}

	public void DeleteResource(ResourceBase resource)
	{
		foreach (var key in _inventory.Keys)
		{
			if (_inventory[key].GetId() == resource.ID)
			{
				if (_inventory[key].DeleteResource(resource) == 0) return;
			}
		}
	}
	
	public bool CheckResource(ResourceBase resource)
	{
		int resourceCount = 0;

		foreach (var key in _inventory.Keys)
			if (_inventory[key].GetId() == resource.ID)
				resourceCount += _inventory[key].GetResourceCount();

		if (resourceCount >= resource.CurrentCount) return true;
		else return false;
	}

	public bool CheckCell(Vector2Int cell)
	{
		if (_inventory.TryGetValue(cell, out ResourceCellObject cellObject))
		{
			return true;
		}
		return false;
	}

	public void CreateCell(Vector2Int cellIndex)
	{
		_inventory[cellIndex] = new ResourceCellObject(cellIndex);
	}

	public void DeleteCell(Vector2Int cellIndex)
	{
		_inventory.Remove(cellIndex);
	}

	public int GetEmptyCellsCount()
	{
		int count = 0;

		foreach (var key in _inventory.Keys)
			if (_inventory[key].GetId() == -1) count++;

		return count;
	}

	public Dictionary<Vector2Int, ResourceCellObject> GetCells() => _inventory;
	public ResourceCellObject GetResourceData(Vector2Int index) => _inventory[index];

	public void UpdateInventory(IResourceFactory factory, float deltaTime)
	{
		foreach (var key in _inventory.Keys) _inventory[key].UpdateCell(this, factory, deltaTime);
	}
}