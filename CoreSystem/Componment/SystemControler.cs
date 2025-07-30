using System;
using System.Diagnostics;
using UnityEngine;

namespace NagaisoraFramework
{
	public class SystemControler : CommMonoScriptObject
	{
		public Stopwatch RunTimeStopwatch;

		public int SetFps;

		public float FPSNow;

		public int frames = 0;
		public float lastInterval;
		public float updateInterval = 0.5f;

		public float timeSpend = 0;

		public int hour;
		public int minute;
		public int second;
		public int millisecond;

		public void Awake()
		{
			RunTimeStopwatch = Stopwatch.StartNew();
		}

		public void Update()
		{
			switch (MainSystem.ConfigData.Frame)
			{
				case 0:
					SetFps = 60;
					break;
				case 1:
					SetFps = 120;
					break;
				case 2:
					SetFps = 0;
					break;
			}

			MainSystem.BGMControl.AudioControl.Volume = MainSystem.ConfigData.MusicVolume * 0.1f;
			MainSystem.SEManager.Volume = MainSystem.ConfigData.SEVolume * 0.1f;

			Application.targetFrameRate = SetFps;

			++frames;
			float timeNow = Time.realtimeSinceStartup;
			if (timeNow >= lastInterval + updateInterval)
			{
				FPSNow = frames / (timeNow - lastInterval);
				frames = 0;
				lastInterval = timeNow;
				MainSystem.Fps = FPSNow;
			}

			MainSystem.RunTime = RunTimeStopwatch.Elapsed;
			MainSystem.TotalRunTime = MainSystem.ScoreData.TotalRunTime + MainSystem.RunTime;

			timeSpend += Time.deltaTime;

			hour = (int)timeSpend / 3600;
			minute = ((int)timeSpend - hour * 3600) / 60;
			second = (int)timeSpend - hour * 3600 - minute * 60;
			millisecond = (int)((timeSpend - (int)timeSpend) * 1000);

			MainSystem.SystemTime = new TimeSpan(0, hour, minute, second, millisecond);
		}
	}
}