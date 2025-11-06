using UnityEngine;
using System.Collections.Generic;

public class Inventory
{
	private int InventoryLineCount;
	private int InventoryColumnCount;
	private List<GameObject> ResourceCellObjects;
	private Cell[,] _Inventory;

	public Inventory(int inventoryLineCount, int inventoryColumnCount, List<GameObject> resourceCellObjects)
	{
		InventoryLineCount = inventoryLineCount;
		InventoryColumnCount = inventoryColumnCount;
		_Inventory = new Cell[InventoryLineCount, InventoryColumnCount];
		ResourceCellObjects = new List<GameObject>(resourceCellObjects);

		for (int i = 0;  i < InventoryLineCount; i++)
		{
			for (int j = 0; j < InventoryColumnCount; j++)
			{
				_Inventory[i, j] = new Cell(ResourceCellObjects[i + j]);
			}
		}
	}

	public void AddResource(ResourceBase resource)
	{
		for (int i = 0; i < InventoryLineCount; i++)
		{
			for (int j = 0; j < InventoryColumnCount; j++)
			{
				if (_Inventory[i, j].GetResourceID() != -1 && _Inventory[i, j].GetResourceID() != resource.ID) continue;

				//if (_Inventory[i, j].GetResourceID() != -1 && _Inventory[i, j].GetResourceID() == resource.ID)
				//Логика добавления ресурса в инвентарь
			}
		}
	}
}