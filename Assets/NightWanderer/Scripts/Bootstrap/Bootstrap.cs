using UnityEngine;

public class Bootstrap : MonoBehaviour
{
	[SerializeField] private Sun _sun;
	[SerializeField] private ShipMovement _shipMovement;
	[SerializeField] private ResourceLibrary _resourceLibrary;
	[SerializeField] private PlayerInventory _playerInventory;
	[SerializeField] private PlayerInventoryBuilder _playerInventorybuilder;

	//Инициализация всех объектов, которые находятся на сцене
	private void Start()
	{
		_shipMovement?.Initializing();
		_sun?.Initializing();
		_resourceLibrary?.Initializing();
		_playerInventory?.Initializing();
		_playerInventorybuilder?.Initializing();
	}
}
