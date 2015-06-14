using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIController : MonoBehaviour
/// <summary>
/// UIController holds references to GUI widgets and acts as data receiver for them.
/// </summary>
{
	public float Speed;
	public Text SpeedText;
	public float Throttle;
	public Slider ThrottleSlider;
	public float EngineThrottle;
	public Slider EngineThrottleSlider;
	public float Altitude;
	public Text AltitudeText;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		SpeedText.text = "IAS: " + (int)(Speed * 1.94f) + " kn";
		AltitudeText.text = "ALT: " + (int)(Altitude * 3.28f) + " ft";
		ThrottleSlider.value = Throttle;
		EngineThrottleSlider.value = EngineThrottle;
	}
}
