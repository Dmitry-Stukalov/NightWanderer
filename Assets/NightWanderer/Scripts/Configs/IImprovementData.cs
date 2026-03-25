using System.Collections.Generic;
using UnityEngine;

public interface IImprovementData 
{
	public Sprite Icon { get; set; }
	public int CurrentLevel { get; set; }
	public string Name { get; set; }
	public string UpgradeDescription { get; set; }
	public List<int> Resource { get; set; }  //ID ресурса
	public List<int> Count { get; set; }     //Количество
}
