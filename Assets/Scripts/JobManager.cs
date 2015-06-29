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
		[SerializeField]
		private int m_jobId = 0;
		[SerializeField]
		private float m_SqrVelThreshold = 0.1f;
		private GameObject m_MsgDispatcher;
		private GameObject m_Canvas;
		// TODO This is only needed as long as the JobGenerator doesn't enforce *different* pickup/dropoff airports.
		private bool m_JustLoaded = false; // Used to load only once at every airport.
		private bool m_AtAirport = false;

		public void AssignJob(GameObject destination, int jobId)
		{
			m_CurrentDestination = destination;
			m_jobId = jobId;
			Debug.Log("New destination: " + m_CurrentDestination.name, this);
		}

		// Use this for initialization
		void Start ()
		{
			m_MsgDispatcher = GameObject.Find("MessageDispatcher");
			if (!m_MsgDispatcher) {
				throw new UnityException("MessageDispatcher not found");
			}
			m_Canvas = GameObject.Find("Canvas");
			if (!m_Canvas) {
				throw new UnityException("Canvas not found");
			}
		}
		
		// Update is called once per frame
		void Update ()
		{
			UpdateCompass ();
			if (!m_JustLoaded) {
				UpdateCargoArea ();
			}
		}

		void UpdateCompass ()
		{
			ExecuteEvents.Execute<IGUIUpdateTarget>(m_Canvas, null, (t, y) => (
				t.UpdateTargetDir(m_CurrentDestination.transform.position - gameObject.transform.position,
			                  gameObject.GetComponent<CameraSwitcher> ().ActiveCamera.transform.rotation)
			));
		}

		void OnTriggerEnter (Collider other)
		{
			Debug.Log("Trigger enter: " + other.transform.parent.gameObject.name, this);
			// TODO Can this crash if other.transform has no parent?
			if (m_CurrentDestination == other.transform.parent.gameObject) {
				m_AtAirport = true;
			}
		}

		void UpdateCargoArea ()
		{
			if (m_AtAirport && gameObject.GetComponent<Rigidbody>().velocity.sqrMagnitude < m_SqrVelThreshold) {
				// We're at an airport and slow enough to load/unload
				ExecuteEvents.Execute<IJobIssuerTarget>(m_MsgDispatcher, null, (t, y) => (t.DeliverJob(m_jobId)));
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
