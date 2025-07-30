using System;

namespace NagaisoraFramework
{
	using STGSystem;

	public interface IFlag
	{
		string FlagName { get; }
		bool MultipleExecutions { get; }

		bool Condition();
		void Action();
	}

	public interface ISTGManagerFlag : IFlag
	{
		STGManager STGManager { get; set; }
	}

	public interface ISTGComponmentFlag : IFlag
	{
		STGComponment Componment { get; set; }
	}

	public interface ITimeLineFlag<T>
	{
		string FlagName { get; }
		TimeSpan Time { get; set; }
		bool MultipleExecutions { get; }

		TimeLineSystem<T> TimeLineSystem { get; set; }

		void OnEnter(T i);

		void OnAction(T i);

		void OnLeave(T i);
	}
}
