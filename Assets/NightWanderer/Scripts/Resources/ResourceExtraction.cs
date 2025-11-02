using UnityEngine;

public class ResourceExtraction : MonoBehaviour
{
	[field: SerializeField] private ShipMovement Ship;
	[field: SerializeField] private float ExtractionTime;
	private Timer ExtractionTimer;


	private void Start()
	{
		ExtractionTimer = new Timer(ExtractionTime);
		ExtractionTimer.OnTimerEnd += ExtractionEnd;
	}

	private void ExtractionEnd()
	{
		Debug.Log("Ресурс добыт");
		ExtractionTimer.ResetTimer(false);
		Ship.IsOnResource = false;
	}

	private void Update()
	{
		if (Ship.IsShipReady) ExtractionTimer.Tick(Time.deltaTime);
	}
}
