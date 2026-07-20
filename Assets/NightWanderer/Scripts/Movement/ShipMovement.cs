 using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using System.Collections.Generic;
using UnityEditor.Rendering;


//’ранит информацию о состо€ни€х игрока, а также базовые значени€ перемещени€ и поворота камеры
public class ShipMovement : MonoBehaviour
{
	[SerializeField] private DeathManager _deathManager;
	[SerializeField] private Sprite _defenseSprite;
	[SerializeField] private Sprite _damageSprite;

	[Header("UI")]
	[SerializeField] private PlayerUIManager _playerUIManager;
	[SerializeField] private BaseUIManager _baseUIManager;
	[SerializeField] private ResearchUIManager _researchUIManager;
	[SerializeField] private ExtractionUIManager _extractionUIManager;

	[Header("Camera")]
	[SerializeField] private GameObject PlayerCameraRotationObject;

	[Header("VacuumCleaner")]
	[SerializeField] private GameObject VacuumCleanerObject;
	[SerializeField] private VacuumCleaner _vacuumCleaner;

	[Header("Looking")]
	[SerializeField] private Searchlights _searchlights;
	[SerializeField] private SearchlightsPower _searchlightsPower;
	[SerializeField] private float LookSpeed;
	[SerializeField] private int ResourceRotationX;
	[SerializeField] private int ResourceDistanceY;

	[Header("Configs")]
	[SerializeField] private ImprovementConfig _fuelConfig;
	[SerializeField] private ImprovementConfig _miningConfig;
	[SerializeField] private ImprovementConfig _enginesConfig;
	[SerializeField] private ImprovementConfig _healthConfig;
	[SerializeField] private ImprovementConfig _defenseConfig;
	[SerializeField] private ImprovementConfig _fireDefenseConfig;
	[SerializeField] private ImprovementConfig _searchlightConfig;
	[SerializeField] private ImprovementConfig _searchlightPowerConfig;

	private ResourceLibrary _resourceLibrary;

	private DefenseSystem _defenseSystem;
	private Fuel _fuel;
	private MiningEquipment _miningEquipment;
	private JetEngines _engines;
	private InputAction MoveAction;
	private InputAction UpDownMoveAction;
	private InputAction LookAction;

	private Sun _sun;
	public Vector3 ResourceSourcePosition { get; set; }
	public Vector3 BasePosition { get; set; }
	public bool IsCanMiningResource { get; set; } = false;
	public bool IsOnResource { get; set; } = false;
	public bool IsShipReady { get; set; } = false;
	public bool IsCanDocking { get; set; } = false;
	public bool IsCanResearch { get; set; } = false;
	private bool IsDead = false;
	private bool IsGameStart = false;
	private bool IsFirstTimeBase = true;
	private bool IsMapFogOn = false;

	private StateMachineManager StateMachineManager = new StateMachineManager();

