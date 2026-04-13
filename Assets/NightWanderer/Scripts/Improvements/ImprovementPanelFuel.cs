using UnityEngine.UIElements;


//Устаревшее
public class ImprovementPanelFuel : ImprovementPanelBase
{
	private ImprovementFuelConfig _fuelConfig;
	private ImprovementFuelData _currentLevel;

	public ImprovementPanelFuel(ImprovementManager manager, VisualElement panel, VisualTreeAsset needResourceGroup, ImprovementConfig config, string improvementName) : base(manager, panel, needResourceGroup, config, improvementName)
	{
		_fuelConfig = (ImprovementFuelConfig)config;

		_currentLevel = _fuelConfig.Levels[0];

		UpdateData();
	}

	protected override void Upgrade(ClickEvent evt)
	{
		if (_manager.TryUpgrade(_improvementName))
		{
			_currentLevel = _fuelConfig.Levels[_currentLevel.CurrentLevel];

			UpdateData();
		}
	}

	protected override void UpdateData()
	{
		_needResourceGroupPlace.hierarchy.Clear();

		for (int i = 0; i < _currentLevel.Resource.Count; i++)
		{
			var newGroup = _needResourceGroup.Instantiate();
			newGroup.Q<VisualElement>("NeedResourceIcon").style.backgroundImage = new StyleBackground();
			newGroup.Q<Label>("NeedResourceCount").text = _currentLevel.Count[i].ToString();
			_needResourceGroupPlace.Add(newGroup);
		}

		_improvementIcon.style.backgroundImage = new StyleBackground(_currentLevel.Icon);
		_improvementVisualName.text = $"{_currentLevel.Name}";
		_improvementStats.text = $"{_currentLevel.UpgradeDescription}";
	}
}
