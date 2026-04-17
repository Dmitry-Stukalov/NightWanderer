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

	public void Initializing()
	{
		var _textBackground = _playerUI.rootVisualElement.Q<VisualElement>("DialoguePanel");
		_textBackground.dataSource = new DialogueWriter(_textBackground, _textBackground.Q<Label>("CharacterName"), _textBackground.Q<Label>("CharacterText"));

		_dialogueWriter = (DialogueWriter)_textBackground.dataSource;
	}

	public void StartNewDialogue()
	{
		if (IsDialogueContinue || _config.Dialogues.Length == _currentDialogue) return;

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
	}

	private void ShowBackground() => _dialogueWriter.ShowBackground();

	private void HideBackground() => _dialogueWriter.HideBackground();
}
