using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.VFX;

public class EffectsManager : MonoBehaviour
{
	[SerializeField] private VisualEffect _laser;
	//[SerializeField] private VisualEffect _vacuumCleaner;
	[SerializeField] private GameObject[] _engineFires;
	private VisualEffect[] _engineFiresEffect;
	private HDAdditionalLightData[] _engineFiresLight;

	public void Initializing()
	{
		_laser.Stop();
		//_vacuumCleaner.Stop();

		_engineFiresEffect = new VisualEffect[_engineFires.Length];
		_engineFiresLight = new HDAdditionalLightData[_engineFires.Length];

		for (int i = 0; i < _engineFires.Length; i++)
		{
			_engineFiresEffect[i] = _engineFires[i].GetComponent<VisualEffect>();
			_engineFiresLight[i] = _engineFires[i].GetComponentInChildren<HDAdditionalLightData>();
		}

		GameEvents.OnLaserExtractionStart += ExtractionStart;
		GameEvents.OnExtractionEnd += ExtractionEnd;
		GameEvents.OnRightExtraction += RightExtraction;

		GameEvents.OnEnginesOnOff += EnginesOnOff;
		GameEvents.OnRunStart += RunStart;
		GameEvents.OnRunEnd += RunEnd;
	}

	private void OnDisable()
	{
		GameEvents.OnLaserExtractionStart -= ExtractionStart;
		GameEvents.OnExtractionEnd -= ExtractionEnd;
		GameEvents.OnRightExtraction -= RightExtraction;

		GameEvents.OnEnginesOnOff -= EnginesOnOff;
		GameEvents.OnRunStart -= RunStart;
		GameEvents.OnRunEnd -= RunEnd;
	}

	private void EnginesOnOff()
	{
		if (_engineFiresEffect[0].GetFloat("LifeTimeValue") == 0.06f)
		{
			for (int i = 0; i < _engineFires.Length; i++)
			{
				_engineFiresEffect[i].SetFloat("LifeTimeValue", 0.13f);
				_engineFiresLight[i].SetIntensity(15f, LightUnit.Ev100);
			}
		}
		else
		{
			for (int i = 0; i < _engineFires.Length; i++)
			{
				_engineFiresEffect[i].SetFloat("LifeTimeValue", 0.06f);
				_engineFiresLight[i].SetIntensity(9f, LightUnit.Ev100);
			}
		}
	}

	private void RunStart()
	{
		for (int i = 0; i < _engineFiresEffect.Length; i++) _engineFiresEffect[i].SetFloat("LifeTimeValue", 0.17f);
	}

	private void RunEnd()
	{
		for (int i = 0; i < _engineFiresEffect.Length; i++) _engineFiresEffect[i].SetFloat("LifeTimeValue", 0.13f);
	}

	private void ExtractionStart() => _laser.Play();

	private void ExtractionEnd() => _laser.Stop();
	private void RightExtraction() => StartCoroutine(LaserCoroutine());

	private IEnumerator LaserCoroutine()
	{
		_laser.SetBool("IsBigParticle", true);

		yield return new WaitForSeconds(0.2f);

		_laser.SetBool("IsBigParticle", false);
	}
}
