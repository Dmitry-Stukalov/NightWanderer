
using UnityEngine;
using UnityEngine.InputSystem;


//’ранит информацию о состо€ни€х игрока, а также базовые значени€ перемещени€ и поворота камеры
public class ShipMovement : MonoBehaviour
{
	[SerializeField] private GameObject PlayerCameraRotationObject;
	[SerializeField] private GameObject VacuumCleanerObject;
	[SerializeField] private VacuumCleaner _vacuumCleaner;
	[SerializeField] private float WalkingSpeed;
	[SerializeField] private float BoostedSpeed;
	[SerializeField] private float UpDownSpeed;
	[SerializeField] private float UpDownBoostedSpeed;
	[SerializeField] private float LookSpeed;
	[SerializeField] private int ResourceRotationX;
	[SerializeField] private int ResourceDistanceY;
	private InputAction MoveAction;
	private InputAction UpDownMoveAction;
	private InputAction LookAction;
	public Vector3 ResourceSourcePosition;
	public Vector3 BasePosition;
	public bool IsCanMiningResource = false;
	public bool IsOnResource = false;
	public bool IsShipReady = false;
	public bool IsCanDocking = false;

	private StateMachineManager StateMachineManager = new StateMachineManager();

	public void Initializing()
	{
		_vacuumCleaner.Initializing(gameObject);

		MoveAction = InputSystem.actions.FindAction("Move");
		UpDownMoveAction = InputSystem.actions.FindAction("UpDownMove");
		LookAction = InputSystem.actions.FindAction("Look");

		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;

		PlayerCameraRotationObject.transform.rotation = Quaternion.Euler(0, 0, 0);

		StateMachineManager.AddState(0, new StateMachineIdle(0, StateMachineManager, PlayerCameraRotationObject, gameObject, transform, VacuumCleanerObject.transform, _vacuumCleaner, MoveAction, UpDownMoveAction, LookAction, WalkingSpeed, UpDownSpeed, LookSpeed));
		StateMachineManager.AddState(1, new StateMachineWalk(1, StateMachineManager, PlayerCameraRotationObject, gameObject, transform, VacuumCleanerObject.transform, _vacuumCleaner, MoveAction, UpDownMoveAction, LookAction, WalkingSpeed, UpDownSpeed, LookSpeed));
		StateMachineManager.AddState(2, new StateMachineRun(2, StateMachineManager, PlayerCameraRotationObject, gameObject, transform, VacuumCleanerObject.transform, _vacuumCleaner, MoveAction, UpDownMoveAction, LookAction, BoostedSpeed, UpDownBoostedSpeed, LookSpeed));
		StateMachineManager.AddState(10, new StateMachineTransition(10, StateMachineManager, transform, PlayerCameraRotationObject.transform));
		StateMachineManager.AddState(11, new StateMachineResourceExtraction1(11, StateMachineManager, transform, PlayerCameraRotationObject));
		StateMachineManager.AddState(20, new StateMachineBase(20, StateMachineManager));

		StateMachineManager.SetState(0);
		if (GetComponent<Animator>() != null) StateMachineManager._Animator = GetComponent<Animator>();
	}

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
		StateMachineManager.Update();
	}
}