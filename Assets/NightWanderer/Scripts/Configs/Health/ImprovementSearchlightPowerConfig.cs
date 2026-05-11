using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(ImprovementSearchlightPowerConfig), menuName = nameof(ImprovementSearchlightPowerConfig))]
public class ImprovementSearchlightPowerConfig : ImprovementConfig, IImprovementConfig<ImprovementSearchlightPowerData>
{
	[SerializeField] private List<ImprovementSearchlightPowerData> levels;
	public List<ImprovementSearchlightPowerData> Levels => levels;
}
