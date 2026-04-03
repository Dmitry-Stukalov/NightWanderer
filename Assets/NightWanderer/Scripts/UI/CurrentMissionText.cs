using UnityEngine;
using UnityEngine.UIElements;

public class CurrentMissionText
{
	private Label _missionsText;
	private MissionsManager _missionsManager;

	public CurrentMissionText(Label missionsText, MissionsManager missionsManager)
	{
		_missionsText = missionsText;
		_missionsManager = missionsManager;
		_missionsManager.OnMissionComplete += UpdateData;

		UpdateData();
	}

	private void UpdateData()
	{
		_missionsText.text = "Цель: " + _missionsManager.GetCurrentMissionText();
	}
}
