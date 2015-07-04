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
		// Dict is mostly used to avoid having to iterate the list manually in DeliverJob
		Dictionary<int, Job> m_AvailableJobs;

		public bool DeliverJob(Job job)
		{
			if (m_AvailableJobs.ContainsKey (job.ID)) {
				// Weee!
				Debug.Log ("Delivered job " + job.ID);
				job.Destination.transform.FindChild ("Target").gameObject.SetActive (false);
				m_AvailableJobs.Remove (job.ID);
				Job nextJob = GenerateJob (job.Destination);
				AssignJob (nextJob);
				return true;
			}

			return false;
		}

		// Use this for initialization
		void Start ()
		{
			m_Player = GameObject.FindGameObjectWithTag ("Player");
			if (!m_Player) {
				throw new UnityException ("Couldn't find player GO");
			}
			m_Airports = GameObject.FindGameObjectsWithTag ("Airport");
			if (m_Airports.Length < 1) {
				throw new UnityException ("Couldn't find any airports");
			}

			// Disable all target arrows
			for (int i = 0; i < m_Airports.Length; i++) {
				m_Airports[i].GetComponent<Airport> ().setIsCurrentTarget (false);
			}

			// The capacity is not strictly necessary, but we probably want multiple jobs later
			m_AvailableJobs = new Dictionary<int, Job> (1);
			AssignJob (GenerateJob (GameObject.Find ("Gas Station"))); // Player spawns at the Gas Station airport
		}
		
		// Update is called once per frame
		void Update ()
		{
		}

		Job GenerateJob (GameObject origin)
		{
			// Make sure the new destination is different from the current location.
			// (Would be quite a nonsensical job, wouldn't it?)
			GameObject destination;
			do {
				int destinationIndex = Random.Range(0, m_Airports.Length);
				destination = m_Airports[destinationIndex];
			} while (destination == origin);

			int id = Random.Range(0, int.MaxValue); // The inspector doesn't like uints
			Job job = new Job(id,
			                  CalculatePay (origin, destination),
			                  origin,
			                  destination);
			m_AvailableJobs.Add (id, job);
			return job;
		}

		void AssignJob (Job job)
		{
			job.Destination.GetComponent<Airport> ().setIsCurrentTarget (true);
			ExecuteEvents.Execute<IJobExecutionTarget>(m_Player, null, (t, y) => (
				t.AssignJob (job)
			));
		}

		int CalculatePay (GameObject origin, GameObject destination)
		{
			return (int)(destination.transform.position - origin.transform.position).magnitude;
		}
	}
}
