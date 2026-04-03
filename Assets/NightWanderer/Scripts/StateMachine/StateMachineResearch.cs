using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class StateMachineResearch : StateMachineState
{
	public StateMachineResearch(int id, StateMachineManager manager): base(id, manager)
	{

	}

	public override void Enter()
	{
		base.Enter();
	}

	public override void Exit()
	{
		base.Exit();
	}

	public override void Update()
	{
		if (Keyboard.current.fKey.wasPressedThisFrame)
		{
			StateManager.SetState(0);
		}

		if (Keyboard.current.spaceKey.wasPressedThisFrame) StateManager.CurrentResearchShip.Search();
	}
}
