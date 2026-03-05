using UnityEngine;

public class Base : MonoBehaviour
{
	[SerializeField] private GameObject DockingPlatform;
	[SerializeField] private Animator _Animator;
	[SerializeField] private GameObject Ship;
	[SerializeField] private PlayerUIController PlayerUI;

	public void MoveDownDockingPlatform()
	{
		Ship.transform.SetParent(DockingPlatform.transform, true);
		_Animator.SetBool("IsDown", true);
	}
	
	public void MoveUpDockingPlatform()
	{
		//Ship.transform.SetParent(null, true);
		_Animator.SetBool("IsDown", false);
	}

	public void Undocking()
	{
		Ship.transform.SetParent(null, true);
	}

	public void OpenCloseBaseUI() => PlayerUI.OnBase();
}
