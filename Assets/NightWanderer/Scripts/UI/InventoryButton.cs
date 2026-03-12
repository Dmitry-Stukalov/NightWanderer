using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

//UI инвентаря
public class InventoryButton : MonoBehaviour
{
	[SerializeField] private UIDocument UI;
	[SerializeField] private List<GameObject> UIToClose;
	[SerializeField] private List<GameObject> UIToOpen;
	private VisualElement UIToClose2;
	private VisualElement UIToOpen2;
	private UnityEngine.UI.Button _InventoryButton;
	private bool IsOpen = false;

	public void Initializing()
	{
		_InventoryButton = GetComponent<UnityEngine.UI.Button>();

		UIToClose2 = UI.rootVisualElement.Q<VisualElement>("WeatherPanel");
		UIToOpen2 = UI.rootVisualElement.Q<VisualElement>("InventoryPanel");
	}

	public void OpenCloseInventory()
	{
		if (!IsOpen)
		{
			//foreach (GameObject ui in UIToClose) ui.SetActive(false);
			//foreach (GameObject ui in UIToOpen) ui.SetActive(true);
			UIToClose2.visible = false;
			UIToOpen2.visible = true;
			UnityEngine.Cursor.lockState = CursorLockMode.None;
			UnityEngine.Cursor.visible = true;
		}
		else
		{
			//foreach (GameObject ui in UIToClose) ui.SetActive(true);
			//foreach (GameObject ui in UIToOpen) ui.SetActive(false);
			UIToClose2.visible = true;
			UIToOpen2.visible = false;
			UnityEngine.Cursor.lockState = CursorLockMode.Locked;
			UnityEngine.Cursor.visible = false;
		}

		IsOpen = !IsOpen;
	}
}
