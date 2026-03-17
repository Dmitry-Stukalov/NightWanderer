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
	[SerializeField] private List<GameObject> UIToClose;
	[SerializeField] private List<GameObject> UIToOpen;
	private VisualElement UIToClose2;
	private VisualElement UIToOpen2;
	private List<VisualElement> Cells;

	private bool IsOpen = false;

	public void Initializing()
	{
		UIToClose2 = UI.rootVisualElement.Q<VisualElement>("WeatherPanel");
		UIToOpen2 = UI.rootVisualElement.Q<VisualElement>("InventoryPanel");

		Cells = UI.rootVisualElement.Query<VisualElement>("CellResource").ToList();

		UIToOpen2.style.display = DisplayStyle.None;
		UIToClose2.style.display = DisplayStyle.Flex;
	}

	public void OpenCloseInventory()
	{
		if (!IsOpen)
		{
			UIToClose2.style.display = DisplayStyle.None;
			UIToOpen2.style.display = DisplayStyle.Flex;

			UnityEngine.Cursor.lockState = CursorLockMode.None;
			UnityEngine.Cursor.visible = true;
		}
		else
		{
			UIToClose2.style.display = DisplayStyle.Flex;
			UIToOpen2.style.display = DisplayStyle.None;

			UnityEngine.Cursor.lockState = CursorLockMode.Locked;
			UnityEngine.Cursor.visible = false;
		}

		IsOpen = !IsOpen;
	}
}
