using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class StateMachineMovement : StateMachineState
{
	protected readonly GameObject PlayerCameraRotationObject;
	protected readonly Transform Ship;
	protected readonly InputAction MoveAction;
	protected readonly InputAction LookAction;
	protected readonly float Speed;
	protected readonly float UpDownSpeed;
	protected readonly float LookSpeed;
	protected Vector3 MoveDirection;
	protected Vector3 ForwardVector;
	protected Vector3 RightVector;
	protected Vector2 MouseAxis;
	protected float SpeedX;
	protected float SpeedY;
	protected float SpeedZ;
	protected float RotationX;
	protected float RotationY;


	public StateMachineMovement(int id, StateMachineManager manager, GameObject playerCameraRotationObject, Transform ship, InputAction moveAction, InputAction lookAction, float speed, float upDownSpeed, float lookSpeed) : base(id, manager) 
	{
		PlayerCameraRotationObject = playerCameraRotationObject;
		Ship = ship;
		MoveAction = moveAction;
		LookAction = lookAction;
		Speed = speed;
		UpDownSpeed = upDownSpeed;
		LookSpeed = lookSpeed;
	}

	public override void Enter()
	{
		RotationX = StateManager.RotationX;
		RotationY = StateManager.RotationY;
	}

	public override void Exit()
	{
		StateManager.RotationX = RotationX;
		StateManager.RotationY = RotationY;
	}

	public override void Update()
	{
		if (Ship.GetComponent<ShipMovement>().IsCanMiningResource && Keyboard.current.fKey.wasPressedThisFrame)
		{
			StateManager.TargetShipRotation = Quaternion.Euler(0, CompareDifference(Ship.rotation.eulerAngles.y), 0);
			StateManager.TargetCameraRotation = Quaternion.Euler(StateManager.ResourceRotationX, CompareDifference(Ship.rotation.eulerAngles.y), 0);

			StateManager.NextState = 11;
			StateManager.SetState(10);
		}
	}

	protected virtual void Move()
	{
		ForwardVector = Ship.transform.TransformDirection(Vector3.forward);
		RightVector = Ship.transform.TransformDirection(Vector3.right);

		if (Keyboard.current.wKey.IsPressed() || Keyboard.current.sKey.IsPressed()) SpeedX = Speed * MoveAction.ReadValue<Vector2>().y;
		else SpeedX = 0;
		if (Keyboard.current.aKey.IsPressed() || Keyboard.current.dKey.IsPressed()) SpeedZ = Speed * MoveAction.ReadValue<Vector2>().x;
		else SpeedZ = 0;

		if (Keyboard.current.qKey.IsPressed() && Keyboard.current.eKey.IsPressed()) SpeedY = 0;
		else if (Keyboard.current.qKey.IsPressed()) SpeedY = UpDownSpeed;
		else if (Keyboard.current.eKey.IsPressed()) SpeedY = -UpDownSpeed;
		else SpeedY = 0;

		MoveDirection = (ForwardVector * SpeedX) + (RightVector * SpeedZ) + (SpeedY * Ship.transform.up);

		Ship.transform.position = Ship.transform.position + MoveDirection;
	}

	protected virtual void Look()
	{
		MouseAxis = LookAction.ReadValue<Vector2>();

		RotationX += MouseAxis.y * LookSpeed;
		RotationY += MouseAxis.x * LookSpeed;

		PlayerCameraRotationObject.transform.rotation = Quaternion.Euler(-RotationX, RotationY, 0);
		Ship.transform.rotation = Quaternion.Euler(0, RotationY, 0);
	}

	protected int CompareDifference(float n1)
	{
		int t = 0;
		int divisionResult = (int)n1 / 90;
		n1 -= 90 * divisionResult;

		if (n1 > 0)
		{
			if (Mathf.Abs(0 - n1) > Mathf.Abs(90 - n1)) t = 1;
			else t = 0;

			return (t + divisionResult) * 90;
		}
		else
		{
			if (Mathf.Abs(-90 - n1) < Mathf.Abs(0 - n1)) t = 1;
			else t = 0;

			return (Mathf.Abs(t) + Mathf.Abs(divisionResult)) * -90;
		}
	}
}