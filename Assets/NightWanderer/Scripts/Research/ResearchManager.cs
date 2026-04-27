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
		GameEvents.OnResearchEnd?.Invoke();
	}

	public void ShipQuit()
	{
		GameEvents.OnResearchQuit?.Invoke();
	}

	private void OnDisable()
	{
		GameEvents.OnResearchNearBy -= UpdateData;

		for (int i = 0; i < _actionButtons.Count; i++) ((ActionButton)_actionButtons[i].dataSource).OnDisable();
	}
}
