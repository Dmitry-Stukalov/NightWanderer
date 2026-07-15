using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public static class CheckInventoryResources
{
	private static Dictionary<int, int> _resources = new Dictionary<int, int>();

	public static bool CheckResources(Inventory[] inventories, Dictionary<int, int> needResources)
	{
		int t = 0;
		Dictionary<int, int> inventoryResources = new Dictionary<int, int>();

		for (int i = 0; i < 20; i++) inventoryResources[i] = 0;

		for (int i = 0; i < inventories.Length; i++)
		{
			foreach (var key in inventories[i].GetCells().Keys)
			{
				var resource = inventories[i].GetResourceData(key);

				if (resource.GetId() != -1) inventoryResources[resource.GetId()] += resource.GetResourceCount();
			}
		}

		_resources = new Dictionary<int, int>(needResources);

		foreach (var key in _resources.Keys)
		{
			if (inventoryResources[key] >= needResources[key]) t++;
		}

		if (t == needResources.Count)
		{
			SubtractResources(inventories, needResources);
			return true;
		}
		else return false;
	}
	
	public static void SubtractResources(Inventory[] inventories, Dictionary<int, int> needResources)
	{
		for (int i = 0; i < inventories.Length; i++)
		{
			foreach (var key in inventories[i].GetCells().Keys)
			{
				var resource = inventories[i].GetResourceData(key);

				if (needResources.TryGetValue(resource.GetId(), out int needResource))
				{
					if (resource.GetResourceCount() > needResources[resource.GetId()])
					{
						resource.SubtractResourceCount(needResources[resource.GetId()]);
						needResources[resource.GetId()] -= 0;
					}
					else
					{
						needResources[resource.GetId()] -= resource.GetResourceCount();
						resource.ResetResource();
					}
				}
				else continue;
			}
		}
	}
}
