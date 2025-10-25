using UnityEngine;
using UnityEngine.InputSystem;

public class ShipMovement : MonoBehaviour
{
	[field: SerializeField] private Camera PlayerCamera;
	[field: SerializeField] private GameObject PlayerCameraRotationObject;
	[field: SerializeField] private float WalkingSpeed;
	[field: SerializeField] private float BoostedSpeed;
	[field: SerializeField] private float UpDownSpeed;
	[field: SerializeField] private float UpDownBoostedSpeed;
	[field: SerializeField] private float LookSpeed;
	[field: SerializeField] private float Gravity;
	private InputAction MoveAction;
	private InputAction LookAction;
	private Vector3 MoveDirection = Vector3.zero;
	private Vector3 ForwardVector;
	private Vector3 RightVector;
	private Vector2 MouseAxis;
	private float SpeedX;
	private float SpeedY;
	private float SpeedZ;
	private float RotationX = 0;
	private float RotationY = 0;
	private bool IsCanMove = true;
	private bool IsBoosted = false;

	private void Start()
	{
		MoveAction = InputSystem.actions.FindAction("Move");
		LookAction = InputSystem.actions.FindAction("Look");

		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	private void Update()
	{
		ForwardVector = transform.TransformDirection(Vector3.forward);
		RightVector = transform.TransformDirection(Vector3.right);

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

		transform.position =  transform.position + MoveDirection;


		MouseAxis = LookAction.ReadValue<Vector2>();

		RotationX += MouseAxis.y * LookSpeed;
		RotationY += MouseAxis.x * LookSpeed;

		PlayerCameraRotationObject.transform.rotation = Quaternion.Euler(-RotationX, RotationY, 0);
		transform.rotation = Quaternion.Euler(0, RotationY, 0);
	}
}
