using System.Security.Cryptography;
using UnityEngine;

public class MeshRendererHide : MonoBehaviour
{
	private MeshRenderer mesh;

	private void Start() => mesh = GetComponent<MeshRenderer>();

	private void OnBecameVisible() => mesh.enabled = true;
	private void OnBecameInvisible() => mesh.enabled = false;
}