	public void Initializing(ImprovementManager improvementManager, InventoryButton inventoryButton)
	{
		StartCoroutine(StartPause());

		GameEvents.OnGameStart += StartGame;

		_searchlights.AddConfig(_searchlightConfig);
		_searchlightsPower.AddConfig(_searchlightPowerConfig);

		_defenseSystem = new DefenseSystem(new HealthFireDefense(_healthConfig), new HealthFireDefense(_defenseConfig), new HealthFireDefense(_fireDefenseConfig), improvementManager, _playerUIManager.GetVisualElement("DamageEffect"), _defenseSprite, _damageSprite);
		_defenseSystem.OnDeath += Death;

		_fuel = new Fuel(_fuelConfig);
		_miningEquipment = new MiningEquipment(_miningConfig, _fuel);
		_engines = new JetEngines(_enginesConfig, _fuel);

		MoveAction = InputSystem.actions.FindAction("Move");
		UpDownMoveAction = InputSystem.actions.FindAction("UpDownMove");
		LookAction = InputSystem.actions.FindAction("Look");

		UnityEngine.Cursor.lockState = CursorLockMode.Locked;
		UnityEngine.Cursor.visible = false;

		PlayerCameraRotationObject.transform.rotation = Quaternion.Euler(0, 0, 0);

		StateMachineManager.AddState(0, new StateMachineIdle(0, StateMachineManager, PlayerCameraRotationObject, gameObject, transform, _playerUIManager, VacuumCleanerObject.transform, _vacuumCleaner, _fuel, _engines, MoveAction, UpDownMoveAction, LookAction, LookSpeed));
		StateMachineManager.AddState(1, new StateMachineWalk(1, StateMachineManager, PlayerCameraRotationObject, gameObject, transform, _playerUIManager, VacuumCleanerObject.transform, _vacuumCleaner, _fuel, _engines, MoveAction, UpDownMoveAction, LookAction, LookSpeed));
		StateMachineManager.AddState(2, new StateMachineRun(2, StateMachineManager, PlayerCameraRotationObject, gameObject, transform, _playerUIManager, VacuumCleanerObject.transform, _vacuumCleaner, _fuel, _engines, MoveAction, UpDownMoveAction, LookAction, LookSpeed));
		StateMachineManager.AddState(3, new StateMachineVoid(3, StateMachineManager, PlayerCameraRotationObject, gameObject, transform, _playerUIManager, VacuumCleanerObject.transform, _vacuumCleaner, _fuel, _engines, MoveAction, UpDownMoveAction, LookAction, LookSpeed));
		StateMachineManager.AddState(10, new StateMachineTransition(10, StateMachineManager, transform, _playerUIManager, PlayerCameraRotationObject.transform));
		StateMachineManager.AddState(15, new StateMachineResearch(15, StateMachineManager, transform, _researchUIManager));
		StateMachineManager.AddState(20, new StateMachineBase(20, StateMachineManager, transform, _baseUIManager));
		StateMachineManager.AddState(50, new StateMachineDeath(50, StateMachineManager, transform, _playerUIManager));

		StateMachineManager.SetState(0);
		StateMachineManager.Inventory = inventoryButton;
		if (GetComponent<Animator>() != null) StateMachineManager._Animator = GetComponent<Animator>();

		_deathManager.OnAlive += Alive;

		GameEvents.OnResourceDrop += DropResource;
		GameEvents.OnTransformLoad += LoadData;
		GameEvents.OnStatsLoad += LoadData;
		GameEvents.OnSave += SaveData;
	}

	public void OpenSceneInitializing()
	{
		_resourceLibrary = GameObject.FindGameObjectWithTag("ResourceLibrary").GetComponent<ResourceLibrary>();
		_sun = FindAnyObjectByType<Sun>();

		_vacuumCleaner.Initializing(_resourceLibrary, gameObject, VacuumCleanerObject, new Vector3(VacuumCleanerObject.transform.localScale.x / 2, VacuumCleanerObject.transform.localScale.y / 2, VacuumCleanerObject.transform.localScale.z / 2));
	}

	private void StartGame() => IsGameStart = true;

	private IEnumerator StartPause()
	{
		yield return new WaitForSeconds(0.5f);

		StateMachineManager.AddState(11, new StateMachineResourceExtraction1(11, StateMachineManager, transform, _extractionUIManager, PlayerCameraRotationObject, _miningEquipment, _fuel, _extractionUIManager.GetMinigameLaser()));
	}

	public DefenseSystem GetPlayerDefenseSystem() => _defenseSystem;
	public Fuel GetPlayerFuel() => _fuel;
	public MiningEquipment GetPlayerMiningEquipment() => _miningEquipment;
	public JetEngines GetPlayerEngines() => _engines;
	public Searchlights GetPlayerSearchlights() => _searchlights;
	public SearchlightsPower GetPlayerSearchlightsPower() => _searchlightsPower;

	private void HitSurface()
	{
		StateMachineManager.HitSurface();

		if (StateMachineManager.GetCurrentState() == 2) _defenseSystem.GetDamage(5);
		else _defenseSystem.GetDamage(2);
	}

