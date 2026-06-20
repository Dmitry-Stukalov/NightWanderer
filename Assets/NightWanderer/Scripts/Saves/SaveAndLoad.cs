using System.IO;
using System.Threading.Tasks;
using Unity.Android.Gradle.Manifest;
using UnityEditor;
using UnityEngine;

public static class SaveAndLoad
{
	public static bool IsLoadGame = false;
	public static bool IsSaveFileExist = false;

	public static void Save(SaveDataClass saveDataClass, string directoryPath)
	{
		var json = JsonUtility.ToJson(saveDataClass);

		var DirectoryPath = directoryPath;
		const string fileName = "DataSave";

		if (!Directory.Exists(DirectoryPath)) Directory.CreateDirectory(DirectoryPath);

		File.WriteAllText($"{DirectoryPath}/{fileName}.json", json);
	}

	public static async Task Load(string directoryPath, string fileName, bool isMapLoad)
	{
		var DirectoryPath = directoryPath;

		if (!Directory.Exists(DirectoryPath))
		{
			Directory.CreateDirectory(DirectoryPath);
			//Debug.LogError($"Cant find directory, so file doesnt exist: {DirectoryPath}");
			FirstLoad();

			return;
		}

		if (!fileName.Contains(".json")) fileName += ".json";

		if (!File.Exists($"{DirectoryPath}/{fileName}"))
		{
			File.Create($"{DirectoryPath}/{fileName}").Close();
			//Debug.LogError($"File doesnt exist: {DirectoryPath}/{fileName}");
			FirstLoad();

			return;
		}

		var json = await File.ReadAllTextAsync($"{DirectoryPath}/{fileName}");

		if (json == "")
		{

			FirstLoad();

			return;
		}

		var dataSave = JsonUtility.FromJson<SaveDataClass>(json);

		//GameEvents.OnSceneLoad?.Invoke(dataSave._currentSceneName);
		//GameEvents.OnCurrentMissionLoad?.Invoke(dataSave._currentMission);

		if (isMapLoad)
		{
			GameEvents.OnSceneLoad?.Invoke(dataSave._currentSceneName);
			return;
		}

		GameEvents.OnTransformLoad?.Invoke(dataSave._shipTransform);
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
	}

	private static void FirstLoad()
	{
		//GameEvents.OnSceneLoad?.Invoke("IntroductionScene");
		GameEvents.OnCurrentMissionLoad?.Invoke(0);
		GameEvents.OnCurrentDialogueLoad?.Invoke(0);
		GameEvents.OnTransformLoad?.Invoke(new SaveDataClass.ShipTransform(0, 0, 0, 0, 0, 0));
		Debug.Log("FirstLoad");
		//GameEvents.OnPositionLoad?.Invoke(Vector3.zero);
	}

	public static void CheckSaveFile(string directoryPath, string fileName)
	{
		var DirectoryPath = directoryPath;

		if (!Directory.Exists(DirectoryPath))
		{
			Directory.CreateDirectory(DirectoryPath);
			return;
		}

		if (!fileName.Contains(".json")) fileName += ".json";

		if (!File.Exists($"{DirectoryPath}/{fileName}"))
		{
			File.Create($"{DirectoryPath}/{fileName}").Close();
			return;
		}

		var json = File.ReadAllText($"{DirectoryPath}/{fileName}");

		if (json == "")
		{
			return;
		}

		IsSaveFileExist = true;
	}
}
