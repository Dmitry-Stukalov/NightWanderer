using UnityEngine;
using UnityEngine.InputSystem;

//Отвечает за управление всем UI
public class PlayerUIController : MonoBehaviour
{
	[SerializeField] private InventoryButton Inventory;
	[SerializeField] private BaseUIManager _baseUI;

	private void Start()
	{
		_baseUI.Initializing();
	}

	public void OnBase() => _baseUI.OpenBaseUI();//Base.OpenCloseBaseUI();
	public void OutBase() => _baseUI.CloseBaseUI();

	private void Update()
	{
		if (Keyboard.current.tabKey.wasPressedThisFrame && !_baseUI.OnBase) Inventory.OpenCloseInventory();
	}
}
