using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;

public interface IImprovementBase
{
	public ImprovementConfig Config { get; set; }
	public int CurrentLevel { get; set; }


	public void Upgrade() { }
}