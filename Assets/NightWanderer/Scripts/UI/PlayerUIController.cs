using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

//Отвечает за управление всем UI
public class PlayerUIController : MonoBehaviour
{
	[SerializeField] private UIDocument PlayerUI;
	[SerializeField] private UIDocument ExtractionGameLaser;
	[SerializeField] private InventoryButton Inventory;
	[SerializeField] private BaseUIManager _baseUI;
	private VisualElement _mainExtractionLaserElement;
	private VisualElement _mainExtractionFuelElement;

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

		//var laserBackground = ExtractionGameLaser.rootVisualElement.Q<VisualElement>("MinigameLaserBackground");
		//laserBackground.dataSource = new MinigameLaser(laserBackground.Q<VisualElement>("ExtractionLaserBackground"));

		_mainExtractionLaserElement.style.display = DisplayStyle.None;
		_mainExtractionFuelElement.style.display = DisplayStyle.None;
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
	}
}
