using Mono.Cecil;
using System.Collections.Generic;
using UnityEngine;


//Хранит информацию о всех типах ресурсов, а также пул объектов ресурсов. Предоставляет источникам объекты из пула когда это требуется.
public class ResourceLibrary : MonoBehaviour
{
	[field: SerializeField] ResourceConfig config;
	[field: SerializeField] private GameObject[] Resources;
	private ResourceBase[] ResourcesBase;
	private List<GameObject> ResourcePool = new List<GameObject>();

	public void Initializing()
	{
		for (int i = 0; i < 4; i++)
		{
			for (int j = 0; j < Resources.Length; j++)
			{
				ResourceBase newResource = new ResourceBase(config.Resources[j].View, config.Resources[j].Name, config.Resources[j].ID, config.Resources[j].MaxCount);
				GameObject obj = Instantiate(Resources[j], transform.position, Quaternion.identity);
				obj.GetComponent<ResourceOnLand>().SetResource(newResource);
				obj.GetComponent<ResourceOnLand>().ChangeParent(this);
				ResourcePool.Add(obj);
				obj.SetActive(false);
			}
		}
		Debug.Log($"В игре всего {Resources.Length} ресурсов");
	}

	//Получение ресурса из пула
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

	//Создание ресурса, если его нет в пуле
	private GameObject CreateObject(int id)
	{
		ResourceBase newResource = new ResourceBase(config.Resources[id].View, config.Resources[id].Name, config.Resources[id].ID, config.Resources[id].MaxCount);
		GameObject obj = Instantiate(Resources[id], transform.position, Quaternion.identity);
		obj.GetComponent<ResourceOnLand>().SetResource(newResource);
		obj.GetComponent<ResourceOnLand>().ChangeParent(this);
		ResourcePool.Add(obj);
		return obj;
	}

	//Помещение объекта обратно в пул
	public void OnRelease(GameObject resource)
	{
		resource.SetActive(false);
		resource.transform.position = transform.position;
	}

	public void OnDestroyResource(GameObject resource) => Destroy(resource);
}
