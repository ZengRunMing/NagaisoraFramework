using UnityEngine;

namespace NagaisoraFramework.STGSystem
{
	public class EnemyControlEx : EnemyControl
	{
		public float HealthPoint
		{
			get => m_HealthPoint;
			set => m_HealthPoint = value;
		}

		[SerializeField]
		protected float m_HealthPoint;
	}
}
