using System.Collections.Generic;
using UnityEngine;

public class EaterBugResource : ResourceBase
{
	private Timer _eatTimer;
	private Timer _lifeTimer;
	private int _leftCell = -1, _rightCell = -1, _upCell = -1, _downCell = -1;
	private int _currentCell = -1;

	public EaterBugResource(Sprite view, string name, int id) : base(view, name, id, 1, 1)
	{
		_eatTimer = new Timer(3);
		_lifeTimer = new Timer(600);
	}

	public override void Tick(Inventory inventory, IResourceFactory factory, int id, float deltaTime)
	{
		_lifeTimer.Tick(deltaTime);

		if (_lifeTimer.TimerIsEnd) inventory.DeleteResource(id, this);

		_eatTimer.Tick(deltaTime);

		if (!_eatTimer.TimerIsEnd) return;

		_leftCell = -1;
		_rightCell = -1;
		_upCell = -1;
		_downCell = -1;
		_currentCell = id;

		if (id % 8 != 0 && id != 0)
		{
			_leftCell = id - 1;

			if (inventory.GetResourceData(_leftCell).GetId() == -1) _leftCell = -2;
		}

		if ((id + 1) % 8 != 0)
		{
			_rightCell = id + 1;

			if (inventory.GetResourceData(_rightCell).GetId() == -1) _rightCell = -2;
		}

		if (id > 7)
		{
			_upCell = id - 8;

			if (inventory.GetResourceData(_upCell).GetId() == -1) _upCell = -2;
		}

		if (id < 24)
		{
			_downCell = id + 8;

			if (inventory.GetResourceData(_downCell).GetId() == -1) _downCell = -2;
		}

		if (_leftCell > -1 || _rightCell > -1 || _upCell > -1 || _downCell > -1) EatResource(inventory);
		else Move(inventory, factory);
	}

	private void EatResource(Inventory inventory)
	{
		List<int> arr = new List<int>();
		if (_leftCell > -1) arr.Add(_leftCell);
		if (_rightCell > -1) arr.Add(_rightCell);
		if (_upCell > -1) arr.Add(_upCell);
		if (_downCell > -1) arr.Add(_downCell);

		int randomResource = arr[Random.Range(0, arr.Count)];

		ResourceBase deletedResource = inventory.GetResourceData(randomResource).GetResource();
		deletedResource = new ResourceBase(deletedResource.View, deletedResource.Name, deletedResource.ID, deletedResource.MaxCount, 1);

		inventory.DeleteResource(randomResource, deletedResource);

		_eatTimer.ResetTimer(false);
	}

	private void Move(Inventory inventory, IResourceFactory factory)
	{
		List<int> arr = new List<int>();
		if (_leftCell == -2) arr.Add(_currentCell - 1);
		if (_rightCell == -2) arr.Add(_currentCell + 1);
		if (_upCell == -2) arr.Add(_currentCell - 8);
		if (_downCell == -2) arr.Add(_currentCell + 8);

		inventory.AddResource(factory, ID, arr[Random.Range(0, arr.Count)], 1);
		inventory.DeleteResource(_currentCell, this);

		_eatTimer.ResetTimer(false);
	}
}
