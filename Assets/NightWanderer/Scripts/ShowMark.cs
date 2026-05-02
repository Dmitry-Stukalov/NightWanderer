using UnityEngine;

public class ShowMark : MonoBehaviour
{
	[SerializeField] private GameObject _canvasObject;
	[SerializeField] private int _markID;

	private void Start()
	{
		_canvasObject.SetActive(false);

		GameEvents.OnMarkShow += OpenMark;
		GameEvents.OnMarkHide += CloseMark;
	}

	private void OpenMark(int id)
	{
		if (id == _markID) _canvasObject.SetActive(true);
	}

	private void CloseMark(int id)
	{
		if (id == _markID) _canvasObject.SetActive(false);
	}

	private void OnDisable()
	{
		GameEvents.OnMarkShow -= OpenMark;
		GameEvents.OnMarkHide -= CloseMark;
	}
}
