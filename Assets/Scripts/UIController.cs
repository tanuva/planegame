using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace PlaneGame
{
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
		private Text m_CashText;
		[SerializeField]
		private Slider m_ThrottleSlider;
		[SerializeField]
		private Slider m_EngineThrottleSlider;

		// Needed for target compass calculations
		private Transform m_HUDTargetTf;

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

		public void UpdateTargetDir(Vector3 targetDir, Quaternion cameraRotation)
		{
			// This is a little ugly, but it works.
			// Cleaner approach: leave the compass camera static and properly calculate the compassTarget rotation to match
			// the direction of the actual camera (but still seen through the static camera). Con: no technical benefits,
			// lots of brain knots instead.
			
			// Set compass camera rotation to the same as the main camera - excluding pitch!
			Vector3 eulers = cameraRotation.eulerAngles;
			eulers.x = 0f;
			cameraRotation.eulerAngles = eulers;
			m_HUDTargetTf.parent.rotation = cameraRotation;
			// Point the target at the desired direction
			m_HUDTargetTf.rotation = Quaternion.LookRotation (targetDir);
		}

		public void SetCash(int amount)
		{
			m_CashText.text = "Cash: " + amount.ToString ();
		}

		// Use this for initialization
		void Start ()
		{
			var tmp = GameObject.Find ("HUDTarget");
			if (!tmp) {
				throw new UnityException("HUDTarget not found");
			}
			m_HUDTargetTf = tmp.transform;
		}
		
		// Update is called once per frame
		void Update ()
		{
		}
	}
}
