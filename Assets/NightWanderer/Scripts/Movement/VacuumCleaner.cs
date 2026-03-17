using UnityEngine;
using UnityEngine.VFX;

public class VacuumCleaner : MonoBehaviour
{
	[SerializeField] private ResourceLibrary _resourceLibrary;
	[SerializeField] private float _cleanerOnPauseValue;
	private Vector3 _sandPosition = new Vector3(0, -2, 0);
	private GameObject _cleaner;
	private Vector3 _halfVectorCleaner;
	private GameObject _ship;
	private Timer _cleanerOnPause;
	private int _sandCounts = 0;

	public void Initializing(GameObject ship, GameObject cleaner, Vector3 halfVectorCleaner)
	{
		_ship = ship;

		_cleaner = cleaner;

		_halfVectorCleaner = halfVectorCleaner;

		_cleanerOnPause = new Timer(0.2f);
		_cleanerOnPause.OnTimerEnd += SandCollection;
		_cleanerOnPause.SetPause();
	}

	public void CleanerOn(/*GameObject cleaner, Vector3 halfVectorCleaner*/)
	{
		/*_cleaner = cleaner;
		_halfVectorCleaner = halfVectorCleaner;*/

		_cleanerOnPause.Continue();

		Debug.Log("Пылесос включен");
	}

	public void CleanerOff()
	{
		_cleanerOnPause.ResetTimer(true);
		SandIsntCollection();

		Debug.Log("Пылесос выключен");
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
					if (_sandCounts == 10)
					{
						resource = _resourceLibrary.GetResource(1);
						_sandCounts = 0;
					}
					else resource = _resourceLibrary.GetResource(2);

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