using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ImprovementSearchlightData : IImprovementData
{
	[field: SerializeField] public Sprite Icon { get; set; }
	[field: SerializeField] public int CurrentLevel { get; set; }
	[field: SerializeField] public string Name { get; set; }
	[field: SerializeField] public string UpgradeDescription { get; set; }
	[field: SerializeField] public List<int> Resource { get; set; }  //ID ресурса
	[field: SerializeField] public List<int> Count { get; set; }     //Количество
}
