using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class CraftPanel
{
	private CraftManager _craftManager;
	private VisualElement _cellResource;
	private ResourceCraftData _resourceCraftData;
	private int _ID;
	private bool IsUnlock;

	public CraftPanel(CraftManager craftManager, VisualElement craftPanel, VisualTreeAsset needResourceGroup, VisualTreeAsset inventoryCell, ResourceCraftData resourceCraftData, int id)
	{
		_resourceCraftData = resourceCraftData;

		_craftManager = craftManager;

		VisualElement craftIcon = craftPanel.Q<VisualElement>("CraftIcon");
		craftIcon.style.backgroundImage = new StyleBackground(_resourceCraftData.View);

		VisualElement needResourceGroupPlace = craftPanel.Q<VisualElement>("NeedResourcesIcons");

		Label craftVisualName = craftPanel.Q<Label>("CraftName");
		craftVisualName.text = resourceCraftData.Name;

		Button craftButton = craftPanel.Q<Button>("CreateButton");
		craftButton.RegisterCallback<ClickEvent>(Create);

		VisualElement createdCell = craftPanel.Q<VisualElement>("CreatedCell");

		for (int i = 0; i < _resourceCraftData.ResourcesIDToCraft.Count; i++)
		{
			var newResourceGroup = needResourceGroup.Instantiate().hierarchy.ElementAt(0);
			newResourceGroup.Q<Label>("NeedResourceCount").text = $"{_resourceCraftData.ResourcesCountToCraft[i]}";
			newResourceGroup.Q<VisualElement>("NeedResourceIcon").style.backgroundImage = new StyleBackground(_craftManager.GetResourceSprite(_resourceCraftData.ResourcesIDToCraft[i]));
			needResourceGroupPlace.Add(newResourceGroup);
		}

		var newCell = inventoryCell.Instantiate();
		newCell.hierarchy.ElementAt(0).dataSource = new CellObject(true);

		_cellResource = newCell.Q<VisualElement>("CellResource");
		newCell.Q<VisualElement>("CellResource").dataSource = new ResourceCellObject(new Vector2Int(0, 0));
		newCell.Q<VisualElement>("CellResource").AddManipulator(new DraggableManipulator(newCell.Q<VisualElement>("CellResource"), false));
		newCell.hierarchy.ElementAt(0).AddToClassList("BorderCell");

		createdCell.Add(newCell);

		IsUnlock = true;
		_ID = id;
	}

	private void Create(ClickEvent evt)
	{
		if (IsUnlock)
		{
			if (_craftManager.TryCraft(_ID)) ((ResourceCellObject)_cellResource.Q<VisualElement>("CellResource").dataSource).AddResource(new ResourceBase(_resourceCraftData.View, _resourceCraftData.Name, _resourceCraftData.ID, _resourceCraftData.MaxCount, 1));
			else Debug.Log("Не хватает ресурсов");
		}
	}

	public void Unlock() => IsUnlock = true;
}
