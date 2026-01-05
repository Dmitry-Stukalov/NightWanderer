using Unity.VisualScripting;
using UnityEngine;


//Родительский класс всех ресурсов, который хранит их общую информацию
public class ResourceBase
{
	public Sprite View { get; set; }
	public string Name { get; set; }
	public int ID { get; set; } = -1;
	public int MaxCount { get; set; } = 0;
	public int CurrentCount { get; set; } = 0;

	public ResourceBase() { }

	public ResourceBase(Sprite view, string name, int iD, int maxCount)
	{
		View = view;
		Name = name;
		ID = iD;
		MaxCount = maxCount;
	}

	public int SetCount(int count) => CurrentCount = count;

	public void ResetValue()
	{
		View = null;
		Name = "0";
		ID = -1;
		MaxCount = 0;
		CurrentCount = 0;
	}
}
