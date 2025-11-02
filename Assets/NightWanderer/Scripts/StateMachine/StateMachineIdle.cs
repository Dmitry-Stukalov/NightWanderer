using UnityEngine;
using UnityEngine.InputSystem;

public class StateMachineIdle : StateMachineMovement
{
	public StateMachineIdle(int id, StateMachineManager manager, GameObject playerCameraRotationObject, Transform ship, InputAction moveAction, InputAction lookAction, float speed, float upDownSpeed, float lookSpeed) : base(id, manager, playerCameraRotationObject, ship, moveAction, lookAction, speed, upDownSpeed, lookSpeed) { }


	public override void Enter()
	{
		base.Enter();
		Debug.Log("Idle");
	}

	public override void Exit()
	{
		base.Exit();
	}

	public override void Update()
	{
		base.Update();

		if (MoveAction.ReadValue<Vector2>() != Vector2.zero) StateManager.SetState(1);

		Look();
	}
}