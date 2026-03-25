using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(ImprovementHealthConfig), menuName = nameof(ImprovementHealthConfig))]
public class ImprovementHealthConfig : ImprovementConfig, IImprovementConfig<ImprovementHealthData>
{
	[SerializeField] private List<ImprovementHealthData> levels;
	public List<ImprovementHealthData> Levels => levels;
}