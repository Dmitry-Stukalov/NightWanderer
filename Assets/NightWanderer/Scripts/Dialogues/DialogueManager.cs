using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogueManager : MonoBehaviour
{
	[SerializeField] private UIDocument _playerUI;
	[SerializeField] private DialogueConfig _config;
	private DialogueWriter _dialogueWriter;
	private int _currentDialogue = 0;
	private bool IsDialogueContinue = false;
	private bool IsTransfer = false;
	private bool IsNextActive = false;

	public void Initializing()
	{
		var _textBackground = _playerUI.rootVisualElement.Q<VisualElement>("DialoguePanel");
		_textBackground.dataSource = new DialogueWriter(_textBackground, _textBackground.Q<Label>("CharacterName"), _textBackground.Q<Label>("CharacterText"));

		_dialogueWriter = (DialogueWriter)_textBackground.dataSource;

		GameEvents.OnDialogueStart += StartNewDialogue;
	}

	public void StartNewDialogue()
	{
		if (_config.Dialogues.Length == _currentDialogue) return;

		if (IsDialogueContinue)
		{
			IsNextActive = true;
			return;
		}

		ShowBackground();

		IsDialogueContinue = true;

		StartCoroutine(TextWriter());
	}

	private IEnumerator TextWriter()
	{
		for (int i = 0; i < _config.Dialogues[_currentDialogue].Name.Length; i++)
		{
			_dialogueWriter.ClearCharacter();
			_dialogueWriter.SetCharacter(_config.Dialogues[_currentDialogue].Name[i]);
			_dialogueWriter.ClearText();

			foreach (var symbol in _config.Dialogues[_currentDialogue].Phrase[i])
			{
				_dialogueWriter.AddChar(symbol);
				yield return new WaitForSeconds(0.03f);
			}

			yield return new WaitForSeconds(2f);
		}

		IsDialogueContinue = false;
		_currentDialogue++;
		HideBackground();

		if (IsNextActive)
		{
			yield return new WaitForSeconds(2);
			IsNextActive = false;
			StartNewDialogue();
		}
	}

	private void ShowBackground() => _dialogueWriter.ShowBackground();

	private void HideBackground() => _dialogueWriter.HideBackground();

	private void OnDisable()
	{
		GameEvents.OnDialogueStart -= StartNewDialogue;
	}
}
