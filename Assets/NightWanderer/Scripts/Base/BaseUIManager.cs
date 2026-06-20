using DG.Tweening;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

//UI на базе (новый)
public class BaseUIManager : UIManager
{
	[SerializeField] private UIDocument _baseUI;
	[SerializeField] private BaseInventory _baseInventory;

	private VisualElement _mainElement;
	private VisualElement _mainBackground;
	private VisualElement _storageBackground;
	private VisualElement _craftBackground;
	private VisualElement _upgradesBackground;
	private VisualElement _blackBackground;

	private Button _storageButton;
	private Button _craftButton;
	private Button _upgradesButton;

	public bool OnBase { get; set; } = false;
	private bool IsFirstTime = true;

	public void Initializing(Fuel fuel, HealthFireDefense health, HealthFireDefense defense, HealthFireDefense fireDefense)
	{
		_mainElement = _baseUI.rootVisualElement.Q<VisualElement>("MainElement");
		_mainBackground = _baseUI.rootVisualElement.Q<VisualElement>("InventoryPanel");

		_blackBackground = _baseUI.rootVisualElement.Q<VisualElement>("BlackBackground");
		GameEvents.OnFirstBaseVisit += OnFirstTimeBase;

		var fuelItemBackground = _baseUI.rootVisualElement.Q<VisualElement>("FuelBackground");
		fuelItemBackground.dataSource = new FuelRecovery(fuel, _baseUI.rootVisualElement.Q<VisualElement>("FuelForeground"));

		var healthItemBackground = _baseUI.rootVisualElement.Q<VisualElement>("HealthBackground");
		healthItemBackground.dataSource = new HealthFireDefenseRecovery(health, _baseUI.rootVisualElement.Q<VisualElement>("HealthForeground"));

		var defenseItemBackground = _baseUI.rootVisualElement.Q<VisualElement>("DefenseBackground");
		defenseItemBackground.dataSource = new HealthFireDefenseRecovery(defense, _baseUI.rootVisualElement.Q<VisualElement>("DefenseForeground"));

		var fireDefenseItemBackground = _baseUI.rootVisualElement.Q<VisualElement>("FireDefenseBackground");
		fireDefenseItemBackground.dataSource = new HealthFireDefenseRecovery(fireDefense, _baseUI.rootVisualElement.Q<VisualElement>("FireDefenseForeground"));

		_storageBackground = _baseUI.rootVisualElement.Q<VisualElement>("StorageBackground");
		_craftBackground = _baseUI.rootVisualElement.Q<VisualElement>("CraftBackground");
		_upgradesBackground = _baseUI.rootVisualElement.Q<VisualElement>("UpgradesBackground");

		_storageButton = _baseUI.rootVisualElement.Q<Button>("StorageButton");
		_craftButton = _baseUI.rootVisualElement.Q<Button>("CraftButton");
		_upgradesButton = _baseUI.rootVisualElement.Q<Button>("UpgradesButton");

		_storageButton.RegisterCallback<ClickEvent>(StorageButtonClick);
		_craftButton.RegisterCallback<ClickEvent>(CraftButtonClick);
		_upgradesButton.RegisterCallback<ClickEvent>(UpgradesButtonClick);

		_craftBackground.style.display = DisplayStyle.None;
		_upgradesBackground.style.display = DisplayStyle.None;
		_storageBackground.style.display = DisplayStyle.None;

		_mainElement.style.display = DisplayStyle.None;
	}

	private void OnFirstTimeBase() => StartCoroutine(OnBasePause());

	private IEnumerator OnBasePause()
	{
		//_baseUI.sortingOrder = -5;
		_blackBackground.style.display = DisplayStyle.Flex;
		DOTween.To(() => _blackBackground.resolvedStyle.opacity, x => _blackBackground.style.opacity = x, 1, 1f);

		yield return new WaitForSeconds(3);
		//yield return new WaitForSeconds(77);

		DOTween.To(() => _blackBackground.resolvedStyle.opacity, x => _blackBackground.style.opacity = x, 1, 1f).OnComplete(() => _blackBackground.style.display = DisplayStyle.None);
		//yield return new WaitForSeconds(4);

		//_baseUI.sortingOrder = 10;
	}

	//Включает отображение UI на базе и выдвигает его вперед
	public override void OpenUI()
	{
		_mainElement.style.display = DisplayStyle.Flex;

		OpenCloseUI("storage");

		if (IsFirstTime) IsFirstTime = false;
		//else baseUI.sortingOrder = 10;

		//_mainBackground.style.display = DisplayStyle.Flex;

		UnityEngine.Cursor.visible = true;
		UnityEngine.Cursor.lockState = CursorLockMode.None;

		//OnBase = true;
	}

	//Выключает отображение UI на базе и задвигает его назад
	public override void CloseUI()
	{
		_mainElement.style.display = DisplayStyle.None;

		//_baseUI.sortingOrder = -5;

		//_mainBackground.style.display = DisplayStyle.None;

		UnityEngine.Cursor.visible = false;
		UnityEngine.Cursor.lockState = CursorLockMode.Locked;

		//OnBase = false;
	}

	private void StorageButtonClick(ClickEvent evt) => OpenCloseUI("");

	private void CraftButtonClick(ClickEvent evt) => OpenCloseUI("craft");

	private void UpgradesButtonClick(ClickEvent evt) => OpenCloseUI("upgrades");

	//Переключает вкладки на базе в зависимости от нажатой кнопки
	private void OpenCloseUI(string name)
	{
		switch(name)
		{
			case "storage":
				_storageBackground.style.display = DisplayStyle.Flex;
				_craftBackground.style.display = DisplayStyle.None;
				_upgradesBackground.style.display = DisplayStyle.None;
			break;

			case "craft":
				_storageBackground.style.display = DisplayStyle.None;
				_craftBackground.style.display = DisplayStyle.Flex;
				_upgradesBackground.style.display = DisplayStyle.None;
			break;

			case "upgrades":
				_storageBackground.style.display = DisplayStyle.None;
				_craftBackground.style.display = DisplayStyle.None;
				_upgradesBackground.style.display = DisplayStyle.Flex;
			break;
		}
	}

	private void OnDisable()
	{
		GameEvents.OnFirstBaseVisit -= OnFirstTimeBase;
	}
}
