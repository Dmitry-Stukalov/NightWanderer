using System;
using UnityEngine;

[Serializable]
public class ImprovementEnginesData : ImprovementData
{
	[field: SerializeField] public float WalkSpeed { get; set; }
	[field: SerializeField] public float RunSpeed { get; set; }
	[field: SerializeField] public float WalkUpSpeed { get; set; }
	[field: SerializeField] public float RunUpSpeed { get; set; }
	[field: SerializeField] public float ConsumptionIdle { get; set; }
	[field: SerializeField] public float ConsumptionWalk { get; set; }
	[field: SerializeField] public float ConsumptionRun { get; set; }
}
