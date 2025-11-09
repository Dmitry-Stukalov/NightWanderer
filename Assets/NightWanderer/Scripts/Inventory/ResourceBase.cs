using Unity.VisualScripting;
using UnityEngine;


//Родительский класс всех ресурсов, который хранит их общую информацию
public class ResourceBase
{
	public Sprite View { get; private set; }
	public string Name { get; private set; }
	public int ID { get; private set; }
	public int MaxCount { get; set; }
	public int CurrentCount { get; set; } = 0;

	public ResourceBase(Sprite view, string name, int iD, int maxCount)
	{
		View = view;
		Name = name;
		ID = iD;
		MaxCount = maxCount;
	}

	public void ChangeCount(int count, bool plus)
	{
		if (plus) CurrentCount += count;
		else CurrentCount -= count;

		//if (CurrentCount < MaxCount) return 0;
		//else return MaxCount - CurrentCount;
	}

	public int SetCount(int count) => CurrentCount = count;
}
