using UnityEngine.UIElements;

public class FireDefenseStatistic
{
	private HealthFireDefense _fireDefense;
	private Label _text;

	public FireDefenseStatistic(Label text, HealthFireDefense fireDefense)
	{
		_text = text;
		_fireDefense = fireDefense;
		_fireDefense.OnUpgrade += UpdateData;
		UpdateData();
	}

	private void UpdateData()
	{
		_text.text = $"Терм. защита: {_fireDefense.GetMaxHealth()} ед.";
	}
}
