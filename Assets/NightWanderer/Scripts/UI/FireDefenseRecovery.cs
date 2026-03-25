using UnityEngine;
using UnityEngine.UIElements;

public class FireDefenseRecovery
{
	private FireDefense _fireDefense;
	private VisualElement _fireDefenseForeground;

	public FireDefenseRecovery(FireDefense fireDefense, VisualElement fireDefenseForeground)
	{
		_fireDefense = fireDefense;
		_fireDefense.OnFireDefenseChange += UpdateData;

		_fireDefenseForeground = fireDefenseForeground;
	}

	public void RecoverFireDefense(float value)
	{
		_fireDefense.RestoreFireDefense(value);

		UpdateData();
	}

	public float NeedToRefueling() => _fireDefense.NeedToRecover();

	private void UpdateData()
	{
		var fuelPercent = _fireDefense.GetCurrentFireDefense() / _fireDefense.GetMaxFireDefense() * 100;

		_fireDefenseForeground.style.width = new Length(Mathf.Clamp(fuelPercent, 0, 100), LengthUnit.Percent);
	}
}
