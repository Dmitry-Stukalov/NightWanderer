using Unity.Properties;
using UnityEngine;
using UnityEngine.UIElements;

public class ImprovementPanelBase
{
	protected ImprovementManager _manager;
	protected VisualTreeAsset _needResourceGroup;
	protected VisualElement _improvementIcon;
	protected VisualElement _needResourceGroupPlace;
	protected Label _improvementVisualName;
	protected Label _improvementStats;
	protected Button _improvementButton;
	protected string _improvementName;
	protected bool IsUnlock;

	public ImprovementPanelBase(ImprovementManager manager, VisualElement panel, VisualTreeAsset needResourceGroup, ImprovementConfig config, string improvementName)
	{
		_manager = manager;

		_needResourceGroup = needResourceGroup;
		_improvementIcon = panel.Q<VisualElement>("UpgradeIcon");
		_improvementVisualName = panel.Q<Label>("UpgradeName");
		_improvementStats = panel.Q<Label>("StatsText");
		_needResourceGroupPlace = panel.Q<VisualElement>("NeedResourcesIcons");
		_improvementButton = panel.Q<Button>("UpgradeButton");

		_improvementName = improvementName;

		_improvementButton.RegisterCallback<ClickEvent>(Upgrade);
		_improvementButton.text = "Заблокировано";

		IsUnlock = false;
	}
	public void Unlock()
	{
		IsUnlock = true;
		_improvementButton.text = "Улучшить";
	}

	protected virtual void Upgrade(ClickEvent evt) { }

	protected virtual void UpdateData() { }
}
