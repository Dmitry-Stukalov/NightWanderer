using UnityEngine;
using UnityEngine.UIElements;

public class CreatedCellObject : VisualElement
{
	public Vector2Int Index { get; private set; }

	public CreatedCellObject(Vector2Int index)
	{
		Index = index;
	}
}
