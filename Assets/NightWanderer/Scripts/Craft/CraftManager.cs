using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using TMPro;

//Нужно доделать
public class CraftManager : MonoBehaviour
{
	[SerializeField] private UIDocument _baseUI;
	[SerializeField] private VisualTreeAsset _craftPanel;
	[SerializeField] private VisualTreeAsset _needResourcesGroup;
	[SerializeField] private VisualTreeAsset _inventoryCell;
	[SerializeField] private ResourceCraftConfig _craftConfig;
	[SerializeField] private ResourceLibrary _library;
	private ScrollView _craftBackground;
	private Inventory _playerInventory;
	private Inventory _baseInventory;
	private Dictionary<int, int> _resources = new Dictionary<int, int>();
	private Dictionary<string, VisualElement> _panels = new Dictionary<string, VisualElement>();

	public void Initializing(Inventory playerInventory, Inventory baseInventory, ResourceLibrary library)
	{
		_library = library;
		_playerInventory = playerInventory;
		_baseInventory = baseInventory;

		for (int i = 0; i < 13; i++)
		{
			_resources[i] = 0;
		}

		_craftBackground = _baseUI.rootVisualElement.Q<ScrollView>("CraftBackground");

		for (int i = 0; i < _craftConfig.CraftResources.Count; i++)
		{
			var newPanel = _craftPanel.Instantiate();
			newPanel.dataSource = new CraftPanel(this, newPanel, _needResourcesGroup, _inventoryCell, _craftConfig.CraftResources[i], i);

			newPanel.style.display = DisplayStyle.None;
			_craftBackground.Add(newPanel);

			_panels[_craftConfig.CraftResources[i].Name] = newPanel;
		}

		GameEvents.OnCraftOpen += UnlockCraft;
		UnlockCraft("Прожектор");
	}

	public Sprite GetResourceSprite(int id) => _library.GetResourceBase(id).View;

	public void UnlockCraft(string name)
	{
		((CraftPanel)_panels[name].dataSource).Unlock();
		_panels[name].style.display = DisplayStyle.Flex;
	}

	public bool TryCraft(int id)
	{
		if (CheckResources(_craftConfig.CraftResources[id]))
		{
			SubtractResources(_craftConfig.CraftResources[id]);
			return true;
		}
		else return false;
	}

	private bool CheckResources(ResourceCraftData craftResources)
	{
		int t = 0;
		Dictionary<int, int> newDictionary = new Dictionary<int, int>();

		for (int i = 0; i < craftResources.ResourcesIDToCraft.Count; i++) _resources[i] = 0;

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


		newDictionary = ListsToDictionary(craftResources);

		foreach (var key in _resources.Keys)
		{
			if (newDictionary.ContainsKey(key))
				if (newDictionary[key] <= _resources[key]) t++;
		}

		if (t == newDictionary.Count) return true;
		else return false;
	}

	private Dictionary<int, int> ListsToDictionary(ResourceCraftData data)
	{
		Dictionary<int, int> newDictionary = new Dictionary<int, int>();

		for (int i = 0; i < data.ResourcesIDToCraft.Count; i++)
		{
			newDictionary[data.ResourcesIDToCraft[i]] = data.ResourcesCountToCraft[i];
		}

		return newDictionary;
	}

	private void SubtractResources(ResourceCraftData craftResources)
	{
		int t = 0;
		Dictionary<int, int> newDictionary = new Dictionary<int, int>();

		newDictionary = ListsToDictionary(craftResources);
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
	}

	private void OnDisable()
	{
		GameEvents.OnCraftOpen -= UnlockCraft;
	}
}
