namespace NagaisoraFramework.STGSystem
{
	public class NFEMain
	{
		public ExecuteSystem BaseExecuteSystem;
		public STGComponent BaseComponment;
		public STGControler BaseControler;

		public NFEMain(ExecuteSystem executeSystem, STGControler controler, STGComponent componment = null)
		{
			BaseExecuteSystem = executeSystem;
			BaseControler = controler;
			BaseComponment = componment;
		}

		public virtual void OnInitializing()
		{

		}

		public virtual void OnInitialized()
		{

		}

		public virtual void OnUpdate()
		{

		}
	}
}
