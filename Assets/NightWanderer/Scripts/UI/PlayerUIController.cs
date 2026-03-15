using UnityEngine;
using UnityEngine.InputSystem;

//Отвечает за управление всем UI
public class PlayerUIController : MonoBehaviour
{
	[SerializeField] private InventoryButton Inventory;
	[SerializeField] private BaseUI Base;
	[SerializeField] private BaseUIManager baseUI;

	private void Start()
	{
		baseUI.Initializing();
	}

	public void OnBase() => baseUI.OpenBaseUI();//Base.OpenCloseBaseUI();
	public void OutBase() => baseUI.CloseBaseUI();

	private void Update()
	{
		if (Keyboard.current.tabKey.wasPressedThisFrame && !Base.IsOpen) Inventory.OpenCloseInventory();
	}
}
