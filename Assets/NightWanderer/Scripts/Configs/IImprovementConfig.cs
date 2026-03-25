using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public interface IImprovementConfig<T> where T : IImprovementData
{
	public List<T> Levels { get; }
}
