using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class StateMachineIdle : StateMachineMovement
{
	public StateMachineIdle(int id, StateMachineManager manager, GameObject playerCameraRotationObject, Transform ship, Transform vacuumCleanerObject, InputAction moveAction, InputAction upDownMoveAction, InputAction lookAction, float speed, float upDownSpeed, float lookSpeed) : base(id, manager, playerCameraRotationObject, ship, vacuumCleanerObject, moveAction, upDownMoveAction, lookAction, speed, upDownSpeed, lookSpeed) { }


	public override void Enter()
	{
		base.Enter();
		//Debug.Log("Idle");
	}

	public override void Exit()
	{
		base.Exit();
	}

	public override void Update()
	{
		base.Update();

		if (MoveAction.ReadValue<Vector2>() != Vector2.zero || UpDownMoveAction.ReadValue<Vector2>() != Vector2.zero) StateManager.SetState(1);

		Look();
	}
}