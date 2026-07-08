using DG.Tweening;
using NUnit.Framework.Constraints;
using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

//Отвечает за управление всем UI
public class PlayerUIManager : UIManager
{
	[SerializeField] private UIDocument _playerUI;
	[SerializeField] private UIDocument _baseUI;
	[SerializeField] private VisualTreeAsset _statusPanel;
	[SerializeField] private Sprite _statusIcon;
	[SerializeField] private InventoryButton Inventory;
	private VisualElement _mainElement;
	private VisualElement _blackBackground;
	private VisualElement _tutorialPanel;
	private VisualElement _hintPanel;
	private VisualElement _criticalPanel;
	private VisualElement _inventoryBackground;
	private VisualElement _recordsBackground;
	private VisualElement _dataBaseBackground;
	private Button _inventoryButton;
	private Button _recordsButton;
	private Button _dataBaseButton;
	private Dictionary<string, VisualElement> _statusPanels;
	private TutorialManager _tutorialManager;

	public void Initializing(Fuel fuel, HealthFireDefense health, HealthFireDefense defense, HealthFireDefense fireDefense)
	{
		//StartCoroutine(StartPause());

		_mainElement = _playerUI.rootVisualElement.Q<VisualElement>("MainElement");

		_blackBackground = _playerUI.rootVisualElement.Q<VisualElement>("BlackBackground");

		_tutorialPanel = _playerUI.rootVisualElement.Q<VisualElement>("TutorialPanel");
		_tutorialPanel.dataSource = new TutorialManager(_tutorialPanel);
		_tutorialManager = (TutorialManager)_tutorialPanel.dataSource;

		_hintPanel = _playerUI.rootVisualElement.Q<VisualElement>("HintPanel");
		_hintPanel.style.opacity = 0;

		_criticalPanel = _playerUI.rootVisualElement.Q<VisualElement>("CriticalPanelPlace");

		var fuelItemBackground = _playerUI.rootVisualElement.Q<VisualElement>("FuelBackground");
		fuelItemBackground.dataSource = new FuelRecovery(fuel, _playerUI.rootVisualElement.Q<VisualElement>("FuelForeground"));

		var healthItemBackground = _playerUI.rootVisualElement.Q<VisualElement>("HealthBackground");
		healthItemBackground.dataSource = new HealthFireDefenseRecovery(health, _playerUI.rootVisualElement.Q<VisualElement>("HealthForeground"));

		var defenseItemBackground = _playerUI.rootVisualElement.Q<VisualElement>("DefenseBackground");
		defenseItemBackground.dataSource = new HealthFireDefenseRecovery(defense, _playerUI.rootVisualElement.Q<VisualElement>("DefenseForeground"));

		var fireDefenseItemBackground = _playerUI.rootVisualElement.Q<VisualElement>("FireDefenseBackground");
		fireDefenseItemBackground.dataSource = new HealthFireDefenseRecovery(fireDefense, _playerUI.rootVisualElement.Q<VisualElement>("FireDefenseForeground"));

		_inventoryBackground = _playerUI.rootVisualElement.Q<VisualElement>("InventoryBackground");
		_recordsBackground = _playerUI.rootVisualElement.Q<VisualElement>("RecordsBackground");
		_dataBaseBackground = _playerUI.rootVisualElement.Q<VisualElement>("DataBaseBackground");

		_inventoryButton = _playerUI.rootVisualElement.Q<Button>("InventoryButton");
		_recordsButton = _playerUI.rootVisualElement.Q<Button>("RecordsButton");
		_dataBaseButton = _playerUI.rootVisualElement.Q<Button>("DataBaseButton");

		_inventoryButton.RegisterCallback<ClickEvent>(OpenInventory);
		_recordsButton.RegisterCallback<ClickEvent>(OpenRecords);
		_dataBaseButton.RegisterCallback<ClickEvent>(OpenDataBase);

		_statusPanels = new Dictionary<string, VisualElement>();

		GameEvents.OnFirstBaseVisit += OnBase;

		GameEvents.OnCriticalStatusShow += ShowStatusPanel;
		GameEvents.OnCriticalStatusHide += HideStatusPanel;

		GameEvents.OnResearchStart += HideHint;
		GameEvents.OnResearchQuit += ShowHint;

		GameEvents.OnLaserExtractionStart += HideHint;
		GameEvents.OnExtractionEnd += ShowHint;

		GameEvents.OnInBase += CloseUI;
		GameEvents.OnOutBase += OpenUI;

		GameEvents.OnDeath += Death;

		if (SaveAndLoad.IsLoadGame) GameEvents.OnGameLoad += StartStartPause;
		else StartCoroutine(StartPause());
	}

