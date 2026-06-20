using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class SettingsUIManager : UIManager
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
	private Stack<Action> _panelsStack = new Stack<Action>();
	public bool IsUIOpen { get; private set; } = false;

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


		_continueButton.RegisterCallback<ClickEvent>(CloseUI);
		_exitMenuButton.RegisterCallback<ClickEvent>(ToMainMenu);
		_exitButton.RegisterCallback<ClickEvent>(QuitGame);
		_controlsButton.RegisterCallback<ClickEvent>(OpenControlsPanel);
		_backButton.RegisterCallback<ClickEvent>(CloseControlsPanel);

		_mainElement.style.display = DisplayStyle.None;
	}

	public override void OpenUI()
	{
		_settingsUI.sortingOrder = 50;

		Time.timeScale = 0;
		_mainElement.style.display = DisplayStyle.Flex;

		if (UnityEngine.Cursor.visible) _hideCursor = false;

		UnityEngine.Cursor.lockState = CursorLockMode.None;
		UnityEngine.Cursor.visible = true;

		_panelsStack.Push(CloseUI);

		IsUIOpen = true;

		GameEvents.OnSettingsOpen?.Invoke();
	}

	public override void CloseUI()
	{
		_settingsUI.sortingOrder = -5;

		Time.timeScale = 1;
		_mainElement.style.display = DisplayStyle.None;

		UnityEngine.Cursor.lockState = CursorLockMode.Locked;
		UnityEngine.Cursor.visible = false;

		IsUIOpen = false;

		GameEvents.OnSettingsClose?.Invoke();
	}

	private void CloseUI(ClickEvent evt) => CloseUI();

	private void OpenControlsPanel(ClickEvent evt)
	{
		_contentBackground.style.display = DisplayStyle.None;
		_controlsBackground.style.display = DisplayStyle.Flex;

		_panelsStack.Push(CloseControlsPanel);
	}

	private void CloseControlsPanel()
	{
		_contentBackground.style.display = DisplayStyle.Flex;
		_controlsBackground.style.display = DisplayStyle.None;
	}

	private void CloseControlsPanel(ClickEvent evt)
	{
		_contentBackground.style.display = DisplayStyle.Flex;
		_controlsBackground.style.display = DisplayStyle.None;

		_panelsStack.Pop();
	}

	private void ToMainMenu(ClickEvent evt)
	{
		GameEvents.OnSave?.Invoke();

		//SceneManager.LoadScene("MainMenu");

		StartCoroutine(ToMainMenuPause());
	}

	private IEnumerator ToMainMenuPause()
	{
		yield return new WaitForSecondsRealtime(3);

		Time.timeScale = 1;

		SceneManager.LoadScene("MainMenu");

		Debug.Log("Главное меню загружено");
	}

	private void QuitGame(ClickEvent evt)
	{
		GameEvents.OnSave?.Invoke();
		
		StartCoroutine(QuitGamePause());
		//Application.Quit();
	}
	private IEnumerator QuitGamePause()
	{
		yield return new WaitForSecondsRealtime(3);

		Application.Quit();
	}

	private void Update()
	{
		if (Keyboard.current.escapeKey.wasPressedThisFrame && _panelsStack.Count > 0)
		{
			Action previousAction = _panelsStack.Pop();
			previousAction.Invoke();
		}
		else if (Keyboard.current.escapeKey.wasPressedThisFrame) OpenUI();
	}

	private void OnDisable()
	{
		_continueButton.UnregisterCallback<ClickEvent>(CloseUI);
		_exitMenuButton.UnregisterCallback<ClickEvent>(ToMainMenu);
		_controlsButton.RegisterCallback<ClickEvent>(OpenControlsPanel);
		_backButton.RegisterCallback<ClickEvent>(CloseControlsPanel);
		_exitButton.RegisterCallback<ClickEvent>(QuitGame);
	}
}