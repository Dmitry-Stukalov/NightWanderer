using Unity.Properties;
using Unity.VisualScripting;
using UnityEngine;


//Родительский класс всех ресурсов, который хранит их общую информацию
public class ResourceBase
{
	[CreateProperty]
	public Sprite View { get; set; }

	[CreateProperty]
	public string Name { get; set; }
	public int ID { get; set; } = -1;
	public int MaxCount { get; set; } = 0;
	[CreateProperty]
	public int CurrentCount { get; set; } = 0;

	public ResourceBase() 
	{
		ID = -1;
		MaxCount = 0;
		CurrentCount = 0;
	}

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
