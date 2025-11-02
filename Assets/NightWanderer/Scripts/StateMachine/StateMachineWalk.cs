using UnityEngine;
using UnityEngine.InputSystem;

public class StateMachineWalk : StateMachineMovement
{
	public StateMachineWalk(int id, StateMachineManager manager, GameObject playerCameraRotationObject, Transform ship, InputAction moveAction, InputAction lookAction, float speed, float upDownSpeed, float lookSpeed) : base(id, manager, playerCameraRotationObject, ship, moveAction, lookAction, speed, upDownSpeed, lookSpeed) { }

	public override void Enter()
	{
		base.Enter();
		Debug.Log("Walk");
	}

	public override void Exit()
	{
		base.Exit();
	}

	public override void Update()
	{
		base.Update();

		if (MoveAction.ReadValue<Vector2>() == Vector2.zero) StateManager.SetState(0);

		if (Keyboard.current.shiftKey.wasPressedThisFrame) StateManager.SetState(2);

		Move();
		Look();
	}
}
