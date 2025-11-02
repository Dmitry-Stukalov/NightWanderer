using UnityEngine;
using UnityEngine.InputSystem;

public class ShipMovement : MonoBehaviour
{
	[field: SerializeField] private GameObject PlayerCameraRotationObject;
	[field: SerializeField] private float WalkingSpeed;
	[field: SerializeField] private float BoostedSpeed;
	[field: SerializeField] private float UpDownSpeed;
	[field: SerializeField] private float UpDownBoostedSpeed;
	[field: SerializeField] private float LookSpeed;
	[field: SerializeField] private int ResourceRotationX;
	[field: SerializeField] private int ResourceDistanceY;
	private InputAction MoveAction;
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

	private void Start()
	{
		MoveAction = InputSystem.actions.FindAction("Move");
		LookAction = InputSystem.actions.FindAction("Look");

		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;

		PlayerCameraRotationObject.transform.rotation = Quaternion.Euler(0, 0, 0);

		StateMachineManager.AddState(0, new StateMachineIdle(0, StateMachineManager, PlayerCameraRotationObject, transform, MoveAction, LookAction, WalkingSpeed, UpDownSpeed, LookSpeed));
		StateMachineManager.AddState(1, new StateMachineWalk(1, StateMachineManager, PlayerCameraRotationObject, transform, MoveAction, LookAction, WalkingSpeed, UpDownSpeed, LookSpeed));
		StateMachineManager.AddState(2, new StateMachineRun(2, StateMachineManager, PlayerCameraRotationObject, transform, MoveAction, LookAction, BoostedSpeed, UpDownBoostedSpeed, LookSpeed));
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
		/*ForwardVector = transform.TransformDirection(Vector3.forward);
		RightVector = transform.TransformDirection(Vector3.right);

		if (Keyboard.current.fKey.wasPressedThisFrame && IsCanMiningResource)
		{
			IsOnResource = !IsOnResource;
			ResourceRotationY = CompareDifference(transform.rotation.eulerAngles.y);
		}

		if (!IsOnResource)
		{
			if (Keyboard.current.shiftKey.wasPressedThisFrame) IsBoosted = !IsBoosted;

			if (Keyboard.current.wKey.IsPressed() || Keyboard.current.sKey.IsPressed()) SpeedX = IsCanMove ? (IsBoosted ? BoostedSpeed : WalkingSpeed) * MoveAction.ReadValue<Vector2>().y : 0;
			else SpeedX = 0;
			if (Keyboard.current.aKey.IsPressed() || Keyboard.current.dKey.IsPressed()) SpeedZ = IsCanMove ? (IsBoosted ? BoostedSpeed : WalkingSpeed) * MoveAction.ReadValue<Vector2>().x : 0;
			else SpeedZ = 0;
			if (Keyboard.current.qKey.IsPressed() && Keyboard.current.eKey.IsPressed()) SpeedY = 0;
			else if (Keyboard.current.qKey.IsPressed()) SpeedY = IsCanMove ? (IsBoosted ? UpDownBoostedSpeed : UpDownSpeed) : 0;
			else if (Keyboard.current.eKey.IsPressed()) SpeedY = IsCanMove ? (IsBoosted ? -UpDownBoostedSpeed : -UpDownSpeed) : 0;
			else SpeedY = 0;

			MoveDirection = (ForwardVector * SpeedX) + (RightVector * SpeedZ) + (SpeedY * transform.up);

			transform.position = transform.position + MoveDirection;
		}
		else
		{
			if (transform.position != new Vector3(ResourceSourcePosition.x, ResourceSourcePosition.y + ResourceDistanceY, ResourceSourcePosition.z))
			{
				IsShipReady = false;
				transform.position = Vector3.MoveTowards(transform.position, new Vector3(ResourceSourcePosition.x, ResourceSourcePosition.y + ResourceDistanceY, ResourceSourcePosition.z), Time.deltaTime * 5);
				transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, ResourceRotationY, 0), Time.deltaTime * 5);
			}
			else IsShipReady = true;
		}

		if (!IsOnResource)
		{
			MouseAxis = LookAction.ReadValue<Vector2>();

			RotationX += MouseAxis.y * LookSpeed;
			RotationY += MouseAxis.x * LookSpeed;

			PlayerCameraRotationObject.transform.rotation = Quaternion.Euler(-RotationX, RotationY, 0);
			transform.rotation = Quaternion.Euler(0, RotationY, 0);
		}
		else
		{
			if (PlayerCameraRotationObject.transform.rotation != Quaternion.Euler(ResourceRotationX, ResourceRotationY, 0))
			{
				PlayerCameraRotationObject.transform.rotation = Quaternion.Slerp(PlayerCameraRotationObject.transform.rotation, Quaternion.Euler(ResourceRotationX, ResourceRotationY, 0), Time.deltaTime * 5);
			}
		}*/
		StateMachineManager.Update();
	}
}