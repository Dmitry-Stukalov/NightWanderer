using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler
{
	[SerializeField] private ResourceLibrary Library;
	[SerializeField] private GameObject ResourceOnLand;
	[SerializeField] private GameObject Player;

	public void OnDrop(PointerEventData eventData)
	{
		GameObject resource = Library.GetResource(eventData.pointerDrag.transform.GetComponent<ResourceCellObject>().ThisResource.ID);
		resource.transform.SetParent(gameObject.transform, false);
		resource.GetComponent<ResourceOnLand>().SetResourceCount(eventData.pointerDrag.transform.GetComponent<ResourceCellObject>().ThisResource.CurrentCount);
		resource.transform.localPosition = new Vector3(Player.transform.position.x, Player.transform.position.y - 5, Player.transform.position.z);

		eventData.pointerDrag.transform.GetComponent<ResourceCellObject>().ResetResource();
		Debug.Log("Z");
	}
}
