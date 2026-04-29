using UnityEngine;

[CreateAssetMenu(fileName = nameof(ResearchConfig), menuName = nameof(ResearchConfig))]
public class ResearchConfig : ScriptableObject
{
	[SerializeField] public ResearchData[] Choices;
	[SerializeField] public string[] ImprovementName;
	[SerializeField] public string[] CraftName;
	[SerializeField] public string[] StoryName;
}
