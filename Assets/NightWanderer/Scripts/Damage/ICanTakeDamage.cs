using UnityEngine;


//Реализует систему здоровья и получения урона
public interface ICanTakeDamage
{
	public DefenseSystem Health { get; set; }
	public float Damage {  get; set; }
	public bool IsFireDamage { get; set; }

	public void GiveDamage(float damage, bool isFireDamage) { }
}
