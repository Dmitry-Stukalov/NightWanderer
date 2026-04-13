using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = nameof(ImprovementEnginesConfig), menuName = nameof(ImprovementEnginesConfig))]
public class ImprovementEnginesConfig : ImprovementConfig, IImprovementConfig<ImprovementEnginesData>
{
	[SerializeField] private List<ImprovementEnginesData> levels;
	public List<ImprovementEnginesData> Levels => levels;
}
