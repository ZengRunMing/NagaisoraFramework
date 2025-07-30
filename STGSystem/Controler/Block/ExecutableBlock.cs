using System.Data;

namespace NagaisoraFramework.STGSystem
{
	public class ExecutableBlock : IBlock
	{
		public string Name { get; set; }
		public string Description { get; set; }

		public DataSet BlockInterface { get; set; }

		public BlockControler Controler { get; set; }

		public string ExecuteCode { get; set; }

		public virtual void Default()
		{
			ExecuteCode =
				$"#region {Name.Replace(' ','_')}-ExecuteMethodUsing\n" +
				$"//Write using code here\n" +
				$"\n" +
				"#endregion\n\n" +
				"public override void Execute()\n" +
				"{\n" +
				"\t//Write block execute code here\n" +
				"\t\n" +
				"\treturn;\n" +
				"}\n";
		}

		public virtual void Init()
		{

		}

		public virtual void Execute()
		{

		}

		public virtual object GetInterfereValue(string tableName, string name)
		{
			if (!BlockInterface.Tables.Contains(tableName))
			{
				return null;
			}

			DataRow row = BlockInterface.Tables[tableName].Rows.Find(name);
			if (row == null)
			{
				return null;
			}

			return row[BlockComColumn.ValueColumn.ColumnName];
		}

		public virtual bool SetInterfereValue(string tableName, string name, object value)
		{
			if (!BlockInterface.Tables.Contains(tableName))
			{
				return false;
			}

			DataRow row = BlockInterface.Tables[tableName].Rows.Find(name);
			if (row == null)
			{
				return true;
			}

			row["Value"] = value;
			return true;
		}
	}
}
