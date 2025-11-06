using System;
using UnityEngine;

[Serializable]
public class ResourceData
{
	[field: SerializeField] public Sprite View { get; set; }
	[field: SerializeField] public string Name { get; set; }
	[field: SerializeField] public int ID { get; set; }
	[field: SerializeField] public int MaxCount { get; set; }
}
