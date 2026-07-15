using System.Collections;
using UnityEngine;

public class GiveStartResources : MonoBehaviour
{
	[SerializeField] private ResourceLibrary _resourceLibrary;
	private PlayerInventoryBuilder _playerInventoryBuilder;
	private Inventory _playerInventory;
	private BaseInventory _BaseInventory;
	private bool IsLoadData = false;

	public void Initializing()
	{
		_playerInventoryBuilder = FindAnyObjectByType<PlayerInventoryBuilder>();
		_playerInventory = _playerInventoryBuilder.GetPlayerInventory();
		_BaseInventory = FindAnyObjectByType<BaseInventory>();

		GameEvents.OnInventoryLoad += LoadPlayerData;
		GameEvents.OnBaseInventoryLoad += LoadBaseData;

		//GameEvents.OnImprovementOpen?.Invoke("Searchlight");
		StartCoroutine(StartPause());
	}
	
	private IEnumerator StartPause()
	{
		yield return new WaitForSeconds(5);

		GameEvents.OnImprovementOpen?.Invoke("Searchlight");

		yield return new WaitForSeconds(4);

		if (!SaveAndLoad.IsLoadGame)
		{
			var newResource = _resourceLibrary.GetResourceBase(3);
			newResource.SetCount(20);
			_BaseInventory.AddResource(newResource);

			newResource = _resourceLibrary.GetResourceBase(4);
			newResource.SetCount(10);
			_BaseInventory.AddResource(newResource);

			newResource = _resourceLibrary.GetResourceBase(1);
			newResource.SetCount(10);
			_BaseInventory.AddResource(newResource);
		}
	}

	private void LoadPlayerData(SaveDataClass.InventoryData inventoryData)
	{
		for (int i = 0; i < inventoryData.ResourceID.Count; i++)
		{
			if (inventoryData.ResourceID[i] == -1) continue;
			var newResource = _resourceLibrary.GetResourceBase(inventoryData.ResourceID[i]);
			newResource.SetCount(inventoryData.ResourceCount[i]);
			_playerInventoryBuilder.AddResource(newResource, false);
		}
	}

	private void LoadBaseData(SaveDataClass.InventoryData inventoryData)
	{
		for (int i = 0; i < inventoryData.ResourceID.Count; i++)
		{
			if (inventoryData.ResourceID[i] == -1) continue;
			var newResource = _resourceLibrary.GetResourceBase(inventoryData.ResourceID[i]);
			newResource.SetCount(inventoryData.ResourceCount[i]);
			_BaseInventory.AddResource(newResource);
		}
	}

	private void OnDisable()
	{
		GameEvents.OnInventoryLoad -= LoadPlayerData;
		GameEvents.OnBaseInventoryLoad -= LoadBaseData;
	}
}
