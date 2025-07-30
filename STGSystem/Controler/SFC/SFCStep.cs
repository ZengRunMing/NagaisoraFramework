using System;
using System.Collections.Generic;

namespace NagaisoraFramework.STGSystem
{
	public class SFCStep<T> : ISFComponment<T>
	{
		public uint Index { get; set; }
		public string Detail { get; set; }

		public bool IsActive = false;

		public SFControler<T> Controler { get; set; }

		public List<SFCTran<T>> NextTrans;

		public SFCStep(uint index, string detail)
		{
			Index = index;
			Detail = detail;
		}

		public Action OnEnter;
		public Action OnAction;
		public Action OnLeave;
	}
}
