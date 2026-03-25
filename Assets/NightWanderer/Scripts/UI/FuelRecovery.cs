using UnityEngine;
using UnityEngine.UIElements;

public class FuelRecovery
{
	private Fuel _fuel;
	private VisualElement _fuelForeground; 

	public FuelRecovery(Fuel fuel, VisualElement fuelForeground)
	{
		_fuel = fuel;
		_fuel.OnFuelChange += UpdateData;

		_fuelForeground = fuelForeground;
	}

	public void RecoverFuel(float value)
	{
		_fuel.Refueling(value);

		UpdateData();
	}

	public float NeedToRefueling() => _fuel.NeedToRefueling();

	private void UpdateData()
	{
		var fuelPercent = _fuel.GetCurrentFuel() / _fuel.GetMaxFuel() * 100;

		_fuelForeground.style.width = new Length (Mathf.Clamp(fuelPercent, 0, 100), LengthUnit.Percent);
	}
}
