using UnityEngine;

public class StateMachineResourceExtraction1 : StateMachineResourceExtraction
{
	public StateMachineResourceExtraction1(int id, StateMachineManager manager, Transform ship, GameObject playerCameraRotationObject) : base(id, manager, ship, playerCameraRotationObject) { }

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
		base.Update();
	}
}