	private void Death()
	{
		if (IsDead) return;

		IsDead = true;
		StateMachineManager.IsDead = true;
		_deathManager.StartDeath();
	}

	private void Alive()
	{
		if (!IsDead) return;

		_defenseSystem.Alive();
		_fuel.Refueling(_fuel.GetMaxFuel());
		IsDead = false;
		StateMachineManager.IsDead = false;
	}

	private void DropResource(int id, int count)
	{
		GameObject resource = _resourceLibrary.GetResource(id);
		resource.GetComponent<ResourceOnLand>().SetResourceCount(count);
		resource.transform.position = new Vector3(transform.position.x, transform.position.y - 6, transform.position.z);
	}

	//ѕри входе в область источника ресурса передает его местоположение в машину состо€ний
	private void OnTriggerEnter(Collider other)
	{
		if (IsDead) return;

		if (other.CompareTag("ResourceSource"))
		{
			IsCanMiningResource = true;
			ResourceSourcePosition = other.transform.position;
			StateMachineManager.TargetShipPosition = /*ResourceSourcePosition + new Vector3(0, ResourceDistanceY, 0)*/other.GetComponent<ResourceSource>().GetExtractionPlace().position;
			StateMachineManager.CurrentResourceSource = other.GetComponent<ResourceSource>();

			_playerUIManager.ShowHint();
		}

		if (other.CompareTag("Base"))
		{
			IsCanDocking = true;
			BasePosition = other.GetComponent<Base>().GetPlatformPosition();
			StateMachineManager.TargetShipPosition = BasePosition;
			StateMachineManager.CurrentBase = other.GetComponent<Base>();

			if (IsFirstTimeBase && !SceneManager.GetSceneByName("OpenMapScene").isLoaded)
			{
				GameEvents.OnBase?.Invoke(other.GetComponent<Base>());
				GameEvents.OnMissionComplete?.Invoke(0);
				GameEvents.OnCraftOpen?.Invoke("ѕрожектор");
				IsFirstTimeBase = false;
			}
			else IsFirstTimeBase = false;

			_playerUIManager.ShowHint();
		}

		if (other.CompareTag("Research"))
		{
			if (other.GetComponent<ResearchShip>().IsDataUpload()) return;

			IsCanResearch = true;
			StateMachineManager.CurrentResearchShip = other.GetComponent<ResearchShip>();
			StateMachineManager.TargetShipPosition = other.GetComponent<ResearchShip>().DockingPlace.transform.position;

			_playerUIManager.ShowHint();

			GameEvents.OnResearchNearBy?.Invoke(other.GetComponent<ResearchShip>());
		}

		if (other.CompareTag("Sand") || other.CompareTag("Block")/* || other.CompareTag("Base")*/) HitSurface();
	}

	//ѕри выходе из области источника ресурса обнул€ет его местоположение в машине состо€ний
	private void OnTriggerExit(Collider other)
	{
		if (IsDead) return;

		if (other.CompareTag("ResourceSource"))
		{
			IsCanMiningResource = false;
			ResourceSourcePosition = Vector3.zero;
			StateMachineManager.TargetShipPosition = Vector3.zero;

			_playerUIManager.HideHint();
		}

		if (other.CompareTag("Base"))
		{
			IsCanDocking = false;
			BasePosition = Vector3.zero;
			StateMachineManager.TargetShipPosition = Vector3.zero;

			GameEvents.OnMissionComplete?.Invoke(2);

			_playerUIManager.HideHint();
		}

		if (other.CompareTag("Research"))
		{
			IsCanResearch = false;
			StateMachineManager.CurrentResearchShip = null;
			StateMachineManager.TargetShipPosition = Vector3.zero;

			_playerUIManager.HideHint();
		}
	}