	private void StartStartPause() => StartCoroutine(StartPause());

	private IEnumerator StartPause()
	{
		if (!SaveAndLoad.IsLoadGame)
		{
			yield return new WaitForSeconds(58f);
		}
		else yield return new WaitForSeconds(5f);

		StartGame();
	}

	private void OnBase() => StartCoroutine(OnBasePause());

	private IEnumerator OnBasePause()
	{
		_blackBackground.style.display = DisplayStyle.Flex;
		DOTween.To(() => _blackBackground.resolvedStyle.opacity, x => _blackBackground.style.opacity = x, 1, 1f);

		yield return new WaitForSeconds(4f);

		GameEvents.OnDialogueStart?.Invoke();

		//yield return new WaitForSeconds(70);

		DOTween.To(() => _blackBackground.resolvedStyle.opacity, x => _blackBackground.style.opacity = x, 0, 2f).OnComplete(() => _blackBackground.style.display = DisplayStyle.None);

		_playerUI.sortingOrder = 0;
	}

	private void Death() => StartCoroutine(DeathPause());

	private IEnumerator DeathPause()
	{
		yield return new WaitForSeconds(2f);

		_blackBackground.style.display = DisplayStyle.Flex;
		DOTween.To(() => _blackBackground.resolvedStyle.opacity, x => _blackBackground.style.opacity = x, 1, 1f);

		yield return new WaitForSeconds(8f);

		DOTween.To(() => _blackBackground.resolvedStyle.opacity, x => _blackBackground.style.opacity = x, 0, 2f).OnComplete(() => _blackBackground.style.display = DisplayStyle.None);
	}

	private void StartGame()
	{
		DOTween.To(() => _blackBackground.resolvedStyle.opacity, x => _blackBackground.style.opacity = x, 0, 3f)
		.OnComplete(() =>
		{
			GameEvents.OnGameStart?.Invoke();
			if (!SaveAndLoad.IsLoadGame) OpenTutorial(new int[] {0, 1, 2, 3, 6});
			_blackBackground.style.display = DisplayStyle.None;
		});
	}

	public override void OpenUI()
	{
		if (_mainElement == null) return;

		if (_mainElement.style.display != DisplayStyle.Flex) _mainElement.style.display = DisplayStyle.Flex;
	}

	public override void CloseUI()
	{
		if (Inventory.IsOpen) Inventory.OpenCloseInventory();

		_mainElement.style.display = DisplayStyle.None;
	}

	public void OpenCloseInventory()
	{
		if (Inventory.IsOpen) OpenInventory();
		Inventory.OpenCloseInventory();
	}

	//Начало методов по открытию и закрытию различных разделов в инвентаре

	public void OpenInventory(ClickEvent evt)
	{
		CloseRecords();
		CloseDataBase();
		_inventoryBackground.style.display = DisplayStyle.Flex;
	}

	public void OpenInventory()
	{
		CloseRecords();
		CloseDataBase();
		_inventoryBackground.style.display = DisplayStyle.Flex;
	}

	public void CloseInventory()
	{
		_inventoryBackground.style.display = DisplayStyle.None;
	}

