using UnityEngine;
using System.Collections.Generic;

public class ProducerBugResource : ResourceBase
{
	private Timer _producerTimer;
	private Timer _lifeTimer;
	private Vector2Int _resultCell = new Vector2Int(-1, -1);
	
	public ProducerBugResource(Sprite view, string name, int iD) : base(view, name, iD, 1, 1)
	{
		_producerTimer = new Timer(15);
		_lifeTimer = new Timer(600);
	}

	public override void Tick(Inventory inventory, IResourceFactory factory, Vector2Int id, float deltaTime)
	{
		_lifeTimer.Tick(deltaTime);

		if (_lifeTimer.TimerIsEnd) inventory.DeleteResource(id, this);

		_producerTimer.Tick(deltaTime);

		if (!_producerTimer.TimerIsEnd) return;

		int resourceID = Random.Range(0, 7);

		_resultCell = new Vector2Int(-1, -1);

		Vector2Int[] directions = new Vector2Int[] { Vector2Int.left, Vector2Int.right, Vector2Int.up, Vector2Int.down};
		List<Vector2Int> existingCells = new List<Vector2Int>();

		foreach (var direction in directions)
		{
			if (inventory.CheckCell(id + direction))
			{
				existingCells.Add(id  + direction);

				if (inventory.GetResourceData(id + direction).GetId() == resourceID)
				{
					_resultCell = id + direction;
					GetResource(inventory, factory, resourceID);
					return;
				}
			}
		}

		int randomNumber = Random.Range(0, existingCells.Count);

		_resultCell = existingCells[randomNumber];
		GetResource(inventory, factory, resourceID);
	}

	private void GetResource(Inventory inventory, IResourceFactory factory, int resource)
	{
		inventory.AddResource(factory, resource, _resultCell, 2);

		_producerTimer.ResetTimer(false);
	}
}
