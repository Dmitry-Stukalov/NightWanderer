using UnityEngine;

public class ResearchShip : MonoBehaviour
{
	[field: SerializeField] public GameObject DockingPlace { get; private set; }
	[SerializeField] private ImprovementManager _improvementManager;
	[SerializeField] private string _searchName;
	private bool IsEmpty = false;

	public void Search()
	{
		if (IsEmpty) return;

		_improvementManager.UnlockImprovement(_searchName);

		IsEmpty = true;
		Debug.Log("Улучшение открыто");
	}
}
