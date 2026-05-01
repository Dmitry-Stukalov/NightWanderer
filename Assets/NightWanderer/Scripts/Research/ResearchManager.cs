using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using static UnityEngine.Rendering.STP;

public class ResearchManager : MonoBehaviour
{
	[SerializeField] private UIDocument _researchUI;
	private Label _researchShipText;
	private List<Button> _actionButtons = new List<Button>();
	private ResearchShip _currentResearchShip;
	private ResearchConfig _currentConfig;
	private bool IsDataUpload = false;

	public void Initializing()
	{
		_researchShipText = _researchUI.rootVisualElement.Q<Label>("ResearchShipText");

		foreach (var button in _researchUI.rootVisualElement.Q<VisualElement>("ActionsBackground").Query<Button>("Action").ToList())
		{
			button.dataSource = new ActionButton(this, button);
			_actionButtons.Add(button);
		}

		GameEvents.OnResearchNearBy += UpdateData;
	}
	
	public void UpdateData(ResearchShip ship)
	{
		_currentResearchShip = ship.gameObject.GetComponent<ResearchShip>();
		_currentConfig = _currentResearchShip.GetResearchConfig();

		IsDataUpload = false;
		DoAction(0);
	}

	public void DoAction(int id)
	{
		/*if (_currentConfig.Choices[id].ResearchText == "Данные получены")
		{
			_currentResearchShip.Search();
			GameEvents.OnResearchEnd?.Invoke();
		}*/

		for (int i = 0; i < _actionButtons.Count; i++) _actionButtons[i].style.display = DisplayStyle.None;

		_researchShipText.text = _currentConfig.Choices[id].ResearchText;

		for (int i = 0; i < _currentConfig.Choices[id].ActionsText.Length; i++)
		{
			if ((_currentConfig.Choices[id].ActionsText[i] == "Загрузить данные с диска" || _currentConfig.Choices[id].ActionsText[i] == "Попытаться найти диск") && IsDataUpload) continue;
			else
			{
				((ActionButton)_actionButtons[i].dataSource).UpdateData(_currentConfig.Choices[id].ActionsText[i], _currentConfig.Choices[id].ActionsWay[i]);
				_actionButtons[i].style.display = DisplayStyle.Flex;
			}
		}
	}

	public void UploadData()
	{
		IsDataUpload = true;
		_currentResearchShip.Search();

		if (_currentConfig.ImprovementName.Length != 0) GameEvents.OnResearchEnd?.Invoke(MatchResearch(_currentConfig.ImprovementName[0]));

		if (_currentConfig.CraftName.Length != 0) GameEvents.OnResearchEnd?.Invoke(MatchResearch(_currentConfig.CraftName[0]));

		if (_currentConfig.StoryName.Length != 0) GameEvents.OnResearchEnd?.Invoke(MatchResearch(_currentConfig.StoryName[0]));
	}

	public void ShipQuit()
	{
		DoAction(0);
		GameEvents.OnResearchQuit?.Invoke();
	}

	private void OnDisable()
	{
		GameEvents.OnResearchNearBy -= UpdateData;

		for (int i = 0; i < _actionButtons.Count; i++) ((ActionButton)_actionButtons[i].dataSource).OnDisable();
	}

	private string MatchResearch(string research)
	{
		string text = "Хз что это";

		switch(research)
		{
			case "Fuel":
				text = "Открыто улучшение топливных баков";
			break;

			case "Health":
				text = "Открыто улучшение здоровья";
			break;

			case "Defense":
				text = "Открыто улучшение защиты";
			break;

			case "FireDefense":
				text = "Открыто улучшение термальной защиты";
			break;

			case "Engines":
				text = "Открыто улучшение двигателей";
			break;

			case "Searchlight":
				text = "Открыто улучшение прожекторов";
			break;

			case "BaseKey":
				text = "Найден ключ доступа северной базы";
				GameEvents.OnMissionComplete?.Invoke(3);
			break;
		}

		return text;
	}
}
