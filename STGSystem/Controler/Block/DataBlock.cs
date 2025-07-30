using System;
using System.Data;
using System.IO;
using System.Text;

namespace NagaisoraFramework.STGSystem
{
	[Serializable]
	public class DataBlock : IBlock
	{
		public string Name { get; set; }
		public string Description { get; set; }

		public DataSet BlockInterface { get; set; }

		public BlockControler Controler { get; set; }

		public virtual void Init()
		{
			BlockInterface = new DataSet("BlockInterface");

			DataTable Static = new DataTable("Static");
			Static.Columns.Add(BlockComColumn.NameColumn.ColumnName, BlockComColumn.NameColumn.DataType);
			Static.Columns.Add(BlockComColumn.TypeColumn.ColumnName, BlockComColumn.TypeColumn.DataType);
			Static.Columns.Add(BlockComColumn.ValueColumn.ColumnName, BlockComColumn.ValueColumn.DataType);
			Static.Columns.Add(BlockComColumn.DefaultValueColumn.ColumnName, BlockComColumn.DefaultValueColumn.DataType);
			Static.Columns.Add(BlockComColumn.MaintainColumn.ColumnName, BlockComColumn.MaintainColumn.DataType);
			Static.Columns.Add(BlockComColumn.NotesColumn.ColumnName, BlockComColumn.NotesColumn.DataType);
			Static.PrimaryKey = new DataColumn[] { Static.Columns[0] };

			BlockInterface.Tables.Add(Static);
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

		public virtual byte[] ToBinary()
		{
			MemoryStream memoryStream = new MemoryStream();
			BinaryWriter binaryWriter = new BinaryWriter(memoryStream, Encoding.UTF8, true);

			binaryWriter.Write(Name ?? string.Empty);
			binaryWriter.Write(Description ?? string.Empty);
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

		public static DataBlock FromBinary(byte[] binary)
		{
			MemoryStream memoryStream = new MemoryStream(binary);
			BinaryReader binaryReader = new BinaryReader(memoryStream, Encoding.UTF8, true);

			DataBlock block = new DataBlock
			{
				Name = binaryReader.ReadString(),
				Description = binaryReader.ReadString(),
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
