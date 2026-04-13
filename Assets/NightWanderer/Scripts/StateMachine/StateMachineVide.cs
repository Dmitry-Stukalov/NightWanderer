using UnityEngine;
using UnityEngine.InputSystem;

public class StateMachineVide : StateMachineMovement
{
	public StateMachineVide(int id, StateMachineManager manager, GameObject playerCameraRotationObject, GameObject shipObject, Transform ship, Transform vacuumCleanerObject, VacuumCleaner vacuumCleaner, Fuel shipFuel, JetEngines shipEngines, InputAction moveAction, InputAction upDownMoveAction, InputAction lookAction,
		float lookSpeed)
		:
		base(id, manager, playerCameraRotationObject, shipObject, ship, vacuumCleanerObject, vacuumCleaner, shipFuel, shipEngines, moveAction, upDownMoveAction, lookAction, lookSpeed) { }

	public override void Enter()
	{
		base.Enter();

		RotationX = StateManager.RotationX;
		RotationY = StateManager.RotationY;

		StateManager.NextState = 0;
		StateManager.DistanceToGround = 0;

		GameEvents.OnEnginesOnOff?.Invoke();
	}

	public override void Exit()
	{
		base.Exit();

		StateManager.RotationX = RotationX;
		StateManager.RotationY = RotationY;

		GameEvents.OnEnginesOnOff?.Invoke();
	}

	public override void Update()
	{
		base.Update();

		if (!ShipFuel.IsFuelEmpty && Keyboard.current.rKey.wasPressedThisFrame) StateManager.SetState(0);

		if (Keyboard.current.spaceKey.wasPressedThisFrame) VacuumCleaner();;
	}
}
