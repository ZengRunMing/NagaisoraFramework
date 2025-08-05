using NagaisoraFramework;
using NagaisoraFramework.STGSystem;

namespace NagaisoraFramework.STGSystem
{
	public class StageControlSystem : CommMonoScriptObject
	{
		public STGControler STGControler;

		public ECLControler[] ECLControlers;

		public void Init(STGControler stgControler, ECLControler[] eclControlers)
		{
			STGControler = stgControler;
			ECLControlers = eclControlers;

			foreach (ECLControler ECLcontroler in ECLControlers)
			{
				ECLcontroler.STGControler = STGControler;
			}

			STGControler.OnUpdate += OnUpdate;
		}

		public void OnUpdate()
		{
			if (ECLControlers != null)
			{
				foreach (ECLControler controler in ECLControlers)
				{
					controler.OnUpdate();
				}
			}
		}
	}
}
