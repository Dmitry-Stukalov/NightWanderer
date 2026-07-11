using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.Mathematics;
using UnityEngine;

public class ProducerBugResource : ResourceBase
{
	private Timer _producerTimer;
	private Timer _lifeTimer;
	private int _leftCell = -1, _rightCell = -1, _upCell = -1, _downCell = -1;
	private int _resultCell = -1;
	
	public ProducerBugResource(Sprite view, string name, int iD) : base(view, name, iD, 1, 1)
	{
		_producerTimer = new Timer(15);
		_lifeTimer = new Timer(600);
	}

	public override void Tick(Inventory inventory, IResourceFactory factory, int id, float deltaTime)
	{
		_lifeTimer.Tick(deltaTime);

		if (_lifeTimer.TimerIsEnd) inventory.DeleteResource(id, this);

		_producerTimer.Tick(deltaTime);

		if (!_producerTimer.TimerIsEnd) return;

		int resourceID = UnityEngine.Random.Range(0, 7);

		_leftCell = -1;
		_rightCell = -1;
		_upCell = -1;
		_downCell = -1;
		_resultCell = -1;

		if (id % 8 != 0 && id != 0)
		{
			_leftCell = id - 1;

			if (inventory.GetResourceData(_leftCell).GetId() == resourceID)
			{
				_resultCell = _leftCell;
				GetResource(inventory, factory, resourceID);
				return;
			}
			if (inventory.GetResourceData(_leftCell).GetId() != -1) _leftCell = -1;
		}

		if ((id + 1) % 8 != 0)
		{
			_rightCell = id + 1;

			if (inventory.GetResourceData(_rightCell).GetId() == resourceID)
			{
				_resultCell = _rightCell;
				GetResource(inventory, factory, resourceID);
				return;
			}
			if (inventory.GetResourceData(_rightCell).GetId() != -1) _rightCell = -1;
		}

		if (id > 7)
		{
			_upCell = id - 8;

			if (inventory.GetResourceData(_upCell).GetId() == resourceID)
			{
				_resultCell = _upCell;
				GetResource(inventory, factory, resourceID);
				return;
			}
			if (inventory.GetResourceData(_upCell).GetId() != -1) _upCell = -1;
		}

		if (id < 24)
		{
			_downCell = id + 8;

			if (inventory.GetResourceData(_downCell).GetId() == resourceID)
			{
				_resultCell = _downCell;
				GetResource(inventory, factory, resourceID);
				return;
			}
			if (inventory.GetResourceData(_downCell).GetId() != -1) _downCell = -1;
		}

		int randomNumber = UnityEngine.Random.Range(0, 32);

		int[] arr = new int[] { _leftCell, _rightCell, _upCell, _downCell };

		int resultNumber = GetClosestNumber(randomNumber, arr);

		if (resultNumber != -1)
		{
			_resultCell = arr[GetClosestNumber(randomNumber, arr)];

			GetResource(inventory, factory, resourceID);
		}
	}

	private void GetResource(Inventory inventory, IResourceFactory factory, int resource)
	{
		inventory.AddResource(factory, resource, _resultCell, 2);

		_producerTimer.ResetTimer(false);
	}

	private int GetClosestNumber(int targetNumber, int[] numberArray)
	{
		int minDiff = 100;
		int minID = -1;

		for (int i = 0; i < numberArray.Length; i++)
		{
			if (math.abs(targetNumber - numberArray[i]) < minDiff && numberArray[i] != -1)
			{
				minDiff = math.abs(targetNumber - numberArray[i]);
				minID = i;
			}
		}

		return minID;
	}
}
