using System.Collections.Generic;
using UnityEngine;

public class ResourceLibrary : MonoBehaviour
{
	[field: SerializeField] ResourceConfig config;
	[field: SerializeField] private GameObject[] Resources;
	private ResourceBase[] ResourcesBase;
	private List<GameObject> ResourcePool = new List<GameObject>();

	public void Initializing()
	{
		ResourcesBase = new ResourceBase[config.Resources.Count];

		foreach (var resource in config.Resources)
		{
			ResourcesBase[resource.ID] = new ResourceBase(resource.View, resource.Name, resource.ID, resource.MaxCount);
		}

		for (int i = 0; i < 4; i++)
		{
			for (int j = 0; j < Resources.Length; j++)
			{
				GameObject obj = Instantiate(Resources[j], transform.position, Quaternion.identity);
				obj.GetComponent<ResourceOnLand>().SetResource(ResourcesBase[j]);
				obj.GetComponent<ResourceOnLand>().ChangeParent(this);
				ResourcePool.Add(obj);
				obj.SetActive(false);
			}
		}

		Debug.Log($"В игре всего {Resources.Length} ресурсов");
	}

	public GameObject GetResource(int id)
	{
		foreach (var obj in ResourcePool)
		{
			if (!obj.activeInHierarchy && obj.gameObject.GetComponent<ResourceOnLand>().GetResource().ID == id)
			{
				obj.SetActive(true);
				return obj;
			}
		}

		return CreateObject(id);
	}

	private GameObject CreateObject(int id)
	{
		GameObject obj = Instantiate(Resources[id], transform.position, Quaternion.identity);
		obj.GetComponent<ResourceOnLand>().SetResource(ResourcesBase[id]);
		obj.GetComponent<ResourceOnLand>().ChangeParent(this);
		ResourcePool.Add(obj);
		return obj;
	}

	public void OnRelease(GameObject resource)
	{
		resource.SetActive(false);
		resource.transform.position = transform.position;
	}

	public void OnDestroyResource(GameObject resource) => Destroy(resource);
}
