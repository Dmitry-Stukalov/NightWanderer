using NUnit.Framework.Constraints;
using UnityEngine;

public class LockRotation : MonoBehaviour
{
	[SerializeField] private GameObject Player;

	private void LateUpdate()
	{
		transform.position = Player.transform.position;
	}
}
