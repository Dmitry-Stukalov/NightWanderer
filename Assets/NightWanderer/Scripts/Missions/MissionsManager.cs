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

		GameEvents.OnBase += CheckMission;
	}

	private IEnumerator ShowTaskPanel()
	{
		DOTween.To(() => _taskPanelBackground.style.opacity.value, x => _taskPanelBackground.style.opacity = x, 1, 1.5f);

		yield return new WaitForSeconds(3f);

		DOTween.To(() => _taskPanelBackground.style.opacity.value, x => _taskPanelBackground.style.opacity = x, 0, 1.5f);
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

		StartCoroutine(ShowTaskPanel());

		OnMissionComplete?.Invoke();
	}

	public string GetCurrentMissionText() => _missions[_currentMission].GetMissionText();

	private void OnDisable()
	{
		GameEvents.OnBase -= CheckMission;
	}
}
