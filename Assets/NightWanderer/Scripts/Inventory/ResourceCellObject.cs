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
	private Vector2Int ID;

	public event Action OnUpdate;

	public ResourceCellObject(Vector2Int id)
	{
		_resource = new ResourceBase();

		_resource.CurrentCount = 0;

		ID = id;

		OnPropertyChanged(nameof(Resource.CurrentCount));
		OnPropertyChanged(nameof(Resource.View));
		OnPropertyChanged(nameof(IsVisible));
		OnPropertyChanged(nameof(IsCountVisible));
	}

	public Vector2Int GetCellID() => ID;
	public void SetCellID(Vector2Int id) => ID = id;
	public int GetId() => _resource.ID;

	public ResourceBase AddResource(ResourceBase resource)
	{
		if (resource == null || resource.ID == -1 || _resource.ID != resource.ID && _resource.ID != -1) return resource;

		if (_resource.ID == -1)
		{
			if (resource is ProducerBugResource) _resource = new ProducerBugResource(resource.View, resource.Name, resource.ID);
			else if (resource is StoneBugResource) _resource = new StoneBugResource(resource.View, resource.Name, resource.ID);
			else if (resource is EaterBugResource) _resource = new EaterBugResource(resource.View, resource.Name, resource.ID);
			else
			{
				_resource = new ResourceBase(resource.View, resource.Name, resource.ID, resource.MaxCount, resource.CurrentCount);

				//_resource.View = resource.View;
				//_resource.Name = resource.Name;
				//_resource.ID = resource.ID;
				//_resource.CurrentCount = resource.CurrentCount;
				//_resource.MaxCount = resource.MaxCount;
			}

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
		OnPropertyChanged(nameof(IsCountVisible));

		OnUpdate?.Invoke();
		return resource;
	}

	public int DeleteResource(ResourceBase resource)
	{
		if (resource == null || resource.ID == -1 || _resource.ID == -1) return 0;

		if (_resource.CurrentCount <= resource.CurrentCount)
		{
			ResetResource();

			OnPropertyChanged(nameof(Resource.CurrentCount));
			OnPropertyChanged(nameof(Resource.View));
			OnPropertyChanged(nameof(Resource.Name));
			OnPropertyChanged(nameof(IsVisible));
			OnPropertyChanged(nameof(IsCountVisible));

			return 0;
		}
		else
		{
			_resource.CurrentCount -= resource.CurrentCount;

			OnPropertyChanged(nameof(Resource.CurrentCount));
			OnPropertyChanged(nameof(Resource.View));
			OnPropertyChanged(nameof(IsVisible));
			OnPropertyChanged(nameof(IsCountVisible));

			return _resource.CurrentCount;
		}
	}

	public void UpdateData()
	{
		OnPropertyChanged(nameof(Resource.CurrentCount));
		OnPropertyChanged(nameof(Resource.View));
		OnPropertyChanged(nameof(Resource.Name));
		OnPropertyChanged(nameof(IsVisible));
		OnPropertyChanged(nameof(IsCountVisible));
	}

	public void UpdateCell(Inventory inventory, IResourceFactory factory, float deltaTime)
	{
		if (_resource.ID != -1) _resource.Tick(inventory, factory, ID, deltaTime);
	}

	public void ResetResource() => _resource.ResetValue();

	public int GetResourceCount() => _resource.CurrentCount;

	public int GetMaxResourceCount() => _resource.MaxCount;

	public int GetEmptyResourceCount() => _resource.MaxCount - _resource.CurrentCount;

	public void SetResourceCount(int count) => _resource.CurrentCount = count;
	public void SubtractResourceCount (int count) => _resource.CurrentCount -= count;

	public ResourceBase GetResource() => _resource;

	[CreateProperty]
	public DisplayStyle IsVisible => _resource.CurrentCount > 0 ? DisplayStyle.Flex : DisplayStyle.None;

	[CreateProperty]
	public DisplayStyle IsCountVisible => _resource.CurrentCount == 1 ? DisplayStyle.None : DisplayStyle.Flex;
	public event PropertyChangedEventHandler PropertyChanged;

	protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}