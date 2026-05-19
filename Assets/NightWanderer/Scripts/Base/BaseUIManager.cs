using DG.Tweening;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

//UI на базе (новый)
public class BaseUIManager : MonoBehaviour
{
	[SerializeField] private UIDocument baseUI;
	[SerializeField] private BaseInventory _baseInventory;

	private VisualElement mainBackground;

	private VisualElement storageBackground;
	private VisualElement craftBackground;
	private VisualElement upgradesBackground;
	private VisualElement blackBackground;

	private Button storageButton;
	private Button craftButton;
	private Button upgradesButton;

	public bool OnBase { get; set; } = false;
	private bool IsFirstTime = true;

	public void Initializing(Fuel fuel, HealthFireDefense health, HealthFireDefense defense, HealthFireDefense fireDefense)
	{
		mainBackground = baseUI.rootVisualElement.Q<VisualElement>("InventoryPanel");

		blackBackground = baseUI.rootVisualElement.Q<VisualElement>("BlackBackground");
		GameEvents.OnFirstBaseVisit += () => StartCoroutine(OnBasePause());

		var fuelItemBackground = baseUI.rootVisualElement.Q<VisualElement>("FuelBackground");
		fuelItemBackground.dataSource = new FuelRecovery(fuel, baseUI.rootVisualElement.Q<VisualElement>("FuelForeground"));

		var healthItemBackground = baseUI.rootVisualElement.Q<VisualElement>("HealthBackground");
		healthItemBackground.dataSource = new HealthFireDefenseRecovery(health, baseUI.rootVisualElement.Q<VisualElement>("HealthForeground"));

		var defenseItemBackground = baseUI.rootVisualElement.Q<VisualElement>("DefenseBackground");
		defenseItemBackground.dataSource = new HealthFireDefenseRecovery(defense, baseUI.rootVisualElement.Q<VisualElement>("DefenseForeground"));

		var fireDefenseItemBackground = baseUI.rootVisualElement.Q<VisualElement>("FireDefenseBackground");
		fireDefenseItemBackground.dataSource = new HealthFireDefenseRecovery(fireDefense, baseUI.rootVisualElement.Q<VisualElement>("FireDefenseForeground"));

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

	private IEnumerator OnBasePause()
	{
		baseUI.sortingOrder = -5;

		//yield return new WaitForSeconds(74);
		yield return new WaitForSeconds(4);

		baseUI.sortingOrder = 10;
	}

	//Включает отображение UI на базе и выдвигает его вперед
	public void OpenBaseUI()
	{
		if (IsFirstTime) IsFirstTime = false;
		else baseUI.sortingOrder = 10;

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

	private void OnDisable()
	{
		GameEvents.OnFirstBaseVisit -= () => StartCoroutine(OnBasePause());
	}
}
