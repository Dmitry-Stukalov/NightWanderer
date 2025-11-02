using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
	[field: SerializeField] private int InventoryLineCount;
	[field: SerializeField] private int InventoryColumnCount;
	//[field: SerializeField] private int CellCapacity;
	[field: SerializeField] private List<Resource> Resources;
	private Cell[,] _Inventory;

	private void Start()
	{
		_Inventory = new Cell[InventoryLineCount, InventoryColumnCount];

		for (int i = 0; i < InventoryLineCount; i++)
		{
			for (int j = 0; j < InventoryColumnCount; j++)
			{
				//_Inventory[i, j].ChangeCapacity(CellCapacity);
				//_Inventory[i, j].GetResource(null);
			}
		}
	}

	public void GetResource(Resource resource)
	{

		for (int i = 0; i < InventoryLineCount; i++)
		{
			for (int j = 0; j < InventoryColumnCount; j++)
			{
				if (_Inventory[i,j].IsEmpty)
				{
					_Inventory[i, j].GetResource(resource);
				}
			}
		}
	}
}