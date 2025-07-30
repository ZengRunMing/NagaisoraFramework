using System;

namespace NagaisoraFramework
{
	public class Timer<T> : TimeControler
	{
		public TimeSpan SetTime;
		public bool MultipleExecutions = false;

		public T Object;
		public Action<T> Action;

		public Timer(T obj)
		{
			Object = obj;
		}

		public void OnUpdate()
		{
			if (Elapsed.Ticks < SetTime.Ticks)
			{
				return;
			}

			Reset();

			if (MultipleExecutions)
			{
				Start();
			}

			Action?.Invoke(Object);
		}
	}
}
