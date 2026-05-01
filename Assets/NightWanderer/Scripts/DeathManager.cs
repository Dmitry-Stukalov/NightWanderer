using System;
using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class DeathManager : MonoBehaviour
{
	[SerializeField] private GameObject _ship;
	[SerializeField] private VisualEffect[] _explosions;
	public Base LastBase;
	private Vector3 _startPosition;

	public event Action OnAlive;

	private void Start()
	{
		_startPosition = _ship.transform.position;

		for (int i = 0; i < _explosions.Length; i++) _explosions[i].Stop();

		GameEvents.OnBase += ChangeBase;
	}

	public void StartDeath()
	{
		StartCoroutine(Death());

		GameEvents.OnDeath?.Invoke();
	}

	private IEnumerator Death()
	{
		for (int i = 0; i < _explosions.Length; i++)
		{
			_explosions[i].Play();

			yield return new WaitForSeconds(0.5f);
		}

		yield return new WaitForSeconds(4f);

		Alive();
	}

	private void Alive()
	{
		if (LastBase == null)
		{
			_ship.transform.position = _startPosition;
		}
		else
		{
			_ship.transform.position = LastBase.transform.position;
		}

		OnAlive?.Invoke();
	}

	private void ChangeBase(Base newBase) => LastBase = newBase;

	private void OnDisable()
	{
		GameEvents.OnBase -= ChangeBase;
	}
}
