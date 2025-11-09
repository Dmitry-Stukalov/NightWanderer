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


	//¬ычисление получени€ урона с учетом физической защиты (1 защита = -1 урон)
	public void GetDamage(float damage)
	{
		if (CurrentDefense <= damage)
		{
			if (CurrentDefense > MinDefense)
			{
				damage -= CurrentDefense;
				CurrentDefense = MinDefense;
				Debug.Log("ѕротивоударна€ защита уничтожена");
			}

			CurrentHealth -= damage;
			if (CurrentHealth <= MinHealth)
			{
				CurrentHealth = MinHealth;
				Debug.Log("—мерть");
			}
		}
		else
		{
			CurrentDefense -= damage;
		}
	}

	//¬ычисление получени€ урона с учетом термальной защиты (1 защита = -1 урон)
	public void GetFireDamage(float firedamage)
	{
		if (CurrentFireDefense <= firedamage)
		{
			if (CurrentFireDefense > MinDefense)
			{
				firedamage -= CurrentFireDefense;
				CurrentFireDefense = MinDefense;
				Debug.Log("“ермическа€ защита уничтожена");
			}

			CurrentHealth -= firedamage;
			if (CurrentHealth <= MinHealth)
			{
				CurrentHealth = MinHealth;
				//Debug.Log("—мерть");
			}
		}
		else
		{
			CurrentFireDefense -= firedamage;
		}

		//Debug.Log(CurrentHealth);
	}

	//¬осстановление физической защиты на указанное значение
	public void RestoreDefense(float restoreValue)
	{
		CurrentDefense += restoreValue;

		if (CurrentDefense > MaxDefense) CurrentDefense = MaxDefense;
	}

	//¬осстановление термальной защиты на указанное значение
	public void RestoreFireDefense(float restoreValue)
	{
		CurrentFireDefense += restoreValue;

		if (CurrentFireDefense > MaxFireDefense) CurrentFireDefense = MaxFireDefense;
	}

	//¬осстановление здоровь€ на указанное значение
	public void Healing(float healValue)
	{
		CurrentHealth += healValue;

		if (CurrentHealth > MaxHealth) CurrentHealth = MaxHealth;
	}

	
}
