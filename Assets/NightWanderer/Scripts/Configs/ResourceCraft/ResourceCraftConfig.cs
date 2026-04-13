using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = nameof(ResourceCraftConfig), menuName = nameof(ResourceCraftConfig))]
public class ResourceCraftConfig : ScriptableObject
{
	[field: SerializeField] public List<ResourceCraftData> CraftResources { get; set; }
}
