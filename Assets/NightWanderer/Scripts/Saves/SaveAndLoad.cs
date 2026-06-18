using System.IO;
using System.Threading.Tasks;
using Unity.Android.Gradle.Manifest;
using UnityEditor;
using UnityEngine;

public class SaveAndLoad
{
	public static void Save(SaveDataClass saveDataClass, string directoryPath)
	{
		var json = JsonUtility.ToJson(saveDataClass);

		var DirectoryPath = directoryPath;
		const string fileName = "DataSave";

		if (!Directory.Exists(DirectoryPath)) Directory.CreateDirectory(DirectoryPath);

		File.WriteAllText($"{DirectoryPath}/{fileName}.json", json);
	}

	public static async Task Load(string directoryPath, string fileName)
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

		GameEvents.OnSceneLoad?.Invoke(dataSave._currentSceneName);
		GameEvents.OnTransformLoad?.Invoke(dataSave._shipTransform);
	}

	private static void FirstLoad()
	{
		GameEvents.OnSceneLoad?.Invoke("IntroductionScene");
		GameEvents.OnTransformLoad?.Invoke(new SaveDataClass.ShipTransform(0, 0, 0, 0, 0, 0));
		Debug.Log("FirstLoad");
		//GameEvents.OnPositionLoad?.Invoke(Vector3.zero);
	}
}
