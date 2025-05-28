using System;

namespace NagaisoraFamework
{
	public class TimeControler
	{
		public bool IsRunning;
	
		public TimeSpan StartTime;
		public TimeSpan CacheDuration;

		public TimeSpan Elapsed => IsRunning ? MainSystem.SystemTime - StartTime + CacheDuration : CacheDuration;

		public void Start()
		{
			StartTime = MainSystem.SystemTime;
			IsRunning = true;
		}

		public void Stop()
		{
			CacheDuration = Elapsed;
			IsRunning = false;
		}

		public void Reset()
		{
			StartTime = TimeSpan.Zero;
			CacheDuration = TimeSpan.Zero;
			IsRunning = false;
		}
	}
}
