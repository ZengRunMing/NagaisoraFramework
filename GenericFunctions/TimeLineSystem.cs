using System;
using System.Linq;
using System.Collections.Generic;

namespace NagaisoraFramework
{
	[Serializable]
	public class TimeLineSystem<T> : TimeControler
	{
		public T Object;

		public Dictionary<string, ITimeLineFlag<T>> Flags;
		public List<ITimeLineFlag<T>> RemovedConditionFlags;
		public Dictionary<string, ITimeLineFlag<T>> RunningFlags;
		public List<ITimeLineFlag<T>> RemovedRunningFlags;

		public TimeLineSystem(T obj)
		{
			Object = obj;

			Flags = new Dictionary<string, ITimeLineFlag<T>>();
			RemovedConditionFlags = new List<ITimeLineFlag<T>>();
			RunningFlags = new Dictionary<string, ITimeLineFlag<T>>();
			RemovedRunningFlags = new List<ITimeLineFlag<T>>();
		}

		public void OnUpdate()
		{
			FlagCheck();
		}

		public virtual void FlagCheck()
		{
			if (!IsRunning)
			{
				return;
			}

			if ((Flags == null || Flags.Count == 0) && (RunningFlags == null || RunningFlags.Count == 0))
			{
				return;
			}

			ITimeLineFlag<T>[] ConditionFlagsArray = Flags.Values.ToArray();
			RemovedConditionFlags.Clear();
			foreach (var flag in ConditionFlagsArray)
			{
				if (Elapsed.Ticks < flag.Time.Ticks)
				{
					continue;
				}

				flag.OnEnter(Object);
				AddRunningFlags(flag);
				RemovedConditionFlags.Add(flag);
			}

			ITimeLineFlag<T>[] RemovedConditionFlagsArray = RemovedConditionFlags.ToArray();
			foreach (var flag in RemovedConditionFlagsArray)
			{
				RemoveFlags(flag);
			}

			ITimeLineFlag<T>[] RunningFlagsArray = RunningFlags.Values.ToArray();
			RemovedRunningFlags.Clear();
			foreach (var flag in RunningFlagsArray)
			{
				flag.OnAction(Object);

				if (!flag.MultipleExecutions)
				{
					flag.OnLeave(Object);
					RemovedRunningFlags.Add(flag);
				}
			}

			ITimeLineFlag<T>[] RemovedRunningFlagsArray = RemovedRunningFlags.ToArray();
			foreach (var flag in RemovedRunningFlagsArray)
			{
				RemoveRunningFlags(flag);
			}
		}

		public virtual void AddFlags(params ITimeLineFlag<T>[] flags)
		{
			foreach (var flag in flags)
			{
				if (Flags.ContainsKey(flag.FlagName))
				{
					throw new Exception($"ConditionFlag {flag.FlagName} already exists in {GetHashCode()}.");
				}
				flag.TimeLineSystem = this;
				Flags.Add(flag.FlagName, flag);
			}
		}

		public virtual void RemoveFlags(params ITimeLineFlag<T>[] flags)
		{
			foreach (var flag in flags)
			{
				Flags.Remove(flag.FlagName);
			}
		}

		public virtual void AddRunningFlags(params ITimeLineFlag<T>[] flags)
		{
			foreach (var flag in flags)
			{
				if (RunningFlags.ContainsKey(flag.FlagName))
				{
					throw new Exception($"RunningFlag {flag.FlagName} already exists in {GetHashCode()}.");
				}
				flag.TimeLineSystem = this;
				RunningFlags.Add(flag.FlagName, flag);
			}
		}

		public virtual void RemoveRunningFlags(params ITimeLineFlag<T>[] flags)
		{
			foreach (var flag in flags)
			{
				RunningFlags.Remove(flag.FlagName);
			}
		}
	}
}
