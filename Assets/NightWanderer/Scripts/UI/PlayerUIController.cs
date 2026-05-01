using DG.Tweening;
using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

//Отвечает за управление всем UI
public class PlayerUIController : MonoBehaviour
{
	[SerializeField] private UIDocument PlayerUI;
	[SerializeField] private UIDocument ExtractionGameLaser;
	[SerializeField] private UIDocument ResearchUI;
	[SerializeField] private VisualTreeAsset _statusPanel;
	[SerializeField] private Sprite _statusIcon;
	[SerializeField] private InventoryButton Inventory;
	[SerializeField] private BaseUIManager _baseUI;
	[SerializeField] private SettingsManager _settingsManager;
	private VisualElement _mainExtractionLaserElement;
	private VisualElement _mainExtractionFuelElement;
	private VisualElement _mainResearchElement;
	private VisualElement _blackBackground;
	private VisualElement _tutorialPanel;
	private VisualElement _hintPanel;
	private VisualElement _researchHintPanel;
	private VisualElement _researchOpenPanel;
	private Dictionary<string, VisualElement> _statusPanels;
	private TutorialManager _tutorialManager;

	public void Initializing(Fuel fuel, HealthFireDefense health, HealthFireDefense defense, HealthFireDefense fireDefense)
	{
		StartCoroutine(StartPause());

		_blackBackground = PlayerUI.rootVisualElement.Q<VisualElement>("BlackBackground");
		GameEvents.OnFirstBaseVisit += () => StartCoroutine(OnBasePause());

		_tutorialPanel = PlayerUI.rootVisualElement.Q<VisualElement>("TutorialPanel");
		_tutorialPanel.dataSource = new TutorialManager(_tutorialPanel);
		_tutorialManager = (TutorialManager)_tutorialPanel.dataSource;

		_hintPanel = PlayerUI.rootVisualElement.Q<VisualElement>("HintPanel");
		_hintPanel.style.opacity = 0;

		//_researchHintPanel = PlayerUI.rootVisualElement.Q<VisualElement>("ResearchOpenPanel");

		var fuelItemBackground = PlayerUI.rootVisualElement.Q<VisualElement>("FuelBackground");
		fuelItemBackground.dataSource = new FuelRecovery(fuel, PlayerUI.rootVisualElement.Q<VisualElement>("FuelForeground"));

		var healthItemBackground = PlayerUI.rootVisualElement.Q<VisualElement>("HealthBackground");
		healthItemBackground.dataSource = new HealthFireDefenseRecovery(health, PlayerUI.rootVisualElement.Q<VisualElement>("HealthForeground"));

		var defenseItemBackground = PlayerUI.rootVisualElement.Q<VisualElement>("DefenseBackground");
		defenseItemBackground.dataSource = new HealthFireDefenseRecovery(defense, PlayerUI.rootVisualElement.Q<VisualElement>("DefenseForeground"));

		var fireDefenseItemBackground = PlayerUI.rootVisualElement.Q<VisualElement>("FireDefenseBackground");
		fireDefenseItemBackground.dataSource = new HealthFireDefenseRecovery(fireDefense, PlayerUI.rootVisualElement.Q<VisualElement>("FireDefenseForeground"));

		_statusPanels = new Dictionary<string, VisualElement>();

		GameEvents.OnCriticalStatusShow += ShowStatusPanel;
		GameEvents.OnCriticalStatusHide += HideStatusPanel;

		_baseUI.Initializing(fuel, health, defense, fireDefense);

		GameEvents.OnLaserExtractionStart += OnExtractionLaserStart;
		GameEvents.OnExtractionEnd += OnExtractionLaserEnd;
		
		GameEvents.OnResearchStart += OnResearchStart;
		GameEvents.OnResearchEnd += OnResearchEnd;
		GameEvents.OnResearchQuit += OnResearchQuit;

		GameEvents.OnDeath += () => StartCoroutine(DeathPause());
	}

