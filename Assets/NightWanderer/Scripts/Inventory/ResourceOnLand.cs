using UnityEngine;

//Скрипт отвечает за объект, который спавнится после добычи источника ресурса. Главная задача - хранение информации о ресурсе и перемещение
public class ResourceOnLand : MonoBehaviour
{
	private ResourceBase ThisResource;
	private ResourceLibrary ParentResource;
	private Vector3 TargetDirection;
	private bool IsCollected = false;

	public void SetResource(ResourceBase resource) => ThisResource = resource;
	public ResourceBase GetResource() => ThisResource;

	public void ChangeParent(ResourceLibrary parent)
	{
		ParentResource = parent;
		transform.SetParent(ParentResource.transform, true);
	}
	public void ChangeTarget(Vector3 targetDirection) => TargetDirection = targetDirection;

	public void Collected() => IsCollected = true;

	public int GetID() => ThisResource.ID;

	//Здесь должна быть логика передачи ThisResource в инвентарь игрока
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			IsCollected = false;
			ParentResource.OnRelease(gameObject);
		}
	}

	private void Update()
	{
		if (IsCollected)
		{
			transform.position = Vector3.MoveTowards(transform.position, TargetDirection, Time.deltaTime * 10);
		}
	}
}
