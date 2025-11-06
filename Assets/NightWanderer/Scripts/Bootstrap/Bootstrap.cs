using UnityEngine;

public class Bootstrap : MonoBehaviour
{
	[field: SerializeField] private Sun _Sun;
	[field: SerializeField] private ShipMovement _ShipMovement;
	[field: SerializeField] private ResourceLibrary _ResourceLibrary;
	//[field: SerializeField] private ResourceSource[] _ResourceSources;

	private void Start()
	{
		_Sun.Initializing();
		_ShipMovement.Initializing();
		_ResourceLibrary.Initializing();

		//for (int i = 0;  i < _ResourceSources.Length; i++) _ResourceSources[i].Initializing();
	}
}
