using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

namespace PlaneGame
{
	/// <summary>
	/// IJobUpdateTarget contains methods to send data to the job issuer.
	/// </summary>
	public interface IJobIssuerTarget : IEventSystemHandler
	{
		/// <summary>
		/// Delivers the job and returns the loan.
		/// </summary>
		/// <returns>The loan</returns>
		/// <param name="id">Job id</param>
		int DeliverJob(int id);
	}

	/// <summary>
	/// IJobExecutionTarget contains methods to send data to the job executor.
	/// </summary>
	public interface IJobExecutionTarget : IEventSystemHandler
	{
		void AssignJob(GameObject destination, int jobId);
	}
}
