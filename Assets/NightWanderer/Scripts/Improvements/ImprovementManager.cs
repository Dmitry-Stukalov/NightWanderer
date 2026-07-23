using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;
using System.Collections;
using DG.Tweening;

public class ImprovementManager : MonoBehaviour
{
	[SerializeField] private UIDocument _baseUI;
	[SerializeField] private VisualTreeAsset _improvementTree;
	[SerializeField] private VisualTreeAsset _upgradePanel;
	[SerializeField] private VisualTreeAsset _needResourceGroup;
	[SerializeField] private float minScale = 0.1f;
	[SerializeField] private float maxScale = 3f;
	[SerializeField] private float zoomStep = 0.1f;
	private ResourceLibrary _library;
	private VisualElement _improvementMessageBackground;
	private VisualElement _improvementTreeBackground;
	private Label _improvementMessageText;
	private Dictionary<string, IImprovementBase> _improvements = new Dictionary<string, IImprovementBase>();
	private Inventory _playerInventory;
	private Inventory _baseInventory;

	private ScrollView _upgradesBackground;

	private Vector2 lastMousePosition;
	private float currentScale = 1;
	private bool isDragging = false;

	public void Initializing(Inventory playerInventory, Inventory baseInventory, ResourceLibrary library)
	{
		_library = library;
		_playerInventory = playerInventory;
		_baseInventory = baseInventory;

		_improvementMessageBackground = _baseUI.rootVisualElement.Q<VisualElement>("ImprovementMessagePanel");
		_improvementMessageText = _baseUI.rootVisualElement.Q<Label>("ImprovementMessageText");

		_upgradesBackground = _baseUI.rootVisualElement.Q<ScrollView>("UpgradesBackground");

		_improvementTreeBackground = _improvementTree.Instantiate().hierarchy.ElementAt(0);
		_upgradesBackground.Add(_improvementTreeBackground);

		_improvementTreeBackground.RegisterCallback<WheelEvent>(OnScrollWheel);

		_improvementTreeBackground.RegisterCallback<MouseDownEvent>(MouseDown);
		_improvementTreeBackground.RegisterCallback<MouseMoveEvent>(MouseMove);
		_improvementTreeBackground.RegisterCallback<MouseUpEvent>(MouseUp);
		_improvementTreeBackground.RegisterCallback<MouseLeaveEvent>(MouseLeave);

		GameEvents.OnImprovementOpen += UnlockImprovement;

		GameEvents.OnImprovementsLoad += LoadData;
		GameEvents.OnImprovementPanelsLoad += LoadData;
		GameEvents.OnSave += SaveData;

		StartCoroutine(StartPause());
	}

	private void OnScrollWheel(WheelEvent evt)
	{
		if (evt.ctrlKey || evt.actionKey)
		{
			evt.StopPropagation();

			if (evt.delta.y < 0) currentScale += zoomStep;
			else if (evt.delta.y > 0) currentScale -= zoomStep;

			currentScale = Mathf.Clamp(currentScale, minScale, maxScale);

			_improvementTreeBackground.style.scale = new Scale(new Vector3(currentScale, currentScale, 1));
		}
		else
		{
			_improvementTreeBackground.focusController.IgnoreEvent(evt);
			evt.StopPropagation();
		}
	}

	private void MouseDown(MouseDownEvent evt)
	{
		if (evt.button == 0)
		{
			isDragging = true;
			lastMousePosition = evt.mousePosition;

			// Захватываем мышь, чтобы движения считывались, даже если выйти за пределы кнопок
			_improvementTreeBackground.CaptureMouse();
			evt.StopPropagation();
		}
	}

	private void MouseMove(MouseMoveEvent evt)
	{
		if (!isDragging) return;

		// Вычисляем, на сколько сместилась мышь с прошлого кадра
		Vector2 delta = evt.mousePosition - lastMousePosition;

		// Двигаем scrollOffset в противоположную от мыши сторону (эффект "тянем холст")
		// Делим на текущий масштаб, чтобы скорость перетаскивания не дергалась при зуме
		_upgradesBackground.scrollOffset -= delta / currentScale;

		// Запоминаем текущую позицию для следующего кадра
		lastMousePosition = evt.mousePosition;
		evt.StopPropagation();
	}

	private void MouseUp(MouseUpEvent evt)
	{
		if (evt.button == 0 && isDragging)
		{
			isDragging = false;
			_improvementTreeBackground.ReleaseMouse(); // Отпускаем захват мыши
			evt.StopPropagation();
		}
	}

	private void MouseLeave(MouseLeaveEvent evt)
	{
		/*if (isDragging)
		{
			isDragging = false;
			_improvementTreeBackground.ReleaseMouse();
		}*/
	}

