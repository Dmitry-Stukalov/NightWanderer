using System.Collections;
using UnityEngine;

public class DoSaveAndLoad : MonoBehaviour
{
	public static DoSaveAndLoad Instance { get; set; }
	private SaveDataClass _saveDataClass = new SaveDataClass();
	private string _directoryPath;

	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(gameObject);
			return;
		}

		Instance = this;

		DontDestroyOnLoad(gameObject);

		_saveDataClass.Initializing();
		_directoryPath = Application.dataPath + "/Source/MissionsSaves";

		GameEvents.OnSave += () => StartCoroutine(SaveDataPause());
		GameEvents.OnMainMenuOut += LoadGame;
	}

	public void LoadGame()
	{
		if (SaveAndLoad.IsLoadGame) StartCoroutine(LoadData());
	}

	private IEnumerator LoadData()
	{
		yield return new WaitForSecondsRealtime(0.5f);

		SaveAndLoad.Load(_directoryPath, "DataSave", true);

		yield return new WaitForSecondsRealtime(5f);

		SaveAndLoad.Load(_directoryPath, "DataSave", false);

		Debug.Log("Данные загружены");
	}

	private IEnumerator SaveDataPause()
	{
		yield return new WaitForSecondsRealtime(2);

		SaveData();
	}

	private void SaveData()
	{
		SaveAndLoad.Save(_saveDataClass, _directoryPath);
	}

	private void OnDisable()
	{
		_saveDataClass.OnDisable();
	}

	private void OnApplicationQuit()
	{
		StartCoroutine(SaveDataPause());
		//SaveAndLoad.Save(_saveDataClass, _directoryPath);
	}
}
