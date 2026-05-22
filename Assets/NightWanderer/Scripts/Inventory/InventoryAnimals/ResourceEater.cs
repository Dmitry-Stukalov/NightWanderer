using UnityEngine;
using UnityEngine.Rendering;

public class ResourceEater
{
	private PlayerInventoryBuilder _inventoryBuilder;
	private Timer _eaterPauseTimer;
	private int _index;
	private int _currentXPosition;
	private int _currentYPosition;

	public ResourceEater(PlayerInventoryBuilder inventoryBuilder, int index)
	{
		_eaterPauseTimer = new Timer(3);
		_eaterPauseTimer.OnTimerEnd += TryEatResource;

		_inventoryBuilder = inventoryBuilder;
		
		_index = index;
		_currentYPosition = (int)(index / 8);
		_currentXPosition = index - _currentYPosition * 8;
	}

	private void TryEatResource()
	{
		int resourceIndex = _inventoryBuilder.GetResourceNearbyIndex(_index);
		int resourceXPosition = (int)(resourceIndex / 8);
		int resourceYPosition = resourceIndex - resourceXPosition * 8;

		if (EatResource(resourceXPosition, resourceYPosition)) return;

		MoveToResource(resourceXPosition, resourceYPosition);
	}

	private bool EatResource(int resourceXPosition, int resourceYPosition)
	{
		if (_currentXPosition == resourceXPosition - 1)
		{
			_inventoryBuilder.EatResource(_index - 1);
			return true;
		}

		if (_currentXPosition == resourceXPosition + 1)
		{
			_inventoryBuilder.EatResource(_index + 1);
			return true;
		}

		if (_currentYPosition == resourceYPosition - 1)
		{
			_inventoryBuilder.EatResource(_index - 8);
			return true;
		}

		if (_currentYPosition == resourceYPosition + 1)
		{
			_inventoryBuilder.EatResource(_index + 8);
			return true;
		}

		return false;
	}

	private void MoveToResource(int resourceXPosition, int resourceYPosition)
	{

	}

	public void Update()
	{
		_eaterPauseTimer.Tick(Time.deltaTime);
	}

	public void OnDisable()
	{
		_eaterPauseTimer.OnTimerEnd -= TryEatResource;
	}
}
