using UnityEngine.UIElements;

public class DefenseStatistic
{
	private HealthFireDefense _defense;
	private Label _text;

	public DefenseStatistic(Label text, HealthFireDefense defense)
	{
		_text = text;
		_defense = defense;
		_defense.OnUpgrade += UpdateData;
		UpdateData();
	}

	private void UpdateData()
	{
		_text.text = $"Защита: {_defense.GetMaxHealth()} ед.";
	}
}
