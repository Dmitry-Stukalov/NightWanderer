using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.InputSystem;

public class StateMachineResourceExtraction : StateMachineState
{
	protected Transform Ship;
	protected GameObject PlayerCameraRotationObject;
	protected MiningEquipment _mining;
	protected Fuel _fuel;

	public StateMachineResourceExtraction(int id, StateMachineManager manager, Transform ship, GameObject playerCameraRotationObject, MiningEquipment mining, Fuel fuel) : base(id, manager)
	{
		Ship = ship;
		PlayerCameraRotationObject = playerCameraRotationObject;
		_mining = mining;
		_fuel = fuel;
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
		if (Keyboard.current.fKey.wasPressedThisFrame) StateManager.SetState(1);

		if (_fuel.IsFuelEmpty) StateManager.SetState(0);
	}
}
