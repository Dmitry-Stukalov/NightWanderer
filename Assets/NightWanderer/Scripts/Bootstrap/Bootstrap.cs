using UnityEngine;

public class Bootstrap : MonoBehaviour
{
	[field: SerializeField] private Sun _Sun;
	[field: SerializeField] private ShipMovement _ShipMovement;
	[field: SerializeField] private ResourceLibrary _ResourceLibrary;
	[field: SerializeField] private PlayerInventory _PlayerInventory;

	//Инициализация всех объектов, которые находятся на сцене
	private void Start()
	{
		_Sun.Initializing();
		_ShipMovement.Initializing();
		_ResourceLibrary.Initializing();
		_PlayerInventory.Initializing();
	}
}
