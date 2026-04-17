using UnityEngine;

public class Billboard : MonoBehaviour
{
	[SerializeField] private Transform _cameraTransform;
	[SerializeField] private float _startScale;
	[SerializeField] private float _scaleFactor;
	private float newScale = 0;

	private void Start()
	{
		_cameraTransform = Camera.main.transform;
		transform.localScale = new Vector3(_startScale, _startScale, _startScale);
	}

	private void Update()
	{
		transform.rotation = _cameraTransform.rotation;


		newScale = Vector3.Distance(transform.position, _cameraTransform.position) * _scaleFactor * _startScale;
		transform.localScale = new Vector3(newScale, newScale, newScale);
	}
}
