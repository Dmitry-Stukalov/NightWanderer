using UnityEngine;
using UnityEngine.UIElements;

public class ImprovementPanel<TConfig, TData> : ImprovementPanelBase
	where TConfig : ImprovementConfig, IImprovementConfig<TData>
	where TData : IImprovementData
{
	private TConfig _config;
	private TData _currentLevelData;

	public ImprovementPanel(ImprovementManager manager, VisualElement panel, VisualTreeAsset needResourceGroup, ImprovementConfig config, string improvementName) : base (manager, panel, needResourceGroup, config, improvementName)
	{
		_config = (TConfig)config;
		_currentLevelData = _config.Levels[0];

		UpdateData();
	}

	protected override void Upgrade(ClickEvent evt)
	{
		if (_manager.TryUpgrade(_improvementName))
		{
			_currentLevelData = _config.Levels[_currentLevelData.CurrentLevel];

			UpdateData();
		}
	}

	protected override void UpdateData()
	{
		_needResourceGroupPlace.hierarchy.Clear();

		for (int i = 0; i < _currentLevelData.Resource.Count; i++)
		{
			var newGroup = _needResourceGroup.Instantiate();
			newGroup.Q<VisualElement>("NeedResourceIcon").style.backgroundImage = new StyleBackground();
			newGroup.Q<Label>("NeedResourceCount").text = _currentLevelData.Count[i].ToString();
			_needResourceGroupPlace.Add(newGroup);
		}

		_improvementIcon.style.backgroundImage = new StyleBackground(_currentLevelData.Icon);
		_improvementVisualName.text = $"{_currentLevelData.Name}";
		_improvementStats.text = $"{_currentLevelData.UpgradeDescription}";
	}
}
