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
		private Job m_currentJob;
		[SerializeField]
		private float m_SqrVelThreshold = 0.1f;
		private GameObject m_GameManager;
		private bool m_AtAirport = false;
		private int m_Cash = 0;
		private AudioSource m_AudioSource;
		[SerializeField]
		private AudioClip m_ClipJobComplete;

		public void AssignJob(Job job)
		{
			m_currentJob = job;
			Debug.Log("New destination: " + m_currentJob.Destination.name);
		}

		// Use this for initialization
		void Start ()
		{
			m_GameManager = GameObject.Find("GameManager");
			if (!m_GameManager) {
				throw new UnityException("GameManager not found");
			}
			ExecuteEvents.Execute<IGUIUpdateTarget>(m_GameManager, null, 
			                                        delegate(IGUIUpdateTarget handler, BaseEventData eventData) {
				handler.SetCash (m_Cash);
			});

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
			ExecuteEvents.Execute<IGUIUpdateTarget>(m_GameManager, null, 
			                                        delegate(IGUIUpdateTarget handler, BaseEventData eventData) {
				handler.UpdateTargetDir(m_currentJob.Destination.transform.position - gameObject.transform.position,
				                        gameObject.GetComponent<CameraSwitcher> ().ActiveCamera.transform.rotation);
			});
		}

		void OnTriggerEnter (Collider other)
		{
			// TODO Can this crash if other.transform has no parent?
			if (m_currentJob.Destination == other.transform.parent.gameObject) {
				m_AtAirport = true;
			}
		}

		void UpdateCargoArea ()
		{
			if (m_AtAirport 
			    && gameObject.GetComponent<Rigidbody>().velocity.sqrMagnitude < m_SqrVelThreshold) {
				// We're at an airport and slow enough to load/unload
				ExecuteEvents.Execute<IJobIssuerTarget>(m_GameManager, null,
				                                        delegate(IJobIssuerTarget handler, BaseEventData eventData) {
					if (handler.DeliverJob(m_currentJob)) {
						m_Cash += m_currentJob.Pay;
					}
				});
				ExecuteEvents.Execute<IGUIUpdateTarget>(m_GameManager, null, 
				                                        delegate(IGUIUpdateTarget handler, BaseEventData eventData) {
					handler.SetCash (m_Cash);
				});
				m_AudioSource.Play ();

				// OnTriggerExit cannot be used to "leave" the airport because the target GO is destroyed after the
				// job was delivered.
				m_AtAirport = false;
			}
		}

		void OnTriggerExit (Collider other)
		{
			if (other.transform.parent.gameObject.CompareTag ("Airport")) {
				m_AtAirport = false;
			}
		}
	}
}
