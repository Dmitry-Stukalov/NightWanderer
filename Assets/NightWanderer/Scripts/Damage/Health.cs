using UnityEngine;


//Доделать нормально
public class Health : IImprovementBase
{
	public ImprovementConfig Config { get; set; }
	public int CurrentLevel { get; set; }

	private ImprovementHealthConfig _config;

	private float _minHealth;
	private float _maxHealth;
	//private float _minDefense;
	//private float _maxDefense;
	//private float _minFireDefense;
	//private float _maxFireDefense;
	private float _currentHealth;
	//private float _currentDefense;
	//private float _currentFireDefense;

	/*private void Start()
	{
		_currentHealth = _maxHealth;
		//_currentDefense = _minDefense;
		//_currentFireDefense = _minFireDefense;
	}*/

	public Health(ImprovementConfig config)
	{
		_config = (ImprovementHealthConfig)config;
		CurrentLevel = 0;

		_maxHealth = _config.Levels[CurrentLevel].MaxHealth;

		_currentHealth = _config.Levels[CurrentLevel].MaxHealth;
	}

	public void GetDamage(float value)
	{
		_currentHealth -= value;

		if (_currentHealth <= 0)
		{
			_currentHealth = _minHealth;

			Debug.Log("Смерть");
		}
	}

		//Вычисление получения урона с учетом физической защиты (1 защита = -1 урон)
		/*public void GetDamage(float damage)
		{
			if (_currentDefense <= damage)
			{
				if (_currentDefense > _minDefense)
				{
					damage -= _currentDefense;
					_currentDefense = _minDefense;
					Debug.Log("Противоударная защита уничтожена");
				}

				_currentHealth -= damage;
				if (_currentHealth <= _minHealth)
				{
					_currentHealth = _minHealth;
					Debug.Log("Смерть");
				}
			}
			else
			{
				_currentDefense -= damage;
			}
		}*/

		//Вычисление получения урона с учетом термальной защиты (1 защита = -1 урон)
		/*public void GetFireDamage(float firedamage)
		{
			if (_currentFireDefense <= firedamage)
			{
				if (_currentFireDefense > _minDefense)
				{
					firedamage -= _currentFireDefense;
					_currentFireDefense = _minDefense;
					Debug.Log("Термическая защита уничтожена");
				}

				_currentHealth -= firedamage;
				if (_currentHealth <= _minHealth)
				{
					_currentHealth = _minHealth;
					//Debug.Log("Смерть");
				}
			}
			else
			{
				_currentFireDefense -= firedamage;
			}

			//Debug.Log(CurrentHealth);
		}*/

		//Восстановление физической защиты на указанное значение
		/*public void RestoreDefense(float restoreValue)
		{
			_currentDefense += restoreValue;

			if (_currentDefense > _maxDefense) _currentDefense = _maxDefense;
		}*/

		//Восстановление термальной защиты на указанное значение
		/*public void RestoreFireDefense(float restoreValue)
		{
			_currentFireDefense += restoreValue;

			if (_currentFireDefense > _maxFireDefense) _currentFireDefense = _maxFireDefense;
		}*/

		//Восстановление здоровья на указанное значение
	public void Healing(float healValue)
	{
		_currentHealth += healValue;

		if (_currentHealth > _maxHealth) _currentHealth = _maxHealth;
	}

	public float GetCurrentHealth() => _currentHealth;

	public void Upgrade()
	{
		CurrentLevel++;

		_maxHealth = _config.Levels[CurrentLevel].MaxHealth;
	}
}
