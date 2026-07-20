using System.Collections.Generic;
using UnityEngine;

public class EaterBugResource : ResourceBase
{
	private Timer _eatTimer;
	private Timer _lifeTimer;
	private bool _leftCell = false, _rightCell = false, _upCell = false, _downCell = false;
	private bool _leftCellEmpty = false, _rightCellEmpty = false, _upCellEmpty = false, _downCellEmpty = false;
	//private int _leftCell = -1, _rightCell = -1, _upCell = -1, _downCell = -1;
	//private Vector2Int _resultCell = new Vector2Int(-1, -1);

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

		_leftCell = false;
		_rightCell = false;
		_upCell = false;
		_downCell = false;

		_leftCellEmpty = false;
		_rightCellEmpty = false;
		_upCellEmpty = false;
		_downCellEmpty = false;

		//_resultCell = new Vector2Int(-1, -1);

		//_leftCell = false;
		//_rightCell = false;
		//_upCell = false;
		//_downCell = false;
		//_currentCell = id;


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
		else EatResource(inventory, resourceCells, id);

		/*if (inventory.CheckCell(id += Vector2Int.left))
		{
			_leftCell = true;

			if (inventory.GetResourceData(id += Vector2Int.left).GetId() == -1) _leftCellEmpty = true;
		}

		if (inventory.CheckCell(id += Vector2Int.right))
		{
			_rightCell = true;

			if (inventory.GetResourceData(id += Vector2Int.right).GetId() == -1) _rightCellEmpty = true;
		}

		if (inventory.CheckCell(id += Vector2Int.up))
		{
			_upCell = true;

			if (inventory.GetResourceData(id += Vector2Int.up).GetId() == -1) _upCellEmpty = true;
		}

		if (inventory.CheckCell(id += Vector2Int.down))
		{
			_downCell = true;

			if (inventory.GetResourceData(id += Vector2Int.down).GetId() == -1) _downCellEmpty = true;
		}*/
		
		/*if (_leftCellEmpty && _rightCellEmpty && _upCellEmpty && _downCellEmpty) EatResource(inventory, id);
		else Move(inventory, factory, id);*/

		//if (id % 8 != 0 && id != 0)
		//{
		//	_leftCell = id - 1;

		//	if (inventory.GetResourceData(_leftCell).GetId() == -1) _leftCell = -2;
		//}

		//if ((id + 1) % 8 != 0)
		//{
		//	_rightCell = id + 1;

		//	if (inventory.GetResourceData(_rightCell).GetId() == -1) _rightCell = -2;
		//}

		//if (id > 7)
		//{
		//	_upCell = id - 8;

		//	if (inventory.GetResourceData(_upCell).GetId() == -1) _upCell = -2;
		//}

		//if (id < 24)
		//{
		//	_downCell = id + 8;

		//	if (inventory.GetResourceData(_downCell).GetId() == -1) _downCell = -2;
		//}

		//if (_leftCell > -1 || _rightCell > -1 || _upCell > -1 || _downCell > -1) EatResource(inventory);
		//else Move(inventory, factory);
	}

	private void EatResource(Inventory inventory, List<Vector2Int> resourceCells, Vector2Int id)
	{
		List<Vector2Int> arr = new List<Vector2Int>();

		if (!_leftCellEmpty && _leftCell) arr.Add(id += Vector2Int.left);
		if (!_rightCellEmpty && _rightCell) arr.Add(id += Vector2Int.right);
		if (!_upCellEmpty && _upCell) arr.Add(id += Vector2Int.up);
		if (!_downCellEmpty && _downCell) arr.Add(id += Vector2Int.down);

		//List<int> arr = new List<int>();
		//if (_leftCell > -1) arr.Add(_leftCell);
		//if (_rightCell > -1) arr.Add(_rightCell);
		//if (_upCell > -1) arr.Add(_upCell);
		//if (_downCell > -1) arr.Add(_downCell);

		Vector2Int randomResource = resourceCells[Random.Range(0, resourceCells.Count)];

		ResourceBase deletedResource = inventory.GetResourceData(randomResource).GetResource();
		deletedResource = new ResourceBase(deletedResource.View, deletedResource.Name, deletedResource.ID, deletedResource.MaxCount, 1);

		inventory.DeleteResource(randomResource, deletedResource);

		_eatTimer.ResetTimer(false);
	}

	private void Move(Inventory inventory, IResourceFactory factory, List<Vector2Int> emptyCells, Vector2Int id)
	{
		List<Vector2Int> arr = new List<Vector2Int>();

		if (_leftCellEmpty && _leftCell) arr.Add(id += Vector2Int.left);
		if (_rightCellEmpty && _rightCell) arr.Add(id += Vector2Int.right);
		if (_upCellEmpty && _upCell) arr.Add(id += Vector2Int.up);
		if (_downCellEmpty && _downCell) arr.Add(id += Vector2Int.down);

		//List<int> arr = new List<int>();
		//if (_leftCell == -2) arr.Add(_currentCell - 1);
		//if (_rightCell == -2) arr.Add(_currentCell + 1);
		//if (_upCell == -2) arr.Add(_currentCell - 8);
		//if (_downCell == -2) arr.Add(_currentCell + 8);

		inventory.AddResource(factory, ID, emptyCells[Random.Range(0, emptyCells.Count)], 1);
		inventory.DeleteResource(id, this);

		_eatTimer.ResetTimer(false);
	}
}
