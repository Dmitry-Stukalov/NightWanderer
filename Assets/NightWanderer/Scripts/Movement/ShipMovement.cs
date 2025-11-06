using UnityEngine;
using UnityEngine.InputSystem;

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
	private Vector3 MoveDirection = Vector3.zero;
	private Vector3 ForwardVector;
	private Vector3 RightVector;
	private Vector2 MouseAxis;
	public Vector3 ResourceSourcePosition;
	private float SpeedX;
	private float SpeedY;
	private float SpeedZ;
	private float RotationX = 0;
	private float RotationY = 0;
	private bool IsCanMove = true;
	private bool IsBoosted = false;
	public bool IsCanMiningResource = false;
	public bool IsOnResource = false;
	public bool IsShipReady = false;
	private int ResourceRotationY = 0;

	private StateMachineManager StateMachineManager = new StateMachineManager();

	public void Initializing()
	{
		MoveAction = InputSystem.actions.FindAction("Move");
		UpDownMoveAction = InputSystem.actions.FindAction("UpDownMove");
		LookAction = InputSystem.actions.FindAction("Look");

		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;

		PlayerCameraRotationObject.transform.rotation = Quaternion.Euler(0, 0, 0);

		StateMachineManager.AddState(0, new StateMachineIdle(0, StateMachineManager, PlayerCameraRotationObject, transform, VacuumCleanerObject.transform, MoveAction, UpDownMoveAction, LookAction, WalkingSpeed, UpDownSpeed, LookSpeed));
		StateMachineManager.AddState(1, new StateMachineWalk(1, StateMachineManager, PlayerCameraRotationObject, transform, VacuumCleanerObject.transform, MoveAction, UpDownMoveAction, LookAction, WalkingSpeed, UpDownSpeed, LookSpeed));
		StateMachineManager.AddState(2, new StateMachineRun(2, StateMachineManager, PlayerCameraRotationObject, transform, VacuumCleanerObject.transform, MoveAction, UpDownMoveAction, LookAction, BoostedSpeed, UpDownBoostedSpeed, LookSpeed));
		StateMachineManager.AddState(10, new StateMachineTransition(10, StateMachineManager, transform, PlayerCameraRotationObject.transform));
		StateMachineManager.AddState(11, new StateMachineResourceExtraction1(3, StateMachineManager, transform, PlayerCameraRotationObject));

		StateMachineManager.SetState(0);
	}

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

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("ResourceSource"))
		{
			IsCanMiningResource = false;
			ResourceSourcePosition = Vector3.zero;
			StateMachineManager.TargetShipPosition = Vector3.zero;
		}
	}

	private int CompareDifference(float n1)
	{
		int t = 0;
		int divisionResult = (int)n1 / 90;
		n1 -= 90 * divisionResult;

		if (n1 > 0)
		{
			if (Mathf.Abs(0 - n1) > Mathf.Abs(90 - n1)) t = 1;
			else t = 0;

			return (t + divisionResult) * 90;
		}
		else
		{
			if (Mathf.Abs(-90 - n1) < Mathf.Abs(0 - n1)) t = 1;
			else t = 0;

			return (Mathf.Abs(t) + Mathf.Abs(divisionResult)) * -90;
		}
	}

	private void Update()
	{
		StateMachineManager.Update();
	}
}