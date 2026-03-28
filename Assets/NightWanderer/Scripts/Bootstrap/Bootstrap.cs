using System.Collections;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
	[Header("Environment")]
	[SerializeField] private Sun _sun;
	[SerializeField] private ResourceLibrary _resourceLibrary;
	[SerializeField] private WeatherPanel _weatherPanel;
	[SerializeField] private WeatherManager _weatherManager;

	[Header("Player")]
	[SerializeField] private ShipMovement _shipMovement;
	[SerializeField] private PlayerInventoryBuilder _playerInventoryBuilder;
	[SerializeField] private InventoryButton _inventoryButton;
	[SerializeField] private ImprovementManager _improvementManager;
	[SerializeField] private PlayerUIController _playerUIController;
	[SerializeField] private SearchlightManager _searchlightManager;

	[Header("Base")]
	[SerializeField] private BaseInventory _baseInventory;

	//Инициализация всех объектов, которые находятся на сцене
	private void Start()
	{
		_shipMovement?.Initializing();
		_playerUIController?.Initializing(_shipMovement.GetPlayerFuel(), _shipMovement.GetPlayerDefenseSystem().GetHealth(), _shipMovement.GetPlayerDefenseSystem().GetDefense(), _shipMovement.GetPlayerDefenseSystem().GetFireDefense());
		_searchlightManager.Initializing();
		_resourceLibrary?.Initializing();
		_playerInventoryBuilder?.Initializing();
		_baseInventory?.Initializing();
		_improvementManager?.Initializing(_playerInventoryBuilder.GetPlayerInventory(), _baseInventory.GetBaseInventory());
		_inventoryButton?.Initializing();
		_sun?.Initializing();
		_weatherManager?.Initializing();
		_weatherPanel?.Initializing(_shipMovement.GetPlayerDefenseSystem().GetHealth(), _weatherManager, _sun);

		StartCoroutine(StartPause());
	}

	private IEnumerator StartPause()
	{
		yield return new WaitForSeconds(1);

		_improvementManager.AddImprovement(_shipMovement.GetPlayerFuel(), "Fuel");
		_improvementManager.AddImprovement(_shipMovement.GetPlayerEngines(), "Engines");
	}
}
