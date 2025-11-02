using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class Cell : MonoBehaviour
{
	[field: SerializeField] private GameObject ResourcePlace;
	public bool IsEmpty = true;
	//private int CellCapacity;

	public int GetResource(Resource resource)
	{
		if (IsEmpty)
		{
			IsEmpty = false;
			resource.transform.SetParent(ResourcePlace.transform, false);
			resource.transform.position = Vector3.zero;
			return resource.CurrentCount;
		}
		else
		{
			return ResourcePlace.GetComponentInChildren<Resource>().ChangeCount(resource.CurrentCount, true);
		}
	}

	public void TakeResource()
	{
		IsEmpty = true;
	}

	//public void ChangeCapacity(int newCapacity) => CellCapacity = newCapacity;
}

