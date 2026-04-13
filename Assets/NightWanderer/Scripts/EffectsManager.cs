using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.VFX;

public class EffectsManager : MonoBehaviour
{
	[SerializeField] private VisualEffect _laser;
	[SerializeField] private GameObject[] _engineFires;
	private VisualEffect[] _engineFiresEffect;
	private HDAdditionalLightData[] _engineFiresLight;

	public void Initializing()
	{
		_laser.Stop();

		_engineFiresEffect = new VisualEffect[_engineFires.Length];
		_engineFiresLight = new HDAdditionalLightData[_engineFires.Length];

		for (int i = 0; i < _engineFires.Length; i++)
		{
			_engineFiresEffect[i] = _engineFires[i].GetComponent<VisualEffect>();
			_engineFiresLight[i] = _engineFires[i].GetComponentInChildren<HDAdditionalLightData>();
		}

		GameEvents.OnLaserExtractionStart += () => _laser.Play();
		GameEvents.OnExtractionEnd += () => _laser.Stop();
		GameEvents.OnRightExtraction += () => StartCoroutine(LaserCoroutine());

		GameEvents.OnEnginesOnOff += EnginesOnOff;
		GameEvents.OnRunStart += () =>
		{
			for (int i = 0; i < _engineFiresEffect.Length; i++) _engineFiresEffect[i].SetFloat("LifeTimeValue", 0.17f);
		};

		GameEvents.OnRunEnd += () => 
		{
			for (int i = 0; i < _engineFiresEffect.Length; i++) _engineFiresEffect[i].SetFloat("LifeTimeValue", 0.13f);
		};
	}

	private void OnDisable()
	{
		GameEvents.OnLaserExtractionStart -= () => _laser.Play();
		GameEvents.OnExtractionEnd -= () => _laser.Stop();
		GameEvents.OnRightExtraction -= () => StartCoroutine(LaserCoroutine());

		GameEvents.OnEnginesOnOff -= EnginesOnOff;
		GameEvents.OnRunStart -= () =>
		{
			for (int i = 0; i < _engineFiresEffect.Length; i++) _engineFiresEffect[i].SetFloat("LifeTimeValue", 0.17f);
		};

		GameEvents.OnRunEnd -= () =>
		{
			for (int i = 0; i < _engineFiresEffect.Length; i++) _engineFiresEffect[i].SetFloat("LifeTimeValue", 0.13f);
		};
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

	private IEnumerator LaserCoroutine()
	{
		_laser.SetBool("IsBigParticle", true);

		yield return new WaitForSeconds(0.2f);

		_laser.SetBool("IsBigParticle", false);
	}
}
