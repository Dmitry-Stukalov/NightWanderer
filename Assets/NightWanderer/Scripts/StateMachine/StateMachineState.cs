
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class StateMachineState
{
	protected readonly StateMachineManager StateManager;
	protected readonly Transform Ship;
	protected UIManager _UIManager;
	public readonly int ID;

	public StateMachineState(int id, StateMachineManager stateManager, Transform ship, UIManager uiManager)
	{
		ID = id;
		StateManager = stateManager;
		Ship = ship;
		_UIManager = uiManager;
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

		if (Keyboard.current.tKey.wasPressedThisFrame) GameEvents.OnOffSearchlights?.Invoke();

		if (ID == 1 || ID == 2) GameEvents.OnSearchlightsStartMove?.Invoke();
		if (ID == 0 || ID == 3) GameEvents.OnSearchlightsStartSearch?.Invoke();
	}
}
