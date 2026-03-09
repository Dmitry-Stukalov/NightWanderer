using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class StateMachineRun : StateMachineMovement
{
	public StateMachineRun(int id, StateMachineManager manager, GameObject playerCameraRotationObject, GameObject shipObject, Transform ship, Transform vacuumCleanerObject, VacuumCleaner vacuumCleaner, Fuel shipFuel, InputAction moveAction, InputAction upDownMoveAction, InputAction lookAction, 
		float speed, float upDownSpeed, float lookSpeed) 
		: 
		base(id, manager, playerCameraRotationObject, shipObject, ship, vacuumCleanerObject, vacuumCleaner, shipFuel, moveAction, upDownMoveAction, lookAction, speed, upDownSpeed, lookSpeed) { }

	public override void Enter()
	{
		base.Enter();

		StateManager.IsCleanerWorking = false;
		VacuumCleaner();
		//Debug.Log("Run");
	}

	public override void Exit()
	{
		base.Exit();
	}

	public override void Update()
	{
		base.Update();

		if (MoveAction.ReadValue<Vector2>() == Vector2.zero && UpDownMoveAction.ReadValue<Vector2>() == Vector2.zero) StateManager.SetState(0);

		if (Keyboard.current.shiftKey.wasPressedThisFrame) StateManager.SetState(1);

		if (!ShipFuel.IsFuelEmpty) Move();
		Look();
	}
}
