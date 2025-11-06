using UnityEngine;
using UnityEngine.Pool;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using Mono.Cecil.Cil;
using UnityEngine.Rendering.RenderGraphModule;

public class ResourceSource : MonoBehaviour
{
	[field: SerializeField] public int ExtractionID { get; private set; }
	[field: SerializeField] private ResourceLibrary Library;
	[field: SerializeField] private GameObject[] Resources;
	[field: SerializeField] private int ResourceCount;
	[field: SerializeField] private int OneResourceCapacity;

	public void ResourceExtracted()
	{
		int id = Random.Range(0, Resources.Length);

		GameObject resource = Library.GetResource(id);
		resource.transform.SetParent(gameObject.transform, false);
		resource.transform.localPosition = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1));
	}
}
