using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ImprovementData
{
	[field: SerializeField] public string Name { get; set; }
	//[field: SerializeField] public int MaxLevel { get; set; }
	[field: SerializeField] public List<int> Resource { get; set; } //ID ресурса
	[field: SerializeField] public List<int> Count { get; set; }     //Количество
}
