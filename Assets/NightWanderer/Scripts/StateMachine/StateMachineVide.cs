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
		StateManager.NextState = 0;
		StateManager.DistanceToGround = 0;
	}

	public override void Exit()
	{
		base.Exit();
	}

	public override void Update()
	{
		base.Update();

		if (!ShipFuel.IsFuelEmpty && Keyboard.current.rKey.wasPressedThisFrame) StateManager.SetState(0);

		if (Keyboard.current.spaceKey.wasPressedThisFrame) VacuumCleaner();;
	}
}
