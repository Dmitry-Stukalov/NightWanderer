using UnityEngine;
using UnityEngine.SceneManagement;

public class MainBootstrap : MonoBehaviour
{
	private void Start()
	{
		SceneManager.LoadScene("OpenMapScene", LoadSceneMode.Additive);
	}
}
