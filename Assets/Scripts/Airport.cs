using UnityEngine;
using System.Collections;

namespace PlaneGame
{
	public class Airport : MonoBehaviour
	{
		[SerializeField]
		private GameObject m_Target;

		public void setIsCurrentTarget(bool isCurrentTarget)
		{
			m_Target.SetActive (isCurrentTarget);
		}

		// Use this for initialization
		void Start ()
		{
			// Throwing a separate exception if Target wasn't found is superfluous.
			m_Target = transform.FindChild ("Target").gameObject;
			m_Target.SetActive (false);
		}
		
		// Update is called once per frame
		void Update ()
		{	
		}
	}
}
