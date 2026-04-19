using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = nameof(ImprovementSearchlightConfig), menuName = nameof(ImprovementSearchlightConfig))]
public class ImprovementSearchlightConfig : ImprovementConfig, IImprovementConfig<ImprovementSearchlightData>
{
	[SerializeField] private List<ImprovementSearchlightData> levels;
	public List<ImprovementSearchlightData> Levels => levels;
}
