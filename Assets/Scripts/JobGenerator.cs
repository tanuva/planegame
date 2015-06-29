using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

/// <summary>
/// Randomly generates jobs for the player.
/// </summary>
public class JobGenerator : MonoBehaviour, IJobIssuerTarget {
	GameObject m_Player;
	GameObject[] m_Airports;
	Dictionary<int, GameObject> m_AvailableJobs;

	public void DeliverJob(int id)
	{
		if (m_AvailableJobs.ContainsKey (id)) {
			// Weee!
			// TODO destroy target arrow GO
			Debug.Log("Delivered job " + id);
			AssignNextJob(m_AvailableJobs[id]);
			m_AvailableJobs.Remove (id);
		}
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
		// TODO Instantiate target arrow GO
		// Make sure the new destination is different from the current location.
		// (Would be quite a nonsensical job, wouldn't it?)
		GameObject destination;
		do {
			int destinationIndex = Random.Range(0, m_Airports.Length);
			destination = m_Airports[destinationIndex];
		} while (destination == lastDestination);

		int jobId = Random.Range(0, int.MaxValue); // The inspector doesn't like uints
		m_AvailableJobs.Add(jobId, destination);
		ExecuteEvents.Execute<IJobExecutionTarget>(m_Player, null, (t, y) => (
			t.AssignJob(destination, jobId)
		));
		Debug.Log("Assigned job " + jobId);
	}
}
