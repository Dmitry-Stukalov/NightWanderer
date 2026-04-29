using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class SettingsManager : MonoBehaviour
{
	[SerializeField] private UIDocument _settingsUI;
	private VisualElement _mainElement;
	private Button _continueButton;
	private Button _settingsButton;
	private Button _exitButton;
	private bool _hideCursor = true;

	public void Initializing()
	{
		_mainElement = _settingsUI.rootVisualElement.Q<VisualElement>("MainElement");
		_continueButton = _settingsUI.rootVisualElement.Q<Button>("ContinueButton");
		_settingsButton = _settingsUI.rootVisualElement.Q<Button>("SettingsButton");
		_exitButton = _settingsUI.rootVisualElement.Q<Button>("ExitButton");


		_continueButton.RegisterCallback<ClickEvent>(CloseSettings);
		_exitButton.RegisterCallback<ClickEvent>(ToMainMenu);

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
	}

	private void CloseSettings(ClickEvent evt)
	{
		Time.timeScale = 1;
		_mainElement.style.display = DisplayStyle.None;

		UnityEngine.Cursor.lockState = CursorLockMode.Locked;
		UnityEngine.Cursor.visible = false;
	}

	private void ToMainMenu(ClickEvent evt)
	{
		SceneManager.LoadScene("MainMenu");
	}

	private void OnDisable()
	{
		_continueButton.UnregisterCallback<ClickEvent>(CloseSettings);
		_exitButton.UnregisterCallback<ClickEvent>(ToMainMenu);
	}
}