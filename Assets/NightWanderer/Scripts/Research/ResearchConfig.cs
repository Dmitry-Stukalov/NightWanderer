using UnityEngine;

[CreateAssetMenu(fileName = nameof(ResearchConfig), menuName = nameof(ResearchConfig))]
public class ResearchConfig : ScriptableObject
{
	[field: SerializeField] public ResearchData[] Choices { get; set; }
	[field: SerializeField] public string[] ImprovementName { get; set; }
	[field: SerializeField] public string[] CraftName { get; set; }
	[field: SerializeField] public string[] StoryName{ get; set; }
	[field: SerializeField] public int[] GiveResourceID { get; set; }
	[field: SerializeField] public int[] GiveResourceCount { get; set; }
	[field: SerializeField] public int[] NeedResourceID { get; set; }
	[field: SerializeField] public int[] NeedResourceCount { get; set; }
}
