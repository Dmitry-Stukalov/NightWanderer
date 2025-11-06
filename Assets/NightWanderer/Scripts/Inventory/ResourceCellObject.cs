using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ResourceCellObject : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
	private Image View;
	private string Name;
	private int ID = -1;
	private int MaxCount;
	private int CurrentCount;

	private Vector3 Offset = Vector3.zero;

	private void Start()
	{
		View = GetComponent<Image>();
	}

	public int GetId() => ID;

	public int AddResource(ResourceBase resource)
	{
		if (ID == resource.ID)
		{
			if (MaxCount < resource.CurrentCount + CurrentCount)
			{
				int returnCount = resource.CurrentCount + CurrentCount - MaxCount;
				CurrentCount = MaxCount;
				return returnCount;
			}
		}

		if (ID == -1)
		{
			View.sprite = resource.View;
			Name = resource.Name;
			ID = resource.ID;
			MaxCount = resource.MaxCount;
			CurrentCount = resource.CurrentCount;

			if (resource.CurrentCount <= resource.MaxCount) return 0;
			else return resource.CurrentCount - resource.MaxCount;
		}

		return -1;
	}

	public int TakeResource(int value)
	{
		if (CurrentCount >= value)
		{
			CurrentCount -= value;
			return value;
		}
		else
		{
			int canGet = value - CurrentCount;
			CurrentCount = 0;
			return canGet;
		}
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		Vector3 mouseWorldPosition = GetMouseWorldPosition();
		Offset = transform.position - mouseWorldPosition;
	}

	public void OnDrag(PointerEventData eventData)
	{
		Vector3 mouseWorldPosition = GetMouseWorldPosition();
		transform.position = mouseWorldPosition + Offset;
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		//if ()  Логика переноса куда-то

		transform.position = Vector3.zero;
	}

	private Vector3 GetMouseWorldPosition()
	{
		Vector3 mouseScreenPosition = Input.mousePosition;
		mouseScreenPosition.z = 0f;
		return Camera.main.ScreenToWorldPoint(mouseScreenPosition);
	}
}