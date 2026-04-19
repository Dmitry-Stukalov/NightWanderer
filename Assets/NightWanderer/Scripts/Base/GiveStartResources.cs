using System.Collections;
using UnityEngine;

public class GiveStartResources : MonoBehaviour
{
	[SerializeField] private BaseInventory _inventory;
	[SerializeField] private ResourceLibrary _resourceLibrary;

	public void Initializing()
	{
		_inventory = FindAnyObjectByType<BaseInventory>();

		var newResource = _resourceLibrary.GetResourceBase(0);
		newResource.SetCount(10);
		_inventory.AddResource(newResource);

		//GameEvents.OnImprovementOpen?.Invoke("Searchlight");
		StartCoroutine(StartPause());
	}
	
	private IEnumerator StartPause()
	{
		yield return new WaitForSeconds(5);

		GameEvents.OnImprovementOpen?.Invoke("Searchlight");
	}
}
