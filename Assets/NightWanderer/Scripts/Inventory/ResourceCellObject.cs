using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEditor;
using TMPro;
using UnityEngine.UIElements;


//Хранит информацию о ресурсе в ячейке + отвечает за перетаскивание этого ресурса в пределах инвентаря
public class ResourceCellObject //: MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
	//public GameObject InventoryBackground;
	//[SerializeField] private GameObject ThisCellObject;
	//[SerializeField] private GameObject ResourceUICount;
	//[SerializeField] private GameObject ResourceName;
	//public GameObject InventoryObject;
	//public PlayerInventory _PlayerInventory;
	//public ResourceBase ThisResource = new ResourceBase();
	//private Vector2 StartPosition = Vector3.zero;
	//private Image View;
	//private Color color = Color.white;

	public event Action OnUpdate;

	private VisualElement _resourceImage;
	private Label _resourceCount;
	private ResourceBase _resource;


	public ResourceCellObject(VisualElement resourceImage, Label resourceCount)
	{
		_resourceImage = resourceImage;
		_resourceCount = resourceCount;

		_resource = new ResourceBase();
		_resourceCount.visible = false;
		_resourceImage.visible = false;
	}

	//public void Initializing()
	//{
	//	View = GetComponent<Image>();
	//	SetViewVisible();
	//	ResourceUICount.GetComponentInChildren<ResourceCount>().Initializing();
	//	ResourceUICount.SetActive(false);

	//	ResourceName.SetActive(false);
	//}

	//public int GetId() => ThisResource.ID;
	public int GetId() => _resource.ID;

	public ResourceBase AddResource(ResourceBase resource)
	{
		if (resource == null || resource.ID == -1) return resource;

		if (_resource.ID == -1)
		{
			Debug.Log(resource.View);
			_resourceImage.style.backgroundImage = resource.View.texture;
			//View.sprite = resource.View;
			//SetViewVisible();
			_resource.View = resource.View;
			_resource.Name = resource.Name;
			_resource.ID = resource.ID;
			_resource.CurrentCount = resource.CurrentCount;
			_resource.MaxCount = resource.MaxCount;
			//ResourceUICount.SetActive(true);
			_resourceCount.visible = true;
			_resourceImage.visible = true;
			resource.CurrentCount = 0;
		}
		else
		{
			if (_resource.CurrentCount + resource.CurrentCount <= _resource.MaxCount)
			{
				_resource.CurrentCount += resource.CurrentCount;
				resource.CurrentCount = 0;
			}
			else
			{
				int countDifference = _resource.MaxCount - _resource.CurrentCount;
				_resource.CurrentCount = _resource.MaxCount;
				resource.CurrentCount -= countDifference;
			}
		}

		_resourceCount.text = _resource.CurrentCount.ToString();

		OnUpdate?.Invoke();
		return resource;
	}

	public void ResetResource()
	{
		_resource.ResetValue();
		_resourceImage.style.backgroundImage = null;
		_resourceCount.visible = false;
		//View.sprite = null;
		//SetViewVisible();
		//ResourceUICount.SetActive(false);
	}

	public int GetResourceCount() => _resource.CurrentCount;

	public int GetMaxResourceCount() => _resource.MaxCount;

	public int GetEmptyResourceCount() => _resource.MaxCount - _resource.CurrentCount;

	public void SetResourceCount(int count) => _resource.CurrentCount = count;

	//public ResourceBase AddResource(ResourceBase resource)
	//{
	//	if (resource == null || resource.ID == -1) return resource;

	//	if (ThisResource.ID == -1)
	//	{
	//		View.sprite = resource.View;
	//		SetViewVisible();
	//		ThisResource.View = resource.View;
	//		ThisResource.Name = resource.Name;
	//		ThisResource.ID = resource.ID;
	//		ThisResource.CurrentCount = resource.CurrentCount;
	//		ThisResource.MaxCount = resource.MaxCount;
	//		ResourceUICount.SetActive(true);
	//		resource.CurrentCount = 0;
	//	}
	//	else
	//	{
	//		if (ThisResource.CurrentCount + resource.CurrentCount <= ThisResource.MaxCount)
	//		{
	//			ThisResource.CurrentCount += resource.CurrentCount;
	//			resource.CurrentCount = 0;
	//		}
	//		else
	//		{
	//			int countDifference = ThisResource.MaxCount - ThisResource.CurrentCount;
	//			ThisResource.CurrentCount = ThisResource.MaxCount;
	//			resource.CurrentCount -= countDifference;
	//		}
	//	}

	//	OnUpdate?.Invoke();
	//	return resource;
	//}

	//public void ResetResource()
	//{
	//	ThisResource.ResetValue(); 
	//	View.sprite = null;
	//	SetViewVisible();
	//	ResourceUICount.SetActive(false);
	//}

	//private void SetViewVisible()
	//{
	//	if (color.a == 0)
	//	{
	//		color.a = 1;
	//		View.raycastTarget = true;
	//	}
	//	else
	//	{
	//		color.a = 0;
	//		View.raycastTarget = false;
	//	}

	//	View.color = color;
	//}

	//public int GetResourceCount() => ThisResource.CurrentCount;

	//public int GetMaxResourceCount() => ThisResource.MaxCount;

	//public int GetEmptyResourceCount() => ThisResource.MaxCount - ThisResource.CurrentCount;

	//public void SetResourceCount(int count) => ThisResource.CurrentCount = count;

	/*public void OnBeginDrag(PointerEventData eventData)
	{
		StartPosition = transform.position;
		transform.SetParent(InventoryBackground.transform, true);
		_PlayerInventory.CanvasGroupBlock(false);
		transform.position = GetMouseUIPosition();

		ResourceName.SetActive(false);
	}

	public void OnDrag(PointerEventData eventData)

	{
		transform.position = GetMouseUIPosition();
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		_PlayerInventory.CanvasGroupBlock(true);
		transform.SetParent(ThisCellObject.transform, true);
		transform.position = StartPosition;
		OnUpdate?.Invoke();
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Right) ResetResource();
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		ResourceName.SetActive(true);
		ResourceName.GetComponent<TextMeshProUGUI>().text = ThisResource.Name;

	}

	public void OnPointerExit(PointerEventData eventData)
	{
		ResourceName.SetActive(false);
	}

	private Vector3 GetMouseUIPosition()
	{
		return Mouse.current.position.ReadValue();
	}*/
}