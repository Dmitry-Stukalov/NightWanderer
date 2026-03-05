using UnityEngine;

public class Weather : MonoBehaviour
{
	[SerializeField] private ParticleSystem _Weather;

	public void StartRain() => _Weather.Play();

	public void EndRain() => _Weather.Stop();

	public void ChangeIntensivity(int value)
	{
		_Weather.maxParticles = value;
	}
}
