using UnityEngine;

public class ResearchShip : MonoBehaviour
{
	[field: SerializeField] public GameObject DockingPlace { get; private set; }
	[SerializeField] private ResearchConfig config;
	private bool IsEmpty = false;

	public void Search()
	{
		if (IsEmpty) return;

		for (int i = 0; i < config.ImprovementName.Length; i++) GameEvents.OnImprovementOpen?.Invoke(config.ImprovementName[i]);

		for (int i = 0; i < config.CraftName.Length; i++) GameEvents.OnCraftOpen?.Invoke(config.CraftName[i]);

		for (int i = 0; i < config.StoryName.Length; i++) GameEvents.OnStoryOpen?.Invoke(config.StoryName[i]);

		IsEmpty = true;
	}

	public void TakeResources()
	{

	}

	public void GiveResources(Inventory inventory)
	{
		if (inventory.GetEmptyCellsCount() < config.NeedResourceID.Length) return;

		for (int i = 0; i < config.NeedResourceID.Length; i++) 
			GameEvents.OnResourceAdd(config.NeedResourceID[i], config.NeedResourceCount[i]);
	}

	public ResearchConfig GetResearchConfig() => config;
	public bool IsDataUpload() => IsEmpty;
	public void LoadData(bool isEmpty) => IsEmpty = isEmpty;
}
