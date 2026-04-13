using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = nameof(ImprovementFuelConfig), menuName = nameof(ImprovementFuelConfig))]
public class ImprovementFuelConfig : ImprovementConfig, IImprovementConfig<ImprovementFuelData>
{
	[SerializeField] private List<ImprovementFuelData> levels;
	public List<ImprovementFuelData> Levels => levels;
}
