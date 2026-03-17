using UnityEngine;

public class Searchlight : MonoBehaviour
{
	[SerializeField] private Sun _sun;
	private Light _light;
	private Timer OnOffTimer;

	public bool IsOn { get; set; } = false;

	public void Initializing()
	{
		_light = GetComponent<Light>();

		OnOffTimer = new Timer(2f);

		_sun.OnDayStart += () => IsOn = false;

		_sun.OnNightStart += () => IsOn = true;
	}

	public void SearchlightOnOff()
	{
		_light.intensity = OnOffTimer.CurrentTime * 1000000;
	}


	private void Update()
	{
		if (IsOn && OnOffTimer.CurrentTime != OnOffTimer.MaxTime)
		{
			OnOffTimer.Tick(Time.deltaTime);
			SearchlightOnOff();
		}

		if (!IsOn && OnOffTimer.CurrentTime != 0)
		{
			OnOffTimer.ReverseTick(Time.deltaTime);
			SearchlightOnOff();
		}
	}
}
