using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


//Хранит информацию о ресурсе в ячейке + отвечает за перетаскивание этого ресурса в пределах инвентаря
public class ResourceCellObject : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
	private RectTransform _RectTransform;
	private Image View;
	private string Name;
	private int ID = -1;
	private int MaxCount;
	private int CurrentCount;

	private Vector2 Offset = Vector3.zero;

	public event Action OnUpdate;


	public void Initializing()
	{
		_RectTransform = GetComponent<RectTransform>();
		View = GetComponent<Image>();
	}

	public int GetId() => ID;

	public void AddResource(ResourceBase resource)
	{
		if (ID == -1)
		{
			View.sprite = resource.View;
			Name = resource.Name;
			ID = resource.ID;
			MaxCount = resource.MaxCount;
			CurrentCount = resource.CurrentCount;
		}
		else
		{
			CurrentCount += resource.CurrentCount;
			if (CurrentCount > MaxCount) CurrentCount = MaxCount;
		}

		OnUpdate?.Invoke();
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

	public int GetResourceCount() => CurrentCount;

	public int GetMaxResourceCount() => MaxCount;

	public int GetEmptyResourceCount() => MaxCount - CurrentCount;

	public void SetResourceCount(int count) => CurrentCount = count;

	public void OnBeginDrag(PointerEventData eventData)
	{
		Vector2 mouseWorldPosition = GetMouseWorldPosition();
		//Offset = _RectTransform.anchoredPosition - mouseWorldPosition;
	}

	public void OnDrag(PointerEventData eventData)
	{
		Vector2 mouseWorldPosition = GetMouseWorldPosition();
		_RectTransform.anchoredPosition = mouseWorldPosition + Offset;
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		//if ()  Логика переноса куда-то

		_RectTransform.anchoredPosition = Vector3.zero;
	}

	private Vector3 GetMouseWorldPosition()
	{
		Vector3 mouseScreenPosition = Input.mousePosition;
		mouseScreenPosition.z = 0f;
		return Camera.main.ScreenToWorldPoint(mouseScreenPosition);
	}
}