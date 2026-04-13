using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class StateMachineRun : StateMachineMovement
{
	public StateMachineRun(int id, StateMachineManager manager, GameObject playerCameraRotationObject, GameObject shipObject, Transform ship, Transform vacuumCleanerObject, VacuumCleaner vacuumCleaner, Fuel shipFuel, JetEngines shipEngines, InputAction moveAction, InputAction upDownMoveAction, InputAction lookAction, 
		float lookSpeed) 
		: 
		base(id, manager, playerCameraRotationObject, shipObject, ship, vacuumCleanerObject, vacuumCleaner, shipFuel, shipEngines, moveAction, upDownMoveAction, lookAction, lookSpeed) { }

	public override void Enter()
	{
		base.Enter();

		if (StateManager.IsCleanerWorking)
		{
			VacuumCleaner();
		}

		GameEvents.OnRunStart?.Invoke();
	}

	public override void Exit()
	{
		base.Exit();

		GameEvents.OnRunEnd?.Invoke();
	}

	public override void Update()
	{
		base.Update();

		if (MoveAction.ReadValue<Vector2>() == Vector2.zero && UpDownMoveAction.ReadValue<Vector2>() == Vector2.zero) StateManager.SetState(0);

		if (Keyboard.current.shiftKey.wasPressedThisFrame) StateManager.SetState(1);

		if (ShipFuel.IsFuelEmpty || Keyboard.current.rKey.wasPressedThisFrame)
		{
			StateManager.NextState = 3;

			StateManager.TargetShipPosition = new Vector3(Ship.transform.position.x, Ship.transform.position.y + 2f - StateManager.DistanceToGround, Ship.transform.position.z);
			StateManager.TargetShipRotation = Ship.transform.rotation;

			StateManager.SetState(10);
		}
	}
}
