using UnityEngine;

public class DefenseSystem
{
	//private Health _health;
	//private Defense _defense;
	//private FireDefense _fireDefense;

	private HealthFireDefense _health;
	private HealthFireDefense _defense;
	private HealthFireDefense _fireDefense;

	public DefenseSystem(/*Health health, Defense defense, FireDefense fireDefense, */HealthFireDefense health, HealthFireDefense defense, HealthFireDefense fireDefense, ImprovementManager manager)
	{
		_health = health;
		_defense = defense;
		_fireDefense = fireDefense;

		manager.AddImprovement(_health, "Health");
		manager.AddImprovement(_defense, "Defense");
		manager.AddImprovement(_fireDefense, "FireDefense");
	}

	public void GetDamage(float damage)
	{
		if (/*_defense.GetCurrentDefense()*/_defense.GetCurrentHealth() <= damage)
		{
			if (_defense./*GetCurrentDefense()*/GetCurrentHealth() > 0)
			{
				damage -= _defense./*GetCurrentDefense()*/GetCurrentHealth();

				_defense.GetDamage(_defense./*GetCurrentDefense()*/GetCurrentHealth());

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
		if (_fireDefense./*GetCurrentFireDefense()*/GetCurrentHealth() <= fireDamage)
		{
			if (_fireDefense./*GetCurrentFireDefense()*/GetCurrentHealth() > 0)
			{
				fireDamage -= _fireDefense./*GetCurrentFireDefense()*/GetCurrentHealth();

				_fireDefense./*GetFireDamage*/GetDamage(_fireDefense./*GetCurrentFireDefense()*/GetCurrentHealth());

				Debug.Log("Термическая защита уничтожена");
			}

			_health.GetDamage(fireDamage);
		}
		else
		{
			_fireDefense./*GetFireDamage*/GetDamage(fireDamage);
		}

		//Debug.Log(CurrentHealth);
	}

	public HealthFireDefense GetHealth() => _health;
	public HealthFireDefense GetDefense() => _defense;
	public HealthFireDefense GetFireDefense() => _fireDefense;
	//public Health GetHealth() => _health;
	//public Defense GetDefense() => _defense;
	//public FireDefense GetFireDefense() => _fireDefense;
}
