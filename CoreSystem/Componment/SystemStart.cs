using System;
using System.IO;

using UnityEngine;

namespace NagaisoraFamework
{
	using Miedia;
	using DataFileSystem;

	using static MainSystem;

	public class SystemStart : CommMonoScriptObject
	{
		public string Name;

		public int PlayerLength;
		public int RankLength;

		public string[] arguments;

		public string ServerPort;
		public string ThisPort;

		public long MaxBullet;

		public void Awake()
		{
			MainSystem.Name = Name;
			
			InitRandom();

			LoadConfigData(DataPathDefine.ConfigData);

			LoadScoreData(DataPathDefine.ScoreData);

			LoadWaveBGMData(DataPathDefine.WaveBGMData);
			LoadMidiBGMData(DataPathDefine.MidiBGMData);

			SetResolution();
		}
	}
}