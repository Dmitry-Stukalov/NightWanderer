using UnityEngine;

public class ResearchShip : MonoBehaviour
{
	[field: SerializeField] public GameObject DockingPlace { get; private set; }
	[SerializeField] private bool IsImprovementOpen;
	[SerializeField] private bool IsCraftOpen;
	[SerializeField] private string _improvementName;
	[SerializeField] private string _craftName;
	private bool IsEmpty = false;

	public void Search()
	{
		if (IsEmpty) return;

		//_improvementManager.UnlockImprovement(_searchName);

		if (IsImprovementOpen) GameEvents.OnImprovementOpen?.Invoke(_improvementName);

		if (IsCraftOpen) GameEvents.OnCraftOpen?.Invoke(_craftName);

		IsEmpty = true;
		Debug.Log("Улучшение открыто");
	}
}
