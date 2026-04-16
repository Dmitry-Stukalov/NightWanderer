
using UnityEngine;

public abstract class StateMachineState
{
	protected readonly StateMachineManager StateManager;
	protected readonly Transform Ship;
	public readonly int ID;

	public StateMachineState(int id, StateMachineManager stateManager, Transform ship)
	{
		ID = id;
		StateManager = stateManager;
		Ship = ship;
	}

	public virtual void Enter()
	{

	}

	public virtual void Exit()
	{

	}

	public virtual void Update()
	{
		if (StateManager.IsDead && ID != 50)
		{
			StateManager.NextState = 50;

			StateManager.TargetShipPosition = new Vector3(Ship.transform.position.x, Ship.transform.position.y - StateManager.DistanceToGround, Ship.transform.position.z);
			StateManager.TargetShipRotation = Ship.transform.rotation;

			StateManager.SetState(10);
		}
	}
}
