using System;
using UnityEngine;

//Отвечает за здоровье, защиту и термальную защиту. Вычисляет получаемый урон
public class DefenseSystem
{
	private HealthFireDefense _health;
	private HealthFireDefense _defense;
	private HealthFireDefense _fireDefense;

	public event Action OnDeath;

	public DefenseSystem(HealthFireDefense health, HealthFireDefense defense, HealthFireDefense fireDefense, ImprovementManager manager)
	{
		_health = health;
		_defense = defense;
		_fireDefense = fireDefense;

		_health.OnHealthChange += CheckHealth;
		_defense.OnHealthChange += CheckDefense;
		_fireDefense.OnHealthChange += CheckFireDefense;

		manager.AddImprovement(_health, "Health");
		manager.AddImprovement(_defense, "Defense");
		manager.AddImprovement(_fireDefense, "FireDefense");
	}

	//Вычисление получаемого урона с учетом защиты (1 защита = 1 урон)
	public void GetDamage(float damage)
	{
		if (_defense.GetCurrentHealth() <= damage)
		{
			if (_defense.GetCurrentHealth() > 0)
			{
				damage -= _defense.GetCurrentHealth();

				_defense.GetDamage(_defense.GetCurrentHealth());

				GameEvents.OnCriticalStatusShow?.Invoke("DefenseDestroy", "Защита уничтожена");
				GameEvents.OnCriticalStatusHide?.Invoke("DefenseCritical");
			}

			_health.GetDamage(damage);

			if (_health.GetCurrentHealth() / _health.GetMaxHealth() * 100 <= 20) GameEvents.OnCriticalStatusShow?.Invoke("HealthCritical", "Корпус поврежден");

			if (_health.GetCurrentHealth() == 0) Death();
		}
		else
		{
			_defense.GetDamage(damage);

			if (_defense.GetCurrentHealth() / _defense.GetMaxHealth() * 100 <= 20) GameEvents.OnCriticalStatusShow?.Invoke("DefenseCritical", "Защита повреждена");
		}
	}

	//Вычисление получения урона с учетом термальной защиты (1 защита = 1 урон)
	public void GetFireDamage(float fireDamage)
	{
		if (_fireDefense.GetCurrentHealth() <= fireDamage)
		{
			if (_fireDefense.GetCurrentHealth() > 0)
			{
				fireDamage -= _fireDefense.GetCurrentHealth();

				_fireDefense.GetDamage(_fireDefense.GetCurrentHealth());

				GameEvents.OnCriticalStatusShow?.Invoke("FireDefenseDestroy", "Терм. защита уничтожена");
				GameEvents.OnCriticalStatusHide?.Invoke("FireDefenseCritical");
			}

			_health.GetDamage(fireDamage);
			
			if (_health.GetCurrentHealth() / _health.GetMaxHealth() * 100 <= 20) GameEvents.OnCriticalStatusShow?.Invoke("HealthCritical", "Корпус поврежден");

			if (_health.GetCurrentHealth() == 0) Death();
		}
		else
		{
			_fireDefense.GetDamage(fireDamage);

			if (_fireDefense.GetCurrentHealth() / _fireDefense.GetMaxHealth() * 100 <= 20) GameEvents.OnCriticalStatusShow?.Invoke("FireDefenseCritical", "Терм. защита повреждена");
		}
	}

	private void CheckHealth()
	{
		if (_health.GetCurrentHealth() / _health.GetMaxHealth() * 100 > 20) GameEvents.OnCriticalStatusHide?.Invoke("HealthCritical");
	}

	private void CheckDefense()
	{
		if (_defense.GetCurrentHealth() / _defense.GetMaxHealth() * 100 <= 20)
		{
			GameEvents.OnCriticalStatusHide?.Invoke("DefenseDestroy");
			GameEvents.OnCriticalStatusShow?.Invoke("DefenseCritical", "Защита повреждена");
		}

		if (_defense.GetCurrentHealth() / _defense.GetMaxHealth() * 100 > 20)
		{
			GameEvents.OnCriticalStatusHide?.Invoke("DefenseDestroy");
			GameEvents.OnCriticalStatusHide?.Invoke("DefenseCritical");
		}
	}

	private void Death()
	{
		OnDeath?.Invoke();
	}

	public void Alive()
	{
		_health.Healing(_health.GetMaxHealth());
	}

	private void CheckFireDefense()
	{
		if (_fireDefense.GetCurrentHealth() / _fireDefense.GetMaxHealth() * 100 <= 20)
		{
			GameEvents.OnCriticalStatusHide?.Invoke("FireDefenseDestroy");
			GameEvents.OnCriticalStatusShow?.Invoke("FireDefenseCritical", "Терм. защита повреждена");
		}

		if (_fireDefense.GetCurrentHealth() / _fireDefense.GetMaxHealth() * 100 > 20)
		{
			GameEvents.OnCriticalStatusHide?.Invoke("FireDefenseDestroy");
			GameEvents.OnCriticalStatusHide?.Invoke("FireDefenseCritical");
		}
	}

	public HealthFireDefense GetHealth() => _health;
	public HealthFireDefense GetDefense() => _defense;
	public HealthFireDefense GetFireDefense() => _fireDefense;
}
