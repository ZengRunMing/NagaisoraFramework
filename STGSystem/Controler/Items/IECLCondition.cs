namespace NagaisoraFramework.STGSystem
{
	public interface IECLCondition : IECLItem
	{
		bool LoopExecution { get; set; }
		bool Flag { get; set; }

		bool Condition();

		void Execute();
	}
}
