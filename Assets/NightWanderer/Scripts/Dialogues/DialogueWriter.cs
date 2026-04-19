using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogueWriter
{
	private VisualElement _background;
	private Label _characterName;
	private Label _text;

	public DialogueWriter(VisualElement background, Label characterName, Label text)
	{
		_background = background;
		_characterName = characterName;
		_text = text;
	}


	public void ClearCharacter() => _characterName.text = "";

	public void ClearText() => _text.text = "";

	public void SetCharacter(string characterName) => _characterName.text = characterName;

	public void AddChar(char text) => _text.text += text;

	public void ShowBackground() => DOTween.To(() => _background.resolvedStyle.opacity, x => _background.style.opacity = x, 1, 2f);

	public void HideBackground() => DOTween.To(() => _background.resolvedStyle.opacity, x => _background.style.opacity = x, 0, 2f);
}
