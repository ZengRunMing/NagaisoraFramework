using System;
using System.Collections.Generic;

namespace NagaisoraFramework.STGSystem
{
	public class SFControler<T>
	{
		public T Object;

		public List<SFCStep<T>> Steps;
		public List<SFCStep<T>> RunningSteps;

		public SFControler()
		{
			Steps = new List<SFCStep<T>>();
			RunningSteps = new List<SFCStep<T>>();
		}

		public SFControler(params SFCStep<T>[] steps)
		{
			Steps = new List<SFCStep<T>>(steps);
			RunningSteps = new List<SFCStep<T>>();
			AddSteps(steps);
		}

		public void Init()
		{
			Steps[0].IsActive = true;
			Steps[0].OnEnter();

			AddRunningSteps(Steps[0]);
			RemoveSteps(Steps[0]);
		}

		public void OnUpdate()
		{
			SFCStep<T>[] steps = RunningSteps.ToArray();
			foreach (SFCStep<T> step in steps)
			{
				step.OnAction?.Invoke();
				Condition(step);

				ActiveCheck(step);
			}
		}

		public void Condition(SFCStep<T> step)
		{
			SFCTran<T>[] trans = step.NextTrans.ToArray();
			foreach (SFCTran<T> tran in trans)
			{
				if (tran.Condition())
				{
					SFCStep<T>[] steps = tran.BindSteps.ToArray();
					foreach (SFCStep<T> i in steps)
					{
						AddRunningSteps(i);
						RemoveSteps(i);

						i.OnEnter?.Invoke();
					}

					step.IsActive = false;
				}
			}
		}

		public void ActiveCheck(SFCStep<T> step)
		{
			if (step.IsActive)
			{
				return;
			}

			step.OnLeave?.Invoke();

			AddSteps(step);
			RemoveRunningSteps(step);
		}

		public void AddSteps(params SFCStep<T>[] steps)
		{
			foreach(SFCStep<T> step in steps)
			{
				step.Controler = this;

				foreach(SFCTran<T> tran in step.NextTrans)
				{
					tran.Controler = this;
				}

				Steps.Add(step);
			}
		}

		public void RemoveSteps(params SFCStep<T>[] steps)
		{
			foreach (SFCStep<T> step in steps)
			{
				Steps.Remove(step);
			}
		}

		public void AddRunningSteps(params SFCStep<T>[] steps)
		{
			RunningSteps.AddRange(steps);
		}

		public void RemoveRunningSteps(params SFCStep<T>[] steps)
		{
			foreach (SFCStep<T> step in steps)
			{
				RunningSteps.Remove(step);
			}
		}
	}
}
