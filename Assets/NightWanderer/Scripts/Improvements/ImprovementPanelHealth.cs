using UnityEngine;
using UnityEngine.UIElements;


//Устаревшее
public class ImprovementPanelHealth : ImprovementPanelBase
{
	private ImprovementHealthConfig _healthConfig;
	private ImprovementHealthData _currentLevel;

	public ImprovementPanelHealth(ImprovementManager manager, VisualElement panel, VisualTreeAsset needResourceGroup, ImprovementConfig config, string improvementName) : base(manager, panel, needResourceGroup, config, improvementName)
	{
		_healthConfig = (ImprovementHealthConfig)config;

		_currentLevel = _healthConfig.Levels[0];

		UpdateData();
	}

	protected override void Upgrade(ClickEvent evt)
	{
		if (_manager.TryUpgrade(_improvementName))
		{
			_currentLevel = _healthConfig.Levels[_currentLevel.CurrentLevel];

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