	public void OpenRecords(ClickEvent evt)
	{
		CloseInventory();
		CloseDataBase();
		_recordsBackground.style.display = DisplayStyle.Flex;
	}

	public void CloseRecords()
	{
		_recordsBackground.style.display = DisplayStyle.None;
	}

	public void OpenDataBase(ClickEvent evt)
	{
		CloseInventory();
		CloseRecords();
		_dataBaseBackground.style.display = DisplayStyle.Flex;
	}

	public void CloseDataBase()
	{
		_dataBaseBackground.style.display = DisplayStyle.None;
	}

	//Конец методов по открытию и закрытию различных разделов в инвентаре

	public void ShowStatusPanel(string name, string panelText)
	{
        if (!_statusPanels.ContainsKey(name))
        {
			var newStatusPanel = _statusPanel.Instantiate();
			newStatusPanel.Q<VisualElement>("StatusIcon").style.backgroundImage = new StyleBackground(_statusIcon);
			newStatusPanel.Q<Label>("StatusText").text = panelText;

			_statusPanels[name] = newStatusPanel.Q<VisualElement>("PanelBackground");
			_playerUI.rootVisualElement.Q<VisualElement>("CriticalPanelPlace").Add(newStatusPanel.Q<VisualElement>("PanelBackground"));
		}
		else
		{
			_statusPanels[name].style.display = DisplayStyle.Flex;
		}
    }

	public void ShowHint()
	{
		DOTween.To(() => _hintPanel.resolvedStyle.opacity, x => _hintPanel.style.opacity = x, 1, 1f);
	}

	public void HideHint()
	{
		DOTween.To(() => _hintPanel.resolvedStyle.opacity, x => _hintPanel.style.opacity = x, 0, 1f);
	}

	public void ShowCriticalPanel()
	{
		DOTween.To(() => _criticalPanel.resolvedStyle.opacity, x => _criticalPanel.style.opacity = x, 1, 1f);
	}

	public void HideCriticalPanel()
	{
		DOTween.To(() => _criticalPanel.resolvedStyle.opacity, x => _criticalPanel.style.opacity = x, 0, 1f);
	}

	public void HideStatusPanel(string name)
	{
		if (!_statusPanels.ContainsKey(name)) return;

		_statusPanels[name].style.display = DisplayStyle.None;
	}

	public void OpenTutorial(int[] id)
	{
		for (int i = 0; i < id.Length; i++) _tutorialManager.OpenPanel(id[i]);

		StartCoroutine(TutorialPause(id));
	}

	private IEnumerator TutorialPause(int[] id)
	{
		yield return new WaitForSeconds(5f);

		CloseTutorial(id);
	}

	public void CloseTutorial(int[] id)
	{
		for (int i = 0; i < id.Length; i++) _tutorialManager.ClosePanel(id[i]);
	}

	public VisualElement GetVisualElement(string name) => _playerUI.rootVisualElement.Q<VisualElement>(name);

	private void OnDisable()
	{
		GameEvents.OnCriticalStatusShow -= ShowStatusPanel;
		GameEvents.OnCriticalStatusHide -= HideStatusPanel;
		GameEvents.OnFirstBaseVisit -= OnBase;

		GameEvents.OnResearchStart -= HideHint;
		GameEvents.OnResearchQuit -= ShowHint;

		GameEvents.OnLaserExtractionStart -= HideHint;
		GameEvents.OnExtractionEnd -= ShowHint;

		GameEvents.OnInBase -= CloseUI;
		GameEvents.OnOutBase -= OpenUI;

		GameEvents.OnDeath -= Death;

		/*if (SaveAndLoad.IsLoadGame)*/
		GameEvents.OnGameLoad -= StartStartPause;

		_inventoryButton.UnregisterCallback<ClickEvent>(OpenInventory);
		_recordsButton.UnregisterCallback<ClickEvent>(OpenRecords);
		_dataBaseButton.UnregisterCallback<ClickEvent>(OpenDataBase);
	}
}