	private void Update()
	{
		if (!IsGameStart || Time.timeScale == 0) return;

		StateMachineManager.Update();

		if (_sun != null)
		{
			if (transform.position.y >= 40 && !IsMapFogOn && !_sun.IsDayNow())
			{
				IsMapFogOn = true;

				GameEvents.OnMapFogOn?.Invoke();
			}
			if (transform.position.y < 40 && IsMapFogOn && !_sun.IsDayNow())
			{
				IsMapFogOn = false;

				GameEvents.OnMapFogOff?.Invoke();
			}
		}

		if (StateMachineManager.NextState != 3)
		{
			Ray ray = new Ray(transform.position, -transform.up);
			RaycastHit hit;

			if (Physics.Raycast(ray, out hit, 100f))
			{
				StateMachineManager.DistanceToGround = hit.distance;
			}
		}
	}

	public void LoadData(SaveDataClass.ShipTransform shipTransform, int currentBase, bool isOnBase)
	{
		Vector3 position = new Vector3(shipTransform.X, shipTransform.Y, shipTransform.Z);

		if (position == Vector3.zero) return;

		if (currentBase != -1)
		{
			Base[] bases = FindObjectsByType<Base>(FindObjectsSortMode.None);

			for (int i = 0; i < bases.Length; i++)
			{
				if (bases[i].GetID() == currentBase)
				{
					StateMachineManager.CurrentBase = bases[i];
					GameEvents.OnBase?.Invoke(bases[i]);

					if (isOnBase)
					{
						IsCanDocking = true;
						transform.position = position;
						StateMachineManager.TargetShipPosition = bases[i].GetPlatformPosition();

					}
					else
					{
						transform.position = position;
						transform.rotation = Quaternion.Euler(shipTransform.RX, shipTransform.RY, shipTransform.RZ);
						PlayerCameraRotationObject.transform.rotation = Quaternion.Euler(shipTransform.RX, shipTransform.RY, shipTransform.RZ);
						StateMachineManager.RotationX = shipTransform.RX;
						StateMachineManager.RotationY = shipTransform.RY;
					}
				}
			}
		}
		else
		{
			transform.position = position;
			transform.rotation = Quaternion.Euler(shipTransform.RX, shipTransform.RY, shipTransform.RZ);
			PlayerCameraRotationObject.transform.rotation = Quaternion.Euler(shipTransform.RX, shipTransform.RY, shipTransform.RZ);
			StateMachineManager.RotationX = shipTransform.RX;
			StateMachineManager.RotationY = shipTransform.RY;
		}
	}

	public void LoadData(IReadOnlyList<float> stats)
	{
		_defenseSystem.GetHealth().SetCurrentHealth(stats[0]);
		_defenseSystem.GetDefense().SetCurrentHealth(stats[1]);
		_defenseSystem.GetFireDefense().SetCurrentHealth(stats[2]);
		_fuel.SetCurrentFuel(stats[3]);
	}

	public void SaveData()
	{
		Scene mainScene = SceneManager.GetSceneByName("OpenMapScene");

		if (mainScene.isLoaded) GameEvents.OnSceneSave?.Invoke("OpenMapScene");
		else GameEvents.OnSceneSave?.Invoke("IntroductionScene");

		if (StateMachineManager.CurrentBase == null) GameEvents.OnTransformSave?.Invoke(transform, 0, false);
		else GameEvents.OnTransformSave?.Invoke(transform, StateMachineManager.CurrentBase.GetID(), StateMachineManager.GetCurrentState() == 20);

		List<float> stats = new List<float>();

		stats.Add(_defenseSystem.GetHealth().GetCurrentHealth());
		stats.Add(_defenseSystem.GetDefense().GetCurrentHealth());
		stats.Add(_defenseSystem.GetFireDefense().GetCurrentHealth());
		stats.Add(_fuel.GetCurrentFuel());

		GameEvents.OnStatsSave?.Invoke(stats);
	}

	private void OnDisable()
	{
		GameEvents.OnGameStart -= StartGame;
		GameEvents.OnResourceDrop -= DropResource;
		GameEvents.OnTransformLoad -= LoadData;
		GameEvents.OnStatsLoad -= LoadData;
		GameEvents.OnSave -= SaveData;

		_defenseSystem.OnDisable();
		_fuel.OnDisable();
	}
}