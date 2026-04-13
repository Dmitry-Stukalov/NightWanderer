using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public class ResourceCraftData : ResourceData
{
	[field: SerializeField] public List<int> ResourcesIDToCraft { get; set; }
	[field: SerializeField] public List<int> ResourcesCountToCraft { get; set; }
}
