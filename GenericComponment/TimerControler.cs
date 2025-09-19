using System;

namespace NagaisoraFramework
{
	[Serializable]
	public class TimerControler<T> : CommMonoScriptObject
	{
		public bool IsRunning;

		public Timer Timer;

		public void Init()
		{
			Timer = new Timer();
		}

		public void TimeLineStart()
		{
			Timer.Start();
			IsRunning = true;
		}

		public void TimeLineStop()
		{
			Timer.Stop();
			IsRunning = false;
		}

		public void TimeLineReset()
		{
			Timer.Reset();
		}

		public void FixedUpdate()
		{
			Timer.OnUpdate();
		}

	}
}
