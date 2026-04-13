using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;

public class MinigameLaser
{
	private Label _activityName;
	private Label _oreName;
	private Label _oreCount;
	private VisualElement _oreIcon;

	private VisualElement _mainElementLaser;
	private VisualElement _rightPlace;
	private VisualElement _gameSlider;
	private ResourceSource _currentResourceSource;
	private Rect _parentRect;
	private Tween _animation;
	private Vector3 _startGameSliderPosition;

	private VisualElement _mainElementFuel;
	private VisualElement _currentFuel;

	private int _needResults = 5;
	private int _rightResults = 0;


	public MinigameLaser(VisualElement mainElementLaser, VisualElement mainElementFuel)
	{
		_mainElementLaser = mainElementLaser;
		_mainElementFuel = mainElementFuel;

		_rightPlace = mainElementLaser.Q<VisualElement>("RightPlace");
		_gameSlider = mainElementLaser.Q<VisualElement>("GameSlider");
		_activityName = mainElementLaser.Q<Label>("ActivityName");
		_oreName = mainElementLaser.Q<Label>("OreName");
		_oreCount = mainElementLaser.Q<Label>("OreCount");
		_oreIcon = mainElementLaser.Q<VisualElement>("OreIcon");

		_parentRect = _rightPlace.parent.layout;


		_currentFuel = mainElementFuel.Q<VisualElement>("FuelCount");

		_rightPlace.RegisterCallback<DetachFromPanelEvent>(OnDetach);
		_startGameSliderPosition = _gameSlider.transform.position;
	}

	private void NewPlace()
	{
		_rightPlace.transform.position = new Vector2(_rightPlace.transform.position.x, Random.Range(0 + _rightPlace.layout.height / 2, _parentRect.height - _rightPlace.layout.height / 2));
	}

	private void ChangeSliderPosition()
	{
		_animation = DOTween.To(() => _gameSlider.style.top.value.value, x => _gameSlider.style.top = new StyleLength(x), _parentRect.height - _gameSlider.layout.height / 2, 3f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
	}

	private void StopSliderPosition()
	{
		_animation.Kill();
	}

	public void StartGame(int needResults)
	{
		_needResults = needResults;

		if (_needResults == 0) WinGame();
		else
		{
			_rightResults = 0;

			NewPlace();
			ChangeSliderPosition();
		}
	}

	private void WinGame()
	{
		StopSliderPosition();

		_rightPlace.style.display = DisplayStyle.None;
		_gameSlider.style.top = new StyleLength(0f);
		_gameSlider.transform.position = _startGameSliderPosition;
		_gameSlider.style.display = DisplayStyle.None;
	}

	public void EndGame()
	{
		StopSliderPosition();

		_rightPlace.style.display = DisplayStyle.Flex;
		_gameSlider.style.top = new StyleLength(0f);
		_gameSlider.transform.position = _startGameSliderPosition;
		_gameSlider.style.display = DisplayStyle.Flex;
	}

	public bool CheckResult()
	{
		if (_rightResults == _needResults) return false;

		if (_rightPlace.worldBound.Overlaps(_gameSlider.worldBound))
		{
			_rightResults++;

			if (_rightResults == _needResults) WinGame();
			else NewPlace();

			return true;
		}
		else
		{
			NewPlace();
			return false;
		}

    }
	
	public void UpdateData()
	{
		_oreCount.text = $"X{_currentResourceSource.GetCurrentResourceCount()}";
	}

	public void UpdateData(ResourceSource source, Fuel fuel)
	{
		_currentResourceSource = source;

		_activityName.text = "Добыча руды";

		_oreName.text = $"Найденная руда:\n{_currentResourceSource.GetCurrentResource().Name}";

		_oreCount.text = $"X{_currentResourceSource.GetCurrentResourceCount()}";

		_oreIcon.style.backgroundImage = new StyleBackground(_currentResourceSource.GetCurrentResource().View);

		_currentFuel.dataSource = fuel;
	}

	private void OnDetach(DetachFromPanelEvent evt)
	{
		_animation.Kill();
	}
}
