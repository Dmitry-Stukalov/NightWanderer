using UnityEngine;

public class CellObject
{
	public bool IsCraftCell { get; private set; }


	public CellObject(bool isCraftCell)
	{
		IsCraftCell = isCraftCell;
	}
}
