using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public static class SaveAndLoad
{
	public static bool IsLoadGame { get; set; } = false;
	public static bool IsLoadBase { get; set; } = false;
	public static bool IsSaveFileExist { get; set; } = false;
	private static string _directoryPath = Path.Combine(Application.persistentDataPath, "MissionsSaves");
	private static string _fileName = "DataSave.json";
	private static string _fullPath = Path.Combine(_directoryPath, _fileName);

	public static void Save(SaveDataClass saveDataClass)
	{
		if (!Directory.Exists(_directoryPath)) Directory.CreateDirectory(_directoryPath);

		var json = JsonUtility.ToJson(saveDataClass);

		File.WriteAllText(_fullPath, json);
	}

	public static async Task Load(bool isMapLoad)
	{
		if (!Directory.Exists(_directoryPath))
		{
			Directory.CreateDirectory(_directoryPath);

			return;
		}

		if (!_fileName.Contains(".json")) _fileName += ".json";

		string fullPath = Path.Combine(_directoryPath, _fileName);

		if (!File.Exists(fullPath))
		{
			return;
		}

		var json = await File.ReadAllTextAsync(fullPath);

		if (json == "")
		{
			return;
		}

		var dataSave = JsonUtility.FromJson<SaveDataClass>(json);

		if (isMapLoad)
		{
			GameEvents.OnSceneLoad?.Invoke(dataSave._currentSceneName);
			return;
		}

		IsLoadBase = dataSave.IsOnBase;

		GameEvents.OnTransformLoad?.Invoke(dataSave._shipTransform, dataSave._currentBase, dataSave.IsOnBase);
		GameEvents.OnCurrentMissionLoad?.Invoke(dataSave._currentMission);
		GameEvents.OnCurrentDialogueLoad?.Invoke(dataSave._currentDialogue);
		GameEvents.OnCurrentDayLoad?.Invoke(dataSave._currentDay);
		GameEvents.OnCurrentTimeLoad?.Invoke(dataSave._currentTime);
		GameEvents.OnInventoryLoad?.Invoke(dataSave.Inventory);
		GameEvents.OnBaseInventoryLoad?.Invoke(dataSave.BaseInventory);
		GameEvents.OnImprovementsLoad?.Invoke(dataSave.Improvements);
		GameEvents.OnStatsLoad?.Invoke(dataSave.Stats);
		for (int i = 0; i < 4; i++) GameEvents.OnResourceSourcesLoad?.Invoke(i, dataSave.ResourceSources[i]);
		GameEvents.OnResearchShipsLoad?.Invoke(dataSave.ResearchShips);
		GameEvents.OnImprovementPanelsLoad?.Invoke(dataSave.ImprovementsUnlock);

		Debug.Log("Файл загружен");
	}

	public static void CheckSaveFile()
	{
		if (!Directory.Exists(_directoryPath))
		{
			return;
		}

		if (!_fileName.Contains(".json")) _fileName += ".json";

		string fullPath = Path.Combine(_directoryPath, _fileName);

		if (!File.Exists(fullPath))
		{
			return;
		}

		var json = File.ReadAllText(fullPath);

		if (json == "")
		{
			return;
		}

		IsSaveFileExist = true;
	}
}
