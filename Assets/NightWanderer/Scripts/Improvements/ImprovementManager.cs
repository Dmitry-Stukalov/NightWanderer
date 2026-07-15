using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;
using System.Collections;
using DG.Tweening;

public class ImprovementManager : MonoBehaviour
{
	[SerializeField] private UIDocument _baseUI;
	[SerializeField] private VisualTreeAsset _upgradePanel;
	[SerializeField] private VisualTreeAsset _needResourceGroup;
	[SerializeField] private ResourceLibrary _library;
	[SerializeField] private int _upgradesCount;
	private VisualElement _improvementMessageBackground;
	private Label _improvementMessageText;
	private Dictionary<string, IImprovementBase> _improvements = new Dictionary<string, IImprovementBase>();
	private Inventory _playerInventory;
	private Inventory _baseInventory;
	private Dictionary<int, int> _resources = new Dictionary<int, int>();

	private List<int> _upgradesList = new List<int>();
	private ScrollView _upgradesBackground;

	public void Initializing(Inventory playerInventory, Inventory baseInventory)
	{
		_library = GameObject.FindGameObjectWithTag("ResourceLibrary").GetComponent<ResourceLibrary>();
		_playerInventory = playerInventory;
		_baseInventory = baseInventory;

		_improvementMessageBackground = _baseUI.rootVisualElement.Q<VisualElement>("ImprovementMessagePanel");
		_improvementMessageText = _baseUI.rootVisualElement.Q<Label>("ImprovementMessageText");

		for (int i = 0; i < 13; i++)
		{
			_resources[i] = 0;
		}

		_upgradesBackground = _baseUI.rootVisualElement.Q<ScrollView>("UpgradesBackground");

		GameEvents.OnImprovementOpen += UnlockImprovement;

		GameEvents.OnImprovementsLoad += LoadData;
		GameEvents.OnImprovementPanelsLoad += LoadData;
		GameEvents.OnSave += SaveData;

		StartCoroutine(StartPause());
	}

	private void ShowMessagePanel(string newText)
	{
		_improvementMessageText.text = newText;

		if (newText == "Недостаточно ресурсов")
		{
			_improvementMessageBackground.RemoveFromClassList("MainBackground");
			_improvementMessageBackground.AddToClassList("ErrorBackground");
		}
		else if (_improvementMessageBackground.ClassListContains("ErrorBackground"))
		{
			_improvementMessageBackground.RemoveFromClassList("ErrorBackground");
			_improvementMessageBackground.AddToClassList("MainBackground");
		}

		DOTween.Kill(_improvementMessageBackground);

		DOTween.To(() => _improvementMessageBackground.style.opacity.value, x => _improvementMessageBackground.style.opacity = x, 1, 1.5f);

		DOTween.To(() => _improvementMessageBackground.style.opacity.value, x => _improvementMessageBackground.style.opacity = x, 0, 1.5f).SetDelay(3f);
	}

	private string MatchName(string improvementName)
	{
		string newText = "";

		switch (improvementName)
		{
			case "Fuel":
				newText = "Топливные баки улучшены";
			break;

			case "Mining":
				newText = "Добывающее оборудование улучшено";
			break;

			case "Health":
				newText = "Здоровье улучшено";
			break;

			case "Defense":
				newText = "Защита улучшена";
			break;

			case "FireDefense":
				newText = "Термическая защита улучшена";
			break;

			case "Engines":
				newText = "Двигатели улучшены";
			break;

			case "Searchlight":
				newText = "Прожектор добавлен";
			break;

			case "SearchlightPower":
				newText = "Прожектора улучшены";
			break;

			case "Resource":
				newText = "Недостаточно ресурсов";
			break;
		}

		return newText;
	}

