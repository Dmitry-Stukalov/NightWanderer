using NUnit.Framework;
using Unity.Hierarchy;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using DG.Tweening;
using UnityEngine.InputSystem;

public class StateMachineTransition : StateMachineState
{
	private Transform PlayerCameraRotationObject;
	private Vector3 TargetShipPosition;
	private Quaternion TargetShipRotation;
	private Quaternion TargetCameraRotation;
	protected float RotationX;
	protected float RotationY;
	private float PositionTolerance = 0.01f;
	private float RotationTolerance = 0.5f;
	private bool PositionReached;
	private bool RotationReached;
	private bool RotationCameraReached;

	public StateMachineTransition(int id, StateMachineManager manager, Transform ship, UIManager uiManager, Transform playerCameraRotationObject): base(id, manager, ship, uiManager) 
	{ 
		PlayerCameraRotationObject = playerCameraRotationObject;
	}
	 
	public override void Enter()
	{
		StateManager._Animator.SetBool("IsIdle", true);

		if (StateManager.NextState == 20) TargetShipPosition = StateManager.CurrentBase.GetPlatformPosition();
		else TargetShipPosition = StateManager.TargetShipPosition;

		TargetShipRotation = StateManager.TargetShipRotation;
		RotationX = StateManager.RotationX;
		RotationY = StateManager.RotationY;
		TargetCameraRotation = StateManager.TargetCameraRotation;
	}

	public override void Exit()
	{
		StateManager.RotationX = RotationX;
		StateManager.RotationY = RotationY;

		//if (StateManager.NextState == 20) _UIManager.CloseUI();

		StateManager._Animator.SetBool("IsIdle", false);
	}

	public override void Update()
	{
		PositionReached = Vector3.Distance(Ship.position, TargetShipPosition) <= PositionTolerance;
		RotationReached = Quaternion.Angle(Ship.rotation /*Quaternion.Euler(StateManager.RotationX, StateManager.RotationY, Ship.rotation.z)*/, TargetShipRotation) <= RotationTolerance;
		if (StateManager.NextState != 3) RotationCameraReached = Quaternion.Angle(PlayerCameraRotationObject.rotation, TargetCameraRotation) <= RotationTolerance;

		if ((StateManager.NextState == 3 || StateManager.NextState == 50) && PositionReached)
		{
			StateManager.SetState(StateManager.NextState);
		}
		else if (PositionReached && RotationReached && RotationCameraReached)
		{
			StateManager.SetState(StateManager.NextState);
		}
		else
		{
			if (StateManager.NextState == 50)
			{
				Ship.position = Vector3.MoveTowards(Ship.position, TargetShipPosition, Time.deltaTime * 2);
				Ship.rotation = Quaternion.Slerp(Ship.rotation, TargetShipRotation, Time.deltaTime * 2);

				PlayerCameraRotationObject.rotation = Quaternion.Slerp(PlayerCameraRotationObject.rotation, TargetCameraRotation, Time.deltaTime * 2);
			}
			else
			{
				Ship.position = Vector3.MoveTowards(Ship.position, TargetShipPosition, Time.deltaTime * 5);
				Ship.rotation = Quaternion.Slerp(Ship.rotation, TargetShipRotation, Time.deltaTime * 5);

				if (StateManager.NextState != 3) PlayerCameraRotationObject.rotation = Quaternion.Slerp(PlayerCameraRotationObject.rotation, TargetCameraRotation, Time.deltaTime * 5);
			}
		}
	}

	protected virtual int CompareDifference(float angle)
	{
		int t = 0;
		int divisionResult = (int)angle / 90;
		angle -= 90 * divisionResult;

		if (angle > 0)
		{
			if (Mathf.Abs(0 - angle) > Mathf.Abs(90 - angle)) t = 1;
			else t = 0;

			return (t + divisionResult) * 90;
		}
		else
		{
			if (Mathf.Abs(-90 - angle) < Mathf.Abs(0 - angle)) t = 1;
			else t = 0;

			return (Mathf.Abs(t) + Mathf.Abs(divisionResult)) * -90;
		}
	}
}
