using UnityEngine;

public class DefenseSystem
{
	private Health _health;
	private Defense _defense;
	private FireDefense _fireDefense;

	public DefenseSystem(Health health, Defense defense, FireDefense fireDefense)
	{
		_health = health;
		_defense = defense;
		_fireDefense = fireDefense;
	}

	public void GetDamage(float damage)
	{
		if (_defense.GetCurrentDefense() <= damage)
		{
			if (_defense.GetCurrentDefense() > 0)
			{
				damage -= _defense.GetCurrentDefense();

				_defense.GetDamage(_defense.GetCurrentDefense());

				Debug.Log("Противоударная защита уничтожена");
			}

			_health.GetDamage(damage);
		}
		else
		{
			_defense.GetDamage(damage);
		}
	}

	//Вычисление получения урона с учетом термальной защиты (1 защита = -1 урон)
	public void GetFireDamage(float fireDamage)
	{
		if (_fireDefense.GetCurrentFireDefense() <= fireDamage)
		{
			if (_fireDefense.GetCurrentFireDefense() > 0)
			{
				fireDamage -= _fireDefense.GetCurrentFireDefense();

				_fireDefense.GetFireDamage(_fireDefense.GetCurrentFireDefense());

				Debug.Log("Термическая защита уничтожена");
			}

			_health.GetDamage(fireDamage);
		}
		else
		{
			_fireDefense.GetFireDamage(fireDamage);
		}

		//Debug.Log(CurrentHealth);
	}
}
