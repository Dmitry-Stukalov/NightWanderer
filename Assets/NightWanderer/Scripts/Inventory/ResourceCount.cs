using TMPro;
using UnityEngine;

//Отображает количество ресурсов в ячейке
public class ResourceCount : MonoBehaviour
{
	[field: SerializeField] private ResourceCellObject ResourceObject;
	private TextMeshProUGUI TextCount;

	public void Initializing()
	{
		TextCount = GetComponent<TextMeshProUGUI>();
		ResourceObject.OnUpdate += UpdateData;
	}

	private void UpdateData()
	{
		TextCount.text = ResourceObject.GetResourceCount().ToString();
	}
}
