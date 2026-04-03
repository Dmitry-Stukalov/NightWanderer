using System;
using UnityEngine;

public static class GameEvents
{
	public static Action OnBase;
	public static Action<int> OnResourceCollected;
	public static Action OnFirstBaseVisit;
}
