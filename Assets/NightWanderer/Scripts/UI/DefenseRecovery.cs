using UnityEngine;
using UnityEngine.UIElements;

public class DefenseRecovery
{
	private Defense _defense;
	private VisualElement _defenseForeground;

	public DefenseRecovery(Defense defense, VisualElement defenseForeground)
	{
		_defense = defense;
		_defense.OnDefenseChange += UpdateData;

		_defenseForeground = defenseForeground;
	}

	public void RecoverDefense(float value)
	{
		_defense.RestoreDefense(value);

		UpdateData();
	}

	public float NeedToRefueling() => _defense.NeedToRecover();

	private void UpdateData()
	{
		var fuelPercent = _defense.GetCurrentDefense() / _defense.GetMaxDefense() * 100;

		_defenseForeground.style.width = new Length(Mathf.Clamp(fuelPercent, 0, 100), LengthUnit.Percent);
	}
}
