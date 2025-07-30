using System;
using System.Data;
using System.IO;
using System.Text;

namespace NagaisoraFramework.STGSystem
{
	[Serializable]
	public class Function : ExecutableBlock
	{
		public override void Init()
		{
			BlockInterface = new DataSet("BlockInterface");

			DataTable Input = new DataTable("Input");
			Input.Columns.Add(BlockComColumn.NameColumn.ColumnName, BlockComColumn.NameColumn.DataType);
			Input.Columns.Add(BlockComColumn.TypeColumn.ColumnName, BlockComColumn.TypeColumn.DataType);
			Input.Columns.Add(BlockComColumn.ValueColumn.ColumnName, BlockComColumn.ValueColumn.DataType);
			Input.Columns.Add(BlockComColumn.DefaultValueColumn.ColumnName, BlockComColumn.DefaultValueColumn.DataType);
			Input.Columns.Add(BlockComColumn.MaintainColumn.ColumnName, BlockComColumn.MaintainColumn.DataType);
			Input.Columns.Add(BlockComColumn.NotesColumn.ColumnName, BlockComColumn.NotesColumn.DataType);
			Input.PrimaryKey = new DataColumn[] { Input.Columns[0] };

			DataTable Output = new DataTable("Output");
			Output.Columns.Add(BlockComColumn.NameColumn.ColumnName, BlockComColumn.NameColumn.DataType);
			Output.Columns.Add(BlockComColumn.TypeColumn.ColumnName, BlockComColumn.TypeColumn.DataType);
			Output.Columns.Add(BlockComColumn.ValueColumn.ColumnName, BlockComColumn.ValueColumn.DataType);
			Output.Columns.Add(BlockComColumn.DefaultValueColumn.ColumnName, BlockComColumn.DefaultValueColumn.DataType);
			Output.Columns.Add(BlockComColumn.MaintainColumn.ColumnName, BlockComColumn.MaintainColumn.DataType);
			Output.Columns.Add(BlockComColumn.NotesColumn.ColumnName, BlockComColumn.NotesColumn.DataType);
			Output.PrimaryKey = new DataColumn[] { Output.Columns[0] };

			DataTable InOut = new DataTable("InOut");
			InOut.Columns.Add(BlockComColumn.NameColumn.ColumnName, BlockComColumn.NameColumn.DataType);
			InOut.Columns.Add(BlockComColumn.TypeColumn.ColumnName, BlockComColumn.TypeColumn.DataType);
			InOut.Columns.Add(BlockComColumn.ValueColumn.ColumnName, BlockComColumn.ValueColumn.DataType);
			InOut.Columns.Add(BlockComColumn.DefaultValueColumn.ColumnName, BlockComColumn.DefaultValueColumn.DataType);
			InOut.Columns.Add(BlockComColumn.MaintainColumn.ColumnName, BlockComColumn.MaintainColumn.DataType);
			InOut.Columns.Add(BlockComColumn.NotesColumn.ColumnName, BlockComColumn.NotesColumn.DataType);
			InOut.PrimaryKey = new DataColumn[] { InOut.Columns[0] };

			DataTable Temp = new DataTable("Temp");
			Temp.Columns.Add(BlockComColumn.NameColumn.ColumnName, BlockComColumn.NameColumn.DataType);
			Temp.Columns.Add(BlockComColumn.TypeColumn.ColumnName, BlockComColumn.TypeColumn.DataType);
			Temp.Columns.Add(BlockComColumn.ValueColumn.ColumnName, BlockComColumn.ValueColumn.DataType);
			Temp.Columns.Add(BlockComColumn.DefaultValueColumn.ColumnName, BlockComColumn.DefaultValueColumn.DataType);
			Temp.Columns.Add(BlockComColumn.MaintainColumn.ColumnName, BlockComColumn.MaintainColumn.DataType);
			Temp.Columns.Add(BlockComColumn.NotesColumn.ColumnName, BlockComColumn.NotesColumn.DataType);
			Temp.PrimaryKey = new DataColumn[] { Temp.Columns[0] };

			DataTable Constant = new DataTable("Constant");
			Constant.Columns.Add(BlockComColumn.NameColumn.ColumnName, BlockComColumn.NameColumn.DataType);
			Constant.Columns.Add(BlockComColumn.TypeColumn.ColumnName, BlockComColumn.TypeColumn.DataType);
			Constant.Columns.Add(BlockComColumn.ValueColumn.ColumnName, BlockComColumn.ValueColumn.DataType);
			Constant.Columns.Add(BlockComColumn.NotesColumn.ColumnName, BlockComColumn.NotesColumn.DataType);
			// Constant does not need DefaultValueColumn
			// Constant does not need MaintainColumn
			Constant.PrimaryKey = new DataColumn[] { Constant.Columns[0] };

			BlockInterface.Tables.Add(Input);
			BlockInterface.Tables.Add(Output);
			BlockInterface.Tables.Add(InOut);
			BlockInterface.Tables.Add(Temp);
			BlockInterface.Tables.Add(Constant);
		}

