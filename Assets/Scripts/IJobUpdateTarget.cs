using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

/// <summary>
/// IJobUpdateTarget contains methods to send data to the job issuer.
/// </summary>
public interface IJobIssuerTarget : IEventSystemHandler
{
	void DeliverJob(int id);
}

/// <summary>
/// IJobExecutionTarget contains methods to send data to the job executor.
/// </summary>
public interface IJobExecutionTarget : IEventSystemHandler
{
	void AssignJob(GameObject destination, int jobId);
}
