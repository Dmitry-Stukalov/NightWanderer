using UnityEngine;
using UnityEngine.InputSystem;

public class StateMachineResourceExtraction1 : StateMachineResourceExtraction
{
	private Timer ExtractionTimer;
	public StateMachineResourceExtraction1(int id, StateMachineManager manager, Transform ship, GameObject playerCameraRotationObject) : base(id, manager, ship, playerCameraRotationObject) { }

	public override void Enter()
	{
		base.Enter();
		ExtractionTimer = new Timer(1);
		ExtractionTimer.OnTimerEnd += EndExtraction;
	}

	public override void Exit() 
	{ 
		base.Exit();
	}

	public override void Update()
	{
		base.Update();

		if (Keyboard.current.spaceKey.IsPressed()) ExtractionTimer.Tick(Time.deltaTime);
	}

	private void EndExtraction()
	{
		StateManager.CurrentResourceSource.ResourceExtracted();
		ExtractionTimer.ResetTimer(false);
		Debug.Log("Ресурс добыт");
	}
}
