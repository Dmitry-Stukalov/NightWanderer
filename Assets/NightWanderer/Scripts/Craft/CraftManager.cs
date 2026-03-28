using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;

//Нужно доделать
public class CraftManager : MonoBehaviour
{
	[SerializeField] private UIDocument _baseUI;
	[SerializeField] private VisualTreeAsset _craftPanel;
	[SerializeField] private int _craftsCount;
	private List<VisualElement> _craftList = new List<VisualElement>();
	private VisualElement _craftBackground;

	public void Initializing()
	{
		_craftBackground = _baseUI.rootVisualElement.Q<VisualElement>("CraftBackground");

		for (int i = 0; i < _craftsCount; i++)
		{
			_craftList.Add(_craftPanel.Instantiate());
		}
	}


}
