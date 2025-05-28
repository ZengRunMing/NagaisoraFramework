using System;
using System.Collections.Generic;

using UnityEngine;

namespace NagaisoraFamework
{
	public class BlendManager : CommMonoScriptObject
	{
		public List<BlendData> BlendSetting;
		public List<BlendData> LaserBlendSetting;

		public Dictionary<BlendMode, Material> Blends;
		public Dictionary<BlendMode, Material> LaserBlends;

		public void OnEnable()
		{
			Blends = new Dictionary<BlendMode, Material>();
			LaserBlends = new Dictionary<BlendMode, Material>();

			if (BlendSetting != null)
			{
				foreach (BlendData data in BlendSetting)
				{
					Blends.Add(data.BlendMode, data.Material);
				}
			}

			if (LaserBlendSetting != null)
			{
				foreach (BlendData data in LaserBlendSetting)
				{
					LaserBlends.Add(data.BlendMode, data.Material);
				}
			}
		}

		[Serializable]
		public struct BlendData
		{
			public BlendMode BlendMode;
			public Material Material;
		}
	}
}
