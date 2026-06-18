using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public static class GameEvents
{
	public static Action OnGameStart;

	//Посещение базы
	public static Action<Base> OnBase;
	public static Action OnInBase;
	public static Action OnOutBase;
	public static Action OnFirstBaseVisit;

	//Подбор ресурса
	public static Action<int> OnResourceCollected;

	//Исследование кораблей
	public static Action<string> OnImprovementOpen;
	public static Action<string> OnCraftOpen;
	public static Action<string> OnStoryOpen;

	//Добыча ресурсов
	public static Action OnLaserExtractionStart;
	//public static Action OnCheckResultLaser;
	public static Action OnRightExtraction;
	public static Action OnExtractionEnd;

	public static Action<ResearchShip> OnResearchNearBy;
	public static Action OnResearchStart;
	public static Action<string> OnResearchEnd;
	public static Action OnResearchQuit;

	//Управление двигателями
	public static Action OnEnginesOnOff;
	public static Action OnRunStart;
	public static Action OnRunEnd;

	public static Action<string, string> OnCriticalStatusShow;
	public static Action<string> OnCriticalStatusHide;

	public static Action OnDialogueStart;
	public static Action<int> OnDialogueWarningStart;

	public static Action<int> OnMissionComplete;
	public static Action<int, int> OnDoMission;

	public static Action OnSkipTimeStart;
	public static Action OnSkipTimeEnd;

	public static Action OnDeath;

	public static Action<int> OnMarkShow;
	public static Action<int> OnMarkHide;

	public static Action<int, int> OnResourceDrop;

	public static Action OnDayStart;
	public static Action OnNightStart;

	public static Action OnMapFogOn;
	public static Action OnMapFogOff;

	public static Action OnVacuumCleanerOn;
	public static Action OnVacuumCleanerOff;

	//Sounds
	public static Action OnMainMenuOut;
	public static Action OnMainMenuIn;
	public static Action OnSettingsOpen;
	public static Action OnSettingsClose;
	public static Action OnSandstormStart;
	public static Action OnSandstormEnd;
	public static Action OnRainStart;
	public static Action OnRainEnd;
	public static Action<int> OnMovementChange;

	//Saves
	public static Action OnSave;

	public static Action<List<Inventory>> OnInventorySave;
	public static Action<List<int>> OnImprovementsSave;
	public static Action<List<int>> OnStatsSave;
	public static Action<List<int>> OnResourceSourcesSave;
	public static Action<List<int>> OnResearchShipsSave;
	public static Action<Vector3> OnPositionSave;
	public static Action<Transform> OnTransformSave;
	public static Action<Base> OnBaseSave;
	public static Action<string> OnSceneSave;
	public static Action<int> OnCurrentDaySave;
	public static Action<float> OnCurrentTimeSave;
	public static Action<int> OnCurrentMissionSave;
	public static Action<int> OnCurrentDialogueSave;

	//Load
	public static Action<List<Inventory>> OnInventoryLoad;
	public static Action<List<int>> OnImprovementsLoad;
	public static Action<List<int>> OnStatsLoad;
	public static Action<List<int>> OnResourceSourcesLoad;
	public static Action<List<int>> OnResearchShipsLoad;
	public static Action<Vector3> OnPositionLoad;
	public static Action<SaveDataClass.ShipTransform> OnTransformLoad;
	public static Action<Base> OnBaseLoad;
	public static Action<string> OnSceneLoad;
	public static Action<int> OnCurrentDayLoad;
	public static Action<float> OnCurrentTimeLoad;
	public static Action<int> OnCurrentMissionLoad;
	public static Action<int> OnCurrentDialogueLoad;
}
