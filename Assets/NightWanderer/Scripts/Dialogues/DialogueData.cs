using System;
using UnityEngine;

[Serializable]
public class DialogueData
{
	[field: SerializeField] public string[] Name { get; set; }
	[TextArea(1,3)] [field: SerializeField] public string[] Phrase { get; set; }
}
