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

		//Ship.position += new Vector3(0, 3, 0);

		//StateManager._Animator.SetBool("IsIdle", false);

		//GameEvents.OnEnginesOnOff?.Invoke();
	}

	public override void Update()
	{
		base.Update();

		if (!StateManager.IsDead && StateManager.CurrentBase != null)
		{
			StateManager.TargetShipRotation = Quaternion.Euler(0, CompareDifference(Ship.rotation.eulerAngles.y), 0);
			StateManager.TargetCameraRotation = Quaternion.Euler(0, CompareDifference(Ship.rotation.eulerAngles.y), 0);

			StateManager.NextState = 20;
			StateManager.SetState(10);
		}
		else if (!StateManager.IsDead)
		{
			Ship.position += new Vector3(0, 3, 0);

			StateManager._Animator.SetBool("IsIdle", false);

			GameEvents.OnEnginesOnOff?.Invoke();

			StateManager.SetState(0);
		}
	}

	protected int CompareDifference(float n1)
	{
		int t = 0;
		int divisionResult = (int)n1 / 90;
		n1 -= 90 * divisionResult;

		if (n1 > 0)
		{
			if (Mathf.Abs(0 - n1) > Mathf.Abs(90 - n1)) t = 1;
			else t = 0;

			return (t + divisionResult) * 90;
		}
		else
		{
			if (Mathf.Abs(-90 - n1) < Mathf.Abs(0 - n1)) t = 1;
			else t = 0;

			return (Mathf.Abs(t) + Mathf.Abs(divisionResult)) * -90;
		}
	}
}
