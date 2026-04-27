using UnityEngine;

[CreateAssetMenu(fileName = nameof(ResearchConfig), menuName = nameof(ResearchConfig))]
public class ResearchConfig : ScriptableObject
{
	[SerializeField] public ResearchData[] Choices;
}
