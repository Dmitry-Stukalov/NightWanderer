using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ImprovementEnginesData : IImprovementData
{
	[field: SerializeField] public float WalkSpeed { get; set; }
	[field: SerializeField] public float RunSpeed { get; set; }
	[field: SerializeField] public float WalkUpSpeed { get; set; }
	[field: SerializeField] public float RunUpSpeed { get; set; }
	[field: SerializeField] public float ConsumptionIdle { get; set; }
	[field: SerializeField] public float ConsumptionWalk { get; set; }
	[field: SerializeField] public float ConsumptionRun { get; set; }

	[field: SerializeField] public Sprite Icon { get; set; }
	[field: SerializeField] public int CurrentLevel { get; set; }
	[field: SerializeField] public string Name { get; set; }
	[field: SerializeField] public string UpgradeDescription { get; set; }
	[field: SerializeField] public List<int> Resource { get; set; }  //ID ресурса
	[field: SerializeField] public List<int> Count { get; set; }     //Количество
}
