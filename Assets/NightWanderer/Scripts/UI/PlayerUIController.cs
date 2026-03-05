using UnityEngine;
using UnityEngine.InputSystem;

//Отвечает за управление всем UI
public class PlayerUIController : MonoBehaviour
{
	[SerializeField] private InventoryButton Inventory;
	[SerializeField] private BaseUI Base;

	public void OnBase() => Base.OpenCloseBaseUI();

	private void Update()
	{
		if (Keyboard.current.tabKey.wasPressedThisFrame && !Base.IsOpen) Inventory.OpenCloseInventory();
	}
}
