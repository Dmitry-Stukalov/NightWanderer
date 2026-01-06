using UnityEngine;

//Скрипт отвечает за объект, который спавнится после добычи источника ресурса. Главная задача - хранение информации о ресурсе и перемещение
public class ResourceOnLand : MonoBehaviour
{
	private ResourceBase ThisResource;
	private ResourceLibrary ParentResource;
	private Vector3 TargetDirection;
	private GameObject Target;
	private bool IsCollected = false;
	private Rigidbody _Rigidbody;

	private void Start()
	{
		_Rigidbody = GetComponent<Rigidbody>();
	}

	public void SetResource(ResourceBase resource) => ThisResource = resource;
	public void SetResourceCount(int count) => ThisResource.SetCount(count);

	public ResourceBase GetResource() => ThisResource;

	public void ChangeParent(ResourceLibrary parent)
	{
		ParentResource = parent;
		transform.SetParent(ParentResource.transform, true);
	}
	public void ChangeTarget(GameObject target) => Target = target;
	public void NullTarget() => Target = null;

	public void Collected() => IsCollected = true;
	public void IsntCollected() => IsCollected = false;

	public int GetID() => ThisResource.ID;

	private void OnTriggerEnter(Collider other)
	{
		if (other.transform.CompareTag("Player"))
		{
			other.transform.GetComponent<PlayerInventory>().AddResource(ThisResource);
			IsCollected = false;
			ParentResource.OnRelease(gameObject);
		}
	}

	private void Update()
	{
		if (IsCollected)
		{
			_Rigidbody.useGravity = false;
			transform.position = Vector3.MoveTowards(transform.position, Target.transform.position, Time.deltaTime * 10);
		}
		else _Rigidbody.useGravity = true;
	}
}
