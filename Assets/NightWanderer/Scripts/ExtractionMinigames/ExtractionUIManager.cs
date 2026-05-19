using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;

public class ExtractionUIManager : UIManager
{
	[SerializeField] private UIDocument _extractionGameLaserUI;
	private VisualElement _mainElement;
	private VisualElement _mainExtractionLaserElement;
	private VisualElement _mainExtractionFuelElement;

	public void Initializing()
	{
		_mainElement = _extractionGameLaserUI.rootVisualElement.Q<VisualElement>("MainElement");
		_mainExtractionLaserElement = _extractionGameLaserUI.rootVisualElement.Q<VisualElement>("ExtractionLaserBackground");
		_mainExtractionFuelElement = _extractionGameLaserUI.rootVisualElement.Q<VisualElement>("FuelBackground");
		_mainExtractionLaserElement.dataSource = new MinigameLaser(_mainExtractionLaserElement, _mainExtractionFuelElement);

		_mainElement.style.display = DisplayStyle.None;
	}

	public override void OpenUI()
	{
		_mainElement.style.display = DisplayStyle.Flex;
	}

	public override void CloseUI()
	{
		_mainElement.style.display = DisplayStyle.None;
	}

	public MinigameLaser GetMinigameLaser() => (MinigameLaser)_mainExtractionLaserElement.dataSource;
}
