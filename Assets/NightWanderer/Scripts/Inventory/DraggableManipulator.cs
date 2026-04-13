using System;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class DraggableManipulator : PointerManipulator
{
	private VisualElement _startParent;
	private VisualElement _parent;
	private VisualElement _elementUnderCursor;
	private VisualElement _newElementUnderCursor;
	private ResourceCellObject _cellResource;
	private Vector3 _startPosition;
	private bool IsEnabled;
	private bool IsBase;

	public DraggableManipulator (VisualElement target, bool isBase)
	{
		this.target = target;
		IsBase = isBase;
	}

	protected override void RegisterCallbacksOnTarget()
	{
		target.RegisterCallback<PointerDownEvent>(OnPointerDown);
		target.RegisterCallback<PointerMoveEvent>(OnPointerMove);
		target.RegisterCallback<PointerUpEvent>(OnPointerUp);

		target.RegisterCallback<PointerEnterEvent>(evt => target.Q<VisualElement>("CellResourceNameBackground").style.display = DisplayStyle.Flex);
		target.RegisterCallback<PointerLeaveEvent>(evt => target.Q<VisualElement>("CellResourceNameBackground").style.display = DisplayStyle.None);
	}

	protected override void UnregisterCallbacksFromTarget()
	{
		target.UnregisterCallback<PointerDownEvent>(OnPointerDown);
		target.UnregisterCallback<PointerMoveEvent>(OnPointerMove);
		target.UnregisterCallback<PointerUpEvent>(OnPointerUp);

		target.UnregisterCallback<PointerEnterEvent>(evt => target.Q<VisualElement>("CellResourceNameBackground").style.display = DisplayStyle.Flex);
		target.UnregisterCallback<PointerLeaveEvent>(evt => target.Q<VisualElement>("CellResourceNameBackground").style.display = DisplayStyle.None);
	}

	private void OnPointerDown(PointerDownEvent evt)
	{
		_startParent = target.parent;

		if (!IsBase) target.panel.visualTree.Q<VisualElement>("Inventory").Children().ElementAt(target.panel.visualTree.Q<VisualElement>("Inventory").childCount - 1).Add(target);
		else target.panel.visualTree.Q<VisualElement>("PlayerInventory").Children().ElementAt(target.panel.visualTree.Q<VisualElement>("PlayerInventory").childCount - 1).Add(target);

		_startPosition = evt.localPosition;
		IsEnabled = true;
		target.CapturePointer(evt.pointerId);
	}

	private void OnPointerMove(PointerMoveEvent evt)
	{
		if (!IsEnabled || !target.HasPointerCapture(evt.pointerId)) return;

		Vector3 delta = evt.localPosition - _startPosition;

		target.transform.position = new Vector3(target.transform.position.x + delta.x, target.transform.position.y + delta.y, 0);

		target.pickingMode = PickingMode.Ignore;
		_newElementUnderCursor = target.panel.Pick(evt.position);
		target.pickingMode = PickingMode.Position;

		if (_elementUnderCursor != null && _elementUnderCursor != _newElementUnderCursor)
		{
			if (_elementUnderCursor.ClassListContains("Cell")) _elementUnderCursor.RemoveFromClassList("BorderCellWide");
			else _elementUnderCursor.RemoveFromClassList("BorderAll");
		}

		if (_newElementUnderCursor != null && _newElementUnderCursor.ClassListContains("Cell") && !((CellObject)_newElementUnderCursor.dataSource).IsCraftCell) _newElementUnderCursor.AddToClassList("BorderCellWide");

		if (_newElementUnderCursor != null && CheckMatch(_newElementUnderCursor.GetClasses().ToArray())) _newElementUnderCursor.AddToClassList("BorderAll");

		_elementUnderCursor = _newElementUnderCursor;
	}

	private void OnPointerUp(PointerUpEvent evt)
	{
		if (!IsEnabled || !target.HasPointerCapture(evt.pointerId)) return;

		target.ReleasePointer(evt.pointerId);
		IsEnabled = false;

		_cellResource = (ResourceCellObject)target.dataSource;

		target.pickingMode = PickingMode.Ignore;
		_elementUnderCursor = target.panel.Pick(evt.position);
		target.pickingMode = PickingMode.Position;

		if (_elementUnderCursor != null)
		{
			if (_elementUnderCursor.ClassListContains("Cell") && _elementUnderCursor.hierarchy.childCount != 0 && !((CellObject)_elementUnderCursor.dataSource).IsCraftCell)
			{
				_elementUnderCursor.RemoveFromClassList("BorderCellWide");

				var cellResource = _elementUnderCursor.hierarchy.Children().ElementAt(0);
				var result = ((ResourceCellObject)cellResource.dataSource).AddResource(_cellResource.GetResource());
				if (result.CurrentCount == 0) _cellResource.ResetResource();
			}
			else
			{
				switch (_cellResource.GetId())
				{
					case 0:
						if (_elementUnderCursor.ClassListContains("FuelRecovery"))
						{
							_elementUnderCursor.RemoveFromClassList("BorderAll");

							int needResource = Convert.ToInt32(Mathf.Round(((FuelRecovery)_elementUnderCursor.dataSource).NeedToRefueling() * 2));

							((FuelRecovery)_elementUnderCursor.dataSource).RecoverFuel(_cellResource.GetResourceCount() * 0.5f);

							if (_cellResource.GetResourceCount() > needResource) _cellResource.SetResourceCount(_cellResource.GetResourceCount() - needResource);
							else _cellResource.ResetResource();
						}
					break;

					case 1:
						if (_elementUnderCursor.ClassListContains("FireDefenseRecovery"))
						{
							_elementUnderCursor.RemoveFromClassList("BorderAll");

							int needResource = Convert.ToInt32(Mathf.Round(((HealthFireDefenseRecovery)_elementUnderCursor.dataSource).NeedToHealing() * 2));

							((HealthFireDefenseRecovery)_elementUnderCursor.dataSource).RecoverHealth(_cellResource.GetResourceCount() * 0.5f);

							if (_cellResource.GetResourceCount() > needResource) _cellResource.SetResourceCount(_cellResource.GetResourceCount() - needResource);
							else _cellResource.ResetResource();
						}
					break;

					case 3:
						if (_elementUnderCursor.ClassListContains("HealthRecovery"))
						{
							_elementUnderCursor.RemoveFromClassList("BorderAll");

							int needResource = Convert.ToInt32(Mathf.Round(((HealthFireDefenseRecovery)_elementUnderCursor.dataSource).NeedToHealing() * 2));

							((HealthFireDefenseRecovery)_elementUnderCursor.dataSource).RecoverHealth(_cellResource.GetResourceCount() * 0.5f);

							if (_cellResource.GetResourceCount() > needResource) _cellResource.SetResourceCount(_cellResource.GetResourceCount() - needResource);
							else _cellResource.ResetResource();
						}
					break;

					case 6:
						if (_elementUnderCursor.ClassListContains("DefenseRecovery"))
						{
							_elementUnderCursor.RemoveFromClassList("BorderAll");

							int needResource = Convert.ToInt32(Mathf.Round(((HealthFireDefenseRecovery)_elementUnderCursor.dataSource).NeedToHealing() * 2));

							((HealthFireDefenseRecovery)_elementUnderCursor.dataSource).RecoverHealth(_cellResource.GetResourceCount() * 0.5f);

							if (_cellResource.GetResourceCount() > needResource) _cellResource.SetResourceCount(_cellResource.GetResourceCount() - needResource);
							else _cellResource.ResetResource();
						}
					break;
				}
			}
		}

		_startParent.Add(target);
		target.transform.position = Vector3.zero;
	}

	private bool CheckMatch(string[] elementClass)
	{
		for (int i = 0; i < elementClass.Length; i++)
		{
			switch (elementClass[i])
			{
				case "FuelRecovery":
					if (((ResourceCellObject)target.dataSource).GetResource().ID == 0) return true;
				break;

				case "HealthRecovery":
					if (((ResourceCellObject)target.dataSource).GetResource().ID == 3) return true;
				break;

				case "DefenseRecovery":
					if (((ResourceCellObject)target.dataSource).GetResource().ID == 6) return true;
				break;

				case "FireDefenseRecovery":
					if (((ResourceCellObject)target.dataSource).GetResource().ID == 1) return true;
				break;
			}
		}

		return false;
	}
}
