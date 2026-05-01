using UnityEngine;
using UnityEngine.UIElements;

public class StatisticsManager : MonoBehaviour
{
	[SerializeField] private UIDocument _playerUI;
	[SerializeField] private UIDocument _baseUI;
	private Label _healthStatisticPlayer;
	private Label _fuelStatisticPlayer;
	private Label _defenseStatisticPlayer;
	private Label _fireDefenseStatisticPlayer;
	private Label _horizontalSpeedStatisticPlayer;
	private Label _verticalSpeedStatisticPlayer;
	private Label _inventoryStatisticPlayer;
	private Label _searchlightsStatisticPlayer;

	private Label _healthStatisticBase;
	private Label _fuelStatisticBase;
	private Label _defenseStatisticBase;
	private Label _fireDefenseStatisticBase;
	private Label _horizontalSpeedStatisticBase;
	private Label _verticalSpeedStatisticBase;
	private Label _inventoryStatisticBase;
	private Label _searchlightsStatisticBase;

	public void Initializing(Fuel fuel, HealthFireDefense health, HealthFireDefense defense, HealthFireDefense fireDefense, JetEngines engines, Inventory inventory, Searchlights searchlights)
	{
		_healthStatisticPlayer = _playerUI.rootVisualElement.Q<Label>("HealthStatistic");
		_healthStatisticPlayer.dataSource = new HealthStatistic(_healthStatisticPlayer, health);

		_healthStatisticBase = _baseUI.rootVisualElement.Q<Label>("HealthStatistic");
		_healthStatisticBase.dataSource = new HealthStatistic(_healthStatisticBase, health);

		_fuelStatisticPlayer = _playerUI.rootVisualElement.Q<Label>("FuelStatistic");
		_fuelStatisticPlayer.dataSource = new FuelStatistic(_fuelStatisticPlayer, fuel);

		_fuelStatisticBase = _baseUI.rootVisualElement.Q<Label>("FuelStatistic");
		_fuelStatisticBase.dataSource = new FuelStatistic(_fuelStatisticBase, fuel);

		_defenseStatisticPlayer = _playerUI.rootVisualElement.Q<Label>("DefenseStatistic");
		_defenseStatisticPlayer.dataSource = new DefenseStatistic(_defenseStatisticPlayer, defense);

		_defenseStatisticBase = _baseUI.rootVisualElement.Q<Label>("DefenseStatistic");
		_defenseStatisticBase.dataSource = new DefenseStatistic(_defenseStatisticBase, defense);

		_fireDefenseStatisticPlayer = _playerUI.rootVisualElement.Q<Label>("FireDefenseStatistic");
		_fireDefenseStatisticPlayer.dataSource = new FireDefenseStatistic(_fireDefenseStatisticPlayer, fireDefense);

		_fireDefenseStatisticBase = _baseUI.rootVisualElement.Q<Label>("FireDefenseStatistic");
		_fireDefenseStatisticBase.dataSource = new FireDefenseStatistic(_fireDefenseStatisticBase, fireDefense);

		_horizontalSpeedStatisticPlayer = _playerUI.rootVisualElement.Q<Label>("HorizontalSpeedStatistic");
		_horizontalSpeedStatisticPlayer.dataSource = new EnginesHorizontalStatistic(_horizontalSpeedStatisticPlayer, engines);

		_horizontalSpeedStatisticBase = _baseUI.rootVisualElement.Q<Label>("HorizontalSpeedStatistic");
		_horizontalSpeedStatisticBase.dataSource = new EnginesHorizontalStatistic(_horizontalSpeedStatisticBase, engines);

		_verticalSpeedStatisticPlayer = _playerUI.rootVisualElement.Q<Label>("VerticalSpeedStatistic");
		_verticalSpeedStatisticPlayer.dataSource = new EnginesVerticalStatistic(_verticalSpeedStatisticPlayer, engines);

		_verticalSpeedStatisticBase = _baseUI.rootVisualElement.Q<Label>("VerticalSpeedStatistic");
		_verticalSpeedStatisticBase.dataSource = new EnginesVerticalStatistic(_verticalSpeedStatisticBase, engines);

		_inventoryStatisticPlayer = _playerUI.rootVisualElement.Q<Label>("InventoryStatistic");
		_inventoryStatisticPlayer.dataSource = new InventoryStatistic(_inventoryStatisticPlayer, inventory);

		_inventoryStatisticBase = _baseUI.rootVisualElement.Q<Label>("InventoryStatistic");
		_inventoryStatisticBase.dataSource = new InventoryStatistic(_inventoryStatisticBase, inventory);

		_searchlightsStatisticPlayer = _playerUI.rootVisualElement.Q<Label>("SearchlightStatistic");
		_searchlightsStatisticPlayer.dataSource = new SearchlightStatistic(_searchlightsStatisticPlayer, searchlights);

		_searchlightsStatisticBase = _baseUI.rootVisualElement.Q<Label>("SearchlightStatistic");
		_searchlightsStatisticBase.dataSource = new SearchlightStatistic(_searchlightsStatisticBase, searchlights);
	}
}
