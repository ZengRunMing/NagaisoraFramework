namespace NagaisoraFramework.STGSystem
{
	public interface IECLInterrupt : IECLItem
	{
		bool Flag { get; set; }

		bool Condition();

		void Execute();
	}
}
