using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

//Конфиг в котором хранятся значения всех ресурсов
[CreateAssetMenu(fileName = nameof(ResourceConfig), menuName = nameof(ResourceConfig))]
public class ResourceConfig : ScriptableObject
{
	[field: SerializeField] public List<ResourceData> Resources;
}
