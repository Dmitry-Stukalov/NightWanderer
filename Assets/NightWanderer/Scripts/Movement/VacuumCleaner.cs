using UnityEngine;
using UnityEngine.VFX;

public class VacuumCleaner : MonoBehaviour
{
	private ResourceLibrary _resourceLibrary;
	private GameObject _cleaner;
	private Vector3 _halfVectorCleaner;
	private GameObject _ship;
	private Timer _cleanerOnPause;
	private int _sandCounts = 0;

	public void Initializing(ResourceLibrary resourceLibrary, GameObject ship, GameObject cleaner, Vector3 halfVectorCleaner)
	{
		_resourceLibrary = resourceLibrary;

		_ship = ship;

		_cleaner = cleaner;

		_halfVectorCleaner = halfVectorCleaner;

		_cleanerOnPause = new Timer(0.2f);
		_cleanerOnPause.OnTimerEnd += SandCollection;
		_cleanerOnPause.SetPause();
	}

	public void CleanerOn() => _cleanerOnPause.Continue();

	public void CleanerOff()

	{
		_cleanerOnPause.ResetTimer(true);
		SandIsntCollection();
	}

	private void SandCollection()
	{
		GameObject resource;

		foreach (var collider in Physics.OverlapBox(_cleaner.transform.position, _halfVectorCleaner, Quaternion.identity))
		{
			if (collider.CompareTag("Sand"))
			{
				for (int i = 0; i < 5; i++)
				{
					if (_sandCounts % 10 == 0 && _sandCounts != 100)
					{
						resource = _resourceLibrary.GetResource(1);
					}
					else if (_sandCounts == 10)
					{
						resource = _resourceLibrary.GetResource(2);
						_sandCounts = 0;
					}
					else resource = _resourceLibrary.GetResource(0);

					resource.GetComponent<ResourceOnLand>().SetResourceCount(1);
					resource.transform.position = new Vector3(Random.Range(_cleaner.transform.position.x - _halfVectorCleaner.x / 2, _cleaner.transform.position.x + _halfVectorCleaner.x / 2), _cleaner.transform.position.y - _halfVectorCleaner.y / 2, Random.Range(_cleaner.transform.position.z - _halfVectorCleaner.z / 2, _cleaner.transform.position.z + _halfVectorCleaner.z / 2));
					_sandCounts++;
				}
			}

			if (collider.CompareTag("ResourceOnLand"))
			{
				collider.GetComponent<ResourceOnLand>().ChangeTarget(_ship);
				collider.GetComponent<ResourceOnLand>().Collected();
			}
		}

		_cleanerOnPause.ResetTimer(false);
	}

	private void SandIsntCollection()
	{
		foreach (var collider in Physics.OverlapBox(_cleaner.transform.position, _halfVectorCleaner, Quaternion.identity))
		{
			if (collider.CompareTag("ResourceOnLand"))
			{
				collider.GetComponent<ResourceOnLand>().IsntCollected();
			}
		}
	}

	private void FixedUpdate()
	{
		_cleanerOnPause?.Tick(Time.fixedDeltaTime);
	}
}