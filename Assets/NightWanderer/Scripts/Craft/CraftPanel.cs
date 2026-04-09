using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class CraftPanel
{
	private CraftManager _craftManager;
	private VisualTreeAsset _inventoryCell;
	private VisualElement _craftPanel;
	private VisualTreeAsset _needResourceGroup;
	private VisualElement _craftIcon;
	private VisualElement _needResourceGroupPlace;
	private VisualElement _createdCell;
	private VisualElement _cellResource;
	private Label _craftVisualName;
	private Button _craftButton;
	private ResourceCraftData _resourceCraftData;
	private int _ID;
	private bool IsUnlock;

	public CraftPanel(CraftManager craftManager, VisualElement craftPanel, VisualTreeAsset needResourceGroup, VisualTreeAsset inventoryCell, ResourceCraftData resourceCraftData, int id)
	{
		_craftManager = craftManager;
		_craftPanel = craftPanel;
		_needResourceGroup = needResourceGroup;
		_craftIcon = craftPanel.Q<VisualElement>("CraftIcon");
		_needResourceGroupPlace = craftPanel.Q<VisualElement>("NeedResourcesIcons");

		_craftVisualName = craftPanel.Q<Label>("CraftName");
		_craftVisualName.text = resourceCraftData.Name;

		_craftButton = craftPanel.Q<Button>("CreateButton");
		_craftButton.RegisterCallback<ClickEvent>(Create);

		_createdCell = craftPanel.Q<VisualElement>("CreatedCell");
		_resourceCraftData = resourceCraftData;

		_inventoryCell = inventoryCell;

		var newCell = _inventoryCell.Instantiate();
		newCell.hierarchy.ElementAt(0).dataSource = new CellObject(true);

		_cellResource = newCell.Q<VisualElement>("CellResource");
		newCell.Q<VisualElement>("CellResource").dataSource = new ResourceCellObject();
		newCell.Q<VisualElement>("CellResource").AddManipulator(new DraggableManipulator(newCell, false));
		newCell.hierarchy.ElementAt(0).AddToClassList("BorderCell");

		_createdCell.Add(newCell);

		IsUnlock = true;
		_ID = id;
	}

	private void Create(ClickEvent evt)
	{
		if (_craftManager.TryCraft(_ID) && IsUnlock)
		{
			((ResourceCellObject)_cellResource.Q<VisualElement>("CellResource").dataSource).AddResource(new ResourceBase(_resourceCraftData.View, _resourceCraftData.Name, _resourceCraftData.ID, _resourceCraftData.MaxCount, 1));
		}
		else Debug.Log("Не хватает ресурсов");
	}

	public void Unlock() => IsUnlock = true;
}
