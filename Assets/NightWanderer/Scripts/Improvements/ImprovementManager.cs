using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine.UI;
using System.Collections;
using NUnit.Framework.Constraints;
using UnityEditorInternal.Profiling.Memory.Experimental;

public class ImprovementManager : MonoBehaviour
{
	[SerializeField] private UIDocument _baseUI;
	[SerializeField] private VisualTreeAsset _upgradePanel;
	[SerializeField] private VisualTreeAsset _needResourceGroup;
	[SerializeField] private ResourceLibrary _library;
	[SerializeField] private int _upgradesCount;
	private Dictionary<string, IImprovementBase> _improvements = new Dictionary<string, IImprovementBase>();
	private Inventory _playerInventory;
	private Inventory _baseInventory;
	private Dictionary<int, int> _resources = new Dictionary<int, int>();

	private List<int> _upgradesList = new List<int>();
	private ScrollView _upgradesBackground;

	public void Initializing(Inventory playerInventory, Inventory baseInventory)
	{
		_playerInventory = playerInventory;
		_baseInventory = baseInventory;

		for (int i = 0; i < 9; i++)
		{
			_resources[i] = 0;
		}

		_upgradesBackground = _baseUI.rootVisualElement.Q<ScrollView>("UpgradesBackground");

		GameEvents.OnImprovementOpen += UnlockImprovement;

		StartCoroutine(StartPause());
	}

	private IEnumerator StartPause()
	{
		yield return new WaitForSeconds(3);

		var newItem = _upgradePanel.Instantiate();

		foreach (var key in _improvements.Keys)
		{
			switch (key)
			{
				case "Fuel":

					newItem = _upgradePanel.Instantiate();
					newItem.dataSource = new ImprovementPanel<ImprovementFuelConfig, ImprovementFuelData>(this, newItem, _needResourceGroup, _improvements[key].Config, key);
					_upgradesBackground.Add(newItem);

				break;

				case "Mining":

					newItem = _upgradePanel.Instantiate();
					newItem.dataSource = new ImprovementPanel<ImprovementMiningConfig, ImprovementMiningData>(this, newItem, _needResourceGroup, _improvements[key].Config, key);
					_upgradesBackground.Add(newItem);
				break;

				case "Health" or "Defense" or "FireDefense":

					newItem = _upgradePanel.Instantiate();
					newItem.dataSource = new ImprovementPanel<ImprovementHealthConfig, ImprovementHealthData>(this, newItem, _needResourceGroup, _improvements[key].Config, key);
					_upgradesBackground.Add(newItem);

				break;

				case "Engines":

					newItem = _upgradePanel.Instantiate();
					newItem.dataSource = new ImprovementPanel<ImprovementEnginesConfig, ImprovementEnginesData>(this, newItem, _needResourceGroup, _improvements[key].Config, key);
					_upgradesBackground.Add(newItem);

				break;
			}
		}
	}

	public Sprite GetResourceSprite(int id) => _library.GetResourceBase(id).View;
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
				}
			i++;
		}
	}

	public bool TryUpgrade(string name)
	{
		if (CheckResources(_improvements[name]))
		{
			SubtractResources(_improvements[name], name);
			return true;
		}
		else return false;
	}

	private bool CheckResources(IImprovementBase improvement)
	{
		int t = 0;
		Dictionary<int, int> newDictionary = new Dictionary<int, int>();

		for (int i = 0; i < _resources.Count; i++) _resources[i] = 0;

		for (int i = 0; i < _playerInventory.GetCellCount(); i++)
		{
			if (_playerInventory.GetResourceData(i).GetId() != -1)
				_resources[_playerInventory.GetResourceData(i).GetId()] += _playerInventory.GetResourceData(i).GetResourceCount();
		}

		for (int i = 0; i < _baseInventory.GetCellCount(); i++)
		{
			if (_baseInventory.GetResourceData(i).GetId() != -1)
				_resources[_baseInventory.GetResourceData(i).GetId()] += _baseInventory.GetResourceData(i).GetResourceCount();
		}


		if (improvement is IImprovementBase improvementBase)
		{
			newDictionary = improvementBase.GetNeedResources();

			foreach (var key in _resources.Keys)
			{
				if (newDictionary.ContainsKey(key))
					if (newDictionary[key] <= _resources[key]) t++;
			}
		}

		if (t == newDictionary.Count) return true;
		else return false;	
	}

	private void SubtractResources(IImprovementBase improvement, string name)
	{
		int t = 0;
		Dictionary<int, int> newDictionary = new Dictionary<int, int>();

		if (improvement is IImprovementBase improvementBase)
		{
			newDictionary = improvementBase.GetNeedResources();
			var keys = new List<int>(newDictionary.Keys);
			//Проходимся по всем нужным ресурсам
			foreach (var key in keys)
			{
				//Проходимся по всем ячейкам инвентаря игрока
				for (int i = 0; i < _playerInventory.GetCellCount(); i++)
				{
					//Если в ячейке лежит нужный ресурс
					if (key == _playerInventory.GetResourceData(i).GetId())
					{
						//Если нужно меньше ресурсов, чем лежит в ячейке, то просто меняем в ней количество / иначе делаем ячейку пустой и идем дальше
						if (newDictionary[key] < _playerInventory.GetResourceData(i).GetResourceCount() && newDictionary[key] != 0)
						{
							_playerInventory.GetResourceData(i).SetResourceCount(_playerInventory.GetResourceData(i).GetResourceCount() - newDictionary[key]);
							newDictionary[key] = 0;
							t++;
							break;
						}
						else
						{
							newDictionary[key] -= _playerInventory.GetResourceData(i).GetResourceCount();
							_playerInventory.GetResourceData(i).SetResourceCount(0);
						}
					}
				}
			}

			if (t != newDictionary.Count)
			{
				foreach (var key in keys)
				{
					//Проходимся по всем ячейкам инвентаря на базе
					for (int i = 0; i < _baseInventory.GetCellCount(); i++)
					{
						//Если в ячейке лежит нужный ресурс
						if (key == _baseInventory.GetResourceData(i).GetId())
						{
							//Если нужно меньше ресурсов, чем лежит в ячейке, то просто меняем в ней количество / иначе делаем ячейку пустой и идем дальше
							if (newDictionary[key] < _baseInventory.GetResourceData(i).GetResourceCount() && newDictionary[key] != 0)
							{
								_baseInventory.GetResourceData(i).SetResourceCount(_baseInventory.GetResourceData(i).GetResourceCount() - newDictionary[key]);
								newDictionary[key] = 0;
								t++;
								break;
							}
							else
							{
								newDictionary[key] -= _baseInventory.GetResourceData(i).GetResourceCount();
								_baseInventory.GetResourceData(i).SetResourceCount(0);
							}
						}
					}
				}
			}

			Upgrade(name);
		}
	}

	public void Upgrade(string improvementName) => _improvements[improvementName].Upgrade();

	private void OnDisable()
	{
		GameEvents.OnImprovementOpen -= UnlockImprovement;
	}
}