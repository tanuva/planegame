using UnityEngine;
using System.Collections;

namespace PlaneGame
{
	/// <summary>
	/// Implementation of a PID controller.
	/// </summary>
	public class PID
	{
		// Internal State
		float dState;
		float iState;
		// User Supplied
		public float IMin = -100;
		public float IMax = 100;
		public float PGain;
		public float IGain;
		public float DGain;

		public float Update (float error, float curPos)
		{
			float pTerm = PGain * error;
			iState += error;
	//		iState = Mathf.Clamp (iState, IMin, IMax);
			float iTerm = IGain * iState;
			float dTerm = DGain * (curPos - dState);
			dState = curPos;
			return pTerm + iTerm - dTerm;
		}
	}
}
