using UnityEngine;

public class UIBootstrap : MonoBehaviour
{
	//[field: SerializeField] private WeatherScaler _WeatherScaler;
	[field: SerializeField] private InventoryBackgroundScaler _InventoryBackgroundScaler;
	[field: SerializeField] private InventoryButton _InventoryButton;
	//[field: SerializeField] private UIInventory _UIInventory;

	//Инициализация всех объектов, которые находятся в UI
	private void Start()
	{
		//_WeatherScaler.Initializing();
		_InventoryBackgroundScaler.Initializing();
		_InventoryButton.Initializing();
		//_UIInventory.Initializing();
	}
}