using UnityEngine;

public class Mark : MonoBehaviour
{
	[SerializeField] private GameObject _markObject;
	private bool IsOpen = false;

	private void Start()
	{
		GameEvents.OnFirstBaseVisit += Open;

		_markObject.SetActive(false);
	}

	public void Open()
	{
		_markObject.SetActive(true);

		IsOpen = true;

		GameEvents.OnFirstBaseVisit -= Open;
	}

	private void OnDisable()
	{
		GameEvents.OnFirstBaseVisit -= Open;
	}
}
