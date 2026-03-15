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
	private UnityEngine.UI.Button _InventoryButton;
	private bool IsOpen = false;

	public void Initializing()
	{
		_InventoryButton = GetComponent<UnityEngine.UI.Button>();

		UIToClose2 = UI.rootVisualElement.Q<VisualElement>("WeatherPanel");
		UIToOpen2 = UI.rootVisualElement.Q<VisualElement>("InventoryPanel");

		Cells = UI.rootVisualElement.Query<VisualElement>("CellResource").ToList();

		foreach (var cell in Cells)
		{
			cell.usageHints = UsageHints.DynamicTransform;

			cell.AddManipulator(new DraggableManipulator(cell));
		}
	}

	public void OpenCloseInventory()
	{
		if (!IsOpen)
		{
			//foreach (GameObject ui in UIToClose) ui.SetActive(false);
			//foreach (GameObject ui in UIToOpen) ui.SetActive(true);
			UIToClose2.style.display = DisplayStyle.None;
			UIToOpen2.style.display = DisplayStyle.Flex;

			foreach (var cell in Cells) cell.style.display = DisplayStyle.Flex;

			UnityEngine.Cursor.lockState = CursorLockMode.None;
			UnityEngine.Cursor.visible = true;
		}
		else
		{
			//foreach (GameObject ui in UIToClose) ui.SetActive(true);
			//foreach (GameObject ui in UIToOpen) ui.SetActive(false);
			UIToClose2.style.display = DisplayStyle.Flex;
			UIToOpen2.style.display = DisplayStyle.None;

			foreach (var cell in Cells) cell.style.display = DisplayStyle.None;

			UnityEngine.Cursor.lockState = CursorLockMode.Locked;
			UnityEngine.Cursor.visible = false;
		}

		IsOpen = !IsOpen;
	}
}
