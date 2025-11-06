using Unity.VisualScripting;
using UnityEngine;

public class ResourceBase
{
	public Sprite View { get; private set; }
	public string Name { get; private set; }
	public int ID { get; private set; }
	public int MaxCount { get; set; }
	public int CurrentCount { get; set; }

	public ResourceBase(Sprite view, string name, int iD, int maxCount)
	{
		View = view;
		Name = name;
		ID = iD;
		MaxCount = maxCount;
	}

	public int ChangeCount(int count, bool plus)
	{
		if (plus) CurrentCount += count;
		else CurrentCount -= count;

		if (CurrentCount < MaxCount) return 0;
		else return MaxCount - CurrentCount;
	}
}
