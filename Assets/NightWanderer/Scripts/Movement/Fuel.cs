using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using Unity.Properties;
using UnityEngine.UIElements;
public class Fuel : IImprovementBase
{
	public string Name { get; set; }
	public Dictionary<int, int> _needResources { get; set; } = new Dictionary<int, int>();
	public ImprovementConfig Config { get; set; }
	public int CurrentLevel { get; set; }

	private ImprovementFuelConfig _config;

	private float _currentFuel;

	[CreateProperty]
	public StyleLength _currentFuelPerCent { get; private set; }
	public bool IsFuelEmpty { get; set; } = false;

	public event Action OnFuelChange;
	public event Action OnFuelEmpty;
	public event Action OnFuelMax;

	public Fuel(ImprovementConfig config)
	{
		Config = config;
		_config = (ImprovementFuelConfig)config;
		CurrentLevel = 0;

		_currentFuel = _config.Levels[CurrentLevel].MaxFuel;
		_currentFuelPerCent = Length.Percent(_currentFuel / _config.Levels[CurrentLevel].MaxFuel * 100);
		OnPropertyChanged(nameof(_currentFuelPerCent));
	}

	public void Consumption(float fuel)
	{
		_currentFuel -= fuel;

		if (_currentFuel <= _config.Levels[CurrentLevel].MinFuel)
		{
			_currentFuel = _config.Levels[CurrentLevel].MinFuel;

			IsFuelEmpty = true;

			GameEvents.OnCriticalStatusShow?.Invoke("FuelEmpty", "Закончилось топливо");
			GameEvents.OnCriticalStatusHide?.Invoke("FuelCritical");

			OnFuelEmpty?.Invoke();
		}

		_currentFuelPerCent = Length.Percent(_currentFuel / _config.Levels[CurrentLevel].MaxFuel * 100);
		OnPropertyChanged(nameof(_currentFuelPerCent));

		if (_currentFuelPerCent.value.value <= 20 && !IsFuelEmpty) GameEvents.OnCriticalStatusShow("FuelCritical", "Мало топлива");

		OnFuelChange?.Invoke();
	}

	public void Refueling(float fuel)
	{
		_currentFuel += fuel;

		IsFuelEmpty = false;

		GameEvents.OnCriticalStatusHide?.Invoke("FuelEmpty");

		if (_currentFuel >= _config.Levels[CurrentLevel].MaxFuel)
		{
			_currentFuel = _config.Levels[CurrentLevel].MaxFuel;

			OnFuelMax?.Invoke();
		}

		_currentFuelPerCent = Length.Percent(_currentFuel / _config.Levels[CurrentLevel].MaxFuel * 100);
		OnPropertyChanged(nameof(_currentFuelPerCent));

		if (_currentFuelPerCent.value.value > 20) GameEvents.OnCriticalStatusHide("FuelCritical");

		OnFuelChange?.Invoke();
	}

	public float NeedToRefueling() => _config.Levels[CurrentLevel].MaxFuel - _currentFuel;

	public Dictionary<int, int> GetNeedResources()
	{
		_needResources?.Clear();

		for (int i = 0; i < _config.Levels[CurrentLevel].Resource.Count; i++)
		{
			_needResources[_config.Levels[CurrentLevel].Resource[i]] = _config.Levels[CurrentLevel].Count[i];
		}

		return _needResources;
	}

	public float GetCurrentFuel() => _currentFuel;
	public float GetMaxFuel() => _config.Levels[CurrentLevel].MaxFuel;


	public event PropertyChangedEventHandler PropertyChanged;
	protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
