using NUnit.Framework.Constraints;
using UnityEngine;

public class LockRotation : MonoBehaviour
{
	[SerializeField] private GameObject Player;

	public void Initializing()
	{
		Player = GameObject.FindGameObjectWithTag("Player");
	}

	private void LateUpdate()
	{
		transform.position = Player.transform.position;
	}
}
