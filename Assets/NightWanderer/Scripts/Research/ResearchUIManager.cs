using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using static UnityEngine.Rendering.STP;
using DG.Tweening;
using System.Collections;

public class ResearchUIManager : UIManager
{
	[SerializeField] private UIDocument _researchUI;
	private VisualElement _mainElement;
	private VisualElement _researchHintPanel;
	private Label _researchShipText;
	private List<Button> _actionButtons = new List<Button>();
	private ResearchShip _currentResearchShip;
	private ResearchConfig _currentConfig;
	private bool IsDataUpload = false;

	public void Initializing()
	{
		_mainElement = _researchUI.rootVisualElement.Q<VisualElement>("MainElement");
		_researchHintPanel = _researchUI.rootVisualElement.Q<VisualElement>("ResearchOpenPanel");

		_researchShipText = _researchUI.rootVisualElement.Q<Label>("ResearchShipText");

		foreach (var button in _researchUI.rootVisualElement.Q<VisualElement>("ActionsBackground").Query<Button>("Action").ToList())
		{
			button.dataSource = new ActionButton(this, button);
			_actionButtons.Add(button);
		}

		GameEvents.OnResearchNearBy += UpdateData;
		GameEvents.OnResearchStart += OnResearchStart;
		GameEvents.OnResearchEnd += OnResearchEnd;
		GameEvents.OnResearchQuit += OnResearchQuit;

		_mainElement.style.display = DisplayStyle.None;
	}
	

	public void UpdateData(ResearchShip ship)
	{
		_currentResearchShip = ship.gameObject.GetComponent<ResearchShip>();
		_currentConfig = _currentResearchShip.GetResearchConfig();

		IsDataUpload = false;
		DoAction(0);
	}

	private void OnResearchStart()
	{
		//_mainElement.style.display = DisplayStyle.Flex;
		UnityEngine.Cursor.lockState = CursorLockMode.None;
		UnityEngine.Cursor.visible = true;
	}

	private void OnResearchEnd(string newText)
	{
		StartCoroutine(ShowResearchOpenPanel(newText));
	}

	private void OnResearchQuit()
	{
		//_mainElement.style.display = DisplayStyle.None;
		UnityEngine.Cursor.lockState = CursorLockMode.Locked;
		UnityEngine.Cursor.visible = false;
	}

	private IEnumerator ShowResearchOpenPanel(string newText)
	{
		_researchHintPanel.Q<Label>("ResearchOpenText").text = newText;

		DOTween.To(() => _researchHintPanel.style.opacity.value, x => _researchHintPanel.style.opacity = x, 1, 1.5f);

		yield return new WaitForSeconds(3f);

		DOTween.To(() => _researchHintPanel.style.opacity.value, x => _researchHintPanel.style.opacity = x, 0, 1.5f);
	}

	public void DoAction(int id)
	{
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

	public override void OpenUI()
	{
		_researchUI.sortingOrder = 5;
		_mainElement.style.display = DisplayStyle.Flex;
	}

	public override void CloseUI()
	{
		_researchUI.sortingOrder = -5;

		_mainElement.style.display = DisplayStyle.None;

		GameEvents.OnResearchQuit?.Invoke();
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
				text = "Открыто улучшение термической защиты";
				GameEvents.OnMissionComplete?.Invoke(5);
				break;

			case "Engines":
				text = "Открыто улучшение двигателей";
			break;

			case "SearchlightPower":
				text = "Открыто улучшение прожекторов";
			break;

			case "BaseKey":
				text = "Найден ключ доступа северной базы";
				GameEvents.OnMissionComplete?.Invoke(6);
			break;
		}

		return text;
	}

	private void OnDisable()
	{
		GameEvents.OnResearchNearBy -= UpdateData;
		GameEvents.OnResearchStart -= OnResearchStart;
		GameEvents.OnResearchEnd -= OnResearchEnd;
		GameEvents.OnResearchQuit -= OnResearchQuit;

		for (int i = 0; i < _actionButtons.Count; i++) ((ActionButton)_actionButtons[i].dataSource).OnDisable();
	}
}
