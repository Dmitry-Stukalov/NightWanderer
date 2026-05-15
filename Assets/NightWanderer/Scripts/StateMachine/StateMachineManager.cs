
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class StateMachineManager
{
	private Dictionary<int, StateMachineState> _states = new Dictionary<int, StateMachineState>();
	private StateMachineState _currentState;
	public Transform Ship { get; set; }
	public Transform PlayerCameraObject { get; set; }
	public Base CurrentBase { get; set; }
	public ResourceSource CurrentResourceSource { get; set; }
	public ResearchShip CurrentResearchShip { get; set; }
	public Animator _Animator { get; set; }
	public InventoryButton Inventory { get; set; }
	public Vector3 TargetShipPosition { get; set; }
	public Quaternion TargetShipRotation { get; set; }
	public Quaternion TargetCameraRotation { get; set; }
	public float RotationX { get; set; } = -15;
	public float RotationY { get; set; } = 0;
	public float ResourceRotationX { get; private set; } = 20;
	public float DistanceToGround { get; set; } = 0;
	public int NextState { get; set; }
	public bool IsCleanerWorking { get; set; } = false;
	public bool IsReverseMove { get; set; } = false;
	public bool IsDead { get; set; } = false;


	public void AddState(int ID, StateMachineState newState)
	{
		_states.Add(ID, newState);
	}

	public void SetState(int ID)
	{
		if (_currentState != null && _currentState.ID == ID) return;

		if (_states.TryGetValue(ID, out var newState))
		{
			_currentState?.Exit();

			_currentState = newState;

			_currentState.Enter();
		}
	}

	public int GetCurrentState() => _currentState.ID;

	public void HitSurface()
	{
		if (_currentState.ID > 2) return;

		IsReverseMove = true;
	}

	public void Update()
	{
		_currentState?.Update();
	}
}
