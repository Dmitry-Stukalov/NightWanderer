using System;
using UnityEngine;

[Serializable]
public class MissionData
{
	[field: SerializeField] public int ID { get; private set; }
	[field: SerializeField] public string Description { get; private set; }
	[field: SerializeField] public bool IsDestinationMission { get; private set; }
	[field: SerializeField] public int NeededResources { get; private set; }
}
