using System;
using System.Collections.Generic;

namespace NagaisoraFramework.STGSystem
{
	[Serializable]
	public class BlockControler
	{
		public bool IsRunning = false;

		public STGControler STGControler;

		public IBlock[] Blocks;

		public OrganizationBlock[] OrganizationBlocks;
		public FunctionBlock[] FunctionBlocks;
		public Function[] Functions;
		public DataBlock[] DataBlocks;

		public Queue<InterruptOrganizationBlock> InterruptingOrganizationBlocks;
		
		public BlockControler(STGControler controler, IBlock[] blocks)
		{
			STGControler = controler;

			Blocks = blocks;

			InterruptingOrganizationBlocks = new Queue<InterruptOrganizationBlock>();
			List<OrganizationBlock> obs = new List<OrganizationBlock>();
			List<FunctionBlock> fbs = new List<FunctionBlock>();
			List<Function> fs = new List<Function>();
			List<DataBlock> dbs = new List<DataBlock>();

			foreach (IBlock block in Blocks)
			{
				block.Controler = this;

				if (block is OrganizationBlock)
				{
					obs.Add(block as OrganizationBlock);
				}
				else if (block is FunctionBlock)
				{
					fbs.Add(block as FunctionBlock);
				}
				else if (block is Function)
				{
					fs.Add(block as Function);
				}
				else if (block is DataBlock)
				{
					dbs.Add(block as DataBlock);
				}
				else
				{
					throw new Exception($"Unknown Block Type {block.GetType()}");
				}
			}

			OrganizationBlocks = obs.ToArray();
			FunctionBlocks = fbs.ToArray();
			Functions = fs.ToArray();
			DataBlocks = dbs.ToArray();

			foreach (DataBlock dataBlock in DataBlocks)
			{
				dataBlock.Init();
			}

			foreach (FunctionBlock functionBlock in FunctionBlocks)
			{
				functionBlock.Init(FindDataBlock(functionBlock.DataBlockName));
			}

			foreach (Function function in Functions)
			{
				function.Init();
			}

			foreach (OrganizationBlock organizationBlock in OrganizationBlocks)
			{
				organizationBlock.Init();
			}
		}

		public void Run()
		{
			if (IsRunning)
			{
				return;
			}

			IsRunning = true;
			foreach (OrganizationBlock block in OrganizationBlocks)
			{
				if (block is StartUpOrganizationBlock)
				{
					block.Execute();
				}
			}
		}

		public void Stop()
		{
			if (!IsRunning)
			{
				return;
			}

			IsRunning = false;
		}

		public void OnUpdate()
		{
			if (!IsRunning)
			{
				return;
			}

			while (InterruptingOrganizationBlocks.Count != 0)
			{
				InterruptingOrganizationBlocks.Dequeue().Execute();
			}

			foreach (OrganizationBlock block in OrganizationBlocks)
			{
				if (block is CycleOrganizationBlock)
				{
					block.Execute();
				}
			}

			foreach (OrganizationBlock block in OrganizationBlocks)
			{
				if (block is InterruptOrganizationBlock interruptOrganizationBlock)
				{
					bool cond = interruptOrganizationBlock.Condition();

					if (cond)
					{
						InterruptingOrganizationBlocks.Enqueue(interruptOrganizationBlock);
					}
				}
			}
		}

		public Function FindFunction(string name)
		{
			foreach(Function function in Functions)
			{
				if (function.Name == name)
				{
					return function;
				}
			}

			return null;
		}

		public FunctionBlock FindFunctionBlock(string name)
		{
			foreach (FunctionBlock functionblock in FunctionBlocks)
			{
				if (functionblock.Name == name)
				{
					return functionblock;
				}
			}

			return null;
		}

		public DataBlock FindDataBlock(string name)
		{
			foreach (DataBlock datablock in DataBlocks)
			{
				if (datablock.Name == name)
				{
					return datablock;
				}
			}

			return null;
		}
	}
}
