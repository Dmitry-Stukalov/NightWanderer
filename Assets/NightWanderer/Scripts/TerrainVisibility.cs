using UnityEngine;

public class TerrainVisibility : MonoBehaviour
{
	[SerializeField] private Camera _camera;
	private Terrain _terrain;
	private TerrainCollider _terrainCollider;

	private void Start()
	{
		_camera = Camera.main;
		_terrain = GetComponent<Terrain>();
		_terrainCollider = GetComponent<TerrainCollider>();
	}

	private void LateUpdate()
	{
		if (Vector3.Distance(transform.position, _camera.transform.position) >= 3000)
		{
			_terrain.enabled = false;
			_terrainCollider.enabled = false;
		}
		else
		{
			_terrain.enabled = true;
			_terrainCollider.enabled = true;
		}
	}
}
