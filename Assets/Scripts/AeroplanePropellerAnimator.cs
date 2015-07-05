using System;
using UnityEngine;

namespace PlaneGame
{
	[RequireComponent(typeof(Rigidbody))]
	public class AeroplanePropellerAnimator : MonoBehaviour
	{
		[SerializeField] private Transform m_PropellerModel;						// The model of the the aeroplane's propeller.
		[SerializeField] private Transform m_PropellerBlurPlane;					// The plane used for the blurred propeller textures.
		[SerializeField] private Texture2D[] m_PropellerBlurTextures;				// An array of increasingly blurred propeller textures.
		[SerializeField] [Range(0f, 1f)] private float m_ThrottleBlurStart = 0.25f;	// The point at which the blurred textures start.
		[SerializeField] [Range(0f, 1f)] private float m_ThrottleBlurEnd = 0.5f;	// The point at which the blurred textures stop changing.
		[SerializeField] private float m_MaxRpm = 2000;								// The maximum speed the propeller can turn at.

		// VMax is necessary to make the propeller rotate not only from throttle input but also due to sheer airstream.
		private float m_VMaxSqr = Mathf.Pow (57, 2);								// The approximate maximum speed of the vehicle.
		[SerializeField] private float m_VMax {
			get { return Mathf.Sqrt (m_VMaxSqr); }
			set { m_VMaxSqr = Mathf.Pow (value, 2); }
		}

		private AeroplaneController m_Plane;	// Reference to the aeroplane controller.
		private int m_PropellerBlurState = -1;	// To store the state of the blurred textures.
		private const float m_RpmToDps = 60f;	// For converting from revs per minute to degrees per second.
		private Renderer m_PropellerModelRenderer;
		private Renderer m_PropellerBlurRenderer;
		private Rigidbody m_Rigidbody;

		private void Awake()
		{
			// Set up the reference to the aeroplane controller.
			m_Plane = GetComponent<AeroplaneController> ();

			m_PropellerModelRenderer = m_PropellerModel.GetComponent<Renderer> ();
			m_PropellerBlurRenderer = m_PropellerBlurPlane.GetComponent<Renderer> ();

			// Set the propeller blur gameobject's parent to be the propeller.
			m_PropellerBlurPlane.parent = m_PropellerModel;

			m_Rigidbody = gameObject.GetComponent<Rigidbody> ();
		}

		private void Update()
		{
			// Rotate the propeller model at a rate proportional to the throttle.
			float throttleFactor = Mathf.Max(m_Rigidbody.velocity.sqrMagnitude / m_VMaxSqr,
			                                 m_Plane.Throttle);
			m_PropellerModel.Rotate(0, 0, m_MaxRpm * throttleFactor * Time.deltaTime * m_RpmToDps);
//			m_PropellerModel.RotateAround (m_PropellerModel.transform.position,
//			                               m_PropellerModel.transform.forward,
//			                               m_MaxRpm * throttleFactor * Time.deltaTime * m_RpmToDps);

			// Create an integer for the new state of the blur textures.
			var newBlurState = 0;

			// Choose between the blurred textures, if the throttle is high enough
			if (throttleFactor > m_ThrottleBlurStart) {
				var throttleBlurProportion = Mathf.InverseLerp (m_ThrottleBlurStart, m_ThrottleBlurEnd, throttleFactor);
				newBlurState = Mathf.FloorToInt (throttleBlurProportion * (m_PropellerBlurTextures.Length - 1));
			}

			// If the blur state has changed
			if (newBlurState != m_PropellerBlurState) {
				m_PropellerBlurState = newBlurState;

				if (m_PropellerBlurState == 0) {
					// switch to using the 'real' propeller model
					m_PropellerModelRenderer.enabled = true;
					m_PropellerBlurRenderer.enabled = false;
				} else {
					// Otherwise turn off the propeller model and turn on the blur.
					m_PropellerModelRenderer.enabled = false;
					m_PropellerBlurRenderer.enabled = true;

					// set the appropriate texture from the blur array
					m_PropellerBlurRenderer.material.mainTexture = m_PropellerBlurTextures[m_PropellerBlurState];
				}
			}
		}
	}
}
