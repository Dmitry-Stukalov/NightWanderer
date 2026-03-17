using UnityEngine;

public class Bootstrap : MonoBehaviour
{
	[SerializeField] private UIBootstrap _uiBootstrap;
	[SerializeField] private Sun _sun;
	[SerializeField] private ShipMovement _shipMovement;
	[SerializeField] private ResourceLibrary _resourceLibrary;
	//[SerializeField] private PlayerInventory _playerInventory;
	[SerializeField] private PlayerInventoryBuilder _playerInventoryBuilder;

	//Инициализация всех объектов, которые находятся на сцене
	private void Start()
	{
		_shipMovement?.Initializing();
		_sun?.Initializing();
		_resourceLibrary?.Initializing();
		//_playerInventory?.Initializing();
		_playerInventoryBuilder?.Initializing();
		_uiBootstrap.Initializing();
	}
}
