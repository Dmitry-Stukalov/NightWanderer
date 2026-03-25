using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = nameof(ImprovementFuelConfig), menuName = nameof(ImprovementFuelConfig))]
public class ImprovementFuelConfig : ImprovementConfig
{
	[field: SerializeField] public List<ImprovementFuelData> Levels { get; set; }
}
