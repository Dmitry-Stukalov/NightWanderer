using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenuManager : MonoBehaviour
{
	private UIDocument _menuUI;
	private VisualElement _confirmExitBackground;
	private VisualElement _confirmNewGameBackground;
	private VisualElement _controlsBackground;
	private VisualElement _menuBackground;
	private Button _continueButton;
	private Button _newGameButton;
	private Button _settingsButton;
	private Button _controlsButton;
	private Button _backButton;
	private Button _exitButton;
	private Button _confirmExitButton;
	private Button _cancelExitButton;
	private Button _confirmNewGameButton;
	private Button _cancelNewGameButton;
	private Button _cancelExitBackgroundButton;

	public void Initializing()
	{
		_menuUI = GetComponent<UIDocument>();

		_confirmExitBackground = _menuUI.rootVisualElement.Q<VisualElement>("ConfirmationExit");
		_confirmNewGameBackground = _menuUI.rootVisualElement.Q<VisualElement>("ConfirmationNewGame");
		_controlsBackground = _menuUI.rootVisualElement.Q<VisualElement>("ControlsBackground");
		_menuBackground = _menuUI.rootVisualElement.Q<VisualElement>("MenuBackground");

		_cancelExitBackgroundButton = _menuUI.rootVisualElement.Q<Button>("CancelExitBackground");

		_continueButton = _menuUI.rootVisualElement.Q<Button>("ContinueButton");
		_newGameButton = _menuUI.rootVisualElement.Q<Button>("NewGameButton");
		_settingsButton = _menuUI.rootVisualElement.Q<Button>("SettingsButton");
		_controlsButton = _menuUI.rootVisualElement.Q<Button>("ControlsButton");
		_backButton = _menuUI.rootVisualElement.Q<Button>("BackButton");
		_exitButton = _menuUI.rootVisualElement.Q<Button>("ExitButton");
		_confirmExitButton = _menuUI.rootVisualElement.Q<Button>("ConfirmExit");
		_cancelExitButton = _menuUI.rootVisualElement.Q<Button>("CancelExit");
		_confirmNewGameButton = _menuUI.rootVisualElement.Q<Button>("ConfirmNewGame");
		_cancelNewGameButton = _menuUI.rootVisualElement.Q<Button>("CancelNewGame");

		_exitButton.RegisterCallback<ClickEvent>(OpenConfirmationExit);
		_confirmExitButton.RegisterCallback<ClickEvent>(QuitGame);
		_cancelExitButton.RegisterCallback<ClickEvent>(CloseConfirmationExit);
		_cancelExitBackgroundButton.RegisterCallback<ClickEvent>(CloseConfirmationExit);

		_newGameButton.RegisterCallback<ClickEvent>(OpenConfirmationNewGame);
		_cancelNewGameButton.RegisterCallback<ClickEvent>(CloseConfirmationNewGame);
		_confirmNewGameButton.RegisterCallback<ClickEvent>(StartGame);

		_controlsButton.RegisterCallback<ClickEvent>(OpenControlsPanel);
		_backButton.RegisterCallback<ClickEvent>(CloseControlsPanel);

		GameEvents.OnMainMenuIn?.Invoke();
	}


	private void OpenConfirmationNewGame(ClickEvent evt)
	{
		_confirmExitBackground.style.display = DisplayStyle.None;
		_confirmNewGameBackground.style.display = DisplayStyle.Flex;

		DOTween.Kill(_cancelExitBackgroundButton);
		//DOTween.To(() => -100, x => _cancelExitBackgroundButton.style.bottom = Length.Percent(x), 0, 1f);
		_cancelExitBackgroundButton.style.display = DisplayStyle.Flex;
		DOTween.To(() => _cancelExitBackgroundButton.resolvedStyle.opacity, x => _cancelExitBackgroundButton.style.opacity = x, 1, 1f);
	}

	private void CloseConfirmationNewGame(ClickEvent evt)
	{
		DOTween.Kill(_cancelExitBackgroundButton);
		//DOTween.To(() => 0, x => _cancelExitBackgroundButton.style.bottom = Length.Percent(x), -100, 1f);
		_cancelExitBackgroundButton.style.display = DisplayStyle.None;
		DOTween.To(() => _cancelExitBackgroundButton.resolvedStyle.opacity, x => _cancelExitBackgroundButton.style.opacity = x, 0, 1f);
	}

	private void OpenControlsPanel(ClickEvent evt)
	{
		_menuBackground.style.display = DisplayStyle.None;
		_controlsBackground.style.display = DisplayStyle.Flex;
	}

	private void CloseControlsPanel(ClickEvent evt)
	{
		_menuBackground.style.display = DisplayStyle.Flex;
		_controlsBackground.style.display = DisplayStyle.None;
	}

	private void OpenConfirmationExit(ClickEvent evt)
	{
		_confirmNewGameBackground.style.display = DisplayStyle.None;
		_confirmExitBackground.style.display = DisplayStyle.Flex;

		DOTween.Kill(_cancelExitBackgroundButton);
		//DOTween.To(() => -100, x => _cancelExitBackgroundButton.style.bottom = Length.Percent(x), 0, 1f);
		_cancelExitBackgroundButton.style.display = DisplayStyle.Flex;
		DOTween.To(() => _cancelExitBackgroundButton.resolvedStyle.opacity, x => _cancelExitBackgroundButton.style.opacity = x, 1, 1f);
	}

	private void CloseConfirmationExit(ClickEvent evt)
	{
		DOTween.Kill(_cancelExitBackgroundButton);
		//DOTween.To(() => 0, x => _cancelExitBackgroundButton.style.bottom = Length.Percent(x), -100, 1f);
		_cancelExitBackgroundButton.style.display = DisplayStyle.None;
		DOTween.To(() => _cancelExitBackgroundButton.resolvedStyle.opacity, x => _cancelExitBackgroundButton.style.opacity = x, 0, 1f);
	}


	private void QuitGame(ClickEvent evt)
	{
		Application.Quit();
	}

	private void StartGame(ClickEvent evt)
	{
		GameEvents.OnMainMenuOut?.Invoke();
		SceneManager.LoadScene("MainScene");
	}

	private void OnDisable()
	{
		_exitButton.UnregisterCallback<ClickEvent>(OpenConfirmationExit);
		_confirmExitButton.UnregisterCallback<ClickEvent>(QuitGame);
		_cancelExitButton.UnregisterCallback<ClickEvent>(CloseConfirmationExit);
		_cancelExitBackgroundButton.UnregisterCallback<ClickEvent>(CloseConfirmationExit);

		_newGameButton.UnregisterCallback<ClickEvent>(OpenConfirmationNewGame);
		_cancelNewGameButton.UnregisterCallback<ClickEvent>(CloseConfirmationNewGame);
		_confirmNewGameButton.UnregisterCallback<ClickEvent>(StartGame);

		_controlsButton.UnregisterCallback<ClickEvent>(OpenControlsPanel);
		_backButton.UnregisterCallback<ClickEvent>(CloseControlsPanel);
	}
}
