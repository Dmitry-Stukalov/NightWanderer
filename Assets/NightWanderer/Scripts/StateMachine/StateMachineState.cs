
public abstract class StateMachineState
{
	protected readonly StateMachineManager StateManager;
	public readonly int ID;

	public StateMachineState(int id, StateMachineManager stateManager)
	{
		ID = id;
		StateManager = stateManager;
	}

	public virtual void Enter()
	{

	}

	public virtual void Exit()
	{

	}

	public virtual void Update()
	{

	}
}
