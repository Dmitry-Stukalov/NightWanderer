using UnityEngine;
using UnityEngine.Pool;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using Mono.Cecil.Cil;
using UnityEngine.Rendering.RenderGraphModule;
using Mono.Cecil;

//Создает ресурс после окончания добычи
public class ResourceSource : MonoBehaviour
{
	[field: SerializeField] public int ExtractionID { get; private set; }
	[field: SerializeField] private ResourceLibrary Library;
	[field: SerializeField] private GameObject[] Resources;
	[field: SerializeField] private int ResourceCount;
	[field: SerializeField] private int MinResourceCapacity;
	[field: SerializeField] private int MaxResourceCapacity;

	public void ResourceExtracted()
	{
		if (ResourceCount == 0) return;

		int id = Random.Range(0, Resources.Length);
		int randomCapacity = Random.Range(MinResourceCapacity, MaxResourceCapacity + 1);

		GameObject resource = Library.GetResource(id);
		resource.transform.SetParent(gameObject.transform, false);
		resource.GetComponent<ResourceOnLand>().SetResourceCount(randomCapacity);
		resource.transform.localPosition = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1));

		Debug.Log($"randomCapacity = {randomCapacity}");
		Debug.Log($"ResourceOnLandCount = {resource.GetComponent<ResourceOnLand>().GetResource().CurrentCount}");
		ResourceCount--;
	}
}
