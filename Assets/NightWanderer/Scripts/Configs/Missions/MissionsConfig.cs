using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = nameof(MissionsConfig), menuName = nameof(MissionsConfig))]
public class MissionsConfig : ScriptableObject
{
	[field: SerializeField] public MissionData[] Missions { get; set; }
}