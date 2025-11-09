using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InventoryButton : MonoBehaviour
{
	[field: SerializeField] private List<GameObject> UIToClose;
	[field: SerializeField] private List<GameObject> UIToOpen;
	private Button _InventoryButton;
	private bool IsOpen = false;

	public void Initializing()
	{
		_InventoryButton = GetComponent<Button>();
	}

	private void OpenCloseInventory()
	{
		if (!IsOpen)
		{
			foreach (GameObject ui in UIToClose) ui.SetActive(false);
			foreach (GameObject ui in UIToOpen) ui.SetActive(true);
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}
		else
		{
			foreach (GameObject ui in UIToClose) ui.SetActive(true);
			foreach (GameObject ui in UIToOpen) ui.SetActive(false);
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}

		IsOpen = !IsOpen;
	}

	private void Update()
	{
		if (Keyboard.current.tabKey.wasPressedThisFrame) OpenCloseInventory();
	}
}
