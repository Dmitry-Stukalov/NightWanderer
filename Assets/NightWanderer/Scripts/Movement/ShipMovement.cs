using UnityEngine;
using UnityEngine.InputSystem;


//’ранит информацию о состо€ни€х игрока, а также базовые значени€ перемещени€ и поворота камеры
public class ShipMovement : MonoBehaviour
{
	//[SerializeField] private Searchlight[] _searchlights;
	[SerializeField] private ImprovementManager _improvementManager;
	[SerializeField] private InventoryButton _inventoryButton;
	[SerializeField] private ResourceLibrary _resourceLibrary;
	[SerializeField] private SearchlightManager _searchlightManager;

	[Header("Camera")]
	[SerializeField] private GameObject PlayerCameraRotationObject;

	[Header("VacuumCleaner")]
	[SerializeField] private GameObject VacuumCleanerObject;
	[SerializeField] private VacuumCleaner _vacuumCleaner;

	[Header("Movement")]
	[SerializeField] private float WalkingSpeed;
	[SerializeField] private float BoostedSpeed;
	[SerializeField] private float UpDownSpeed;
	[SerializeField] private float UpDownBoostedSpeed;
	[SerializeField] private float _consumptionIdleValue;
	[SerializeField] private float _consumptionWalkValue;
	[SerializeField] private float _consumptionRunValue;

	[Header("Looking")]
	[SerializeField] private float LookSpeed;
	[SerializeField] private int ResourceRotationX;
	[SerializeField] private int ResourceDistanceY;

	[Header("Configs")]
	[SerializeField] private ImprovementConfig _fuelConfig;
	[SerializeField] private ImprovementConfig _enginesConfig;
	[SerializeField] private ImprovementConfig _healthConfig;
	[SerializeField] private ImprovementConfig _defenseConfig;
	[SerializeField] private ImprovementConfig _fireDefenseConfig;

	private DefenseSystem _defenseSystem;
	private Fuel _fuel;
	private JetEngines _engines;
	private Timer _fuelConsumptionTimer;
	private InputAction MoveAction;
	private InputAction UpDownMoveAction;
	private InputAction LookAction;
	public Vector3 ResourceSourcePosition { get; set; }
	public Vector3 BasePosition { get; set; }
	public bool IsCanMiningResource { get; set; } = false;
	public bool IsOnResource { get; set; } = false;
	public bool IsShipReady { get; set; } = false;
	public bool IsCanDocking { get; set; } = false;

	private StateMachineManager StateMachineManager = new StateMachineManager();

	public void Initializing()
	{
		_vacuumCleaner.Initializing(_resourceLibrary, gameObject, VacuumCleanerObject, new Vector3(VacuumCleanerObject.transform.localScale.x / 2, VacuumCleanerObject.transform.localScale.y / 2, VacuumCleanerObject.transform.localScale.z / 2));
		
		//for (int i = 0; i < _searchlights.Length; i++) _searchlights[i].Initializing();

		_defenseSystem = new DefenseSystem(new HealthFireDefense(_healthConfig), new HealthFireDefense(_defenseConfig), new HealthFireDefense(_fireDefenseConfig), _improvementManager);

		_fuel = new Fuel(_fuelConfig);
		_engines = new JetEngines(_enginesConfig, _fuel);

		MoveAction = InputSystem.actions.FindAction("Move");
		UpDownMoveAction = InputSystem.actions.FindAction("UpDownMove");
		LookAction = InputSystem.actions.FindAction("Look");

		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;

		PlayerCameraRotationObject.transform.rotation = Quaternion.Euler(0, 0, 0);
		
		StateMachineManager.AddState(0, new StateMachineIdle(0, StateMachineManager, PlayerCameraRotationObject, gameObject, transform, VacuumCleanerObject.transform, _vacuumCleaner, _fuel, _engines, MoveAction, UpDownMoveAction, LookAction, LookSpeed));
		StateMachineManager.AddState(1, new StateMachineWalk(1, StateMachineManager, PlayerCameraRotationObject, gameObject, transform, VacuumCleanerObject.transform, _vacuumCleaner, _fuel, _engines, MoveAction, UpDownMoveAction, LookAction,LookSpeed));
		StateMachineManager.AddState(2, new StateMachineRun(2, StateMachineManager, PlayerCameraRotationObject, gameObject, transform, VacuumCleanerObject.transform, _vacuumCleaner, _fuel, _engines, MoveAction, UpDownMoveAction, LookAction, LookSpeed));
		StateMachineManager.AddState(3, new StateMachineVide(3, StateMachineManager, PlayerCameraRotationObject, gameObject, transform, VacuumCleanerObject.transform, _vacuumCleaner, _fuel, _engines, MoveAction, UpDownMoveAction, LookAction, LookSpeed));
		StateMachineManager.AddState(10, new StateMachineTransition(10, StateMachineManager, transform, PlayerCameraRotationObject.transform));
		StateMachineManager.AddState(11, new StateMachineResourceExtraction1(11, StateMachineManager, transform, PlayerCameraRotationObject));
		StateMachineManager.AddState(20, new StateMachineBase(20, StateMachineManager));

		StateMachineManager.SetState(0);
		StateMachineManager.Inventory = _inventoryButton;
		if (GetComponent<Animator>() != null) StateMachineManager._Animator = GetComponent<Animator>();
	}

	public DefenseSystem GetPlayerDefenseSystem() => _defenseSystem;
	public Fuel GetPlayerFuel () => _fuel;
	public JetEngines GetPlayerEngines () => _engines;

	private void HitSurface() => StateMachineManager.HitSurface();

	//ѕри входе в область источника ресурса передает его местоположение в машину состо€ний
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("ResourceSource"))
		{
			IsCanMiningResource = true;
			ResourceSourcePosition = other.transform.position;
			StateMachineManager.TargetShipPosition = ResourceSourcePosition + new Vector3(0, ResourceDistanceY, 0);
			StateMachineManager.CurrentResourceSource = other.GetComponent<ResourceSource>();
		}

		if (other.CompareTag("Base"))
		{
			IsCanDocking = true;
			BasePosition = other.transform.GetChild(0).position;
			StateMachineManager.TargetShipPosition = BasePosition;
			StateMachineManager.CurrentBase = other.GetComponent<Base>();
		}

		if (other.CompareTag("Sand")) HitSurface();
	}

	//ѕри выходе из области источника ресурса обнул€ет его местоположение в машине состо€ний
	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("ResourceSource"))
		{
			IsCanMiningResource = false;
			ResourceSourcePosition = Vector3.zero;
			StateMachineManager.TargetShipPosition = Vector3.zero;
		}

		if (other.CompareTag("Base"))
		{
			IsCanDocking = false;
			BasePosition = Vector3.zero;
			StateMachineManager.TargetShipPosition = Vector3.zero;
		}
	}

	private void Update()
	{
		if (Keyboard.current.tKey.wasPressedThisFrame) _searchlightManager.SearchlightOnOff();
			//for (int i = 0; i < _searchlights.Length; i++) _searchlights[i].IsOn = !_searchlights[i].IsOn;

		StateMachineManager.Update();

		if (StateMachineManager.GetCurrentState() == 1 || StateMachineManager.GetCurrentState() == 2) _searchlightManager.StartMove();
		if (StateMachineManager.GetCurrentState() == 0 || StateMachineManager.GetCurrentState() == 3) _searchlightManager.StartSearch();

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
}