using System;
using UnityEngine;

[Serializable]
public class ImprovementFuelData : ImprovementData
{
	[field: SerializeField] public float MinFuel { get; set; }
	[field: SerializeField] public float MaxFuel { get; set; }
}
