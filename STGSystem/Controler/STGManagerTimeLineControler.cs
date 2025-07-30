using System;
using System.Collections.Generic;

namespace NagaisoraFramework.STGSystem
{
	[Serializable]
	public class STGManagerTimeLineControler : TimeLineControler<STGManager>
	{
		public STGManager STGManager;

		public List<ITimeLineFlag<STGManager>> Flags;

		public bool Running = false;

		public int h;
		public int m;
		public int s;
		public int ms;

		public virtual void Start()
		{
			if (STGManager == null)
			{
				STGManager = GetComponent<STGManager>();
			}

			Init(STGManager);

			if (Flags != null && Flags.Count > 0)
			{
				AddFlag(Flags.ToArray());
			}
		}

		public virtual void Update()
		{
			TimeSpan timeSpan = TimeLineSystem.Elapsed;

			h = timeSpan.Hours;
			m = timeSpan.Minutes;
			s = timeSpan.Seconds;
			ms = timeSpan.Milliseconds;

			if (Running)
			{
				if (!IsRunning)
				{
					TimeLineStart();
				}
			}
			else
			{
				if (IsRunning)
				{
					TimeLineStop();
				}
			}
		}
	}
}
