using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
	private DialogueManager _dialogueManager;
	private bool IsActivated = false;

	private void Start()
	{
		_dialogueManager = FindAnyObjectByType<DialogueManager>();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (!other.CompareTag("Player") || IsActivated) return;

		IsActivated = true;
		_dialogueManager.StartNewDialogue();
	}
}
