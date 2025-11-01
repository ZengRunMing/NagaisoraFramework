namespace NagaisoraFramework.STGSystem
{
	public class StageControlSystem : CommMonoScriptObject
	{
		public STGControler STGControler;

		public NFEMain[] ECLControlers;

		public void Init(STGControler stgControler, NFEMain[] eclControlers)
		{
			STGControler = stgControler;
			ECLControlers = eclControlers;

			foreach (NFEMain ECLcontroler in ECLControlers)
			{
				//ECLcontroler.STGControler = STGControler;
			}
		}

		public void OnUpdate()
		{
			if (ECLControlers != null)
			{
				foreach (NFEMain controler in ECLControlers)
				{
					controler.OnUpdate();
				}
			}
		}
	}
}
