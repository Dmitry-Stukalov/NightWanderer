using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;


//»нвентарь игрока
public class Inventory
{
	private int InventoryCellCount;
	private ResourceCellObject[] _Inventory;
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

	public void AddResource(ResourceBase resource)
	{
		int i = -1;
		while (++i < InventoryCellCount)
		{
			if (resource.CurrentCount == 0) return;

			//–есурс есть, но он другой
			if (_Inventory[i].GetId() != -1 && _Inventory[i].GetId() != resource.ID) continue;

			//–есурса нет (€чейка пуста€)
			if (_Inventory[i].GetId() == -1)
			{
				if (j == -1) j = i;

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

		_Inventory[j].AddResource(resource);
		j = -1;
	}
}