	private void ShowMessagePanel(string newText)
	{
		_improvementMessageText.text = newText;

		if (newText == "Недостаточно ресурсов")
		{
			_improvementMessageBackground.RemoveFromClassList("MainBackground");
			_improvementMessageBackground.AddToClassList("ErrorBackground");
		}
		else if (_improvementMessageBackground.ClassListContains("ErrorBackground"))
		{
			_improvementMessageBackground.RemoveFromClassList("ErrorBackground");
			_improvementMessageBackground.AddToClassList("MainBackground");
		}

		DOTween.Kill(_improvementMessageBackground);

		DOTween.To(() => _improvementMessageBackground.style.opacity.value, x => _improvementMessageBackground.style.opacity = x, 1, 1.5f);

		DOTween.To(() => _improvementMessageBackground.style.opacity.value, x => _improvementMessageBackground.style.opacity = x, 0, 1.5f).SetDelay(3f);
	}

	private string MatchName(string improvementName)
	{
		string newText = "";

		switch (improvementName)
		{
			case "Fuel":
				newText = "Топливные баки улучшены";
			break;

			case "Mining":
				newText = "Добывающее оборудование улучшено";
			break;

			case "Health":
				newText = "Здоровье улучшено";
			break;

			case "Defense":
				newText = "Защита улучшена";
			break;

			case "FireDefense":
				newText = "Термическая защита улучшена";
			break;

			case "Engines":
				newText = "Двигатели улучшены";
			break;

			case "Searchlight":
				newText = "Прожектор добавлен";
			break;

			case "SearchlightPower":
				newText = "Прожектора улучшены";
			break;

			case "Resource":
				newText = "Недостаточно ресурсов";
			break;
		}

		return newText;
	}

	private IEnumerator StartPause()
	{
		yield return new WaitForSecondsRealtime(1.5f);

		var newItem = _upgradePanel.Instantiate().hierarchy.ElementAt(0);

		foreach (var key in _improvements.Keys)
		{
			switch (key)
			{
				case "Fuel":

					newItem = _upgradePanel.Instantiate().hierarchy.ElementAt(0);
					newItem.dataSource = new ImprovementPanel<ImprovementFuelConfig, ImprovementFuelData>(this, newItem, _needResourceGroup, _improvements[key].Config, key);
					_upgradesBackground.Add(newItem);

				break;

				case "Mining":

					newItem = _upgradePanel.Instantiate().hierarchy.ElementAt(0);
					newItem.dataSource = new ImprovementPanel<ImprovementMiningConfig, ImprovementMiningData>(this, newItem, _needResourceGroup, _improvements[key].Config, key);
					_upgradesBackground.Add(newItem);
				break;

				case "Health" or "Defense" or "FireDefense":

					newItem = _upgradePanel.Instantiate().hierarchy.ElementAt(0);
					newItem.dataSource = new ImprovementPanel<ImprovementHealthConfig, ImprovementHealthData>(this, newItem, _needResourceGroup, _improvements[key].Config, key);
					_upgradesBackground.Add(newItem);

				break;

				case "Engines":

					newItem = _upgradePanel.Instantiate().hierarchy.ElementAt(0);
					newItem.dataSource = new ImprovementPanel<ImprovementEnginesConfig, ImprovementEnginesData>(this, newItem, _needResourceGroup, _improvements[key].Config, key);
					_upgradesBackground.Add(newItem);

				break;

				case "Searchlight":

					newItem = _upgradePanel.Instantiate().hierarchy.ElementAt(0);
					newItem.dataSource = new ImprovementPanel<ImprovementSearchlightConfig, ImprovementSearchlightData>(this, newItem, _needResourceGroup, _improvements[key].Config, key);
					_upgradesBackground.Add(newItem);
				break;

				case "SearchlightPower":
					newItem = _upgradePanel.Instantiate().hierarchy.ElementAt(0);
					newItem.dataSource = new ImprovementPanel<ImprovementSearchlightPowerConfig, ImprovementSearchlightPowerData>(this, newItem, _needResourceGroup, _improvements[key].Config, key);
					_upgradesBackground.Add(newItem);
					break;
			}
		}
	}

	public Sprite GetResourceSprite(int id)
	{
		if (id <= 6) return _library.GetResourceBase(id).View;
		else return _library.GetCraftResourceBase(id-10).View;
	}

	public void AddImprovement(IImprovementBase improvement, string name) => _improvements[name] = improvement;

	public void UnlockImprovement(string name)
	{
		return;///

		int i = 0;

		foreach (var key in _improvements.Keys)
		{
			if (key == name)
				switch (key)
				{
					case "Fuel":
						((ImprovementPanel<ImprovementFuelConfig, ImprovementFuelData>)_upgradesBackground.contentContainer[i].dataSource).Unlock();
					break;

					case "Health":
						((ImprovementPanel<ImprovementHealthConfig, ImprovementHealthData>)_upgradesBackground.contentContainer[i].dataSource).Unlock();
					break;

					case "Defense":
						((ImprovementPanel<ImprovementHealthConfig, ImprovementHealthData>)_upgradesBackground.contentContainer[i].dataSource).Unlock();
					break;

					case "FireDefense":
						((ImprovementPanel<ImprovementHealthConfig, ImprovementHealthData>)_upgradesBackground.contentContainer[i].dataSource).Unlock();
					break;

					case "Engines":
						((ImprovementPanel<ImprovementEnginesConfig, ImprovementEnginesData>)_upgradesBackground.contentContainer[i].dataSource).Unlock();
					break;

					case "Searchlight":
						((ImprovementPanel<ImprovementSearchlightConfig, ImprovementSearchlightData>)_upgradesBackground.contentContainer[i].dataSource).Unlock();
					break;

					case "SearchlightPower":
						((ImprovementPanel<ImprovementSearchlightPowerConfig, ImprovementSearchlightPowerData>)_upgradesBackground.contentContainer[i].dataSource).Unlock();
					break;
				}
			i++;
		}
	}

