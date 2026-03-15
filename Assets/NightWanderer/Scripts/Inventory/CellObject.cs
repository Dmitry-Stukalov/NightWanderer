using UnityEngine;
using UnityEngine.EventSystems;

public class CellObject : MonoBehaviour, IDropHandler
{
	[field: SerializeField] private ResourceCellObject _ResourceCellObject;
	private ResourceBase _ResourceBase = new ResourceBase();
	public void OnDrop(PointerEventData eventData)
	{
		//_ResourceBase = _ResourceCellObject.AddResource(eventData.pointerDrag.transform.GetComponent<ResourceCellObject>().ThisResource);
		//if (_ResourceBase.CurrentCount == 0 && _ResourceBase.ID != -1) eventData.pointerDrag.transform.GetComponent<ResourceCellObject>().ResetResource();
	}
}
