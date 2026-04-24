using UnityEngine;
using UnityEngine.UIElements;

public class InventoryStatistic
{
	private Inventory _inventory;
	private Label _text;

	public InventoryStatistic(Label text, Inventory inventory)
	{
		_text = text;
		_inventory = inventory;
		//_inventory.OnUpgrade += UpdateData;
		UpdateData();
	}

	private void UpdateData()
	{
		_text.text = $"»нвентарь: {_inventory.GetCellCount()} €чеек.";
	}
}
