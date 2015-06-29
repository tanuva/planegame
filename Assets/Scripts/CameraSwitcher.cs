using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(InputHandler))]
public class CameraSwitcher : MonoBehaviour {
	[SerializeField]
	private string m_DefaultCameraName = "Main Camera";
	private GameObject[] m_Cameras;
	private int m_ActiveCamera;
	[SerializeField]
	private InputHandler m_InputHandler;

	public GameObject ActiveCamera {
		get { return m_Cameras[m_ActiveCamera]; }
	}

	// Use this for initialization
	void Start ()
	{
		m_Cameras = GameObject.FindGameObjectsWithTag ("MainCamera");
		if (m_Cameras.Length < 1) {
			throw new UnityException ("No cameras found!");
		}

		// Disable all cameras but the first one
		for (int i = 0; i < m_Cameras.Length; i++) {
			if (m_Cameras[i].name == m_DefaultCameraName) {
				m_Cameras[i].SetActive (true);
				// Enable the smooth follow behavior for the first camera
				// This should ideally be done configurably, but I didn't find a nice way so far.
				m_Cameras[i].AddComponent<SmoothFollow> ();
				m_Cameras[i].GetComponent<SmoothFollow> ().Target = gameObject.transform;
				m_ActiveCamera = i;
			} else {
				m_Cameras[i].SetActive (false);
			}
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (m_InputHandler.Buttons[Button.CHANGE_CAMERA]) {
			NextCamera ();
		}
	}

	void NextCamera ()
	{
		m_Cameras[m_ActiveCamera].SetActive (false);
		Destroy (m_Cameras[m_ActiveCamera].GetComponent<AudioListener> ());
		m_ActiveCamera = (m_ActiveCamera + 1) % m_Cameras.Length;
		m_Cameras[m_ActiveCamera].SetActive (true);
		m_Cameras[m_ActiveCamera].AddComponent<AudioListener> ();
	}
}
