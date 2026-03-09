using System;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.VFX;
using static UnityEngine.GraphicsBuffer;

public class StateMachineMovement : StateMachineState
{
	protected readonly GameObject PlayerCameraRotationObject;
	protected readonly GameObject ShipObject;
	protected readonly Transform Ship;
	protected readonly Transform VacuumCleanerObject;
	protected readonly InputAction MoveAction;
	protected readonly InputAction UpDownMoveAction;
	protected readonly InputAction LookAction;
	protected readonly VacuumCleaner Cleaner;
	protected readonly Fuel ShipFuel;
	protected readonly float Speed;
	protected readonly float UpDownSpeed;
	protected readonly float LookSpeed;
	protected Vector3 MoveDirection;
	protected Vector3 ForwardVector;
	protected Vector3 RightVector;
	protected Vector3 UpVector;
	protected Vector3 HalfVectorVacuum;
	protected Vector2 MouseAxis;
	protected float SpeedX;
	protected float SpeedY;
	protected float SpeedZ;
	protected float RotationX;
	protected float RotationY;
	protected bool IsCleanerWorking;


	public StateMachineMovement(int id, StateMachineManager manager, GameObject playerCameraRotationObject, GameObject shipObject, Transform ship, Transform vacuumCleanerObject, VacuumCleaner vacuumCleaner, Fuel shipFuel, InputAction moveAction, InputAction upDownMoveAction, InputAction lookAction, float speed, float upDownSpeed, float lookSpeed) : base(id, manager) 
	{
		PlayerCameraRotationObject = playerCameraRotationObject;
		ShipObject = shipObject;
		Ship = ship;
		VacuumCleanerObject = vacuumCleanerObject;
		Cleaner = vacuumCleaner;
		ShipFuel = shipFuel;
		MoveAction = moveAction;
		UpDownMoveAction = upDownMoveAction;
		LookAction = lookAction;
		Speed = speed;
		UpDownSpeed = upDownSpeed;
		LookSpeed = lookSpeed;
	}

	public override void Enter()
	{
		HalfVectorVacuum = new Vector3(VacuumCleanerObject.transform.localScale.x / 2, VacuumCleanerObject.transform.localScale.y / 2, VacuumCleanerObject.transform.localScale.z / 2);
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
		if (Keyboard.current.spaceKey.wasPressedThisFrame) VacuumCleaner();

		if (Ship.GetComponent<ShipMovement>().IsCanMiningResource && Keyboard.current.fKey.wasPressedThisFrame)
		{
			StateManager.IsCleanerWorking = false;
			VacuumCleaner();

			StateManager.TargetShipRotation = Quaternion.Euler(0, CompareDifference(Ship.rotation.eulerAngles.y), 0);
			StateManager.TargetCameraRotation = Quaternion.Euler(StateManager.ResourceRotationX, CompareDifference(Ship.rotation.eulerAngles.y), 0);

			StateManager.NextState = StateManager.CurrentResourceSource.ExtractionID;
			StateManager.SetState(10);
		}

		if (Ship.GetComponent<ShipMovement>().IsCanDocking && Keyboard.current.fKey.wasPressedThisFrame)
		{
			StateManager.IsCleanerWorking = false;
			VacuumCleaner();

			StateManager.TargetShipRotation = Quaternion.Euler(0, CompareDifference(Ship.rotation.eulerAngles.y), 0);
			StateManager.TargetCameraRotation = Quaternion.Euler(StateManager.ResourceRotationX, CompareDifference(Ship.rotation.eulerAngles.y), 0);

			StateManager.NextState = 20;
			StateManager.SetState(10);
		}
	}

	protected virtual void Move()
	{
		ForwardVector = Ship.transform.TransformDirection(Vector3.forward);
		RightVector = Ship.transform.TransformDirection(Vector3.right);
		UpVector = Ship.transform.TransformDirection(Vector3.up);

		if (Keyboard.current.wKey.IsPressed() || Keyboard.current.sKey.IsPressed()) SpeedX = -Speed * MoveAction.ReadValue<Vector2>().y;
		else SpeedX = 0;
		if (Keyboard.current.aKey.IsPressed() || Keyboard.current.dKey.IsPressed()) SpeedZ = -Speed * MoveAction.ReadValue<Vector2>().x;
		else SpeedZ = 0;
		if (StateManager._Animator != null)
		{
			StateManager._Animator.SetFloat("SpeedX", Mathf.Lerp(StateManager._Animator.GetFloat("SpeedX"), MoveAction.ReadValue<Vector2>().y, 7 * Time.deltaTime));
			StateManager._Animator.SetFloat("SpeedY", Mathf.Lerp(StateManager._Animator.GetFloat("SpeedY"), MoveAction.ReadValue<Vector2>().x, 7 * Time.deltaTime));

			if (SpeedX == 0 && SpeedZ == 0) StateManager._Animator.SetBool("IsIdle", true);
			else StateManager._Animator.SetBool("IsIdle", false);
		}

		if (Keyboard.current.qKey.IsPressed() || Keyboard.current.eKey.IsPressed()) SpeedY = UpDownSpeed * UpDownMoveAction.ReadValue<Vector2>().y;
		else SpeedY = 0;

		MoveDirection = (ForwardVector * SpeedX) + (RightVector * SpeedZ) + (UpVector * SpeedY);

		Ship.transform.position += MoveDirection;
	}

	protected virtual void Look()
	{
		MouseAxis = LookAction.ReadValue<Vector2>();

		RotationX += -MouseAxis.y * LookSpeed;
		RotationX = Mathf.Clamp(RotationX, -90, 90);

		RotationY += -MouseAxis.x * LookSpeed;

		PlayerCameraRotationObject.transform.rotation = Quaternion.Euler(-RotationX, -RotationY, 0);
		Ship.transform.rotation = Quaternion.Euler(0, -RotationY, 0);
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

	protected void VacuumCleaner()
	{
		if (StateManager.IsCleanerWorking) Cleaner.CleanerOn(VacuumCleanerObject.gameObject, HalfVectorVacuum);
		else Cleaner.CleanerOff();

		StateManager.IsCleanerWorking = !StateManager.IsCleanerWorking;
	}
}