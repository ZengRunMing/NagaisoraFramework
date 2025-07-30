using System;
using System.Collections.Generic;

namespace NagaisoraFramework.STGSystem
{
	public class SFCTran<T> : ISFComponment<T>
	{
		public uint Index { get; private set; }
		public string Detail { get; private set; }
		public SFControler<T> Controler { get; set; }

		public List<SFCStep<T>> BindSteps;

		public SFCTran(uint index, string detail)
		{
			Index = index;
			Detail = detail;
		}

		public Func<bool> Condition;
	}

}
