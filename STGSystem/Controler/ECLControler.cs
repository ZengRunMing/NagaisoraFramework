using System;
using System.Collections.Generic;
using System.Reflection;

namespace NagaisoraFramework.STGSystem
{
	public class ECLControler
	{
		public string ECLName;

		public STGControler STGControler;

		public Assembly Assembly;

		public void Run()
		{

		}

		public void Stop()
		{

		}

		public void Init(Assembly assembly, STGControler controler)
		{
			STGControler = controler;

			Assembly = assembly;
			ECLName = Assembly.FullName;
			Type[] type = Assembly.GetExportedTypes();
		}

		public void Init(ECLData ecldata)
		{

		}

		public void GetAssemblyMethod(string name)
		{
			
		}

		public void OnUpdate()
		{

		}
	}
}
