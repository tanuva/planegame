using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

namespace PlaneGame
{
	/// <summary>
	/// IGUIUpdateTarget contains methods that set certain information for display in the GUI.
	/// </summary>
	public interface IGUIUpdateTarget : IEventSystemHandler
	{
		void UpdateSpeed(float speed);
		void UpdateThrottle(float throttle);
		void UpdateEngineThrottle(float engineThrottle);
		void UpdateAltitude(float altitude);
		void UpdateTargetDir(Vector3 targetDir, Quaternion cameraRotation);
	}
}
