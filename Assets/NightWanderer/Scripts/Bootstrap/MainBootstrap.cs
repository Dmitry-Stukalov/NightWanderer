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
	[SerializeField] private PlayerUIController _playerUIController;
	[SerializeField] private SearchlightManager _searchlightManager;
	[SerializeField] private MissionsManager _missionsManager;
	[SerializeField] private CraftManager _craftManager;
	[SerializeField] private EffectsManager _effectsManager;
	[SerializeField] private DialogueManager _dialogueManager;

	[Header("Base")]
	[SerializeField] private BaseInventory _baseInventory;

	private void Start()
	{
		SceneManager.sceneLoaded += CheckLoadScene;

		SceneManager.LoadScene("BaseScene", LoadSceneMode.Additive);
		SceneManager.LoadScene("IntroductionScene", LoadSceneMode.Additive);

		_effectsManager?.Initializing();
		_shipMovement?.Initializing();
		_playerUIController?.Initializing(_shipMovement.GetPlayerFuel(), _shipMovement.GetPlayerDefenseSystem().GetHealth(), _shipMovement.GetPlayerDefenseSystem().GetDefense(), _shipMovement.GetPlayerDefenseSystem().GetFireDefense());
		_searchlightManager.Initializing();
		_playerInventoryBuilder?.Initializing();
		_baseInventory?.Initializing();
		_inventoryButton?.Initializing();
		_missionsManager?.Initializing();
		_dialogueManager?.Initializing();

		//StartCoroutine(StartPause());
	}

	public void IntroductionSceneInitializing()
	{
		_dialogueManager.StartNewDialogue();
	}

	public void OpenSceneInitializing()
	{
		_shipMovement.OpenSceneInitializing();
		_weatherPanel?.Initializing();
		_improvementManager?.Initializing(_playerInventoryBuilder.GetPlayerInventory(), _baseInventory.GetBaseInventory());
		_improvementManager.AddImprovement(_shipMovement.GetPlayerFuel(), "Fuel");
		_improvementManager.AddImprovement(_shipMovement.GetPlayerMiningEquipment(), "Mining");
		_improvementManager.AddImprovement(_shipMovement.GetPlayerEngines(), "Engines");
		_improvementManager.AddImprovement(_shipMovement.GetPlayerSearchlights(), "Searchlight");
		_craftManager.Initializing(_playerInventoryBuilder.GetPlayerInventory(), _baseInventory.GetBaseInventory(), GameObject.FindGameObjectWithTag("ResourceLibrary").GetComponent<ResourceLibrary>());
	}

	private void CheckLoadScene(Scene scene, LoadSceneMode mode)
	{
		if (scene.name == "IntroductionScene") IntroductionSceneInitializing();

		if (scene.name == "OpenMapScene") OpenSceneInitializing();
	}

	/*private IEnumerator StartPause()
	{
		yield return new WaitForSeconds(1);

		_improvementManager?.Initializing(_playerInventoryBuilder.GetPlayerInventory(), _baseInventory.GetBaseInventory());
		_improvementManager.AddImprovement(_shipMovement.GetPlayerFuel(), "Fuel");
		_improvementManager.AddImprovement(_shipMovement.GetPlayerMiningEquipment(), "Mining");
		_improvementManager.AddImprovement(_shipMovement.GetPlayerEngines(), "Engines");
		_craftManager.Initializing(_playerInventoryBuilder.GetPlayerInventory(), _baseInventory.GetBaseInventory(), GameObject.FindGameObjectWithTag("ResourceLibrary").GetComponent<ResourceLibrary>());
		_weatherPanel?.Initializing();

		_dialogueManager.StartNewDialogue();
	}*/
}
