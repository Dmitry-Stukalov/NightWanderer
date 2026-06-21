using System.Collections;
using System.IO;
using UnityEngine;

public class DoSaveAndLoad : MonoBehaviour
{
	public static DoSaveAndLoad Instance { get; set; }
	private SaveDataClass _saveDataClass = new SaveDataClass();

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

		GameEvents.OnSave += StartSaveData;
		GameEvents.OnMainMenuOut += LoadGame;
	}

	public void LoadGame()
	{
		if (SaveAndLoad.IsLoadGame) StartCoroutine(LoadData());
	}

	private IEnumerator LoadData()
	{
		yield return new WaitForSecondsRealtime(0.5f);

		SaveAndLoad.Load(true);

		yield return new WaitForSecondsRealtime(5f);

		SaveAndLoad.Load(false);
	}

	private void StartSaveData() => StartCoroutine(SaveDataPause());

	private IEnumerator SaveDataPause()
	{
		yield return new WaitForSecondsRealtime(1);

		SaveData();
	}

	private void SaveData()
	{
		SaveAndLoad.Save(_saveDataClass);
	}

	private void OnDisable()
	{
		_saveDataClass.OnDisable();
		GameEvents.OnSave -= StartSaveData;
	}

	private void OnApplicationQuit()
	{
		StartCoroutine(SaveDataPause());
		//SaveAndLoad.Save(_saveDataClass, _directoryPath);
	}
}