	public bool TryUpgrade(string name)
	{
		if (CheckInventoryResources.CheckResources(new Inventory[] { _playerInventory, _baseInventory }, _improvements[name].GetNeedResources()))
		{
			ShowMessagePanel(MatchName(name));
			return true;
		}
		else
		{
			ShowMessagePanel(MatchName("Resource"));
			return false;
		}
	}

	public void Upgrade(string improvementName) => _improvements[improvementName].Upgrade();

	private void LoadData(SaveDataClass.ImprovementData improvements)
	{
		for (int i = 0; i < improvements.ImprovementName.Count; i++)
		{
			for (int j = 0; j < improvements.ImprovementLevel[i]; j++)
			{
				Upgrade(improvements.ImprovementName[i]);
			}
		}
	}

	private void LoadData(SaveDataClass.ImprovementUnlockData improvements)
	{
		for (int i = 0; i < improvements.ImprovementName.Count; i++)
		{
			if (improvements.ImprovementUnlock[i]) UnlockImprovement(improvements.ImprovementName[i]);
		}
	}

	private void SaveData()
	{
		GameEvents.OnImprovementsSave?.Invoke(_improvements);

		Dictionary<string, bool> unlockedPanels = new Dictionary<string, bool>();

		int i = 0;

		foreach (var key in _improvements.Keys)
		{
			switch (key)
			{
				case "Fuel":
					if (((ImprovementPanel<ImprovementFuelConfig, ImprovementFuelData>)_upgradesBackground.contentContainer[i].dataSource).UnlockStatus()) unlockedPanels["Fuel"] = true;
					else unlockedPanels["Fuel"] = false;
					break;

				case "Health":
					if (((ImprovementPanel<ImprovementHealthConfig, ImprovementHealthData>)_upgradesBackground.contentContainer[i].dataSource).UnlockStatus()) unlockedPanels["Health"] = true;
					else unlockedPanels["Health"] = false;
					break;

				case "Defense":
					if (((ImprovementPanel<ImprovementHealthConfig, ImprovementHealthData>)_upgradesBackground.contentContainer[i].dataSource).UnlockStatus()) unlockedPanels["Defense"] = true;
					else unlockedPanels["Defense"] = false;
					break;

				case "FireDefense":
					if (((ImprovementPanel<ImprovementHealthConfig, ImprovementHealthData>)_upgradesBackground.contentContainer[i].dataSource).UnlockStatus()) unlockedPanels["FireDefense"] = true;
					else unlockedPanels["FireDefense"] = false;
					break;

				case "Engines":
					if (((ImprovementPanel<ImprovementEnginesConfig, ImprovementEnginesData>)_upgradesBackground.contentContainer[i].dataSource).UnlockStatus()) unlockedPanels["Engines"] = true;
					else unlockedPanels["Engines"] = false;
					break;

				case "Searchlight":
					if (((ImprovementPanel<ImprovementSearchlightConfig, ImprovementSearchlightData>)_upgradesBackground.contentContainer[i].dataSource).UnlockStatus()) unlockedPanels["Searchlight"] = true;
					else unlockedPanels["Searchlight"] = false;
					break;

				case "SearchlightPower":
					if (((ImprovementPanel<ImprovementSearchlightPowerConfig, ImprovementSearchlightPowerData>)_upgradesBackground.contentContainer[i].dataSource).UnlockStatus()) unlockedPanels["SearchlightPower"] = true;
					else unlockedPanels["SearchlightPower"] = false;
					break;
			}
			i++;
		}

		GameEvents.OnImprovementPanelsSave?.Invoke(unlockedPanels);
	}

	private void OnDisable()
	{
		_upgradesBackground.UnregisterCallback<WheelEvent>(OnScrollWheel);
		_improvementTreeBackground.UnregisterCallback<MouseDownEvent>(MouseDown);
		_improvementTreeBackground.UnregisterCallback<MouseMoveEvent>(MouseMove);
		_improvementTreeBackground.UnregisterCallback<MouseUpEvent>(MouseUp);
		_improvementTreeBackground.UnregisterCallback<MouseLeaveEvent>(MouseLeave);

		GameEvents.OnImprovementOpen -= UnlockImprovement;
		GameEvents.OnImprovementsLoad -= LoadData;
		GameEvents.OnSave -= SaveData;
	}
}