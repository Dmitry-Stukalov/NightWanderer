using UnityEngine;
using System.Collections.Generic;


//»нвентарь игрока
public class Inventory
{
	private GameObject CellObject;
	private int InventoryLineCount;
	private int InventoryColumnCount;
	private ResourceCellObject[,] _Inventory; 

	public Inventory(GameObject cellObject, int inventoryLineCount, int inventoryColumnCount)
	{
		CellObject = cellObject;
		InventoryLineCount = inventoryLineCount;
		InventoryColumnCount = inventoryColumnCount;
		_Inventory = new ResourceCellObject[InventoryLineCount, InventoryColumnCount];

		//for (int i = 0;  i < InventoryLineCount; i++)
		//{
		//	for (int j = 0; j < InventoryColumnCount; j++)
		//	{
				
		//	}
		//}
	}

	public void InitializeArray(ResourceCellObject obj, int i, int j)
	{
		_Inventory[i, j] = obj;
	}


	public void AddResource(ResourceBase resource)
	{
		for (int i = 0; i < InventoryLineCount; i++)
		{
			for (int j = 0; j < InventoryColumnCount; j++)
			{
				//–есурс есть, но он другой
				if (_Inventory[i, j].GetId() != -1 && _Inventory[i, j].GetId() != resource.ID) continue;

				//–есурса нет (€чейка пуста€)
				if (_Inventory[i, j].GetId() == -1)																				
				{
					_Inventory[i, j].AddResource(resource);
					return;
				}

				//–есурс есть, он тот же, и места в €чейке хватает дл€ получени€ / не хватает дл€ получени€
				if (_Inventory[i, j].GetId() == resource.ID && _Inventory[i, j].GetEmptyResourceCount() >= resource.CurrentCount)
				{
					_Inventory[i, j].AddResource(resource);
					return;
				}
				else
				{
					int countDifference = resource.CurrentCount - _Inventory[i, j].GetEmptyResourceCount();
					_Inventory[i, j].AddResource(resource);
					resource.CurrentCount = countDifference;
				}
			}
		}
	}
}