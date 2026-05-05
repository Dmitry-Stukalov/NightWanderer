using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class SettingsManager : MonoBehaviour
{
	[SerializeField] private UIDocument _settingsUI;
	private VisualElement _mainElement;
	private VisualElement _contentBackground;
	private VisualElement _controlsBackground;
	private Button _continueButton;
	private Button _settingsButton;
	private Button _controlsButton;
	private Button _backButton;
	private Button _exitMenuButton;
	private Button _exitButton;
	private bool _hideCursor = true;

	public void Initializing()
	{
		_mainElement = _settingsUI.rootVisualElement.Q<VisualElement>("MainElement");
		_contentBackground = _settingsUI.rootVisualElement.Q<VisualElement>("ContentBackground");
		_controlsBackground = _settingsUI.rootVisualElement.Q<VisualElement>("ControlsBackground");

		_continueButton = _settingsUI.rootVisualElement.Q<Button>("ContinueButton");
		_settingsButton = _settingsUI.rootVisualElement.Q<Button>("SettingsButton");
		_controlsButton = _settingsUI.rootVisualElement.Q<Button>("ControlsButton");
		_exitMenuButton = _settingsUI.rootVisualElement.Q<Button>("ExitMenuButton");
		_exitButton = _settingsUI.rootVisualElement.Q<Button>("ExitButton");
		_backButton = _settingsUI.rootVisualElement.Q<Button>("BackButton");


		_continueButton.RegisterCallback<ClickEvent>(CloseSettings);
		_exitMenuButton.RegisterCallback<ClickEvent>(ToMainMenu);
		_exitButton.RegisterCallback<ClickEvent>(QuitGame);
		_controlsButton.RegisterCallback<ClickEvent>(OpenControlsPanel);
		_backButton.RegisterCallback<ClickEvent>(CloseControlsPanel);

		_mainElement.style.display = DisplayStyle.None;
	}

	public void OpenSettings()
	{
		if (Time.timeScale == 0)
		{
			CloseSettings();
			return;
		}

		_settingsUI.sortingOrder = 50;

		Time.timeScale = 0;
		_mainElement.style.display = DisplayStyle.Flex;

		if (UnityEngine.Cursor.visible) _hideCursor = false;

		UnityEngine.Cursor.lockState = CursorLockMode.None;
		UnityEngine.Cursor.visible = true;

		GameEvents.OnSettingsOpen?.Invoke();
	}

	private void CloseSettings()
	{
		_settingsUI.sortingOrder = -5;

		Time.timeScale = 1;
		_mainElement.style.display = DisplayStyle.None;

		if (_hideCursor)
		{
			UnityEngine.Cursor.lockState = CursorLockMode.Locked;
			UnityEngine.Cursor.visible = false;
		}
		else _hideCursor = true;

		GameEvents.OnSettingsClose?.Invoke();
	}

	private void CloseSettings(ClickEvent evt)
	{
		Time.timeScale = 1;
		_mainElement.style.display = DisplayStyle.None;

		UnityEngine.Cursor.lockState = CursorLockMode.Locked;
		UnityEngine.Cursor.visible = false;

		GameEvents.OnSettingsClose?.Invoke();
	}

	private void OpenControlsPanel(ClickEvent evt)
	{
		_contentBackground.style.display = DisplayStyle.None;
		_controlsBackground.style.display = DisplayStyle.Flex;
	}

	private void CloseControlsPanel(ClickEvent evt)
	{
		_contentBackground.style.display = DisplayStyle.Flex;
		_controlsBackground.style.display = DisplayStyle.None;
	}

	private void ToMainMenu(ClickEvent evt)
	{
		SceneManager.LoadScene("MainMenu");
	}

	private void QuitGame(ClickEvent evt)
	{
		Application.Quit();
	}


	private void OnDisable()
	{
		_continueButton.UnregisterCallback<ClickEvent>(CloseSettings);
		_exitMenuButton.UnregisterCallback<ClickEvent>(ToMainMenu);
		_controlsButton.RegisterCallback<ClickEvent>(OpenControlsPanel);
		_backButton.RegisterCallback<ClickEvent>(CloseControlsPanel);
		_exitButton.RegisterCallback<ClickEvent>(QuitGame);
	}
}