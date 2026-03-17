using System.Collections.Generic;
using UnityEngine;

//UI на базе (Старый)
public class BaseUI : MonoBehaviour
{
	[field: SerializeField] private List<GameObject> UIToClose;
	[field: SerializeField] private List<GameObject> UIToOpen;
	public bool IsOpen { get; private set; } = false;

	public void OpenCloseBaseUI()
	{
		if (!IsOpen)
		{
			foreach (GameObject ui in UIToClose) ui.SetActive(false);
			foreach (GameObject ui in UIToOpen) ui.SetActive(true);
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}
		else
		{
			foreach (GameObject ui in UIToClose) ui.SetActive(true);
			foreach (GameObject ui in UIToOpen) ui.SetActive(false);
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}

		IsOpen = !IsOpen;
	}
}
