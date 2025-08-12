namespace NagaisoraFramework.STGSystem
{
	public interface IECLItem
	{
		string Name { get; set; }
		string Description { get; set; }

		string ExecuteCode { get; set; }

		void Init();
	}
}
