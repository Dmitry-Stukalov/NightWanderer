using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

//Отвечает за управление всем UI
public class PlayerUIController : MonoBehaviour
{
	[SerializeField] private UIDocument PlayerUI;
	[SerializeField] private UIDocument ExtractionGameLaser;
	[SerializeField] private VisualTreeAsset _statusPanel;
	[SerializeField] private Sprite _statusIcon;
	[SerializeField] private InventoryButton Inventory;
	[SerializeField] private BaseUIManager _baseUI;
	private VisualElement _mainExtractionLaserElement;
	private VisualElement _mainExtractionFuelElement;
	private Dictionary<string, VisualElement> _statusPanels;

	public void Initializing(Fuel fuel, HealthFireDefense health, HealthFireDefense defense, HealthFireDefense fireDefense)
	{
		StartCoroutine(StartPause());

		var fuelItemBackground = PlayerUI.rootVisualElement.Q<VisualElement>("FuelBackground");
		fuelItemBackground.dataSource = new FuelRecovery(fuel, PlayerUI.rootVisualElement.Q<VisualElement>("FuelForeground"));

		var healthItemBackground = PlayerUI.rootVisualElement.Q<VisualElement>("HealthBackground");
		healthItemBackground.dataSource = new HealthFireDefenseRecovery(health, PlayerUI.rootVisualElement.Q<VisualElement>("HealthForeground"));

		var defenseItemBackground = PlayerUI.rootVisualElement.Q<VisualElement>("DefenseBackground");
		defenseItemBackground.dataSource = new HealthFireDefenseRecovery(defense, PlayerUI.rootVisualElement.Q<VisualElement>("DefenseForeground"));

		var fireDefenseItemBackground = PlayerUI.rootVisualElement.Q<VisualElement>("FireDefenseBackground");
		fireDefenseItemBackground.dataSource = new HealthFireDefenseRecovery(fireDefense, PlayerUI.rootVisualElement.Q<VisualElement>("FireDefenseForeground"));

		_statusPanels = new Dictionary<string, VisualElement>();

		GameEvents.OnCriticalStatusShow += ShowStatusPanel;
		GameEvents.OnCriticalStatusHide += HideStatusPanel;

		_baseUI.Initializing();

		GameEvents.OnLaserExtractionStart += OnExtractionLaserStart;
		GameEvents.OnExtractionEnd += OnExtractionLaserEnd;
	}

	private IEnumerator StartPause()
	{
		yield return new WaitForSeconds(1f);

		_mainExtractionLaserElement = ExtractionGameLaser.rootVisualElement.Q<VisualElement>("ExtractionLaserBackground");
		_mainExtractionFuelElement = ExtractionGameLaser.rootVisualElement.Q<VisualElement>("FuelBackground");
		_mainExtractionLaserElement.dataSource = new MinigameLaser(_mainExtractionLaserElement, _mainExtractionFuelElement);

		_mainExtractionLaserElement.style.display = DisplayStyle.None;
		_mainExtractionFuelElement.style.display = DisplayStyle.None;
	}

	public void ShowStatusPanel(string name, string panelText)
	{
        if (!_statusPanels.ContainsKey(name))
        {
			var newStatusPanel = _statusPanel.Instantiate();
			newStatusPanel.Q<VisualElement>("StatusIcon").style.backgroundImage = new StyleBackground(_statusIcon);
			newStatusPanel.Q<Label>("StatusText").text = panelText;

			_statusPanels[name] = newStatusPanel.Q<VisualElement>("PanelBackground");
			PlayerUI.rootVisualElement.Q<VisualElement>("CriticalPanelPlace").Add(newStatusPanel.Q<VisualElement>("PanelBackground"));
		}
		else
		{
			_statusPanels[name].style.display = DisplayStyle.Flex;
		}
    }

	public void HideStatusPanel(string name)
	{
		if (!_statusPanels.ContainsKey(name)) return;

		_statusPanels[name].style.display = DisplayStyle.None;
	}

	public void OnBase()
	{
		_baseUI.OpenBaseUI();
		Inventory.CloseWeatherPanel();
		Inventory.CloseInventoryPanel();
	}

	public void OutBase()
	{
		_baseUI.CloseBaseUI();
		Inventory.OpenWeatherPanel();
	}

	private void OnExtractionLaserStart()
	{
		_mainExtractionLaserElement.style.display = DisplayStyle.Flex;
		_mainExtractionFuelElement.style.display = DisplayStyle.Flex;
	}

	private void OnExtractionLaserEnd()
	{
		_mainExtractionLaserElement.style.display = DisplayStyle.None;
		_mainExtractionFuelElement.style.display = DisplayStyle.None;
	}

	public MinigameLaser GetMinigameLaser() => (MinigameLaser)_mainExtractionLaserElement.dataSource;

	private void Update()
	{
		if (Keyboard.current.tabKey.wasPressedThisFrame && !_baseUI.OnBase) Inventory.OpenCloseInventory();
	}

	private void OnDisable()
	{
		GameEvents.OnLaserExtractionStart -= OnExtractionLaserStart;
		GameEvents.OnExtractionEnd -= OnExtractionLaserEnd;
		GameEvents.OnCriticalStatusShow -= ShowStatusPanel;
		GameEvents.OnCriticalStatusHide -= HideStatusPanel;
	}
}
