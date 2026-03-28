using DG.Tweening;
using UnityEngine;


//Очень простой класс, надо доделать
public class Searchlight : MonoBehaviour
{
	[SerializeField] private bool IsDownSearchlight;
	[SerializeField] private Vector3[] _searchPath;
	[SerializeField] private Vector3 _fightPosition;
	private Vector3 _startRotation;
	private GameObject _searchlightObject;
	private Light _light;
	private float _startIntensity;
	private Sequence _sequence;

	public bool IsOn { get; set; } = false;

	public void Initializing(Sun sun)
	{
		DOTween.Init();

		_light = GetComponent<Light>();
		_searchlightObject = transform.parent.gameObject;

		_startRotation = _searchlightObject.transform.localRotation.eulerAngles;

		sun.OnDayStart += () => IsOn = false;

		sun.OnNightStart += () => IsOn = true;
	}

	public void SearchlightOnOff()
	{
		_light.DOKill();

		if (IsOn)
		{
			_light.DOIntensity(0, 0.5f).SetEase(Ease.OutQuad);
		}
		else
		{
			_light.DOIntensity(_startIntensity, 2f).SetEase(Ease.OutQuad);
		}

		IsOn = !IsOn;
	}

	public void PasteValues(float innerSpotAngle, float outerSpotAngle, float intensity, float range)
	{
		_light.innerSpotAngle = innerSpotAngle;
		if (!IsDownSearchlight) _light.spotAngle = outerSpotAngle;
		else _light.spotAngle = 100;
		_light.intensity = intensity;
		_startIntensity = intensity;
		_light.range = range;
	}

	public void StartSearch()
	{
		if (IsDownSearchlight) return;

		_sequence.Kill();
		_searchlightObject.transform.DOKill();
		_sequence = DOTween.Sequence();

		_sequence.Append(_searchlightObject.transform.DOLocalRotate(_searchPath[0], 2f).SetEase(Ease.Linear));
		_sequence.Append(_searchlightObject.transform.DOLocalRotate(_searchPath[1], 2f).SetEase(Ease.Linear));
		_sequence.Append(_searchlightObject.transform.DOLocalRotate(_searchPath[2], 2f).SetEase(Ease.Linear));
		_sequence.Append(_searchlightObject.transform.DOLocalRotate(_searchPath[3], 2f).SetEase(Ease.Linear));
		_sequence.Append(_searchlightObject.transform.DOLocalRotate(_searchPath[0], 2f).SetEase(Ease.Linear));
		_sequence.SetEase(Ease.Linear);
		_sequence.SetLoops(-1, LoopType.Restart);
	}

	public void StartMove()
	{
		if (IsDownSearchlight) return;

		_sequence.Kill();

		_searchlightObject.transform.DOLocalRotate(_startRotation, 2f).SetEase(Ease.Linear);
	}

	public void StartFight()
	{
		if (IsDownSearchlight) return;

		_sequence.Kill();
		_searchlightObject.transform.DOKill();

		_searchlightObject.transform.DOLocalRotate(_fightPosition, 2f).SetEase(Ease.Linear);
	}

	public Light GetLight() => _light;
}
