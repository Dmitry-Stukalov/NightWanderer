using UnityEngine;

public class InventoryBackgroundScaler : MonoBehaviour
{
	private RectTransform WeatherPanel;
	private float Width;
	private float Height;

	public void Initializing()
	{
		WeatherPanel = GetComponent<RectTransform>();

		Width = Screen.width / 10 * 7;
		Height = Screen.height / 10 * 7;

		WeatherPanel.sizeDelta = new Vector2(Width, Height);
		gameObject.SetActive(false);
	}
}
