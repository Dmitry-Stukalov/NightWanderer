using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI.MessageBox;

//UI инвентаря
public class InventoryButton : MonoBehaviour
{
	[SerializeField] private UIDocument UI;
	private VisualElement UIToClose2;
	private VisualElement UIToOpen2;

	public bool IsOpen { get; private set; } = false;

	public void Initializing()
	{
		UIToClose2 = UI.rootVisualElement.Q<VisualElement>("WeatherPanel");
		UIToOpen2 = UI.rootVisualElement.Q<VisualElement>("InventoryPanel");

		UIToOpen2.style.display = DisplayStyle.None;
		UIToClose2.style.display = DisplayStyle.Flex;
	}

	public void OpenCloseInventory()
	{
		if (!IsOpen)
		{
			CloseWeatherPanel();
			OpenInventoryPanel();

			UnityEngine.Cursor.lockState = CursorLockMode.None;
			UnityEngine.Cursor.visible = true;
		}
		else
		{
			OpenWeatherPanel();
			CloseInventoryPanel();

			UnityEngine.Cursor.lockState = CursorLockMode.Locked;
			UnityEngine.Cursor.visible = false;
		}

		IsOpen = !IsOpen;
	}

	public void CloseWeatherPanel() => UIToClose2.style.display = DisplayStyle.None;
	public void OpenWeatherPanel() => UIToClose2.style.display = DisplayStyle.Flex;
	public void CloseInventoryPanel() => UIToOpen2.style.display = DisplayStyle.None;
	public void OpenInventoryPanel() => UIToOpen2.style.display = DisplayStyle.Flex;
}
