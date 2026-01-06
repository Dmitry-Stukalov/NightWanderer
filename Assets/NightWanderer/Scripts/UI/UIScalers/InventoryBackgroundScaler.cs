using UnityEngine;

public class InventoryBackgroundScaler : MonoBehaviour
{
	[field: SerializeField] private GameObject DropZone;
	private RectTransform InventoryBackground;
	private float Width;
	private float Height;

	public void Initializing()
	{
		InventoryBackground = GetComponent<RectTransform>();

		/*Width = Screen.width / 10 * 7;
		Height = Screen.height / 10 * 7;

		InventoryBackground.sizeDelta = new Vector2(Width, Height);*/
		DropZone.SetActive(false);
		gameObject.SetActive(false);
	}
}
