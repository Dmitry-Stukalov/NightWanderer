using UnityEngine;

public class Health : MonoBehaviour
{
	[field: SerializeField] private float MinHealth;
	[field: SerializeField] private float MaxHealth;
	[field: SerializeField] private float MinDefense;
	[field: SerializeField] private float MaxDefense;
	[field: SerializeField] private float MinFireDefense;
	[field: SerializeField] private float MaxFireDefense;
	private float CurrentHealth;
	private float CurrentDefense;
	private float CurrentFireDefense;

	private void Start()
	{
		CurrentHealth = MaxHealth;
		CurrentDefense = MinDefense;
		CurrentFireDefense = MinFireDefense;
	}

	public void GetDamage(float damage)
	{
		if (CurrentDefense <= damage)
		{
			if (CurrentDefense > MinDefense)
			{
				damage -= CurrentDefense;
				CurrentDefense = MinDefense;
				Debug.Log("Противоударная защита уничтожена");
			}

			CurrentHealth -= damage;
			if (CurrentHealth <= MinHealth)
			{
				CurrentHealth = MinHealth;
				Debug.Log("Смерть");
			}
		}
		else
		{
			CurrentDefense -= damage;
		}
	}

	public void GetFireDamage(float firedamage)
	{
		if (CurrentFireDefense <= firedamage)
		{
			if (CurrentFireDefense > MinDefense)
			{
				firedamage -= CurrentFireDefense;
				CurrentFireDefense = MinDefense;
				Debug.Log("Термическая защита уничтожена");
			}

			CurrentHealth -= firedamage;
			if (CurrentHealth <= MinHealth)
			{
				CurrentHealth = MinHealth;
				//Debug.Log("Смерть");
			}
		}
		else
		{
			CurrentFireDefense -= firedamage;
		}

		//Debug.Log(CurrentHealth);
	}

	public void RestoreDefense(float restoreValue)
	{
		CurrentDefense += restoreValue;

		if (CurrentDefense > MaxDefense) CurrentDefense = MaxDefense;
	}
	public void RestoreFireDefense(float restoreValue)
	{
		CurrentFireDefense += restoreValue;

		if (CurrentFireDefense > MaxFireDefense) CurrentFireDefense = MaxFireDefense;
	}


	public void Healing(float healValue)
	{
		CurrentHealth += healValue;

		if (CurrentHealth > MaxHealth) CurrentHealth = MaxHealth;
	}

	
}