	private IEnumerator StartPause()
	{
		yield return new WaitForSeconds(1f);

		_mainExtractionLaserElement = ExtractionGameLaser.rootVisualElement.Q<VisualElement>("ExtractionLaserBackground");
		_mainExtractionFuelElement = ExtractionGameLaser.rootVisualElement.Q<VisualElement>("FuelBackground");
		_mainExtractionLaserElement.dataSource = new MinigameLaser(_mainExtractionLaserElement, _mainExtractionFuelElement);

		_mainExtractionLaserElement.style.display = DisplayStyle.None;
		_mainExtractionFuelElement.style.display = DisplayStyle.None;

		_mainResearchElement = ResearchUI.rootVisualElement.Q<VisualElement>("MainElement");

		_researchHintPanel = PlayerUI.rootVisualElement.Q<VisualElement>("ResearchOpenPanel");
		//_researchHintPanel.style.display = DisplayStyle.None;
		_mainResearchElement.style.display = DisplayStyle.None;

		//yield return new WaitForSeconds(60f);
		StartGame();
	}

	private IEnumerator OnBasePause()
	{
		_blackBackground.style.display = DisplayStyle.Flex;
		DOTween.To(() => _blackBackground.resolvedStyle.opacity, x => _blackBackground.style.opacity = x, 1, 1f);

		yield return new WaitForSeconds(4f);

		GameEvents.OnDialogueStart?.Invoke();

		yield return new WaitForSeconds(70);

		DOTween.To(() => _blackBackground.resolvedStyle.opacity, x => _blackBackground.style.opacity = x, 0, 2f).OnComplete(() => _blackBackground.style.display = DisplayStyle.None);
	}

	private IEnumerator DeathPause()
	{
		yield return new WaitForSeconds(2f);

		_blackBackground.style.display = DisplayStyle.Flex;
		DOTween.To(() => _blackBackground.resolvedStyle.opacity, x => _blackBackground.style.opacity = x, 1, 1f);

		yield return new WaitForSeconds(7f);

		DOTween.To(() => _blackBackground.resolvedStyle.opacity, x => _blackBackground.style.opacity = x, 0, 2f).OnComplete(() => _blackBackground.style.display = DisplayStyle.None);
	}

	private void StartGame()
	{
		DOTween.To(() => _blackBackground.resolvedStyle.opacity, x => _blackBackground.style.opacity = x, 0, 5f)
		.OnComplete(() =>
		{
			GameEvents.OnGameStart?.Invoke();
			OpenTutorial(new int[] {0, 1, 2, 3, 6});
		});
	}

	public void ShowStatusPanel(string name, string panelText)
	{
        if (!_statusPanels.ContainsKey(name))
        {
			var newStatusPanel = _statusPanel.Instantiate();
			newStatusPanel.Q<VisualElement>("StatusIcon").style.backgroundImage = new StyleBackground(_statusIcon);
			newStatusPanel.Q<Label>("StatusText").text = panelText;

			_statusPanels[name] = newStatusPanel.Q<VisualElement>("PanelBackground");
			PlayerUI.rootVisualElement.Q<VisualElement>("CriticalPanelPlace").Add(newStatusPanel.Q<VisualElement>("PanelBackground"));
		}
		else
		{
			_statusPanels[name].style.display = DisplayStyle.Flex;
		}
    }

	private IEnumerator ShowResearchOpenPanel(string newText)
	{
		_researchHintPanel.Q<Label>("ResearchOpenText").text = newText;

		DOTween.To(() => _researchHintPanel.style.opacity.value, x => _researchHintPanel.style.opacity = x, 1, 1.5f);

		yield return new WaitForSeconds(3f);

		DOTween.To(() => _researchHintPanel.style.opacity.value, x => _researchHintPanel.style.opacity = x, 0, 1.5f);
	}

