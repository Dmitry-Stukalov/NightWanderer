using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

//Добавить на любую базу
public class Base : MonoBehaviour
{
	[SerializeField] private GameObject _dockingPlatform;
	[SerializeField] private Animator _animator;
	[SerializeField] private Animator _entryAnimator;
	/*[SerializeField]*/ private GameObject _ship;
	/*[SerializeField]*/ private PlayerUIController _playerUI;
	private bool IsFirstVisit = true;

	private void Start()
	{
		_playerUI = FindAnyObjectByType<PlayerUIController>();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			_ship = other.gameObject;

			_entryAnimator.SetBool("IsOpen", true);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			_entryAnimator.SetBool("IsOpen", false);
		}
	}

	public void MoveDownDockingPlatform()
	{
		_ship.transform.SetParent(_dockingPlatform.transform, true);
		_animator.SetBool("IsDown", true);

		if (IsFirstVisit)
		{
			GameEvents.OnFirstBaseVisit?.Invoke();
			IsFirstVisit = false;

			StartCoroutine(LoadOpenScene());
		}
	}
	
	private IEnumerator LoadOpenScene()
	{
		yield return new WaitForSeconds(2f);

		SceneManager.LoadScene("OpenMapScene", LoadSceneMode.Additive);

		yield return new WaitForSeconds(2f);

		SceneManager.UnloadSceneAsync("IntroductionScene");
		Resources.UnloadUnusedAssets();
	}

	public void MoveUpDockingPlatform() => _animator.SetBool("IsDown", false);

	public void Undocking() => _ship.transform.SetParent(null, true);

	public void OpenCloseBaseUI() => _playerUI.OnBase();

	public void CloseBaseUI() => _playerUI.OutBase();

	public Vector3 GetPlatformPosition() => new Vector3(_dockingPlatform.transform.position.x, _dockingPlatform.transform.position.y + 0.7f, _dockingPlatform.transform.position.z);
}
