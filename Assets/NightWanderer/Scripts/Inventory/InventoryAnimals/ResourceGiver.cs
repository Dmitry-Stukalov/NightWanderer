using UnityEngine;

public class ResourceGiver
{
	private PlayerInventoryBuilder _inventoryBuilder;
	private Timer _givePauseTimer;
	private int _index;

	public ResourceGiver(PlayerInventoryBuilder inventoryBuilder, int index)
	{
		_inventoryBuilder = inventoryBuilder;
		_index = index;

		_givePauseTimer = new Timer(5);
		_givePauseTimer.OnTimerEnd += TryGiveResource;
	}

	private void TryGiveResource()
	{
		if (_index % 8 != 0 && _inventoryBuilder.GetResourceBase(_index - 1).ID == -1) Debug.Log(true); 
	}
}
