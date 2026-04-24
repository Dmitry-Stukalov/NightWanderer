using System;

public static class GameEvents
{
	public static Action OnGameStart;

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
	public static Action OnRightExtraction;
	public static Action OnExtractionEnd;

	public static Action OnResearchStart;
	public static Action OnResearchEnd;

	//Управление двигателями
	public static Action OnEnginesOnOff;
	public static Action OnRunStart;
	public static Action OnRunEnd;

	public static Action<string, string> OnCriticalStatusShow;
	public static Action<string> OnCriticalStatusHide;

	public static Action OnDialogueStart;

	public static Action OnMissionComplete;
	public static Action<int> OnDoMission;

	public static Action OnSkipTimeStart;
	public static Action OnSkipTimeEnd;
}
