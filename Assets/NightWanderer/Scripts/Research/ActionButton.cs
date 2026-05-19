using UnityEngine;
using UnityEngine.UIElements;

public class ActionButton
{
	private ResearchUIManager _manager;
	private Button _button;
	private int _id;

	public ActionButton(ResearchUIManager manager, Button button)
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
		if (_button.text == "Отстыковаться") _manager.CloseUI();
		else if (_button.text == "Загрузить данные с диска")
		{
			_manager.UploadData();
			_manager.DoAction(_id);
		}
		else _manager.DoAction(_id);
	}

	public void OnDisable()
	{
		_button.UnregisterCallback<ClickEvent>(OnClick);
	}
}
