using System;
using UnityEditor.PackageManager;
using UnityEngine;

public static class GameEvents
{
	//Посещение базы
	public static Action OnBase;
	public static Action OnFirstBaseVisit;

	//Подбор ресурса
	public static Action<int> OnResourceCollected;

	//Исследование кораблей
	public static Action<string> OnImprovementOpen;
	public static Action<string> OnCraftOpen;

	//Добыча ресурсов
	public static Action OnLaserExtractionStart;
	//public static Action OnCheckResultLaser;
	public static Action OnExtractionEnd;
}
