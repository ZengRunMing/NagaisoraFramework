using System;

namespace NagaisoraFamework
{
	[Serializable]
	public class TimerControler<T> : CommMonoScriptObject
	{
		public bool IsRunning;

		public Timer<T> Timer;

		public void Init(T obj)
		{
			Timer = new Timer<T>(obj);
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
