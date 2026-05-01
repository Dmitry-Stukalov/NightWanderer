using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class StateMachineResearch : StateMachineState
{
	public StateMachineResearch(int id, StateMachineManager manager, Transform ship): base(id, manager, ship)
	{

	}

	public override void Enter()
	{
		base.Enter();

		GameEvents.OnResearchStart?.Invoke();
		GameEvents.OnResearchQuit += () => StateManager.SetState(0);
	}

	public override void Exit()
	{
		base.Exit();
		GameEvents.OnResearchQuit -= () => StateManager.SetState(0);
	}

	public override void Update()
	{
		base.Update();
	}
}
