using UnityEngine;

public class LayerVisibility : MonoBehaviour
{
	private Camera _camera;

	private void Start()
	{
		_camera = Camera.main;

		float[] distances = new float[32];

		int layerIndex = LayerMask.NameToLayer("Mark");
		distances[layerIndex] = 20000f;

		_camera.layerCullDistances = distances;
	}
}