		public virtual byte[] ToBinary()
		{
			MemoryStream memoryStream = new MemoryStream();
			BinaryWriter binaryWriter = new BinaryWriter(memoryStream, Encoding.UTF8, true);

			binaryWriter.Write(Name ?? string.Empty);
			binaryWriter.Write(Description ?? string.Empty);
			binaryWriter.Write(ExecuteCode ?? string.Empty);
			binaryWriter.Write(BlockInterface.Tables.Count);
			foreach (DataTable table in BlockInterface.Tables)
			{
				binaryWriter.Write(table.TableName);
				binaryWriter.Write(table.Columns.Count);
				foreach (DataColumn column in table.Columns)
				{
					binaryWriter.Write(column.ColumnName);
					binaryWriter.Write(column.DataType.AssemblyQualifiedName);
				}
				binaryWriter.Write(table.Rows.Count);
				foreach (DataRow row in table.Rows)
				{
					foreach (DataColumn column in table.Columns)
					{
						object value = row[column];
						if (value is null)
						{
							binaryWriter.Write(string.Empty);
						}
						else
						{
							binaryWriter.Write(value.ToString());
						}
					}
				}
			}

			binaryWriter.Close();
			return memoryStream.ToArray();
		}

		public static Function FromBinary(byte[] binary)
		{
			MemoryStream memoryStream = new MemoryStream(binary);
			BinaryReader binaryReader = new BinaryReader(memoryStream, Encoding.UTF8, true);

			Function block = new Function
			{
				Name = binaryReader.ReadString(),
				Description = binaryReader.ReadString(),
				ExecuteCode = binaryReader.ReadString(),
				BlockInterface = new DataSet("BlockInterface")
			};

			int tableCount = binaryReader.ReadInt32();
			for (int i = 0; i < tableCount; i++)
			{
				string tableName = binaryReader.ReadString();
				int columnCount = binaryReader.ReadInt32();
				DataTable table = new DataTable(tableName);
				for (int j = 0; j < columnCount; j++)
				{
					string columnName = binaryReader.ReadString();
					string columnType = binaryReader.ReadString();
					table.Columns.Add(columnName, Type.GetType(columnType));
				}
				int rowCount = binaryReader.ReadInt32();
				for (int k = 0; k < rowCount; k++)
				{
					DataRow row = table.NewRow();
					foreach (DataColumn column in table.Columns)
					{
						string value = binaryReader.ReadString();

						if (column.DataType == typeof(Type))
						{
							row[column] = Type.GetType(value);
							continue;
						}

						row[column] = value;
					}
					table.Rows.Add(row);
				}
				block.BlockInterface.Tables.Add(table);
			}

			binaryReader.Close();
			return block;
		}
	}
}
