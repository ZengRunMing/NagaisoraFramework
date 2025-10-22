using UnityEngine;

namespace NagaisoraFramework
{
	public class ObjectRotate : CommMonoScriptObject
	{
		public float Speed;

		public Vector3 RotateR;

		public void FixedUpdate()
		{
			transform.localEulerAngles += Speed * RotateR;
		}
	}
}
