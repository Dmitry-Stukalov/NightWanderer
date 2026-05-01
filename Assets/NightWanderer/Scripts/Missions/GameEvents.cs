using System;

public static class GameEvents
{
	public static Action OnGameStart;

	//Посещение базы
	public static Action<Base> OnBase;
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
	public static Action OnResearchEnd;
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
}
