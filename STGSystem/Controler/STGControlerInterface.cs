using UnityEngine;
using UnityEngine.UI;

namespace NagaisoraFramework.STGSystem
{
	public class STGControlerInterface : CommMonoScriptObject
	{
		public Camera Camera;
		public GameObject Parent;
		public RawImage BackgroundImage;

		public Player Player;
		public GameObject PlayerDeterminePoint;
	}
}
