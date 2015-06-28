using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// UIController holds references to GUI widgets and acts as data receiver for them.
/// </summary>
public class UIController : MonoBehaviour, IGUIUpdateTarget
{
	[SerializeField]
	private Text m_SpeedText;
	[SerializeField]
	private Text m_AltitudeText;
	[SerializeField]
	private Slider m_ThrottleSlider;
	[SerializeField]
	private Slider m_EngineThrottleSlider;

	public void UpdateSpeed(float speed)
	{
		m_SpeedText.text = "IAS: " + (int)(speed * 1.94f) + " kn";
	}

	public void UpdateThrottle(float throttle)
	{
		m_ThrottleSlider.value = throttle;
	}

	public void UpdateEngineThrottle(float engineThrottle)
	{
		m_EngineThrottleSlider.value = engineThrottle;
	}

	public void UpdateAltitude(float altitude)
	{
		m_AltitudeText.text = "ALT: " + (int)(altitude * 3.28f) + " ft";
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
