using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(ImprovementMiningConfig), menuName = nameof(ImprovementMiningConfig))]
public class ImprovementMiningConfig : ImprovementConfig, IImprovementConfig<ImprovementMiningData>
{
	[SerializeField] private List<ImprovementMiningData> levels;
	public List<ImprovementMiningData> Levels => levels;
}
