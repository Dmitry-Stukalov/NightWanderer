using UnityEngine;

public class Resource : ResourceBase
{
	[field: SerializeField] public string Name { get; private set; }
	[field: SerializeField] public int ID { get; private set; }
	[field: SerializeField] public int MaxCount { get; set; }
	public int CurrentCount { get; set; }

	public int ChangeCount(int count, bool plus)
	{
		if (plus) CurrentCount += count;
		else CurrentCount -= count;

		if (CurrentCount < MaxCount) return 0;
		else return MaxCount - CurrentCount;
	}
}
