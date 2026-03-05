using UnityEngine;

//Создает ресурс после окончания добычи
public class ResourceSource : MonoBehaviour
{
	[field: SerializeField] public int ExtractionID { get; private set; }
	[SerializeField] private ResourceLibrary Library;
	[SerializeField] private GameObject[] Resources;
	[SerializeField] private GameObject SpawnResourceZone;
	[SerializeField] private int ResourceCount;
	[SerializeField] private int MinResourceCapacity;
	[SerializeField] private int MaxResourceCapacity;

	public void ResourceExtracted()
	{
		if (ResourceCount == 0) return;

		int id = Random.Range(0, Resources.Length);
		int randomCapacity = Random.Range(MinResourceCapacity, MaxResourceCapacity + 1);

		GameObject resource = Library.GetResource(id);
		resource.transform.SetParent(gameObject.transform, true);
		resource.GetComponent<ResourceOnLand>().SetResourceCount(randomCapacity);
		resource.transform.position = new Vector3(SpawnResourceZone.transform.position.x + Random.Range(-1, 1), SpawnResourceZone.transform.position.y, SpawnResourceZone.transform.position.z + Random.Range(1, 2));

		Debug.Log($"ResourceOnLandCount = {resource.GetComponent<ResourceOnLand>().GetResource().CurrentCount}");
		ResourceCount--;
	}
}