	public void ShowHint()
	{
		DOTween.To(() => _hintPanel.resolvedStyle.opacity, x => _hintPanel.style.opacity = x, 1, 2f);
	}

	public void HideHint()
	{
		DOTween.To(() => _hintPanel.resolvedStyle.opacity, x => _hintPanel.style.opacity = x, 0, 2f);
	}

	public void HideStatusPanel(string name)
	{
		if (!_statusPanels.ContainsKey(name)) return;

		_statusPanels[name].style.display = DisplayStyle.None;
	}

	public void OnBase()
	{
		_baseUI.OpenBaseUI();
		Inventory.CloseWeatherPanel();
		Inventory.CloseInventoryPanel();

		HideHint();
	}

	public void OutBase()
	{
		_baseUI.CloseBaseUI();
		Inventory.OpenWeatherPanel();

		ShowHint();
	}

	private void OnExtractionLaserStart()
	{
		_mainExtractionLaserElement.style.display = DisplayStyle.Flex;
		_mainExtractionFuelElement.style.display = DisplayStyle.Flex;
	}

	private void OnExtractionLaserEnd()
	{
		_mainExtractionLaserElement.style.display = DisplayStyle.None;
		_mainExtractionFuelElement.style.display = DisplayStyle.None;
	}

	private void OnResearchStart()
	{
		ResearchUI.sortingOrder = 10;
		_mainResearchElement.style.display = DisplayStyle.Flex;
		UnityEngine.Cursor.lockState = CursorLockMode.None;
		UnityEngine.Cursor.visible = true;
		//_researchHintPanel.style.display = DisplayStyle.Flex;
	}

	private void OnResearchEnd(string newText)
	{
		StartCoroutine(ShowResearchOpenPanel(newText));
	}

	private void OnResearchQuit()
	{
		ResearchUI.sortingOrder = 0;
		_mainResearchElement.style.display = DisplayStyle.None;
		UnityEngine.Cursor.lockState = CursorLockMode.Locked;
		UnityEngine.Cursor.visible = false;
		//_researchHintPanel.style.display = DisplayStyle.None;
	}

	public MinigameLaser GetMinigameLaser() => (MinigameLaser)_mainExtractionLaserElement.dataSource;

	public void OpenTutorial(int[] id)
	{
		for (int i = 0; i < id.Length; i++) _tutorialManager.OpenPanel(id[i]);

		StartCoroutine(TutorialPause(id));
	}

	private IEnumerator TutorialPause(int[] id)
	{
		yield return new WaitForSeconds(5f);

		CloseTutorial(id);
	}

	public void CloseTutorial(int[] id)
	{
		for (int i = 0; i < id.Length; i++) _tutorialManager.ClosePanel(id[i]);
	}

	private void Update()
	{
		//if (!SceneManager.GetSceneByName("OpenMapScene").isLoaded) return;

		if (Keyboard.current.tabKey.wasPressedThisFrame && !_baseUI.OnBase) Inventory.OpenCloseInventory();

		if (Keyboard.current.escapeKey.wasPressedThisFrame) _settingsManager.OpenSettings();
	}

	public VisualElement GetVisualElement(string name) => PlayerUI.rootVisualElement.Q<VisualElement>(name);

	private void OnDisable()
	{
		GameEvents.OnLaserExtractionStart -= OnExtractionLaserStart;
		GameEvents.OnExtractionEnd -= OnExtractionLaserEnd;
		GameEvents.OnCriticalStatusShow -= ShowStatusPanel;
		GameEvents.OnCriticalStatusHide -= HideStatusPanel;
		GameEvents.OnFirstBaseVisit -= () => StartCoroutine(OnBasePause());
		GameEvents.OnResearchStart -= OnResearchStart;
		GameEvents.OnResearchEnd -= OnResearchEnd;
		GameEvents.OnResearchQuit -= OnResearchQuit;

		GameEvents.OnDeath -= () => StartCoroutine(DeathPause());
	}
}
