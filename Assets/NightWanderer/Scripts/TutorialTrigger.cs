using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
	[SerializeField] private int[] id;
	[SerializeField] private bool IsOneTime;
	private PlayerUIManager _manager;
	private bool IsActivated = false;

	private void Start()
	{
		_manager = FindAnyObjectByType<PlayerUIManager>();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (IsOneTime && IsActivated) return;

		if (other.CompareTag("Player"))
		{
			_manager.OpenTutorial(id);
			IsActivated = true;
		}
	}
}
