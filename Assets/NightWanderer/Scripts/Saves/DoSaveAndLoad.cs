using System.Collections;
using UnityEngine;

public class DoSaveAndLoad : MonoBehaviour
{
	private SaveDataClass _saveDataClass = new SaveDataClass();
	private string _directoryPath;

	private void Awake()
	{
		_saveDataClass.Initializing();
		_directoryPath = Application.dataPath + "/Source/MissionsSaves";

		GameEvents.OnSave += () => StartCoroutine(SaveDataPause());

		StartCoroutine(LoadData());
	}

	private IEnumerator LoadData()
	{
		yield return new WaitForSecondsRealtime(4);

		SaveAndLoad.Load(_directoryPath, "DataSave");

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
