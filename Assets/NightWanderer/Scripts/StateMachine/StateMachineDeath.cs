using UnityEngine;

public class StateMachineDeath : StateMachineState
{
	public StateMachineDeath(int id, StateMachineManager manager, Transform ship): base(id, manager, ship)
	{

	}

	public override void Enter()
	{
		base.Enter();

		StateManager._Animator.SetBool("IsIdle", true);

		StateManager.NextState = 0;
		StateManager.DistanceToGround = 0;

		GameEvents.OnEnginesOnOff?.Invoke();
	}

	public override void Exit()
	{
		base.Exit();

		Ship.position += new Vector3(0, 3, 0);

		StateManager._Animator.SetBool("IsIdle", false);

		GameEvents.OnEnginesOnOff?.Invoke();
	}

	public override void Update()
	{
		base.Update();

		if (!StateManager.IsDead) StateManager.SetState(0);
	}
}
