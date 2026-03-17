using UnityEngine;
using UnityEngine.InputSystem;

public class StateMachineBase : StateMachineState
{
	public StateMachineBase(int id, StateMachineManager manager): base(id, manager)
	{

	}

	public override void Enter()
	{
		StateManager.CurrentBase.MoveDownDockingPlatform();
	}

	public override void Exit() 
	{
		StateManager.CurrentBase.MoveUpDockingPlatform();
	}

	public override void Update()
	{
		if (Keyboard.current.fKey.wasPressedThisFrame)
		{
			StateManager.TargetShipPosition = StateManager.CurrentBase.transform.GetChild(0).position;
			StateManager.NextState = 1;
			StateManager.SetState(10);
		}
	}
}
