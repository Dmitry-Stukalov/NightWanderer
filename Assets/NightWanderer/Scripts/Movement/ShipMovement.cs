using UnityEngine;
using UnityEngine.InputSystem;


//’ранит информацию о состо€ни€х игрока, а также базовые значени€ перемещени€ и поворота камеры
public class ShipMovement : MonoBehaviour
{
	[field: SerializeField] private GameObject PlayerCameraRotationObject;
	[field: SerializeField] private GameObject VacuumCleanerObject;
	[field: SerializeField] private float WalkingSpeed;
	[field: SerializeField] private float BoostedSpeed;
	[field: SerializeField] private float UpDownSpeed;
	[field: SerializeField] private float UpDownBoostedSpeed;
	[field: SerializeField] private float LookSpeed;
	[field: SerializeField] private int ResourceRotationX;
	[field: SerializeField] private int ResourceDistanceY;
	private InputAction MoveAction;
	private InputAction UpDownMoveAction;
	private InputAction LookAction;
	public Vector3 ResourceSourcePosition;
	public bool IsCanMiningResource = false;
	public bool IsOnResource = false;
	public bool IsShipReady = false;

	private StateMachineManager StateMachineManager = new StateMachineManager();

	public void Initializing()
	{
		MoveAction = InputSystem.actions.FindAction("Move");
		UpDownMoveAction = InputSystem.actions.FindAction("UpDownMove");
		LookAction = InputSystem.actions.FindAction("Look");

		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;

		PlayerCameraRotationObject.transform.rotation = Quaternion.Euler(0, 0, 0);

		StateMachineManager.AddState(0, new StateMachineIdle(0, StateMachineManager, PlayerCameraRotationObject, gameObject, transform, VacuumCleanerObject.transform, MoveAction, UpDownMoveAction, LookAction, WalkingSpeed, UpDownSpeed, LookSpeed));
		StateMachineManager.AddState(1, new StateMachineWalk(1, StateMachineManager, PlayerCameraRotationObject, gameObject, transform, VacuumCleanerObject.transform, MoveAction, UpDownMoveAction, LookAction, WalkingSpeed, UpDownSpeed, LookSpeed));
		StateMachineManager.AddState(2, new StateMachineRun(2, StateMachineManager, PlayerCameraRotationObject, gameObject, transform, VacuumCleanerObject.transform, MoveAction, UpDownMoveAction, LookAction, BoostedSpeed, UpDownBoostedSpeed, LookSpeed));
		StateMachineManager.AddState(10, new StateMachineTransition(10, StateMachineManager, transform, PlayerCameraRotationObject.transform));
		StateMachineManager.AddState(11, new StateMachineResourceExtraction1(3, StateMachineManager, transform, PlayerCameraRotationObject));

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
	}

	private void Update()
	{
		StateMachineManager.Update();
	}
}