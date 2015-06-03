using UnityEngine;

public class SmoothFollow : MonoBehaviour
{

	// The target we are following
	[SerializeField]
	private Transform
		target;
	// The distance in the x-z plane to the target
	[SerializeField]
	private float
		distance = 10.0f;
	// the height we want the camera to be above the target
	[SerializeField]
	private float
		height = 5.0f;
	[SerializeField]
	private float
		_XAngle = 0;
	[SerializeField]
	private float
		damping = 1;

	private Rigidbody targetBody;

	// Use this for initialization
	void Start ()
	{
		targetBody = target.gameObject.GetComponent<Rigidbody> ();
	}

	// Update is called once per frame
	void LateUpdate ()
	{
		// Early out if we don't have a target
		if (!target)
			return;

		// TODO don't lerp from one position to the other but lerp angle and x distance separately, keeping z distance constant
		var curPos = transform.position;

		float wantedRotationAngle = target.eulerAngles.y;
		float wantedHeight = target.position.y + height * Mathf.InverseLerp (60, 40, targetBody.velocity.magnitude);
		float currentRotationAngle = transform.eulerAngles.y;
		float currentHeight = transform.position.y;
		// Damp the rotation around the y-axis
		currentRotationAngle = Mathf.LerpAngle (currentRotationAngle, wantedRotationAngle, damping * Time.deltaTime);
		// Damp the height
		currentHeight = Mathf.Lerp (currentHeight, wantedHeight, damping * Time.deltaTime);
		// Convert the angle into a rotation
		Quaternion currentRotation = Quaternion.Euler (0, currentRotationAngle, 0);
		// Set the position of the camera on the x-z plane to:
		// distance meters behind the target
		transform.position = target.position;
		transform.position -= currentRotation * Vector3.forward * distance;
		// Set the height of the camera
		transform.position = new Vector3 (transform.position.x, wantedHeight, transform.position.z);


		// Calculate our next smoooooth position
//		var desiredDistance = target.position - distance * target.forward;
//		var distanceLerped = Vector3.Lerp (transform.position, desiredDistance, Time.deltaTime * distanceStiffness);
//		var desiredPos = desiredDistance + height * Vector3.up + _XAngle * Vector3.right;
//		var rotationLerp = Vector3.Lerp (transform.position, desiredPos, Time.deltaTime * damping);
//
//		transform.position = target.position + (rotationLerp - target.position).normalized * distance;
		transform.LookAt (target);
	}
}