using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class DraggableManipulator : PointerManipulator
{
	private VisualElement _startParent;
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
	}

	private void OnPointerUp(PointerUpEvent evt)
	{
		if (!IsEnabled || !target.HasPointerCapture(evt.pointerId)) return;

		target.ReleasePointer(evt.pointerId);
		IsEnabled = false;

		target.pickingMode = PickingMode.Ignore;
		VisualElement elementUnderCursor = target.panel.Pick(evt.position);
		target.pickingMode = PickingMode.Position;

		if (elementUnderCursor != null && elementUnderCursor.ClassListContains("Cell") && elementUnderCursor.hierarchy.childCount != 0)
		{
			var cellResource = elementUnderCursor.hierarchy.Children().ElementAt(0);
			var result = ((ResourceCellObject)cellResource.dataSource).AddResource(((ResourceCellObject)target.dataSource).GetResource());
			if (result.CurrentCount == 0) ((ResourceCellObject)target.dataSource).ResetResource();
		}

		_startParent.Add(target);
		target.transform.position = Vector3.zero;
	}
}
