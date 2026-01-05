using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class StateMachineWalk : StateMachineMovement
{
	public StateMachineWalk(int id, StateMachineManager manager, GameObject playerCameraRotationObject, GameObject shipObject, Transform ship, Transform vacuumCleanerObject, InputAction moveAction, InputAction upDownMoveAction, InputAction lookAction, float speed, float upDownSpeed, float lookSpeed) : base(id, manager, playerCameraRotationObject, shipObject, ship, vacuumCleanerObject, moveAction, upDownMoveAction, lookAction, speed, upDownSpeed, lookSpeed) { }

	public override void Enter()
	{
		base.Enter();
		//Debug.Log("Walk");
	}

	public override void Exit()
	{
		base.Exit();
	}

	public override void Update()
	{
		base.Update();

		if (MoveAction.ReadValue<Vector2>() == Vector2.zero && UpDownMoveAction.ReadValue<Vector2>() == Vector2.zero) StateManager.SetState(0);

		if (Keyboard.current.shiftKey.wasPressedThisFrame) StateManager.SetState(2);

		Move();
		Look();
	}
}
