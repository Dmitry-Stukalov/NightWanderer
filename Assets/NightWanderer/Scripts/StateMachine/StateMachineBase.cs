using UnityEngine;
using UnityEngine.InputSystem;

public class StateMachineBase : StateMachineState
{
	public StateMachineBase(int id, StateMachineManager manager, Transform ship): base(id, manager, ship)
	{

	}

	public override void Enter()
	{
		StateManager.CurrentBase.MoveDownDockingPlatform();

		StateManager._Animator.SetBool("IsIdle", true);
	}

	public override void Exit() 
	{
		StateManager.CurrentBase.MoveUpDockingPlatform();

		StateManager._Animator.SetBool("IsIdle", false);
	}

	public override void Update()
	{
		base.Update();

		if (Keyboard.current.fKey.wasPressedThisFrame)
		{
			StateManager.TargetShipPosition = StateManager.CurrentBase.transform.GetChild(0).position;
			StateManager.NextState = 1;
			StateManager.SetState(10);
		}
	}
}
