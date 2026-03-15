using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class DraggableManipulator : PointerManipulator
{
	private Vector3 _startPosition;
	private bool IsEnabled;

	public DraggableManipulator (VisualElement target)
	{
		this.target = target;
	}

	protected override void RegisterCallbacksOnTarget()
	{
		target.RegisterCallback<PointerDownEvent>(OnPointerDown);
		target.RegisterCallback<PointerMoveEvent>(OnPointerMove);
		target.RegisterCallback<PointerUpEvent>(OnPointerUp);
	}

	protected override void UnregisterCallbacksFromTarget()
	{
		target.UnregisterCallback<PointerDownEvent>(OnPointerDown);
		target.UnregisterCallback<PointerMoveEvent>(OnPointerMove);
		target.UnregisterCallback<PointerUpEvent>(OnPointerUp);
	}

	private void OnPointerDown(PointerDownEvent evt)
	{
		_startPosition = evt.localPosition;
		IsEnabled = true;
		target.CapturePointer(evt.pointerId);

		target.BringToFront();
	}

	private void OnPointerMove(PointerMoveEvent evt)
	{
		if (!IsEnabled || !target.HasPointerCapture(evt.pointerId)) return;

		Vector3 delta = (Vector3)evt.localPosition - _startPosition;

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

		if (elementUnderCursor != null && elementUnderCursor.ClassListContains("Cell"))
		{
			target.transform.position = Vector3.zero;

			elementUnderCursor.Add(target);
		}
		else
		{
			target.transform.position = Vector3.zero;
		}
	}
}
