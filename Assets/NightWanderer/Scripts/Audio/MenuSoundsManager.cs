using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UIElements;

public class MenuSoundsManager : MonoBehaviour
{
	[SerializeField] private UIDocument _menuUI;
	[SerializeField] private AudioMixer _audioMixer;
	[SerializeField] private AudioSource _backgroundMusic;
	[SerializeField] private AudioSource _buttonClickSound;
	[SerializeField] private AudioSource _buttonHoverSound;
	[SerializeField] private AudioMixerSnapshot _menuMusic;
	[SerializeField] private AudioMixerSnapshot _menuMute;
	[SerializeField] private float _transitDuration;
	private Button[] _buttons;

	public void Initializing()
	{
		GameEvents.OnMainMenuIn += FadeIn;
		GameEvents.OnMainMenuOut += FadeOut;

		_buttons = _menuUI.rootVisualElement.Query<Button>().ToList().ToArray();

		for (int i = 0; i < _buttons.Length; i++)
		{
			_buttons[i].RegisterCallback<ClickEvent>(PlayClickSound);
			_buttons[i].RegisterCallback<PointerEnterEvent>(PlayHoverSound);
		}

			//_backgroundMusic?.Play();
	}

	private void FadeIn()
	{
		_menuMusic.TransitionTo(_transitDuration);
	}

	private void FadeOut()
	{
		_menuMute.TransitionTo(_transitDuration);
	}

	private void PlayClickSound(ClickEvent evt)
	{
		_buttonClickSound.Play();
	}

	private void PlayHoverSound(PointerEnterEvent evt)
	{
		//_buttonHoverSound.Play();
	}

	public void OnOffMusic()
	{
		if (_backgroundMusic == null) return;

		if (_backgroundMusic.isPlaying) _backgroundMusic.Stop();
		else _backgroundMusic.Play();
	}

	public void OnOffSounds()
	{
		if (_buttonClickSound.enabled) _buttonClickSound.enabled = false;
		else _buttonClickSound.enabled = true;
	}

	private void OnDisable()
	{
		for (int i = 0; i < _buttons.Length; i++)
		{
			_buttons[i].UnregisterCallback<ClickEvent>(PlayClickSound);
			_buttons[i].UnregisterCallback<PointerEnterEvent>(PlayHoverSound);
		}

		GameEvents.OnMainMenuIn -= FadeIn;
		GameEvents.OnMainMenuOut -= FadeOut;
	}
}
