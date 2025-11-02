using Unity.Hierarchy;
using UnityEngine;

public class StateMachineTransition : StateMachineState
{
	private Transform Ship;
	private Transform PlayerCameraRotationObject;
	private Vector3 TargetShipPosition;
	private Quaternion TargetShipRotation;
	private Quaternion TargetCameraRotation;
	private float PositionTolerance = 0.01f;
	private float RotationTolerance = 0.5f;
	private bool PositionReached;
	private bool RotationReached;
	private bool RotationCameraReached;

	public StateMachineTransition(int id, StateMachineManager manager, Transform ship, Transform playerCameraRotationObject): base(id, manager) 
	{ 
		Ship = ship;
		PlayerCameraRotationObject = playerCameraRotationObject;
	}
	 
	public override void Enter()
	{
		TargetShipPosition = StateManager.TargetShipPosition;
		TargetShipRotation = StateManager.TargetShipRotation;
		TargetCameraRotation = StateManager.TargetCameraRotation;
		Debug.Log("Transition");
	}

	public override void Exit()
	{
		StateManager.RotationX = -TargetCameraRotation.eulerAngles.x;
		StateManager.RotationY = TargetCameraRotation.eulerAngles.y;
	}

	public override void Update()
	{
		PositionReached = Vector3.Distance(Ship.position, TargetShipPosition) <= PositionTolerance;
		RotationReached = Quaternion.Angle(Ship.rotation, TargetShipRotation) <= RotationTolerance;
		RotationCameraReached = Quaternion.Angle(PlayerCameraRotationObject.rotation, TargetCameraRotation) <= RotationTolerance;

		if (PositionReached && RotationReached && RotationCameraReached)
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
