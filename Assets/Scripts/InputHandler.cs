﻿using UnityEngine;
using System.Collections.Generic;
using XboxCtrlrInput;

public enum Axis
{
	AILERON,
	ELEVATOR,
	RUDDER,
	THROTTLE,
	FLAPS,
	WHEELBRAKES
}

public class InputHandler : MonoBehaviour
{
	Dictionary<Axis, float> _axes = new Dictionary<Axis, float> ();
	public Dictionary<Axis, float> Axes {
		get { return _axes; }
	}

	public float ThrottleChangeRate;
	public float FlapsChangeRate;
	public float BrakesChangeRate;

	// Use this for initialization
	void Start ()
	{
		_axes [Axis.AILERON] = 0.0f;
		_axes [Axis.ELEVATOR] = 0.0f;
		_axes [Axis.RUDDER] = 0.0f;
		_axes [Axis.THROTTLE] = 0.0f;
		_axes [Axis.FLAPS] = 0.0f;
		_axes [Axis.WHEELBRAKES] = 0.0f;

		Debug.Log (XCI.GetNumPluggedCtrlrs ());
	}
	
	// Update is called once per frame
	void Update ()
	{
		_axes [Axis.AILERON] = pickGreaterInput (XboxAxis.LeftStickX, "Roll");
		_axes [Axis.ELEVATOR] = pickGreaterInput (XboxAxis.LeftStickY, "Pitch");
		_axes [Axis.RUDDER] = pickGreaterInput (XboxAxis.RightStickX, "Yaw"); // TODO use triggers here
		UpdateThrottle ();
		UpdateFlaps ();
		UpdateWheelBrakes ();
	}

	/// <summary>
	/// Picks the more effective input from a keyboard and a controller input axis.
	/// (If the axis is controlled via the keyboard, that input wins over any analog
	/// input from the controller.)
	/// </summary>
	/// <returns>The axis input value.</returns>
	/// <param name="xboxAxis">Xbox axis.</param>
	/// <param name="otherAxis">Other axis.</param>
	float pickGreaterInput (XboxAxis xboxAxis, string keyboardAxis)
	{
		float xbox = XCI.GetAxis (xboxAxis);
		float keyboard = Input.GetAxis (keyboardAxis);
		return keyboard != 0 ? keyboard : xbox;
	}

	void UpdateThrottle ()
	{
		float throttle = _axes [Axis.THROTTLE];
		if (XCI.GetButton (XboxButton.RightBumper) || Input.GetButton ("Thrust+")) {
			throttle += ThrottleChangeRate;
		} else if (XCI.GetButton (XboxButton.LeftBumper) || Input.GetButton ("Thrust-")) {
			throttle -= ThrottleChangeRate;
		}
		_axes [Axis.THROTTLE] = Mathf.Clamp (throttle, -1, 1);
	}

	void UpdateFlaps ()
	{
		float flaps = _axes [Axis.FLAPS];
		if (XCI.GetButton (XboxButton.Y) || Input.GetButton ("Flaps+")) {
			flaps += FlapsChangeRate;
		} else if (XCI.GetButton (XboxButton.X) || Input.GetButton ("Flaps-")) {
			flaps -= FlapsChangeRate;
		}
		_axes [Axis.FLAPS] = Mathf.Clamp (flaps, -1, 1);
	}

	void UpdateWheelBrakes ()
	{
		float current = _axes [Axis.WHEELBRAKES];
		if (XCI.GetButton (XboxButton.B) || Input.GetButton ("WheelBrakes")) {
			current += BrakesChangeRate;
		} else {
			current -= BrakesChangeRate * 2.0f;
		}
		_axes [Axis.WHEELBRAKES] = Mathf.Clamp (current, 0, 1);
	}
}