using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class KnockedDownResource : MonoBehaviour
{
	[SerializeField] private VisualEffect _knockedDownEffect;
	[SerializeField] private int _resourceID;
	[SerializeField] private int _minResourceCapacity;
	[SerializeField] private int _maxResourceCapacity;
	private ResourceLibrary _resourceLibrary;

	private void Start()
	{
		_resourceLibrary = FindAnyObjectByType<ResourceLibrary>();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			_knockedDownEffect.Play();
			StartCoroutine(DeathCoroutine());

			int randomCapacity = UnityEngine.Random.Range(_minResourceCapacity, _maxResourceCapacity + 1);

			GameObject resource = _resourceLibrary.GetResource(_resourceID);
			resource.transform.SetParent(gameObject.transform.parent, true);
			resource.GetComponent<ResourceOnLand>().SetResourceCount(randomCapacity);
			resource.transform.position = transform.position;

			gameObject.GetComponent<MeshRenderer>().enabled = false;
			gameObject.GetComponent<BoxCollider>().enabled = false;
		}
	}

	private IEnumerator DeathCoroutine()
	{
		yield return new WaitForSeconds(0.3f);

		gameObject.GetComponent<MeshRenderer>().enabled = true;
		gameObject.GetComponent<BoxCollider>().enabled = true;
		gameObject.SetActive(false);
	}
}
