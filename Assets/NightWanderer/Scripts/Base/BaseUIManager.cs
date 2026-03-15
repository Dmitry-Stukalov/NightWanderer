using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class BaseUIManager : MonoBehaviour
{
	[SerializeField] private UIDocument baseUI;

	private VisualElement mainBackground;

	private VisualElement storageBackground;
	private VisualElement craftBackground;
	private VisualElement upgradesBackground;

	private Button storageButton;
	private Button craftButton;
	private Button upgradesButton;

	public void Initializing()
	{
		mainBackground = baseUI.rootVisualElement.Q<VisualElement>("InventoryPanel");

		storageBackground = baseUI.rootVisualElement.Q<VisualElement>("StorageBackground");
		craftBackground = baseUI.rootVisualElement.Q<VisualElement>("CraftBackground");
		upgradesBackground = baseUI.rootVisualElement.Q<VisualElement>("UpgradesBackground");

		storageButton = baseUI.rootVisualElement.Q<Button>("StorageButton");
		craftButton = baseUI.rootVisualElement.Q<Button>("CraftButton");
		upgradesButton = baseUI.rootVisualElement.Q<Button>("UpgradesButton");

		storageButton.RegisterCallback<ClickEvent>(StorageButtonClick);
		craftButton.RegisterCallback<ClickEvent>(CraftButtonClick);
		upgradesButton.RegisterCallback<ClickEvent>(UpgradesButtonClick);

		mainBackground.style.display = DisplayStyle.None;
	}

	public void OpenBaseUI()
	{
		mainBackground.style.display = DisplayStyle.Flex;

		UnityEngine.Cursor.visible = true;
		UnityEngine.Cursor.lockState = CursorLockMode.None;
	}

	public void CloseBaseUI()
	{
		mainBackground.style.display = DisplayStyle.None;

		UnityEngine.Cursor.visible = false;
		UnityEngine.Cursor.lockState = CursorLockMode.Locked;
	}

	private void StorageButtonClick(ClickEvent evt) => OpenCloseUI("storage");

	private void CraftButtonClick(ClickEvent evt) => OpenCloseUI("craft");

	private void UpgradesButtonClick(ClickEvent evt) => OpenCloseUI("upgrades");

	private void OpenCloseUI(string name)
	{
		switch(name)
		{
			case "storage":
				storageBackground.style.display = DisplayStyle.Flex;
				craftBackground.style.display = DisplayStyle.None;
				upgradesBackground.style.display = DisplayStyle.None;
			break;

			case "craft":
				storageBackground.style.display = DisplayStyle.None;
				craftBackground.style.display = DisplayStyle.Flex;
				upgradesBackground.style.display = DisplayStyle.None;
			break;

			case "upgrades":
				storageBackground.style.display = DisplayStyle.None;
				craftBackground.style.display = DisplayStyle.None;
				upgradesBackground.style.display = DisplayStyle.Flex;
			break;
		}
	}
}
