using System;
using UnityEngine;

[Serializable]
public class DialogueData
{
	[field: SerializeField] public string[] Name { get; set; }
	[field: SerializeField] public string[] Phrase { get; set; }
}
