using UnityEngine;

public class UIBootstrap : MonoBehaviour
{
	//[field: SerializeField] private WeatherScaler _WeatherScaler;
	[SerializeField] private InventoryBackgroundScaler _inventoryBackgroundScaler;
	[SerializeField] private InventoryButton _inventoryButton;
	//[field: SerializeField] private UIInventory _UIInventory;

	//Инициализация всех объектов, которые находятся в UI
	private void Start()
	{
		//_WeatherScaler.Initializing();
		_inventoryBackgroundScaler.Initializing();
		_inventoryButton.Initializing();
		//_UIInventory.Initializing();
	}
}