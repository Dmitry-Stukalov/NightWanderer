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

		for (int i = 0; i < config.ImprovementName.Length; i++) GameEvents.OnImprovementOpen?.Invoke(config.ImprovementName[i]);

		for (int i = 0; i < config.CraftName.Length; i++) GameEvents.OnCraftOpen?.Invoke(config.CraftName[i]);

		for (int i = 0; i < config.StoryName.Length; i++) GameEvents.OnStoryOpen?.Invoke(config.StoryName[i]);

		IsEmpty = true;
	}

	public ResearchConfig GetResearchConfig() => config;
	public bool IsDataUpload() => IsEmpty;
}
