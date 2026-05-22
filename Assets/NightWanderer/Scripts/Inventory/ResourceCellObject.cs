using System;
using UnityEngine.UIElements;
using System.ComponentModel;
using Unity.Properties;
using UnityEngine;

//Хранит информацию о ресурсе в ячейке + отвечает за перетаскивание этого ресурса в пределах инвентаря
public class ResourceCellObject
{
	[CreateProperty]
	public ResourceBase _resource { get; private set; }
	private ResourceGiver _resourceGiver;
	//private PlayerInventoryBuilder _inventoryBuilder;
	//private Timer _eaterPauseTimer;
	//private int _index;
	//private int _currentXPosition;
	//private int _currentYPosition;


	public event Action OnUpdate;

	public ResourceCellObject(/*PlayerInventoryBuilder inventoryBuilder, int index*/)
	{
		_resource = new ResourceBase();

		_resource.CurrentCount = 0;

		//_eaterPauseTimer = new Timer(3);
		//_eaterPauseTimer.OnTimerEnd += EatResource;
		//_eaterPauseTimer.SetPause();

		//_inventoryBuilder = inventoryBuilder;
		//_index = index;

		OnPropertyChanged(nameof(Resource.CurrentCount));
		OnPropertyChanged(nameof(Resource.View));
		OnPropertyChanged(nameof(IsVisible));
	}

	public int GetId() => _resource.ID;

	public ResourceBase AddResource(ResourceBase resource)
	{
		if (resource == null || resource.ID == -1 || _resource.ID != resource.ID && _resource.ID != -1) return resource;

		if (_resource.ID == -1)
		{
			_resource.View = resource.View;
			_resource.Name = resource.Name;
			_resource.ID = resource.ID;
			_resource.CurrentCount = resource.CurrentCount;
			_resource.MaxCount = resource.MaxCount;
			resource.CurrentCount = 0;

		}
		else
		{
			if (_resource.CurrentCount + resource.CurrentCount <= _resource.MaxCount)
			{
				_resource.CurrentCount += resource.CurrentCount;
				resource.CurrentCount = 0;
			}
			else
			{
				int countDifference = _resource.MaxCount - _resource.CurrentCount;
				_resource.CurrentCount = _resource.MaxCount;
				resource.CurrentCount -= countDifference;
			}
		}
		
		OnPropertyChanged(nameof(Resource.CurrentCount));
		OnPropertyChanged(nameof(Resource.View));
		OnPropertyChanged(nameof(Resource.Name));
		OnPropertyChanged(nameof(IsVisible));

		OnUpdate?.Invoke();
		return resource;
	}

	public int DeleteResource(ResourceBase resource)
	{
		if (resource == null || resource.ID == -1 || _resource.ID == -1) return 0;

		if (_resource.CurrentCount <= resource.CurrentCount)
		{
			_resource.ResetValue();

			OnPropertyChanged(nameof(Resource.CurrentCount));
			OnPropertyChanged(nameof(Resource.View));
			OnPropertyChanged(nameof(Resource.Name));
			OnPropertyChanged(nameof(IsVisible));

			return 0;
		}
		else
		{
			_resource.CurrentCount -= resource.CurrentCount;

			OnPropertyChanged(nameof(Resource.CurrentCount));
			OnPropertyChanged(nameof(Resource.View));
			OnPropertyChanged(nameof(IsVisible));

			return _resource.CurrentCount;
		}
	}

	//private void EatResource()
	//{
	//	int resourceIndex = _inventoryBuilder.GetResourceNearbyIndex(_index);

	//	if (resourceIndex == _index - 1 || resourceIndex == _index + 1 || resourceIndex == _index - 8 || resourceIndex == _index + 8)
	//	{
	//		_inventoryBuilder.EatResource(resourceIndex);
	//	}
	//	else
	//	{
	//		switch (resourceIndex)
	//		{
	//			case resourceIndex - index
	//		}
	//	}
	//}

	public void ResetResource() => _resource.ResetValue();

	public int GetResourceCount() => _resource.CurrentCount;

	public int GetMaxResourceCount() => _resource.MaxCount;

	public int GetEmptyResourceCount() => _resource.MaxCount - _resource.CurrentCount;

	public void SetResourceCount(int count) => _resource.CurrentCount = count;

	public ResourceBase GetResource() => _resource;

	[CreateProperty]
	public DisplayStyle IsVisible => _resource.CurrentCount > 0 ? DisplayStyle.Flex : DisplayStyle.None;
	public event PropertyChangedEventHandler PropertyChanged;

	protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}