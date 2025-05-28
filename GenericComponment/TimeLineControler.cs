using System;

namespace NagaisoraFamework
{
	[Serializable]
	public class TimeLineControler<T> : CommMonoScriptObject
	{
		public bool IsRunning;

		public TimeLineSystem<T> TimeLineSystem;

		public void Init(T obj)
		{
			TimeLineSystem = new TimeLineSystem<T>(obj);
		}

		public void TimeLineStart()
		{
			TimeLineSystem.Start();
			IsRunning = true;
		}

		public void TimeLineStop()
		{
			TimeLineSystem.Stop();
			IsRunning = false;
		}

		public void TimeLineReset()
		{
			TimeLineSystem.Reset();
		}

		public void FixedUpdate()
		{
			TimeLineSystem.OnUpdate();
		}

		public void ClearFlag()
		{
			TimeLineSystem.Flags.Clear();
			TimeLineSystem.RunningFlags.Clear();

			TimeLineSystem.RemovedConditionFlags.Clear();
			TimeLineSystem.RemovedRunningFlags.Clear();
		}

		public void AddFlag(params ITimeLineFlag<T>[] flags)
		{
			TimeLineSystem.AddFlags(flags);
		}

		public void RemoveFlag(params ITimeLineFlag<T>[] flags)
		{
			TimeLineSystem.RemoveFlags(flags);
		}
	}
}
