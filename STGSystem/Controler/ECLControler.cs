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
		public BlockControler BlockControler;

		public void Run()
		{
			BlockControler.Run();
		}

		public void Stop()
		{
			BlockControler.Stop();
		}

		public void Init(Assembly assembly, STGControler controler)
		{
			STGControler = controler;

			Assembly = assembly;
			ECLName = Assembly.FullName;
			Type[] type = Assembly.GetExportedTypes();

			List<IBlock> blocks = new List<IBlock>();
			foreach (Type t in type)
			{
				if (t is IBlock block)
				{
					blocks.Add(block);
				}
			}

			BlockControler = new BlockControler(STGControler, blocks.ToArray());
		}

		public void Init(IBlock[] blocks)
		{
			BlockControler = new BlockControler(STGControler, blocks);
		}

		public void GetAssemblyMethod(string name)
		{
			
		}

		public void OnUpdate()
		{
			BlockControler?.OnUpdate();
		}
	}
}
