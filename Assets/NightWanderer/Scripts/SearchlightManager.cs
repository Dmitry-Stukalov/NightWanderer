using UnityEngine;

public class SearchlightManager : MonoBehaviour
{
	[SerializeField] private float _innerSpotAngle;
	[SerializeField] private float _outerSpotAngle;
	[SerializeField] private float _intensity;
	[SerializeField] private float _range;
	[SerializeField] private Sun _sun;
	[SerializeField] private Searchlight[] _searchlights;
	private int _searchState = 0;

	public void Initializing()
	{
		for (int i = 0; i < _searchlights.Length; i++)
		{
			_searchlights[i].Initializing(_sun);
			_searchlights[i].PasteValues(_innerSpotAngle, _outerSpotAngle, _intensity, _range);
			_searchlights[i].GetLight().shadows = LightShadows.Hard;
		}
	}

	public void SearchlightOnOff()
	{
		for (int i = 0; i < _searchlights.Length; i++) _searchlights[i].SearchlightOnOff();
	}

	public void StartSearch()
	{
		if (_searchState == 1) return;

		for (int i = 0; i < _searchlights.Length; i++) _searchlights[i].StartSearch();

		_searchState = 1;
	}

	public void StartMove()
	{
		if (_searchState == 2) return;

		for (int i = 0; i < _searchlights.Length; i++) _searchlights[i].StartMove();

		_searchState = 2;
	}
	
	public void StartFight()
	{
		if (_searchState == 3) return;

		for (int i = 0; i < _searchlights.Length; i++) _searchlights[i].StartFight();

		_searchState = 3;
	}
}
