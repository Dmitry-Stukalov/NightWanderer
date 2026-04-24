using UnityEngine.UIElements;

public class FuelStatistic
{
	private Fuel _fuel;
	private Label _text;

	public FuelStatistic(Label text, Fuel fuel)
	{
		_text = text;
		_fuel = fuel;
		_fuel.OnUpgrade += UpdateData;
		UpdateData();
	}

	private void UpdateData()
	{
		_text.text = $"Топливо: {_fuel.GetMaxFuel()} л";
	}
}
