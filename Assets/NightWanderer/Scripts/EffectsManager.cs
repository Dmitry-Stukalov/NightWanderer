using UnityEngine;
using UnityEngine.VFX;

public class EffectsManager : MonoBehaviour
{
	[SerializeField] private VisualEffect _laser;

	public void Initializing()
	{
		_laser.Stop();

		GameEvents.OnLaserExtractionStart += () => _laser.Play();
		GameEvents.OnExtractionEnd += () => _laser.Stop();
	}

	private void OnDisable()
	{
		GameEvents.OnLaserExtractionStart -= () => _laser.Play();
		GameEvents.OnExtractionEnd -= () => _laser.Stop();
	}
}
