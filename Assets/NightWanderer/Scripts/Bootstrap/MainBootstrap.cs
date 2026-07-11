using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainBootstrap : MonoBehaviour
{
	[Header("Environment")]
	//[SerializeField] private Sun _sun;
	[SerializeField] private WeatherPanel _weatherPanel;

	[Header("Player")]
	[SerializeField] private ShipMovement _shipMovement;
	[SerializeField] private PlayerInventoryBuilder _playerInventoryBuilder;
	[SerializeField] private InventoryButton _inventoryButton;
	[SerializeField] private ImprovementManager _improvementManager;
	[SerializeField] private SearchlightManager _searchlightManager;
	[SerializeField] private MissionsManager _missionsManager;
	[SerializeField] private CraftManager _craftManager;
	[SerializeField] private EffectsManager _effectsManager;
	[SerializeField] private DialogueManager _dialogueManager;
	[SerializeField] private StatisticsManager _statisticsManager;
	[SerializeField] private PlayerUIManager _playerUIManager;
	[SerializeField] private BaseUIManager _baseUIManager;
	[SerializeField] private ResearchUIManager _researchUIManager;
	[SerializeField] private ExtractionUIManager _extractionUIManager;
	[SerializeField] private SettingsUIManager _settingsUIManager;
	[SerializeField] private ShipSoundsManager _shipSoundsManager;

	[Header("Base")]
	[SerializeField] private BaseInventory _baseInventory;

	private bool IsDataLoad = false;


	private void Awake()
	{
		SceneManager.sceneLoaded += CheckLoadScene;

		_effectsManager?.Initializing();

		_searchlightManager.Initializing(); 
		_baseInventory?.Initializing();
		_missionsManager?.Initializing();
		_dialogueManager?.Initializing();
		_settingsUIManager?.Initializing();

		GameEvents.OnSceneLoad += LoadData;

		StartCoroutine(StartPause());
	}

	private IEnumerator StartPause()
	{
		yield return new WaitForSecondsRealtime(1.5f);

		_playerInventoryBuilder?.Initializing();
		_inventoryButton?.Initializing();
		_shipMovement?.Initializing();
		_playerUIManager?.Initializing(_shipMovement.GetPlayerFuel(), _shipMovement.GetPlayerDefenseSystem().GetHealth(), _shipMovement.GetPlayerDefenseSystem().GetDefense(), _shipMovement.GetPlayerDefenseSystem().GetFireDefense());
		_baseUIManager?.Initializing(_shipMovement.GetPlayerFuel(), _shipMovement.GetPlayerDefenseSystem().GetHealth(), _shipMovement.GetPlayerDefenseSystem().GetDefense(), _shipMovement.GetPlayerDefenseSystem().GetFireDefense());
		_statisticsManager?.Initializing(_shipMovement.GetPlayerFuel(), _shipMovement.GetPlayerDefenseSystem().GetHealth(), _shipMovement.GetPlayerDefenseSystem().GetDefense(), _shipMovement.GetPlayerDefenseSystem().GetFireDefense(), _shipMovement.GetPlayerEngines(), _playerInventoryBuilder.GetPlayerInventory(), _shipMovement.GetPlayerSearchlights());
		_researchUIManager?.Initializing();
		_extractionUIManager?.Initializing();
		_settingsUIManager?.Initializing();

		yield return new WaitForSecondsRealtime(1.5f);

		if (!IsDataLoad)
		{
			SceneManager.LoadScene("BaseScene", LoadSceneMode.Additive);
			SceneManager.LoadScene("IntroductionScene", LoadSceneMode.Additive);
		}
	}

	public void IntroductionSceneInitializing()
	{
		//_dialogueManager.StartNewDialogue();
	}

	public IEnumerator OpenSceneInitializing()
	{
		yield return new WaitForSecondsRealtime(1f);

		_shipMovement.OpenSceneInitializing();
		_weatherPanel?.Initializing();
		_improvementManager?.Initializing(_playerInventoryBuilder.GetPlayerInventory(), _baseInventory.GetBaseInventory());
		_improvementManager.AddImprovement(_shipMovement.GetPlayerFuel(), "Fuel");
		_improvementManager.AddImprovement(_shipMovement.GetPlayerMiningEquipment(), "Mining");
		_improvementManager.AddImprovement(_shipMovement.GetPlayerEngines(), "Engines");
		_improvementManager.AddImprovement(_shipMovement.GetPlayerSearchlights(), "Searchlight");
		_improvementManager.AddImprovement(_shipMovement.GetPlayerSearchlightsPower(), "SearchlightPower");
		_craftManager.Initializing(_playerInventoryBuilder.GetPlayerInventory(), _baseInventory.GetBaseInventory(), GameObject.FindGameObjectWithTag("ResourceLibrary").GetComponent<ResourceLibrary>());
		_shipSoundsManager?.Initializing(FindAnyObjectByType<Sun>());
		_playerInventoryBuilder?.InitializeInventoryLibrary(GameObject.FindGameObjectWithTag("ResourceLibrary").GetComponent<ResourceLibrary>());
		_baseInventory?.InitializeInventoryLibrary(GameObject.FindGameObjectWithTag("ResourceLibrary").GetComponent<ResourceLibrary>());
	}

	private void CheckLoadScene(UnityEngine.SceneManagement.Scene scene, LoadSceneMode mode)
	{
		if (scene.name == "IntroductionScene") IntroductionSceneInitializing();

		if (scene.name == "OpenMapScene") StartCoroutine(OpenSceneInitializing());
	}

	private void LoadData(string sceneName)
	{
		SceneManager.LoadScene("BaseScene", LoadSceneMode.Additive);

		if (sceneName == "IntroductionScene" || sceneName == "") SceneManager.LoadScene("IntroductionScene", LoadSceneMode.Additive);

		if (sceneName == "OpenMapScene") SceneManager.LoadScene("OpenMapScene", LoadSceneMode.Additive);

		IsDataLoad = true;
	}

	private void OnDisable()
	{
		SceneManager.sceneLoaded -= CheckLoadScene;
		GameEvents.OnSceneLoad -= LoadData;
	}
}