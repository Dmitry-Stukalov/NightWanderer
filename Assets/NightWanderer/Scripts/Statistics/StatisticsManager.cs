using UnityEngine;
using UnityEngine.UIElements;

public class StatisticsManager : MonoBehaviour
{
	[SerializeField] private UIDocument _playerUI;
	[SerializeField] private UIDocument _baseUI;
	private Label _healthStatistic;
	private Label _fuelStatistic;
	private Label _defenseStatistic;
	private Label _fireDefenseStatistic;
	private Label _horizontalSpeedStatistic;
	private Label _verticalSpeedStatistic;
	private Label _inventoryStatistic;
	private Label _searchlightsStatistic;

	public void Initializing(Fuel fuel, HealthFireDefense health, HealthFireDefense defense, HealthFireDefense fireDefense, JetEngines engines, Inventory inventory, Searchlights searchlights)
	{
		_healthStatistic = _playerUI.rootVisualElement.Q<Label>("HealthStatistic");
		_healthStatistic.dataSource = new HealthStatistic(_healthStatistic, health);
		_baseUI.rootVisualElement.Q<Label>("HealthStatistic").dataSource = _healthStatistic.dataSource;

		_fuelStatistic = _playerUI.rootVisualElement.Q<Label>("FuelStatistic");
		_fuelStatistic.dataSource = new FuelStatistic(_fuelStatistic, fuel);
		_baseUI.rootVisualElement.Q<Label>("FuelStatistic").dataSource = _healthStatistic.dataSource;

		_defenseStatistic = _playerUI.rootVisualElement.Q<Label>("DefenseStatistic");
		_defenseStatistic.dataSource = new DefenseStatistic(_defenseStatistic, defense);
		_baseUI.rootVisualElement.Q<Label>("DefenseStatistic").dataSource = _healthStatistic.dataSource;

		_fireDefenseStatistic = _playerUI.rootVisualElement.Q<Label>("FireDefenseStatistic");
		_fireDefenseStatistic.dataSource = new FireDefenseStatistic(_fireDefenseStatistic, fireDefense);
		_baseUI.rootVisualElement.Q<Label>("FireDefenseStatistic").dataSource = _healthStatistic.dataSource;

		_horizontalSpeedStatistic = _playerUI.rootVisualElement.Q<Label>("HorizontalSpeedStatistic");
		_horizontalSpeedStatistic.dataSource = new EnginesHorizontalStatistic(_horizontalSpeedStatistic, engines);
		_baseUI.rootVisualElement.Q<Label>("HorizontalSpeedStatistic").dataSource = _healthStatistic.dataSource;

		_verticalSpeedStatistic = _playerUI.rootVisualElement.Q<Label>("VerticalSpeedStatistic");
		_verticalSpeedStatistic.dataSource = new EnginesVerticalStatistic(_verticalSpeedStatistic, engines);
		_baseUI.rootVisualElement.Q<Label>("VerticalSpeedStatistic").dataSource = _healthStatistic.dataSource;

		_inventoryStatistic = _playerUI.rootVisualElement.Q<Label>("InventoryStatistic");
		_inventoryStatistic.dataSource = new InventoryStatistic(_inventoryStatistic, inventory);
		_baseUI.rootVisualElement.Q<Label>("InventoryStatistic").dataSource = _healthStatistic.dataSource;

		_searchlightsStatistic = _playerUI.rootVisualElement.Q<Label>("SearchlightStatistic");
		_searchlightsStatistic.dataSource = new SearchlightStatistic(_searchlightsStatistic, searchlights);
		_baseUI.rootVisualElement.Q<Label>("SearchlightStatistic").dataSource = _healthStatistic.dataSource;
	}
}
