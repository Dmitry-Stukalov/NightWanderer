using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrap : MonoBehaviour
{
	[Header("Environment")]
	[SerializeField] private Sun _sun;
	[SerializeField] private ResourceLibrary _resourceLibrary;
	[SerializeField] private WeatherManager _weatherManager;

	[SerializeField] private LockRotation _lockRotation;
	[SerializeField] private GiveStartResources _giveStartResources;

	//Инициализация всех объектов, которые находятся на сцене
	private void Awake()
	{
		_resourceLibrary?.Initializing();
		_sun?.Initializing();
		_weatherManager?.Initializing();
		_lockRotation?.Initializing();
		_giveStartResources?.Initializing();
	}
}
