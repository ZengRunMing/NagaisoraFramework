using UnityEngine;
using UnityEngine.UI;

namespace NagaisoraFramework
{
	public class DoubleLayeredText : MonoBehaviour
	{
		public string text;

		public Text TextForge;
		public Text TextBack;

		public void Update()
		{
			if (TextForge is null || TextBack is null)
			{
				Debug.LogWarning("TextForge or TextBack is not assigned in DoubleLayeredText.");
				return;
			}

			TextForge.text = text;
			TextBack.text = text;
		}
	}
}
