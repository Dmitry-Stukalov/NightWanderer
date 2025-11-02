using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.InputSystem;

public class StateMachineResourceExtraction : StateMachineState
{
	protected Transform Ship;
	protected GameObject PlayerCameraRotationObject;
	public StateMachineResourceExtraction(int id, StateMachineManager manager, Transform ship, GameObject playerCameraRotationObject) : base(id, manager)
	{
		Ship = ship;
		PlayerCameraRotationObject = playerCameraRotationObject;
	}

	public override void Enter()
	{
		Debug.Log("ResourceExtraction");
	}

	public override void Exit()
	{
		
	}

	public override void Update()
	{
		if (Keyboard.current.fKey.wasPressedThisFrame) StateManager.SetState(0);
	}
}
