using NUnit.Framework;
using Unity.Hierarchy;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class StateMachineTransition : StateMachineState
{
	private Transform Ship;
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
	private int PositionsIndex;

	public StateMachineTransition(int id, StateMachineManager manager, Transform ship, Transform playerCameraRotationObject): base(id, manager) 
	{ 
		Ship = ship;
		PlayerCameraRotationObject = playerCameraRotationObject;
	}
	 
	public override void Enter()
	{
		PositionsIndex = 0;
		TargetShipPosition = StateManager.TargetShipPosition;
		TargetShipRotation = StateManager.TargetShipRotation;
		RotationX = StateManager.RotationX;
		RotationY = StateManager.RotationY;
		TargetCameraRotation = StateManager.TargetCameraRotation;
	}

	public override void Exit()
	{
		//StateManager.RotationX = -TargetCameraRotation.eulerAngles.x;
		//StateManager.RotationY = TargetCameraRotation.eulerAngles.y;
		StateManager.RotationX = RotationX;
		StateManager.RotationY = RotationY;
		//StateManager.TargetCameraRotation = TargetCameraRotation;
	}

	public override void Update()
	{
		PositionReached = Vector3.Distance(Ship.position, TargetShipPosition) <= PositionTolerance;
		RotationReached = Quaternion.Angle(Ship.rotation /*Quaternion.Euler(StateManager.RotationX, StateManager.RotationY, Ship.rotation.z)*/, TargetShipRotation) <= RotationTolerance;
		RotationCameraReached = Quaternion.Angle(PlayerCameraRotationObject.rotation, TargetCameraRotation) <= RotationTolerance;

		if (StateManager.NextState == 3 && PositionReached)
		{
			StateManager.SetState(StateManager.NextState);
		}
		else if (PositionReached && RotationReached && RotationCameraReached)
		{
			StateManager.SetState(StateManager.NextState);
		}
		else
		{
			Ship.position = Vector3.MoveTowards(Ship.position, TargetShipPosition, Time.deltaTime * 5);
			Ship.rotation = Quaternion.Slerp(Ship.rotation, TargetShipRotation, Time.deltaTime * 5);

			PlayerCameraRotationObject.rotation = Quaternion.Slerp(PlayerCameraRotationObject.rotation, TargetCameraRotation, Time.deltaTime * 5);
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
