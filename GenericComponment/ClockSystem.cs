using System;
using System.Diagnostics;

namespace NagaisoraFramework
{
	public class ClockSystem : CommMonoScriptObject
	{
		public Stopwatch Stopwatch;
		public TimeSpan TimeSpan;

		public bool Updated;

		public delegate void Clock(object sender, Stopwatch stopwatch);

		public event Clock ClockEvent;

		public void OnDisable()
		{
			ClockStop();
		}

		public void FixedUpdate()
		{
			if (!Updated)
			{
				return;
			}

			if (Stopwatch.Elapsed.Ticks % TimeSpan.Ticks == 0)
			{
				ClockEvent?.Invoke(this, Stopwatch);
			}
		}

		public void ClockStart()
		{
			Stopwatch = Stopwatch.StartNew();
		}

		public void ClockStop()
		{
			Stopwatch.Stop();
		}

		public void UpdateStart()
		{
			Updated = true;
		}

		public void UpdateStop()
		{
			Updated = false;
		}
	}
}
