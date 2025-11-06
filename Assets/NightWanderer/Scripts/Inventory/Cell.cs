using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class Cell
{
	private GameObject ResourceObject;
	//private ResourceBase ThisResource;
	private ResourceCellObject CellObject;
	public bool IsEmpty = true;

	public Cell(GameObject resourceObject)
	{
		ResourceObject = resourceObject;
		CellObject = ResourceObject.GetComponentInChildren<ResourceCellObject>();
	}

	public int GetResourceID() => CellObject.GetId();

	public void AddResource(ResourceBase resource)
	{
		if (!IsEmpty && resource.ID != CellObject.GetId()) return;

		CellObject.AddResource(resource);
	}

	public int TakeResource(int value) => CellObject.TakeResource(value);
}