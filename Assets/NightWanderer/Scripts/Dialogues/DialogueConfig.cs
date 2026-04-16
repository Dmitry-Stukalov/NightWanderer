using UnityEngine;

[CreateAssetMenu(fileName = nameof(DialogueConfig), menuName = nameof(DialogueConfig))]
public class DialogueConfig : ScriptableObject
{
	[SerializeField] public DialogueData[] Dialogues;
}
