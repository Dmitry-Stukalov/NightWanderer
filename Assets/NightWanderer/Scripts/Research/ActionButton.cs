using UnityEngine;
using UnityEngine.UIElements;

public class ActionButton
{
	private ResearchManager _manager;
	private Button _button;
	private int _id;

	public ActionButton(ResearchManager manager, Button button)
	{
		_manager = manager;
		_button = button;

		_button.RegisterCallback<ClickEvent>(OnClick);
	}

	public void UpdateData(string text, int id)
	{
		_button.text = text;
		_id = id;
	}

	public void OnClick(ClickEvent evt)
	{
		if (_button.text == "Покинуть корабль") _manager.ShipQuit();
		else if (_button.text == "Загрузить данные с диска")
		{
			_manager.UploadData();
			//_button.style.display = DisplayStyle.None;
			_manager.DoAction(_id);
		}
		else _manager.DoAction(_id);
	}

	public void OnDisable()
	{
		_button.UnregisterCallback<ClickEvent>(OnClick);
	}
}
