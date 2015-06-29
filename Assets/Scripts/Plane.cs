using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

namespace PlaneGame
{
	/// <summary>
	/// An experimental flight behavior script.
	/// </summary>
	public class Plane : MonoBehaviour
	{
		public InputHandler IH;
		public Transform CenterOfMass;
		public Transform CenterOfLift;
		public Transform TailLiftPoint;
		public Transform EngineTf;
		public WheelCollider NoseWheel;
		public float MaxThrust;
		public float MaxElevatorForce;
		public float Flaps;
		public float AirSpeed;
		public float Lift;
		public float MaxSteerAngle;
		public float CruiseSpeed;
		public float AerodynamicEffect;

		PID pitchPid = new PID ();
		public float PitchPGain;
		public float PitchIGain;
		public float PitchDGain;
		PID yawPid = new PID ();
		public float YawPGain;
		public float YawIGain;
		public float YawDGain;

		Rigidbody rigidbody;
		GameObject _canvas;

		// Use this for initialization
		void Start ()
		{
			_canvas = GameObject.Find("Canvas");

			rigidbody = GetComponent<Rigidbody> ();
			rigidbody.centerOfMass = CenterOfMass.position;
			GetComponent<Rigidbody> ().velocity = new Vector3 (0, 0, 62.7f);
	//		Debug.Log ("In: " + rigidbody.inertiaTensor);
			Debug.Log ("CL: " + transform.localToWorldMatrix * CenterOfLift.position);
	//		GetComponent<Rigidbody> ().inertiaTensor = new Vector3 (2000, 4000, 2000);
	//		GetComponent<Rigidbody> ().inertiaTensorRotation = Quaternion.identity;

			pitchPid.PGain = PitchPGain;
			pitchPid.IGain = PitchIGain;
			pitchPid.DGain = PitchDGain;
			yawPid.PGain = YawPGain;
			yawPid.IGain = YawIGain;
			yawPid.DGain = YawDGain;
		}

		void FixedUpdate ()
		{
	//		AirSpeed = rigidbody.velocity.magnitude;
			AirSpeed = Mathf.Max (0, transform.InverseTransformDirection (rigidbody.velocity).z);
			ApplyEngineForces ();
			UpdateLandingGear ();
			ApplyWingForces ();
			// Align our hunk of metal with the airstream
			ApplyAerodynamics ();
			ApplyControlSurfaces ();

	//		Vector3 liftDir = Vector3.up;
	//		Lift = (rigidbody.mass - 10) * 9.81f;
	//		rigidbody.AddForceAtPosition (Lift * liftDir, CenterOfLift.position);
	//		rigidbody.AddForceAtPosition (Lift * liftDir, CenterOfMass.position);
	//		rigidbody.AddForce (Lift * liftDir);
	//		Debug.DrawLine (CenterOfLift.position, CenterOfLift.position + liftDir, Color.red);
		}
		
		// Update is called once per frame
		void Update ()
		{
			ExecuteEvents.Execute<IGUIUpdateTarget>(_canvas, null, (t, y) => t.UpdateSpeed(AirSpeed));
			ExecuteEvents.Execute<IGUIUpdateTarget>(_canvas, null, (t, y) => t.UpdateThrottle(IH.Axes [Axis.THROTTLE]));
		}

		void ApplyEngineForces ()
		{
			rigidbody.AddForceAtPosition (MaxThrust * IH.Axes [Axis.THROTTLE] * EngineTf.forward, EngineTf.position);
	//		rigidbody.AddForce (MaxThrust * IH.Axes [Axis.THROTTLE] * EngineTf.forward);
			Debug.DrawLine (EngineTf.position, 
			                EngineTf.position + EngineTf.forward * IH.Axes [Axis.THROTTLE], 
			                Color.green);
			Debug.DrawLine (EngineTf.position,
			                EngineTf.position + rigidbody.velocity.normalized,
			                Color.red);
		}

		void UpdateLandingGear ()
		{
			// Keep those sleepy wheel colliders awake
			// TODO Only do this while thrust || velocity != 0
			var noseWheel = GameObject.Find ("WheelNose").GetComponent<WheelCollider> ();
			noseWheel.motorTorque = 0.00001f;
			noseWheel.steerAngle = MaxSteerAngle * IH.Axes [Axis.RUDDER];
			var leftWheel = GameObject.Find ("WheelLeft").GetComponent<WheelCollider> ();
			leftWheel.motorTorque = 0.00001f;
			var rightWheel = GameObject.Find ("WheelRight").GetComponent<WheelCollider> ();
			rightWheel.motorTorque = 0.00001f;
		}

