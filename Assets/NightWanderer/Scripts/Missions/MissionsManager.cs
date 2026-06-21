using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.AI;
using UnityEngine.UIElements;

public class MissionsManager : MonoBehaviour
{
	[SerializeField] private UIDocument _playerUI;
	[SerializeField] private MissionsConfig _missionsConfig;
	private VisualElement _taskPanelBackground;
	private Mission[] _missions;
	private int _currentMission = 0;

	public event Action OnMissionComplete;


	public void Initializing()
	{
		_missions = new Mission[_missionsConfig.Missions.Length];

		for (int i = 0; i < _missionsConfig.Missions.Length; i++)
			_missions[i] = new Mission(_missionsConfig.Missions[i]);

		_taskPanelBackground = _playerUI.rootVisualElement.Q<VisualElement>("UpdateTaskPanel");

		GameEvents.OnMissionComplete += CheckMission;
		GameEvents.OnDoMission += CheckMission;
		GameEvents.OnSave += SaveData;
		GameEvents.OnCurrentMissionLoad += LoadData;
	}

	private IEnumerator ShowTaskPanel()
	{
		DOTween.To(() => _taskPanelBackground.style.opacity.value, x => _taskPanelBackground.style.opacity = x, 1, 1.5f);

		yield return new WaitForSeconds(3f);

		DOTween.To(() => _taskPanelBackground.style.opacity.value, x => _taskPanelBackground.style.opacity = x, 0, 1.5f);
	}
	
	private void CheckMission(int id)
	{
		if (id == _currentMission && _missions[_currentMission].UpdateMission(0))
		{
			_missions[_currentMission].CompleteMission();
			_currentMission++;

			CompleteMission();
		}
	}

	private void CheckMission(int id, int value)
	{
		if (id == _currentMission && _missions[_currentMission].UpdateMission(value))
		{
			_missions[_currentMission].CompleteMission();
			_currentMission++;

			CompleteMission();
		}
	}

	public void CompleteMission()
	{
		StartCoroutine(ShowTaskPanel());

		if (_currentMission == 3) GameEvents.OnDialogueStart();

		if (_currentMission == 5)
		{
			GameEvents.OnDialogueStart?.Invoke();
			GameEvents.OnMarkShow?.Invoke(0);
		}

		if (_currentMission == 6)
		{
			GameEvents.OnMarkHide?.Invoke(0);
			GameEvents.OnDialogueStart?.Invoke();
		}

		OnMissionComplete?.Invoke();
	}

	public string GetCurrentMissionText() => _missions[_currentMission].GetMissionText();
	public bool GetMissionStatus(int id) => _missions[id].IsMissionComplete();

	public void LoadData(int currentMission)
	{
		_currentMission = currentMission;
		OnMissionComplete?.Invoke();
	}

	public void SaveData() => GameEvents.OnCurrentMissionSave?.Invoke(_currentMission);

	private void OnDisable()
	{
		GameEvents.OnMissionComplete -= CheckMission;
		GameEvents.OnDoMission -= CheckMission;
		GameEvents.OnCurrentMissionLoad -= LoadData;
	}
}
