using UnityEngine;

public class BlockCameraMove : MonoBehaviour
{
	[SerializeField] private Transform _transform;
	private bool IsCameraBlock = false;

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Sand")) IsCameraBlock = true;
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Sand")) IsCameraBlock = false;
	}

	public void CameraRotate(Quaternion angle)
	{
		if (IsCameraBlock) return;

		_transform.rotation = angle;
	}
}
