namespace NagaisoraFramework.STGSystem
{
	public interface ISFComponment<T>
	{
		uint Index { get; }
		string Detail { get; }
		SFControler<T> Controler { get; set; }
	}
}
