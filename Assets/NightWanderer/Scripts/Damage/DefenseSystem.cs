using UnityEngine;

//ќтвечает за здоровье, защиту и термальную защиту. ¬ычисл€ет получаемый урон
public class DefenseSystem
{
	private HealthFireDefense _health;
	private HealthFireDefense _defense;
	private HealthFireDefense _fireDefense;

	public DefenseSystem(HealthFireDefense health, HealthFireDefense defense, HealthFireDefense fireDefense, ImprovementManager manager)
	{
		_health = health;
		_defense = defense;
		_fireDefense = fireDefense;

		manager.AddImprovement(_health, "Health");
		manager.AddImprovement(_defense, "Defense");
		manager.AddImprovement(_fireDefense, "FireDefense");
	}

	//¬ычисление получаемого урона с учетом защиты (1 защита = 1 урон)
	public void GetDamage(float damage)
	{
		if (_defense.GetCurrentHealth() <= damage)
		{
			if (_defense.GetCurrentHealth() > 0)
			{
				damage -= _defense.GetCurrentHealth();

				_defense.GetDamage(_defense.GetCurrentHealth());

				Debug.Log("ѕротивоударна€ защита уничтожена");
			}

			_health.GetDamage(damage);
		}
		else
		{
			_defense.GetDamage(damage);
		}
	}

	//¬ычисление получени€ урона с учетом термальной защиты (1 защита = 1 урон)
	public void GetFireDamage(float fireDamage)
	{
		if (_fireDefense.GetCurrentHealth() <= fireDamage)
		{
			if (_fireDefense.GetCurrentHealth() > 0)
			{
				fireDamage -= _fireDefense.GetCurrentHealth();

				_fireDefense.GetDamage(_fireDefense.GetCurrentHealth());

				Debug.Log("“ермическа€ защита уничтожена");
			}

			_health.GetDamage(fireDamage);
		}
		else
		{
			_fireDefense.GetDamage(fireDamage);
		}
	}

	public HealthFireDefense GetHealth() => _health;
	public HealthFireDefense GetDefense() => _defense;
	public HealthFireDefense GetFireDefense() => _fireDefense;
}
