using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

namespace PlaneGame
{
	/// <summary>
	/// JobManager keeps track of our current job and handles pickup/delivery.
	/// </summary>
	[RequireComponent(typeof(Rigidbody))]
	[RequireComponent(typeof(CameraSwitcher))]
	public class JobManager : MonoBehaviour, IJobExecutionTarget
	{
		[SerializeField]
		private GameObject m_CurrentDestination;
		private int m_jobId = 0;
		[SerializeField]
		private float m_SqrVelThreshold = 0.1f;
		private GameObject m_GameManager;
		private bool m_JustLoaded = false; // Used to load only once at every airport.
		private bool m_AtAirport = false;
		private int m_Cash = 0;
		private AudioSource m_AudioSource;
		[SerializeField]
		private AudioClip m_ClipJobComplete;

		public void AssignJob(GameObject destination, int jobId)
		{
			m_CurrentDestination = destination;
			m_jobId = jobId;
			Debug.Log("New destination: " + m_CurrentDestination.name, this);
		}

		// Use this for initialization
		void Start ()
		{
			m_GameManager = GameObject.Find("GameManager");
			if (!m_GameManager) {
				throw new UnityException("GameManager not found");
			}
			ExecuteEvents.Execute<IGUIUpdateTarget>(m_GameManager, null, (t, y) => (
				t.SetCash (m_Cash)
			));

			m_AudioSource = gameObject.AddComponent<AudioSource> ();
			m_AudioSource.playOnAwake = false;
			m_AudioSource.loop = false;
			m_AudioSource.clip = m_ClipJobComplete;
		}
		
		// Update is called once per frame
		void Update ()
		{
			UpdateCompass ();
			UpdateCargoArea ();
		}

		void UpdateCompass ()
		{
			ExecuteEvents.Execute<IGUIUpdateTarget>(m_GameManager, null, (t, y) => (
				t.UpdateTargetDir(m_CurrentDestination.transform.position - gameObject.transform.position,
			                  gameObject.GetComponent<CameraSwitcher> ().ActiveCamera.transform.rotation)
			));
		}

		void OnTriggerEnter (Collider other)
		{
			// TODO Can this crash if other.transform has no parent?
			if (m_CurrentDestination == other.transform.parent.gameObject) {
				m_AtAirport = true;
			}
		}

		void UpdateCargoArea ()
		{
			if (!m_JustLoaded 
			    && m_AtAirport 
			    && gameObject.GetComponent<Rigidbody>().velocity.sqrMagnitude < m_SqrVelThreshold) {
				Debug.Log ("dropoff");
				// We're at an airport and slow enough to load/unload
				ExecuteEvents.Execute<IJobIssuerTarget>(m_GameManager, null, (t, y) => (m_Cash += t.DeliverJob(m_jobId)));
				ExecuteEvents.Execute<IGUIUpdateTarget>(m_GameManager, null, (t, y) => (t.SetCash (m_Cash)));
				m_AudioSource.Play ();
				m_JustLoaded = true;
			}
		}

		void OnTriggerExit (Collider other)
		{
			if (other.transform.parent.gameObject.CompareTag ("Airport")) {
				m_JustLoaded = false;
				m_AtAirport = false;
			}
		}
	}
}
