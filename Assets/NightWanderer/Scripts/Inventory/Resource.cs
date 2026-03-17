using UnityEngine;

//Реализует каждый отдельный ресурс
public class Resource : ResourceBase
{
	public Resource(Sprite view, string name, int iD, int maxCount): base(view, name, iD, maxCount) { }
}
