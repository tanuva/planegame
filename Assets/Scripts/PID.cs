using UnityEngine;
using System.Collections;

public class PID
{
	/*
	typedef struct {
		double dState; // Last position input 
		double iState; // Integrator state 
		double iMax, iMin; // Maximum and minimum allowable integrator state 
		double iGain, // integral gain
			pGain, // proportional gain
			dGain; // derivative gain 
	} SPid;
	
	double UpdatePID(SPid * pid, double error, double position) {
		double pTerm, dTerm, iTerm;
		pTerm = pid->pGain * error;
		// calculate the proportional term
		// calculate the integral state with appropriate limiting
		pid->iState += error;
		if (pid->iState > pid->iMax) 
			pid->iState = pid->iMax; 
		else if (pid->iState < pid->iMin) 
			pid->iState = pid->iMin;
		iTerm = pid->iGain * iState; // calculate the integral term 
		dTerm = pid->dGain * (position - pid->dState); 
		pid->dState = position;
		return pTerm + iTerm - dTerm;
	}
	*/
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
