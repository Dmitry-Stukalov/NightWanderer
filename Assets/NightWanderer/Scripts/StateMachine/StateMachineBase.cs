using UnityEngine;
using UnityEngine.InputSystem;

public class StateMachineBase : StateMachineState
{
	public StateMachineBase(int id, StateMachineManager manager, Transform ship, UIManager uiManager) : base(id, manager, ship, uiManager)
	{

	}

	public override void Enter()
	{
		StateManager.CurrentBase.MoveDownDockingPlatform();
		StateManager._Animator.SetBool("IsIdle", true);

		_UIManager.OpenUI();

		GameEvents.OnInBase?.Invoke();
		GameEvents.OnEnginesOnOff?.Invoke();
	}

	public override void Exit() 
	{
		StateManager.CurrentBase.MoveUpDockingPlatform();
		StateManager._Animator.SetBool("IsIdle", false);

		_UIManager.CloseUI();

		GameEvents.OnOutBase?.Invoke();
		GameEvents.OnEnginesOnOff?.Invoke();
	}

	public override void Update()
	{
		base.Update();

		if (Keyboard.current.fKey.wasPressedThisFrame)
		{
			StateManager.TargetShipPosition = StateManager.CurrentBase.GetPlatformPosition();
			//StateManager.NextState = 1;
			//StateManager.SetState(10);
			StateManager.SetState(1);
		}

		if (Keyboard.current.rKey.wasPressedThisFrame) GameEvents.OnSkipTimeStart?.Invoke();
		if (Keyboard.current.rKey.wasReleasedThisFrame) GameEvents.OnSkipTimeEnd?.Invoke();
	}
}
