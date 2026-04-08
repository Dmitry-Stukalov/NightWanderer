using UnityEditorInternal;
using UnityEngine;
using UnityEngine.InputSystem;

public class StateMachineResourceExtraction1 : StateMachineResourceExtraction
{
	private MinigameLaser _minigameLaser;
	private Timer ExtractionRestTimer;
	public StateMachineResourceExtraction1(int id, StateMachineManager manager, Transform ship, GameObject playerCameraRotationObject, MiningEquipment mining, Fuel fuel, MinigameLaser minigameLaser) : base(id, manager, ship, playerCameraRotationObject, mining, fuel) 
	{
		_minigameLaser = minigameLaser;
	}

	public override void Enter()
	{
		base.Enter();
		ExtractionRestTimer = new Timer(1);
		ExtractionRestTimer.OnTimerEnd += RestConsumption;

		_minigameLaser.UpdateData(StateManager.CurrentResourceSource, Ship.gameObject.GetComponent<ShipMovement>().GetPlayerFuel());
		_minigameLaser.StartGame(StateManager.CurrentResourceSource.GetCurrentResourceCount());
		GameEvents.OnLaserExtractionStart?.Invoke();
	}

	public override void Exit() 
	{ 
		base.Exit();

		_minigameLaser.EndGame();
		GameEvents.OnExtractionEnd?.Invoke();
	}

	public override void Update()
	{
		base.Update();

		ExtractionRestTimer.Tick(Time.deltaTime);

		if (Keyboard.current.spaceKey.wasPressedThisFrame)
		{
			if (_minigameLaser.CheckResult())
			{
				EndExtraction();
				_minigameLaser.UpdateData();
			}
		}

	}

	private void EndExtraction()
	{
		StateManager.CurrentResourceSource.ResourceExtracted();
		_mining.MiningLaser();
	}

	private void RestConsumption()
	{
		_mining.MiningRest();
		ExtractionRestTimer.ResetTimer(false);
	}
}