	private IEnumerator StartPause()
	{
		yield return new WaitForSecondsRealtime(1.5f);

		var newItem = _upgradePanel.Instantiate().hierarchy.ElementAt(0);

		foreach (var key in _improvements.Keys)
		{
			switch (key)
			{
				case "Fuel":

					newItem = _upgradePanel.Instantiate().hierarchy.ElementAt(0);
					newItem.dataSource = new ImprovementPanel<ImprovementFuelConfig, ImprovementFuelData>(this, newItem, _needResourceGroup, _improvements[key].Config, key);
					_upgradesBackground.Add(newItem);

				break;

				case "Mining":

					newItem = _upgradePanel.Instantiate().hierarchy.ElementAt(0);
					newItem.dataSource = new ImprovementPanel<ImprovementMiningConfig, ImprovementMiningData>(this, newItem, _needResourceGroup, _improvements[key].Config, key);
					_upgradesBackground.Add(newItem);
				break;

				case "Health" or "Defense" or "FireDefense":

					newItem = _upgradePanel.Instantiate().hierarchy.ElementAt(0);
					newItem.dataSource = new ImprovementPanel<ImprovementHealthConfig, ImprovementHealthData>(this, newItem, _needResourceGroup, _improvements[key].Config, key);
					_upgradesBackground.Add(newItem);

				break;

				case "Engines":

					newItem = _upgradePanel.Instantiate().hierarchy.ElementAt(0);
					newItem.dataSource = new ImprovementPanel<ImprovementEnginesConfig, ImprovementEnginesData>(this, newItem, _needResourceGroup, _improvements[key].Config, key);
					_upgradesBackground.Add(newItem);

				break;

				case "Searchlight":

					newItem = _upgradePanel.Instantiate().hierarchy.ElementAt(0);
					newItem.dataSource = new ImprovementPanel<ImprovementSearchlightConfig, ImprovementSearchlightData>(this, newItem, _needResourceGroup, _improvements[key].Config, key);
					_upgradesBackground.Add(newItem);
				break;

				case "SearchlightPower":
					newItem = _upgradePanel.Instantiate().hierarchy.ElementAt(0);
					newItem.dataSource = new ImprovementPanel<ImprovementSearchlightPowerConfig, ImprovementSearchlightPowerData>(this, newItem, _needResourceGroup, _improvements[key].Config, key);
					_upgradesBackground.Add(newItem);
					break;
			}
		}
	}

	public Sprite GetResourceSprite(int id)
	{
		if (id <= 6) return _library.GetResourceBase(id).View;
		else return _library.GetCraftResourceBase(id-10).View;
	}

	public void AddImprovement(IImprovementBase improvement, string name) => _improvements[name] = improvement;

	public void UnlockImprovement(string name)
	{
		int i = 0;

		foreach (var key in _improvements.Keys)
		{
			if (key == name)
				switch (key)
				{
					case "Fuel":
						((ImprovementPanel<ImprovementFuelConfig, ImprovementFuelData>)_upgradesBackground.contentContainer[i].dataSource).Unlock();
					break;

					case "Health":
						((ImprovementPanel<ImprovementHealthConfig, ImprovementHealthData>)_upgradesBackground.contentContainer[i].dataSource).Unlock();
					break;

					case "Defense":
						((ImprovementPanel<ImprovementHealthConfig, ImprovementHealthData>)_upgradesBackground.contentContainer[i].dataSource).Unlock();
					break;

					case "FireDefense":
						((ImprovementPanel<ImprovementHealthConfig, ImprovementHealthData>)_upgradesBackground.contentContainer[i].dataSource).Unlock();
					break;

					case "Engines":
						((ImprovementPanel<ImprovementEnginesConfig, ImprovementEnginesData>)_upgradesBackground.contentContainer[i].dataSource).Unlock();
					break;

					case "Searchlight":
						((ImprovementPanel<ImprovementSearchlightConfig, ImprovementSearchlightData>)_upgradesBackground.contentContainer[i].dataSource).Unlock();
					break;

					case "SearchlightPower":
						((ImprovementPanel<ImprovementSearchlightPowerConfig, ImprovementSearchlightPowerData>)_upgradesBackground.contentContainer[i].dataSource).Unlock();
					break;
				}
			i++;
		}
	}

