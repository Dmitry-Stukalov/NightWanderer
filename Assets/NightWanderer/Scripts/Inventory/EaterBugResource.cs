using System.Collections.Generic;
using UnityEngine;

public class EaterBugResource : ResourceBase
{
	private Timer _eatTimer;
	private Timer _lifeTimer;

	public EaterBugResource(Sprite view, string name, int id) : base(view, name, id, 1, 1)
	{
		_eatTimer = new Timer(3);
		_lifeTimer = new Timer(600);
	}

	public override void Tick(Inventory inventory, IResourceFactory factory, Vector2Int id, float deltaTime)
	{
		_lifeTimer.Tick(deltaTime);

		if (_lifeTimer.TimerIsEnd) inventory.DeleteResource(id, this);

		_eatTimer.Tick(deltaTime);

		if (!_eatTimer.TimerIsEnd) return;

		Vector2Int[] directions = new Vector2Int[] { Vector2Int.left, Vector2Int.right, Vector2Int.up, Vector2Int.down };
		List<Vector2Int> existingCells = new List<Vector2Int>();
		List<Vector2Int> emptyCells = new List<Vector2Int>();
		List<Vector2Int> resourceCells = new List<Vector2Int>();

		foreach (var direction in directions)
		{
			if (inventory.CheckCell(id + direction))
			{
				existingCells.Add(id + direction);

				if (inventory.GetResourceData(id + direction).GetId() == -1) emptyCells.Add(id + direction);
				else resourceCells.Add(id + direction);
			}
		}

		if (emptyCells.Count == existingCells.Count) Move(inventory, factory, emptyCells, id);
		else EatResource(inventory, resourceCells);
	}

	private void EatResource(Inventory inventory, List<Vector2Int> resourceCells)
	{
		Vector2Int randomResource = resourceCells[Random.Range(0, resourceCells.Count)];

		ResourceBase deletedResource = inventory.GetResourceData(randomResource).GetResource();
		deletedResource = new ResourceBase(deletedResource.View, deletedResource.Name, deletedResource.ID, deletedResource.MaxCount, 1);

		inventory.DeleteResource(randomResource, deletedResource);

		_eatTimer.ResetTimer(false);
	}

	private void Move(Inventory inventory, IResourceFactory factory, List<Vector2Int> emptyCells, Vector2Int id)
	{
		inventory.AddResource(factory, ID, emptyCells[Random.Range(0, emptyCells.Count)], 1);
		inventory.DeleteResource(id, this);

		_eatTimer.ResetTimer(false);
	}
}
