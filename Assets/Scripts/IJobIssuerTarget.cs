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
		/// <returns>True if the job was completed successfully.</returns>
		/// <param name="job">Job</param>
		bool DeliverJob(Job job);
	}

	/// <summary>
	/// IJobExecutionTarget contains methods to send data to the job executor.
	/// </summary>
	public interface IJobExecutionTarget : IEventSystemHandler
	{
		void AssignJob(Job job);
	}
}
