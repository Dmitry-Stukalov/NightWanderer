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
	private Inventory _playerInventory;
	private Inventory _baseInventory;
	private Dictionary<string, VisualElement> _panels = new Dictionary<string, VisualElement>();

	public void Initializing(Inventory playerInventory, Inventory baseInventory, ResourceLibrary library)
	{
		_library = library;
		_playerInventory = playerInventory;
		_baseInventory = baseInventory;

		ScrollView craftBackground = _baseUI.rootVisualElement.Q<ScrollView>("CraftBackground");

		for (int i = 0; i < _craftConfig.CraftResources.Count; i++)
		{
			var newPanel = _craftPanel.Instantiate().hierarchy.ElementAt(0);
			newPanel.dataSource = new CraftPanel(this, newPanel, _needResourcesGroup, _inventoryCell, _craftConfig.CraftResources[i], i);

			newPanel.style.display = DisplayStyle.None;
			craftBackground.Add(newPanel);

			_panels[_craftConfig.CraftResources[i].Name] = newPanel;
		}

		GameEvents.OnCraftOpen += UnlockCraft;
		UnlockCraft("Прожектор");
		UnlockCraft("Ячейка");
	}

	public Sprite GetResourceSprite(int id) => _library.GetResourceBase(id).View;

	public void UnlockCraft(string name)
	{
		((CraftPanel)_panels[name].dataSource).Unlock();
		_panels[name].style.display = DisplayStyle.Flex;
	}

	public bool TryCraft(int id)
	{
		if (CheckInventoryResources.CheckResources(new Inventory[] { _playerInventory, _baseInventory }, ListsToDictionary(_craftConfig.CraftResources[id]))) return true;
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

	private void OnDisable()
	{
		GameEvents.OnCraftOpen -= UnlockCraft;
	}
}
