using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace PlaneGame
{
	/// <summary>
	/// A Job is a delivery task to be completed by the player.
	/// </summary>
	public class Job
	{
		private int m_ID;
		private int m_Pay;
		private GameObject m_Origin;
		private GameObject m_Destination;
		public int ID					{ get { return m_ID; } }
		public int Pay					{ get { return m_Pay; } }
		public GameObject Origin		{ get { return m_Origin; } }
		public GameObject Destination	{ get { return m_Destination; } }

		public Job (int id, int pay, GameObject origin, GameObject destination)
		{
			m_ID = id;
			m_Pay = pay;
			m_Origin = origin;
			m_Destination = destination;
		}
	}
}

