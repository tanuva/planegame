using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// UIController holds references to GUI widgets and acts as data receiver for them.
/// </summary>
public class UIController : MonoBehaviour, IGUIUpdateTarget
{
	public Text SpeedText;
	public Slider ThrottleSlider;
	public Slider EngineThrottleSlider;
	public Text AltitudeText;

	public void UpdateSpeed(float speed)
	{
		SpeedText.text = "IAS: " + (int)(speed * 1.94f) + " kn";
	}

	public void UpdateThrottle(float throttle)
	{
		ThrottleSlider.value = throttle;
	}
	public void UpdateEngineThrottle(float engineThrottle)
	{
		EngineThrottleSlider.value = engineThrottle;
	}
	public void UpdateAltitude(float altitude)
	{
		AltitudeText.text = "ALT: " + (int)(altitude * 3.28f) + " ft";
	}

	// Use this for initialization
	void Start ()
	{
	}
	
	// Update is called once per frame
	void Update ()
	{
	}
}
