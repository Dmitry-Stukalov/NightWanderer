using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
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
	private Stack<Action> _panelsStack = new Stack<Action>();

	public void Initializing()
	{
		SaveAndLoad.CheckSaveFile();

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

		_continueButton.RegisterCallback<ClickEvent>(ContinueGame);
		_newGameButton.RegisterCallback<ClickEvent>(OpenConfirmationNewGame);
		_cancelNewGameButton.RegisterCallback<ClickEvent>(CloseConfirmationNewGame);
		_confirmNewGameButton.RegisterCallback<ClickEvent>(StartGame);

		_controlsButton.RegisterCallback<ClickEvent>(OpenControlsPanel);
		_backButton.RegisterCallback<ClickEvent>(CloseControlsPanel);

		if (SaveAndLoad.IsSaveFileExist) ActiveContinueButton();

		GameEvents.OnMainMenuIn?.Invoke();
	}

	private void ActiveContinueButton()
	{
		_continueButton.RemoveFromClassList("UnactiveBackground");
		_continueButton.RemoveFromClassList("StandartUnactiveLabel");
		_continueButton.RemoveFromClassList("BorderAll");

		_continueButton.AddToClassList("Button");
		_continueButton.AddToClassList("GlowButton");
		_continueButton.AddToClassList("StandartLabel");
		_continueButton.pickingMode = PickingMode.Position;
	}

	private void ContinueGame(ClickEvent evt)
	{
		SaveAndLoad.IsLoadGame = true;

		GameEvents.OnMainMenuOut?.Invoke();

		SceneManager.LoadScene("MainScene");
	}

	private void OpenConfirmationNewGame(ClickEvent evt)
	{
		_confirmExitBackground.style.display = DisplayStyle.None;
		_confirmNewGameBackground.style.display = DisplayStyle.Flex;

		DOTween.Kill(_cancelExitBackgroundButton);
		_cancelExitBackgroundButton.style.display = DisplayStyle.Flex;
		DOTween.To(() => _cancelExitBackgroundButton.resolvedStyle.opacity, x => _cancelExitBackgroundButton.style.opacity = x, 1, 1f);

		_panelsStack.Push(CloseConfirmationNewGame);
	}

	private void CloseConfirmationNewGame()
	{
		DOTween.Kill(_cancelExitBackgroundButton);
		_cancelExitBackgroundButton.style.display = DisplayStyle.None;
		DOTween.To(() => _cancelExitBackgroundButton.resolvedStyle.opacity, x => _cancelExitBackgroundButton.style.opacity = x, 0, 1f);
	}

	private void CloseConfirmationNewGame(ClickEvent evt)
	{
		DOTween.Kill(_cancelExitBackgroundButton);
		_cancelExitBackgroundButton.style.display = DisplayStyle.None;
		DOTween.To(() => _cancelExitBackgroundButton.resolvedStyle.opacity, x => _cancelExitBackgroundButton.style.opacity = x, 0, 1f);

		_panelsStack.Pop();
	}

	private void OpenControlsPanel(ClickEvent evt)
	{
		_menuBackground.style.display = DisplayStyle.None;
		_controlsBackground.style.display = DisplayStyle.Flex;

		_panelsStack.Push(CloseControlsPanel);
	}

	private void CloseControlsPanel()
	{
		_menuBackground.style.display = DisplayStyle.Flex;
		_controlsBackground.style.display = DisplayStyle.None;
	}

	private void CloseControlsPanel(ClickEvent evt)
	{
		_menuBackground.style.display = DisplayStyle.Flex;
		_controlsBackground.style.display = DisplayStyle.None;

		_panelsStack.Pop();
	}

	private void OpenConfirmationExit(ClickEvent evt)
	{
		_confirmNewGameBackground.style.display = DisplayStyle.None;
		_confirmExitBackground.style.display = DisplayStyle.Flex;

		DOTween.Kill(_cancelExitBackgroundButton);
		_cancelExitBackgroundButton.style.display = DisplayStyle.Flex;
		DOTween.To(() => _cancelExitBackgroundButton.resolvedStyle.opacity, x => _cancelExitBackgroundButton.style.opacity = x, 1, 1f);

		_panelsStack.Push(CloseConfirmationExit);
	}

	private void CloseConfirmationExit()
	{
		DOTween.Kill(_cancelExitBackgroundButton);
		_cancelExitBackgroundButton.style.display = DisplayStyle.None;
		DOTween.To(() => _cancelExitBackgroundButton.resolvedStyle.opacity, x => _cancelExitBackgroundButton.style.opacity = x, 0, 1f);
	}

	private void CloseConfirmationExit(ClickEvent evt)
	{
		DOTween.Kill(_cancelExitBackgroundButton);
		_cancelExitBackgroundButton.style.display = DisplayStyle.None;
		DOTween.To(() => _cancelExitBackgroundButton.resolvedStyle.opacity, x => _cancelExitBackgroundButton.style.opacity = x, 0, 1f);

		_panelsStack.Pop();
	}


	private void QuitGame(ClickEvent evt)
	{
		GameEvents.OnSave?.Invoke();
		StartCoroutine(QuiGamePause());
	}

	private IEnumerator QuiGamePause()
	{
		yield return new WaitForSecondsRealtime(1.5f);

		Application.Quit();
	}

	private void StartGame(ClickEvent evt)
	{
		SaveAndLoad.IsLoadGame = false;
		GameEvents.OnMainMenuOut?.Invoke();
		SceneManager.LoadScene("MainScene");
	}

	private void Update()
	{
		if (Keyboard.current.escapeKey.wasPressedThisFrame && _panelsStack.Count > 0)
		{
			Action previousAction = _panelsStack.Pop();
			previousAction.Invoke();
		}
	}

	private void OnDisable()
	{
		_exitButton.UnregisterCallback<ClickEvent>(OpenConfirmationExit);
		_confirmExitButton.UnregisterCallback<ClickEvent>(QuitGame);
		_cancelExitButton.UnregisterCallback<ClickEvent>(CloseConfirmationExit);

		_continueButton.UnregisterCallback<ClickEvent>(ContinueGame);
		_newGameButton.UnregisterCallback<ClickEvent>(OpenConfirmationNewGame);
		_cancelNewGameButton.UnregisterCallback<ClickEvent>(CloseConfirmationNewGame);
		_confirmNewGameButton.UnregisterCallback<ClickEvent>(StartGame);

		_controlsButton.UnregisterCallback<ClickEvent>(OpenControlsPanel);
		_backButton.UnregisterCallback<ClickEvent>(CloseControlsPanel);
	}
}
