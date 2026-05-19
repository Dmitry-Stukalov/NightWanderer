using UnityEngine;

public class ResourceEater : ResourceBase
{
	private int _currentXPosition;
	private int _currentYPosition;

	public void AddedInInventory(int x, int y)
	{
		_currentXPosition = x;
		_currentYPosition = y;
	}
}
