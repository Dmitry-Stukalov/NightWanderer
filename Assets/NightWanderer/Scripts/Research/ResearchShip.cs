using UnityEngine;

public class ResearchShip : MonoBehaviour
{
	[field: SerializeField] public GameObject DockingPlace { get; private set; }
	[SerializeField] private ResearchConfig config;
	[SerializeField] private bool IsImprovementOpen;
	[SerializeField] private bool IsCraftOpen;
	[SerializeField] private bool IsStoryOpen;
	[SerializeField] private string _improvementName;
	[SerializeField] private string _craftName;
	[SerializeField] private string _storyName;
	private bool IsEmpty = false;

	public void Search()
	{
		if (IsEmpty) return;

		//_improvementManager.UnlockImprovement(_searchName);

		if (IsImprovementOpen) GameEvents.OnImprovementOpen?.Invoke(_improvementName);

		if (IsCraftOpen) GameEvents.OnCraftOpen?.Invoke(_craftName);

		if (IsStoryOpen)
		{
			if (_storyName == "BaseKey") GameEvents.OnMissionComplete?.Invoke(3);
			//GameEvents.OnStoryOpen?.Invoke(_storyName);
		}

		IsEmpty = true;
		Debug.Log("Улучшение открыто");
	}

	public ResearchConfig GetResearchConfig() => config;
	public bool IsDataUpload() => IsEmpty;
}
