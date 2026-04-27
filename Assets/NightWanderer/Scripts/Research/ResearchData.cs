using JetBrains.Annotations;
using System;
using UnityEngine;


[Serializable]
public class ResearchData
{
	[field: SerializeField] public string ResearchText { get; set; }
	[field: SerializeField] public string[] ActionsText { get; set; }
	[field: SerializeField] public int[] ActionsWay { get; set; }
}
