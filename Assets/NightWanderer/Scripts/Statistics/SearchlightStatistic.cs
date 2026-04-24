using UnityEngine;
using UnityEngine.UIElements;

public class SearchlightStatistic
{
	private Searchlights _searchlights;
	private Label _text;

	public SearchlightStatistic(Label text, Searchlights searchlights)
	{
		_text = text;
		_searchlights = searchlights;
		_searchlights.OnUpgrade += UpdateData;
		UpdateData();
	}

	private void UpdateData()
	{
		_text.text = $"Прожектора: {_searchlights.GetActiveSearchlights()} шт.";
	}
}
