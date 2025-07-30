using System;
using System.Data;

namespace NagaisoraFramework.STGSystem
{
	public interface IBlock
	{
		string Name { get; set; }
		string Description { get; set; }

		DataSet BlockInterface { get; set; }

		BlockControler Controler { get; set; }

		object GetInterfereValue(string tableName, string name);

		bool SetInterfereValue(string Tablename, string name, object value);
	}
}
