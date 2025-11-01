namespace NagaisoraFramework
{
	public class ClockSystem : CommMonoScriptObject
	{
		public delegate void Clock(object sender);

		public event Clock FixedUpdateClockEvent;
		public event Clock UpdateClockEvent;

		public void FixedUpdate()
		{
			FixedUpdateClockEvent?.Invoke(this);
		}

		public void Update()
		{
			UpdateClockEvent?.Invoke(this);
		}

	}
}
