using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;
using TMPro;
using System.Runtime.CompilerServices;


//»нвентарь игрока
public class Inventory
{
	private int InventoryCellCount;
	private ResourceCellObject[] _Inventory;
	private List<int> emptyCells = new List<int>();
	private int j = -1;

	public Inventory(int inventoryCellCount)
	{
		InventoryCellCount = inventoryCellCount;
		_Inventory = new ResourceCellObject[InventoryCellCount];
	}

	public void InitializeArray(ResourceCellObject obj, int i)
	{
		_Inventory[i] = obj;
	}

	public void AddResource(ResourceBase resource, bool randomAdd)
	{
		int i = -1;
		j = -1;
		emptyCells.Clear();

		while (++i < InventoryCellCount - 1)
		{
			if (resource.CurrentCount == 0) return; 

			//–есурс есть, но он другой
			if (_Inventory[i].GetId() != -1 && _Inventory[i].GetId() != resource.ID) continue;

			//–есурса нет (€чейка пуста€)
			if (_Inventory[i].GetId() == -1)
			{
				if (j == -1) j = i;

				emptyCells.Add(i);

				continue;
			}

			//–есурс есть, он тот же, и места в €чейке хватает дл€ получени€ / не хватает дл€ получени€
			if (_Inventory[i].GetId() == resource.ID && _Inventory[i].GetEmptyResourceCount() >= resource.CurrentCount)
			{
				_Inventory[i].AddResource(resource);
				j = -1;
				return;
			}
			else
			{
				if (_Inventory[i].GetEmptyResourceCount() != 0)
				{
					int countDifference = resource.CurrentCount - _Inventory[i].GetEmptyResourceCount();
					_Inventory[i].AddResource(resource);
					resource.CurrentCount = countDifference;
					j = -1;
					i = -1;
					continue;
				}
				else continue;
			}
		}

		if (randomAdd) _Inventory[emptyCells[Random.Range(0, emptyCells.Count)]].AddResource(resource);
		else _Inventory[j].AddResource(resource);
	}

	public void AddResource(IResourceFactory factory, int resource, int index, int count)
	{
        if (_Inventory[index].GetId() == -1 || _Inventory[index].GetId() == resource)
        {
			ResourceBase newResource = factory.GetResourceBase(resource);
			newResource.CurrentCount = count;

			_Inventory[index].AddResource(newResource);
        }
    }

	public void DeleteResource(int index, ResourceBase resource)
	{
		if (_Inventory[index].GetId() != resource.ID) return;

		_Inventory[index].DeleteResource(resource);
	}

	public int GetCellCount() => InventoryCellCount - 1;
	public ResourceCellObject GetResourceData(int index) => _Inventory[index];

	public void UpdateInventory(IResourceFactory factory, float deltaTime)
	{
		for (int i = 0; i < InventoryCellCount; i++) _Inventory[i].UpdateCell(this, factory, deltaTime);
	}
}