using UnityEngine;

public interface ICanTakeDamage
{
	public Health PlayerHealth { get; set; }
	public float Damage {  get; set; }
	public bool IsFireDamage { get; set; }

	public void TakeDamage()
	{
		if (IsFireDamage) PlayerHealth.GetFireDamage(Damage);
		else PlayerHealth.GetDamage(Damage);
	}
}
