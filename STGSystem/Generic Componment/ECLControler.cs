using System;
using System.Collections.Generic;

namespace NagaisoraFamework.STGSystem
{
	public class ECLControler : IDisposable
	{
		public STGManager STGManager;

		public List<SFControler> SFControler;

		public ECLControler(STGManager manager)
		{
			SFControler = new List<SFControler>();

			STGManager = manager;
		}

		public void Run()
		{
			STGManager.OnUpdate += OnUpdate;

			STGManager.SystemReset();
			STGManager.Run();
		}

		public void OnUpdate()
		{
			SFControler[] controlers = SFControler.ToArray();
			foreach (SFControler Controler in controlers)
			{
				Controler.OnUpdate();
			}
		}

		public void AddSFControler(SFControler controler)
		{
			SFControler.Add(controler);
		}

		public void Dispose()
		{
			STGManager.OnUpdate -= OnUpdate;

			SFControler[] controlers = SFControler.ToArray();
			foreach (SFControler Controler in controlers)
			{
				SFControler.Remove(Controler);
			}
		}
	}
}
