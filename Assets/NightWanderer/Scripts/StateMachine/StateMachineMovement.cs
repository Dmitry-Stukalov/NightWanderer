using DG.Tweening;
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
	protected readonly JetEngines ShipEngines;
	protected float Speed;
	protected float UpDownSpeed;
	protected float LookSpeed;
	protected Timer ReverseMoveTimer;
	protected Timer FuelConsumptionTimer;
	protected Vector3 MoveDirection;
	protected Vector3 ReverseDirection;
	protected Vector3 ForwardVector;
	protected Vector3 RightVector;
	protected Vector3 UpVector;
	protected Vector2 MouseAxis;
	protected float SpeedX;
	protected float SpeedY;
	protected float SpeedZ;
	protected float RotationX;
	protected float RotationY;
	protected bool IsCleanerWorking;


	public StateMachineMovement(int id, StateMachineManager manager, GameObject playerCameraRotationObject, GameObject shipObject, Transform ship, Transform vacuumCleanerObject, VacuumCleaner vacuumCleaner, Fuel shipFuel, JetEngines shipEngines, InputAction moveAction, InputAction upDownMoveAction, InputAction lookAction, float lookSpeed) : base(id, manager) 
	{
		PlayerCameraRotationObject = playerCameraRotationObject;
		ShipObject = shipObject;
		Ship = ship;
		VacuumCleanerObject = vacuumCleanerObject;
		Cleaner = vacuumCleaner;

		ShipFuel = shipFuel;

		ShipEngines = shipEngines;
		ShipEngines.OnUpgrade += EnginesUpgrade;

		MoveAction = moveAction;
		UpDownMoveAction = upDownMoveAction;
		LookAction = lookAction;
		LookSpeed = lookSpeed;

		EnginesUpgrade();

		ReverseMoveTimer = new Timer(0.5f);
		ReverseMoveTimer.OnTimerEnd += ReverseMoveEnd;
		ReverseMoveTimer.SetPause();

		FuelConsumptionTimer = new Timer(3f);
		FuelConsumptionTimer.OnTimerEnd += FuelConsumption;
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
		if (Ship.GetComponent<ShipMovement>().IsCanMiningResource && Keyboard.current.fKey.wasPressedThisFrame && ID != 3)
		{
			if (StateManager.IsCleanerWorking) VacuumCleaner();

			StateManager.TargetShipRotation = Quaternion.Euler(0, CompareDifference(Ship.rotation.eulerAngles.y), 0);
			StateManager.TargetCameraRotation = Quaternion.Euler(StateManager.ResourceRotationX, CompareDifference(Ship.rotation.eulerAngles.y), 0);

			StateManager.NextState = StateManager.CurrentResourceSource.ExtractionID;
			StateManager.SetState(10);
		}

		if (Ship.GetComponent<ShipMovement>().IsCanDocking && Keyboard.current.fKey.wasPressedThisFrame && ID != 3)
		{
			if (StateManager.IsCleanerWorking) VacuumCleaner();

			StateManager.TargetShipRotation = Quaternion.Euler(0, CompareDifference(Ship.rotation.eulerAngles.y), 0);
			StateManager.TargetCameraRotation = Quaternion.Euler(StateManager.ResourceRotationX, CompareDifference(Ship.rotation.eulerAngles.y), 0);

			StateManager.NextState = 20;
			StateManager.SetState(10);
		}

		if (Ship.GetComponent<ShipMovement>().IsCanResearch && Keyboard.current.fKey.wasPressedThisFrame && ID != 3)
		{
			if (StateManager.IsCleanerWorking) VacuumCleaner();

			StateManager.TargetShipRotation = Quaternion.Euler(0, CompareDifference(Ship.rotation.eulerAngles.y), 0);
			StateManager.TargetCameraRotation = Quaternion.Euler(StateManager.ResourceRotationX, CompareDifference(Ship.rotation.eulerAngles.y), 0);

			StateManager.NextState = 15;
			StateManager.SetState(10);
		}

		ReverseMoveTimer.Tick(Time.deltaTime);
		//FuelConsumptionTimer.Tick(Time.deltaTime);

		if (ID == 1 || ID == 2) Move();
		if (!StateManager.Inventory.IsOpen) Look();
	}

	protected virtual void Move()
	{
		ForwardVector = Ship.transform.TransformDirection(Vector3.forward);
		RightVector = Ship.transform.TransformDirection(Vector3.right);
		UpVector = Ship.transform.TransformDirection(Vector3.up);

		if (StateManager.IsReverseMove)
		{
			if (MoveDirection.y != 0)
			{
				Ship.transform.position -= new Vector3(0, MoveDirection.y, 0) / 2;
			}
			else Ship.transform.position -= MoveDirection / 2;

			ReverseMoveTimer.Continue();
			return;
		}

		if (Keyboard.current.wKey.IsPressed() || Keyboard.current.sKey.IsPressed()) SpeedX = -Speed * MoveAction.ReadValue<Vector2>().y;
		else SpeedX = 0;
		if (Keyboard.current.aKey.IsPressed() || Keyboard.current.dKey.IsPressed()) SpeedZ = -Speed * MoveAction.ReadValue<Vector2>().x;
		else SpeedZ = 0;
		if (StateManager._Animator != null)
		{
			if (ID != 2)
			{
				StateManager._Animator.SetFloat("SpeedX", Mathf.Clamp(Mathf.Lerp(StateManager._Animator.GetFloat("SpeedX"), MoveAction.ReadValue<Vector2>().y, 7 * Time.deltaTime), -0.5f, 0.5f));
				StateManager._Animator.SetFloat("SpeedY", Mathf.Clamp(Mathf.Lerp(StateManager._Animator.GetFloat("SpeedY"), MoveAction.ReadValue<Vector2>().x, 7 * Time.deltaTime), -0.5f, 0.5f));
			}
			else
			{
				StateManager._Animator.SetFloat("SpeedX", Mathf.Clamp(Mathf.Lerp(StateManager._Animator.GetFloat("SpeedX"), MoveAction.ReadValue<Vector2>().y, 7 * Time.deltaTime), -1f, 1f));
				StateManager._Animator.SetFloat("SpeedY", Mathf.Clamp(Mathf.Lerp(StateManager._Animator.GetFloat("SpeedY"), MoveAction.ReadValue<Vector2>().x, 7 * Time.deltaTime), -1f, 1f));
			}


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

	protected virtual void ReverseMoveEnd()
	{
		StateManager.IsReverseMove = false;
		ReverseMoveTimer.ResetTimer(true);
	}

	protected virtual void Look()
	{
		MouseAxis = LookAction.ReadValue<Vector2>();

		RotationX += -MouseAxis.y * LookSpeed;
		RotationX = Mathf.Clamp(RotationX, -90, 90);

		RotationY += -MouseAxis.x * LookSpeed;

		PlayerCameraRotationObject.transform.rotation = Quaternion.Euler(-RotationX, -RotationY, 0);
		if (!Mouse.current.rightButton.isPressed) Ship.transform.DORotate(new Vector3(0, -RotationY, 0), 1f).SetEase(Ease.Linear); //Ship.transform.rotation = Quaternion.Euler(0, -RotationY, 0);
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
		if (!StateManager.IsCleanerWorking) Cleaner.CleanerOn();
		else Cleaner.CleanerOff();

		StateManager.IsCleanerWorking = !StateManager.IsCleanerWorking;
	}

	protected void EnginesUpgrade()
	{

		if (ID == 0)
		{
			Speed = 0;
			UpDownSpeed = 0;
		}

		if (ID == 1)
		{
			Speed = ShipEngines.GetWalkSpeed();
			UpDownSpeed = ShipEngines.GetWalkSpeedUp();
		}

		if (ID == 2)
		{
			Speed = ShipEngines.GetRunSpeed();
			UpDownSpeed = ShipEngines.GetRunSpeedUp();
		}
	}

	protected void FuelConsumption()
	{
		ShipEngines.EnginesRunning(ID);
		FuelConsumptionTimer.ResetTimer(false);
	}
}