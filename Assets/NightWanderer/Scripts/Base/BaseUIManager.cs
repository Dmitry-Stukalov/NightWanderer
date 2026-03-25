using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

//UI на базе (новый)
public class BaseUIManager : MonoBehaviour
{
	[SerializeField] private UIDocument baseUI;
	[SerializeField] private BaseInventory _baseInventory;
	[SerializeField] private CraftManager _craftManager;

	private VisualElement mainBackground;

	private VisualElement storageBackground;
	private VisualElement craftBackground;
	private VisualElement upgradesBackground;

	private Button storageButton;
	private Button craftButton;
	private Button upgradesButton;

	public bool OnBase { get; set; } = false;

	public void Initializing()
	{
		_craftManager.Initializing();

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

		craftBackground.style.display = DisplayStyle.None;
		upgradesBackground.style.display = DisplayStyle.None;
		mainBackground.style.display = DisplayStyle.None;
	}

	//Включает отображение UI на базе и выдвигает его вперед
	public void OpenBaseUI()
	{
		baseUI.sortingOrder = 5;

		mainBackground.style.display = DisplayStyle.Flex;

		UnityEngine.Cursor.visible = true;
		UnityEngine.Cursor.lockState = CursorLockMode.None;

		OnBase = true;
	}

	//Выключает отображение UI на базе и задвигает его назад
	public void CloseBaseUI()
	{
		baseUI.sortingOrder = -5;

		mainBackground.style.display = DisplayStyle.None;

		UnityEngine.Cursor.visible = false;
		UnityEngine.Cursor.lockState = CursorLockMode.Locked;

		OnBase = false;
	}

	private void StorageButtonClick(ClickEvent evt) => OpenCloseUI("storage");

	private void CraftButtonClick(ClickEvent evt) => OpenCloseUI("craft");

	private void UpgradesButtonClick(ClickEvent evt) => OpenCloseUI("upgrades");

	//Переключает вкладки на базе в зависимости от нажатой кнопки
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
