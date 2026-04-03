using System;
using UnityEngine;
using UnityEngine.Experimental.AI;

public class MissionsManager : MonoBehaviour
{
	[SerializeField] private MissionsConfig _missionsConfig;
	private Mission[] _missions;
	private int _currentMission = 0;

	public event Action OnMissionComplete;


	public void Initializing()
	{
		_missions = new Mission[_missionsConfig.Missions.Length];

		for (int i = 0; i < _missionsConfig.Missions.Length; i++)
			_missions[i] = new Mission(_missionsConfig.Missions[i]);

		GameEvents.OnBase += CheckMission;
	}
	
	private void CheckMission()
	{
		if (_missions[_currentMission].UpdateMission(0))
			CompleteMission();
	}

	private void CheckMission(int value)
	{
		if (_missions[_currentMission].UpdateMission(value))
			CompleteMission();
	}

	public void CompleteMission()
	{
		_missions[_currentMission].CompleteMission();
		_currentMission++;

		OnMissionComplete?.Invoke();
	}

	public string GetCurrentMissionText() => _missions[_currentMission].GetMissionText();

	private void OnDisable()
	{
		GameEvents.OnBase -= CheckMission;
	}
}
