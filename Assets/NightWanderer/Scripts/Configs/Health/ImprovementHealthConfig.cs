using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(ImprovementHealthConfig), menuName = nameof(ImprovementHealthConfig))]
public class ImprovementHealthConfig : ImprovementConfig
{
	[field: SerializeField] public List<ImprovementHealthData> Levels { get; set; }
}