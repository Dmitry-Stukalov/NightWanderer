using UnityEngine;

public class MainMenuBootstrap : MonoBehaviour
{
	[SerializeField] private MainMenuManager _mainMenuManager;
	[SerializeField] private MenuSoundsManager _soundsManager;

	private void Start()
	{
		_soundsManager?.Initializing();
		_mainMenuManager?.Initializing();
	}
}
