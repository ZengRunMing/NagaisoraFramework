using System.Collections.Generic;

namespace NagaisoraFamework
{
	public class SFControler
	{
		public List<SFCStep> Steps;
		public List<SFCStep> RunningSteps;

		public SFControler()
		{
			Steps = new List<SFCStep>();
			RunningSteps = new List<SFCStep>();
		}

		public SFControler(params SFCStep[] steps)
		{
			Steps = new List<SFCStep>(steps);
			RunningSteps = new List<SFCStep>();
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
			SFCStep[] steps = RunningSteps.ToArray();

			foreach (SFCStep step in steps)
			{
				step.OnAction();
				Condition(step);

				ActiveCheck(step);
			}
		}

		public void Condition(SFCStep step)
		{
			foreach (SFCTran tran in step.NextTrans)
			{
				if (tran.Condition())
				{
					foreach (SFCStep i in tran.BindSteps)
					{
						AddRunningSteps(i);
						RemoveSteps(i);

						i.OnEnter();
					}

					step.IsActive = false;
				}
			}
		}

		public void ActiveCheck(SFCStep step)
		{
			if (step.IsActive)
			{
				return;
			}

			step.OnLeave();

			AddSteps(step);
			RemoveRunningSteps(step);
		}

		public void AddSteps(params SFCStep[] steps)
		{
			foreach(SFCStep step in steps)
			{
				step.Controler = this;

				foreach(SFCTran tran in step.NextTrans)
				{
					tran.Controler = this;
				}

				Steps.Add(step);
			}
		}

		public void RemoveSteps(params SFCStep[] steps)
		{
			foreach (SFCStep step in steps)
			{
				Steps.Remove(step);
			}
		}

		public void AddRunningSteps(params SFCStep[] steps)
		{
			RunningSteps.AddRange(steps);
		}

		public void RemoveRunningSteps(params SFCStep[] steps)
		{
			foreach (SFCStep step in steps)
			{
				RunningSteps.Remove(step);
			}
		}
	}

	public class SFCStep : ISFComponment
	{
		public uint Index { get; set; }
		public string Detail { get; set; }

		public bool IsActive = false;

		public SFControler Controler { get; set; }

		public SFCTran[] NextTrans;

		public SFCStep(uint index, string detail)
		{
			Index = index;
			Detail = detail;
		}
		
		public virtual void OnEnter()
		{
			
		}
		
		public virtual void OnAction()
		{

		}
		
		public virtual void OnLeave()
		{

		}
	}

	public class SFCTran : ISFComponment
	{
		public uint Index { get; private set; }
		public string Detail { get; private set; }
		public SFControler Controler { get; set; }

		public SFCStep[] BindSteps;

		public SFCTran(uint index, string detail)
		{
			Index = index;
			Detail = detail;
		}

		public virtual bool Condition()
		{
			return true;
		}
	}

	public interface ISFComponment
	{
		uint Index { get; }
		string Detail { get; }
		SFControler Controler { get; set; }
	}
}