	public bool TryUpgrade(string name)
	{
		if (CheckInventoryResources.CheckResources(new Inventory[] { _playerInventory, _baseInventory }, _improvements[name].GetNeedResources()))
		{
			ShowMessagePanel(MatchName(name));
			return true;
		}
		else
		{
			ShowMessagePanel(MatchName("Resource"));
			return false;
		}
	}

	public void Upgrade(string improvementName) => _improvements[improvementName].Upgrade();

	private void LoadData(SaveDataClass.ImprovementData improvements)
	{
		for (int i = 0; i < improvements.ImprovementName.Count; i++)
		{
			for (int j = 0; j < improvements.ImprovementLevel[i]; j++)
			{
				Upgrade(improvements.ImprovementName[i]);
			}
		}
	}

	private void LoadData(SaveDataClass.ImprovementUnlockData improvements)
	{
		for (int i = 0; i < improvements.ImprovementName.Count; i++)
		{
			if (improvements.ImprovementUnlock[i]) UnlockImprovement(improvements.ImprovementName[i]);
		}
	}

	private void SaveData()
	{
		GameEvents.OnImprovementsSave?.Invoke(_improvements);

		Dictionary<string, bool> unlockedPanels = new Dictionary<string, bool>();

		int i = 0;

		foreach (var key in _improvements.Keys)
		{
			switch (key)
			{
				case "Fuel":
					if (((ImprovementPanel<ImprovementFuelConfig, ImprovementFuelData>)_upgradesBackground.contentContainer[i].dataSource).UnlockStatus()) unlockedPanels["Fuel"] = true;
					else unlockedPanels["Fuel"] = false;
					break;

				case "Health":
					if (((ImprovementPanel<ImprovementHealthConfig, ImprovementHealthData>)_upgradesBackground.contentContainer[i].dataSource).UnlockStatus()) unlockedPanels["Health"] = true;
					else unlockedPanels["Health"] = false;
					break;

				case "Defense":
					if (((ImprovementPanel<ImprovementHealthConfig, ImprovementHealthData>)_upgradesBackground.contentContainer[i].dataSource).UnlockStatus()) unlockedPanels["Defense"] = true;
					else unlockedPanels["Defense"] = false;
					break;

				case "FireDefense":
					if (((ImprovementPanel<ImprovementHealthConfig, ImprovementHealthData>)_upgradesBackground.contentContainer[i].dataSource).UnlockStatus()) unlockedPanels["FireDefense"] = true;
					else unlockedPanels["FireDefense"] = false;
					break;

				case "Engines":
					if (((ImprovementPanel<ImprovementEnginesConfig, ImprovementEnginesData>)_upgradesBackground.contentContainer[i].dataSource).UnlockStatus()) unlockedPanels["Engines"] = true;
					else unlockedPanels["Engines"] = false;
					break;

				case "Searchlight":
					if (((ImprovementPanel<ImprovementSearchlightConfig, ImprovementSearchlightData>)_upgradesBackground.contentContainer[i].dataSource).UnlockStatus()) unlockedPanels["Searchlight"] = true;
					else unlockedPanels["Searchlight"] = false;
					break;

				case "SearchlightPower":
					if (((ImprovementPanel<ImprovementSearchlightPowerConfig, ImprovementSearchlightPowerData>)_upgradesBackground.contentContainer[i].dataSource).UnlockStatus()) unlockedPanels["SearchlightPower"] = true;
					else unlockedPanels["SearchlightPower"] = false;
					break;
			}
			i++;
		}

		GameEvents.OnImprovementPanelsSave?.Invoke(unlockedPanels);
	}

	private void OnDisable()
	{
		GameEvents.OnImprovementOpen -= UnlockImprovement;

		GameEvents.OnImprovementsLoad -= LoadData;
		GameEvents.OnSave -= SaveData;
	}
}