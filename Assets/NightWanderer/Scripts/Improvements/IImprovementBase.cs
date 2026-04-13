using System;
using System.Collections.Generic;
public interface IImprovementBase
{
	public string Name { get; set; }
	public Dictionary<int, int> _needResources { get; set; }
	public ImprovementConfig Config { get; set; }
	public int CurrentLevel { get; set; }

	public Dictionary<int, int> GetNeedResources();
	public void Upgrade() 
	{
		CurrentLevel++;
	}
}