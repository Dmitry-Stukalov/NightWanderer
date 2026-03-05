
using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class StateMachineManager
{
	private Dictionary<int, StateMachineState> States = new Dictionary<int, StateMachineState>();
	private StateMachineState CurrentState;
	public Transform Ship { get; set; }
	public Transform PlayerCameraObject { get; set; }
	public Base CurrentBase { get; set; }
	public ResourceSource CurrentResourceSource { get; set; }
	public Animator _Animator { get; set; }
	public Vector3 TargetShipPosition { get; set; }
	public Quaternion TargetShipRotation { get; set; }
	public Quaternion TargetCameraRotation { get; set; }
	public float RotationX { get; set; } = -15;
	public float RotationY { get; set; } = 0;
	public float ResourceRotationX { get; private set; } = 0;
	public int NextState { get; set; }
	public bool IsCleanerWorking { get; set; } = false;



	public void AddState(int ID, StateMachineState newState)
	{
		States.Add(ID, newState);
	}

	public void SetState(int ID)
	{
		if (CurrentState != null && CurrentState.ID == ID) return;

		if (States.TryGetValue(ID, out var newState))
		{
			CurrentState?.Exit();

			CurrentState = newState;

			CurrentState.Enter();
		}
	}

	public void Update()
	{
		CurrentState?.Update();
	}
}
