using System;
using System.Collections;
using UnityEngine;

namespace PlaneGame
{
	public class DragRigidbody : MonoBehaviour
	{
		[SerializeField]
		float m_Spring = 50.0f;
		[SerializeField]
		float m_Damper = 5.0f;
		[SerializeField]
		float m_Drag = 10.0f;
		[SerializeField]
		float m_AngularDrag = 5.0f;
		[SerializeField]
		float m_Distance = 0.2f;
		[SerializeField]
		bool m_AttachToCenterOfMass = false;

		private SpringJoint m_SpringJoint;

		private void Update()
		{
			// We only work on request
			if (!Input.GetMouseButtonDown (0)) {
				return;
			}

			Camera mainCamera = FindCamera();
			RaycastHit hit = new RaycastHit();
			if (!Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition).origin,
			                     mainCamera.ScreenPointToRay(Input.mousePosition).direction, out hit, 100,
			                     Physics.DefaultRaycastLayers)) {
				return;
			}
			// We need to hit a rigidbody that is not kinematic
			if (!hit.rigidbody || hit.rigidbody.isKinematic) {
				return;
			}

			if (!m_SpringJoint) {
				GameObject go = new GameObject ("Rigidbody dragger");
				Rigidbody body = go.AddComponent<Rigidbody> ();
				m_SpringJoint = go.AddComponent<SpringJoint> ();
				body.isKinematic = true;
			}

			m_SpringJoint.transform.position = hit.point;
			m_SpringJoint.anchor = Vector3.zero;

			m_SpringJoint.spring = m_Spring;
			m_SpringJoint.damper = m_Damper;
			m_SpringJoint.maxDistance = m_Distance;
			m_SpringJoint.connectedBody = hit.rigidbody;

			StartCoroutine ("DragObject", hit.distance);
		}

		private IEnumerator DragObject (float distance)
		{
			var oldDrag = m_SpringJoint.connectedBody.drag;
			var oldAngularDrag = m_SpringJoint.connectedBody.angularDrag;
			m_SpringJoint.connectedBody.drag = m_Drag;
			m_SpringJoint.connectedBody.angularDrag = m_AngularDrag;
			Camera mainCamera = FindCamera ();

			while (Input.GetMouseButton (0)) {
				var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
				m_SpringJoint.transform.position = ray.GetPoint(distance);
				yield return null;
			}

			if (m_SpringJoint.connectedBody) {
				m_SpringJoint.connectedBody.drag = oldDrag;
				m_SpringJoint.connectedBody.angularDrag = oldAngularDrag;
				m_SpringJoint.connectedBody = null;
			}
		}

		private Camera FindCamera ()
		{
			return GetComponent<Camera> () ? GetComponent<Camera> () : Camera.main;
		}
	}
}
