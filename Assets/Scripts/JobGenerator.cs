using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

/// <summary>
/// Randomly generates jobs for the player.
/// </summary>
public class JobGenerator : MonoBehaviour, IJobIssuerTarget {
	GameObject m_Player;
	GameObject[] m_Airports;
	List<int> m_AvailableJobs;

	public void DeliverJob(int id)
	{
		if (m_AvailableJobs.Contains(id)) {
			// Weee!
			// TODO destroy target arrow GO
			Debug.Log("Delivered job " + id);
			AssignNextJob(m_Player);
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
		// This is not strictly necessary, but we probably want multiple jobs later
		m_AvailableJobs = new List<int>(1);

		AssignNextJob(m_Player);
	}
	
	// Update is called once per frame
	void Update ()
	{
	}

	void AssignNextJob(GameObject target)
	{
		// TODO Allow deliveries to the same airport for now as we only have one...
		// TODO Instantiate target arrow GO
		int destinationIndex = Random.Range(0, m_Airports.Length);
		int jobId = Random.Range(0, int.MaxValue); // The inspector doesn't like uints
		m_AvailableJobs.Add(jobId);
		ExecuteEvents.Execute<IJobExecutionTarget>(m_Player, null, (t, y) => (
			t.AssignJob(m_Airports[destinationIndex], jobId)
		));
		Debug.Log("Assigned job " + jobId);
	}
}
