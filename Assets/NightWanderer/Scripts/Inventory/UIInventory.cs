using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

//Здесь должно быть отображение инвентаря в UI
public class UIInventory : MonoBehaviour
{
	/*[field: SerializeField] private PlayerInventory _PlayerInventoryScript;
	[field: SerializeField] private GameObject InventoryLine;
	[field: SerializeField] private GameObject CellObject;
	[field: SerializeField] private int InventoryLineCount, InventoryColumnCount;
	private Inventory _PlayerInventory;
	private List<ResourceBase> ResourceQueue = new List<ResourceBase>();
	private bool IsProcessing = false;

	public void Initializing()
	{
		GameObject newLine;
		GameObject newResource;

		_PlayerInventory = _PlayerInventoryScript.GetInventory();

		_PlayerInventory = new Inventory(CellObject, InventoryLineCount, InventoryColumnCount);

		for (int i = 0; i < InventoryLineCount; i++)
		{
			newLine = Instantiate(InventoryLine, transform);
			for (int j = 0; j < InventoryColumnCount; j++)
			{
				newResource = Instantiate(CellObject, newLine.transform);
				newResource.GetComponentInChildren<ResourceCellObject>().Initializing();
				newResource.GetComponentInChildren<ResourceCount>().Initializing();
				_PlayerInventory.InitializeArray(newResource.GetComponentInChildren<ResourceCellObject>(), i, j);
			}
		}
	}

	public void AddResource(ResourceBase newResource)
	{
		Debug.Log(newResource.CurrentCount);

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

		Debug.Log(newResource.CurrentCount); 
		//_PlayerInventory.AddResource(newresource);
	}

	private IEnumerator ProcessResourceQueue()
	{
		IsProcessing = true;

		while (ResourceQueue.Count > 0)
		{
			_PlayerInventory.AddResource(ResourceQueue[0]);
			ResourceQueue.RemoveAt(0);
			yield return null;
		}

		IsProcessing = false;
	}*/

	//private void LateUpdate()
	//{
	//	if (ResourceQueue.Count > 0)
	//	{
	//		_PlayerInventory.AddResource(ResourceQueue[0]);
	//		ResourceQueue.RemoveAt(0);
	//	}
	//}
}
