using UnityEngine;

//Добавить на любую базу
public class Base : MonoBehaviour
{
	[SerializeField] private GameObject _dockingPlatform;
	[SerializeField] private Animator _animator;
	[SerializeField] private GameObject _ship;
	[SerializeField] private PlayerUIController _playerUI;

	public void MoveDownDockingPlatform()
	{
		_ship.transform.SetParent(_dockingPlatform.transform, true);
		_animator.SetBool("IsDown", true);
	}
	
	public void MoveUpDockingPlatform() => _animator.SetBool("IsDown", false);

	public void Undocking() => _ship.transform.SetParent(null, true);

	public void OpenCloseBaseUI() => _playerUI.OnBase();

	public void CloseBaseUI() => _playerUI.OutBase();
}
