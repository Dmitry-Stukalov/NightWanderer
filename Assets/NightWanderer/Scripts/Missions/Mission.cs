using UnityEngine;

public class Mission
{
	private int _ID;
	private string _description;
	private int _neededResources;
	private int _currentResource;
	private bool IsComplete;
	private bool IsDestinationMission;

	public Mission(MissionData data)
	{
		_ID = data.ID;
		_description = data.Description;
		IsComplete = false;
		IsDestinationMission = data.IsDestinationMission;
		_neededResources = data.NeededResources;
		_currentResource = 0;
	}

	public bool UpdateMission(int value)
	{
		if (IsDestinationMission)
		{
			CompleteMission();
			return IsMissionComplete();
		}
		else
		{
			_currentResource += value;

			if (_currentResource >= _neededResources)
			{
				CompleteMission();
				return IsMissionComplete();
			}
			return false;
		}
	}

	public void CompleteMission() => IsComplete = true;
	public bool IsMissionComplete() => IsComplete;
	public string GetMissionText() => _description;
}
