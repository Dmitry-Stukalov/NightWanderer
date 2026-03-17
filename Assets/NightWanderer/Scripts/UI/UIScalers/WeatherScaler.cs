using UnityEngine;

public class WeatherScaler : MonoBehaviour
{
	private RectTransform WeatherPanel;
	private float Width;
	private float Height;

	public void Initializing()
	{
		WeatherPanel = GetComponent<RectTransform>();

		Width = Screen.width / 2.5f; 
		Height = Screen.height / 10;

		WeatherPanel.sizeDelta = new Vector2(Width, Height);
	}
}
