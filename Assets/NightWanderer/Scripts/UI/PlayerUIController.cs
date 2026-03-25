using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

//Отвечает за управление всем UI
public class PlayerUIController : MonoBehaviour
{
	[SerializeField] private UIDocument PlayerUI;
	[SerializeField] private InventoryButton Inventory;
	[SerializeField] private BaseUIManager _baseUI;

	public void Initializing(Fuel fuel, HealthFireDefense health, HealthFireDefense defense, HealthFireDefense fireDefense)
	{
		var fuelItemBackground = PlayerUI.rootVisualElement.Q<VisualElement>("FuelBackground");
		fuelItemBackground.dataSource = new FuelRecovery(fuel, PlayerUI.rootVisualElement.Q<VisualElement>("FuelForeground"));

		var healthItemBackground = PlayerUI.rootVisualElement.Q<VisualElement>("HealthBackground");
		healthItemBackground.dataSource = new HealthFireDefenseRecovery(health, PlayerUI.rootVisualElement.Q<VisualElement>("HealthForeground"));

		var defenseItemBackground = PlayerUI.rootVisualElement.Q<VisualElement>("DefenseBackground");
		defenseItemBackground.dataSource = new HealthFireDefenseRecovery(defense, PlayerUI.rootVisualElement.Q<VisualElement>("DefenseForeground"));

		var fireDefenseItemBackground = PlayerUI.rootVisualElement.Q<VisualElement>("FireDefenseBackground");
		fireDefenseItemBackground.dataSource = new HealthFireDefenseRecovery(fireDefense, PlayerUI.rootVisualElement.Q<VisualElement>("FireDefenseForeground"));

		_baseUI.Initializing();
	}

	public void OnBase()
	{
		_baseUI.OpenBaseUI();
		Inventory.CloseWeatherPanel();
		Inventory.CloseInventoryPanel();
	}
	public void OutBase()
	{
		_baseUI.CloseBaseUI();
		Inventory.OpenWeatherPanel();
	}

	private void Update()
	{
		if (Keyboard.current.tabKey.wasPressedThisFrame && !_baseUI.OnBase) Inventory.OpenCloseInventory();
	}
}
