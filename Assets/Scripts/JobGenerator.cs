using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace PlaneGame
{
	/// <summary>
	/// Randomly generates jobs for the player.
	/// </summary>
	public class JobGenerator : MonoBehaviour, IJobIssuerTarget
	{
		GameObject m_Player;
		GameObject[] m_Airports;
		Dictionary<int, GameObject> m_AvailableJobs;

		public int DeliverJob(int id)
		{
			if (m_AvailableJobs.ContainsKey (id)) {
				// Weee!
				Debug.Log ("Delivered job " + id);
				m_AvailableJobs[id].transform.FindChild ("Target").gameObject.SetActive (false);
				AssignNextJob (m_AvailableJobs[id]);
				m_AvailableJobs.Remove (id);
				return 100;
			}

			return 0;
		}

		// Use this for initialization
		void Start ()
		{
			m_Player = GameObject.FindGameObjectWithTag("Player");
			if (!m_Player) {
				throw new UnityException("Couldn't find player GO");
			}
			m_Airports = GameObject.FindGameObjectsWithTag("Airport");
			if (m_Airports.Length < 1) {
				throw new UnityException("Couldn't find airports");
			}

			// Disable all target arrows
			for (int i = 0; i < m_Airports.Length; i++) {
				m_Airports[i].transform.FindChild ("Target").gameObject.SetActive (false);
			}

			// The capacity is not strictly necessary, but we probably want multiple jobs later
			m_AvailableJobs = new Dictionary<int, GameObject> (1);
			AssignNextJob(null);
		}
		
		// Update is called once per frame
		void Update ()
		{
		}

		void AssignNextJob(GameObject lastDestination)
		{
			// Make sure the new destination is different from the current location.
			// (Would be quite a nonsensical job, wouldn't it?)
			GameObject destination;
			do {
				int destinationIndex = Random.Range(0, m_Airports.Length);
				destination = m_Airports[destinationIndex];
			} while (destination == lastDestination);

			destination.transform.FindChild ("Target").gameObject.SetActive (true);
			int jobId = Random.Range(0, int.MaxValue); // The inspector doesn't like uints
			m_AvailableJobs.Add(jobId, destination);
			ExecuteEvents.Execute<IJobExecutionTarget>(m_Player, null, (t, y) => (
				t.AssignJob(destination, jobId)
			));
		}
	}
}