		void ApplyWingForces ()
		{
			// Target: 7670 N at 62.7 m/s to fly level at cruise speed
			// -> Lift formula: f(x) = 7670/62.7x
			// TODO Lift is also influenced by the vertical AoA
			// TODO Don't forget drag with flaps out! (curDrag = origDrag * flapFactor?)
			// TODO Stalling through non-linear lift curve: significant decrease < 47 kn (24.18 m/s)
			// The direction that the lift force is applied is at right angles to the plane's velocity (usually, this is 'up'!)
			// I'm not convinced about this...
			//		Vector3 liftDir = Vector3.Cross (rigidbody.velocity, transform.right).normalized;
			Vector3 liftDir = transform.up;
			float angleOfAttack = Mathf.Acos (Vector3.Dot (rigidbody.velocity.normalized, transform.forward));
			Debug.Log ("AoA: " + angleOfAttack);
			Lift = rigidbody.mass * 9.81f / CruiseSpeed * AirSpeed * Flaps;
			//		Lift = 
			rigidbody.AddForceAtPosition (Lift * liftDir,
			                              CenterOfLift.position);
			if (Lift < rigidbody.mass * 9.81f) {
				Debug.DrawLine (CenterOfLift.position, 
				                CenterOfLift.position + liftDir * (Lift / 10000f), 
				                Color.red);
			} else {
				Debug.DrawLine (CenterOfLift.position, 
				                CenterOfLift.position + liftDir * (Lift / 10000f), 
				                Color.green);
			}
		}

		void ApplyAerodynamics ()
		{
			// To compute the desired tail position:
			// Raycast backwards from EngineTf in direction (-rigidbody.velocity.normalized) until on XZ plane of TailLiftPoint
			UnityEngine.Plane tailPlane = new UnityEngine.Plane (transform.forward, TailLiftPoint.position);
			float distanceToTailplane;
			tailPlane.Raycast (new Ray (EngineTf.position, -rigidbody.velocity.normalized), out distanceToTailplane);
			Vector3 desiredPos = (EngineTf.position + distanceToTailplane * -rigidbody.velocity.normalized);
			Vector3 error = desiredPos - TailLiftPoint.position;
			// Pitch stabilization
			float elevatorForce = pitchPid.Update (error.y * AirSpeed, TailLiftPoint.position.y);
	//		rigidbody.AddForceAtPosition (new Vector3 (0, elevatorForce, 0), TailLiftPoint.position);
			// Yaw stabilization
			float rudderForce = yawPid.Update (error.x * AirSpeed, TailLiftPoint.position.x);
	//		rigidbody.AddForceAtPosition (new Vector3 (rudderForce, 0, 0), transform.localToWorldMatrix * new Vector3 (0, -0.46f, TailLiftPoint.localPosition.z));

			Debug.DrawLine (desiredPos, desiredPos - transform.up);
			Debug.DrawLine (desiredPos, desiredPos - transform.right);
			Debug.DrawLine (TailLiftPoint.position, TailLiftPoint.position + new Vector3 (0, error.y, error.z), Color.red);
			Debug.DrawLine (TailLiftPoint.position, TailLiftPoint.position + new Vector3 (0, elevatorForce / 100, 0), Color.green);
			Debug.DrawLine (TailLiftPoint.position, TailLiftPoint.position + new Vector3 (rudderForce / 100, 0, 0), Color.green);


			// compare the direction we're pointing with the direction we're moving:
			float aeroFactor = Vector3.Dot (transform.forward, rigidbody.velocity.normalized);
			// multipled by itself results in a desirable rolloff curve of the effect
			aeroFactor *= aeroFactor;
			// Finally we calculate a new velocity by bending the current velocity direction towards
			// the the direction the plane is facing, by an amount based on this aeroFactor
			var newVelocity = Vector3.Lerp (rigidbody.velocity, 
			                                transform.forward * AirSpeed,
			                                aeroFactor * AirSpeed * AerodynamicEffect * Time.deltaTime);
			rigidbody.velocity = newVelocity;
			
			// also rotate the plane towards the direction of movement - this should be a very small effect, but means the plane ends up
			// pointing downwards in a stall
			rigidbody.rotation = Quaternion.Slerp (rigidbody.rotation,
			                                       Quaternion.LookRotation (rigidbody.velocity, transform.up),
			                                       AerodynamicEffect * Time.deltaTime);
		}

		void ApplyControlSurfaces ()
		{
			rigidbody.AddForceAtPosition (transform.up * MaxElevatorForce * IH.Axes [Axis.ELEVATOR],
			                              TailLiftPoint.position);

			Debug.DrawLine (TailLiftPoint.position, 
			                TailLiftPoint.position + IH.Axes [Axis.ELEVATOR] * transform.up, 
			                Color.blue);
		}
	}
}
