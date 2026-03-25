using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = nameof(ImprovementEnginesConfig), menuName = nameof(ImprovementEnginesConfig))]
public class ImprovementEnginesConfig : ImprovementConfig
{
	[field: SerializeField] public List<ImprovementEnginesData> Levels { get; set; }
}
