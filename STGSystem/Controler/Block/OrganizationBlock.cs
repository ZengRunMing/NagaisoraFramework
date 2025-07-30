using System;
using System.IO;
using System.Text;
using System.Data;

namespace NagaisoraFramework.STGSystem
{
	[Serializable]
	public class OrganizationBlock : ExecutableBlock
	{
		public override void Init()
		{
			BlockInterface = new DataSet("BlockInterface");

			DataTable Temp = new DataTable("Temp");
			Temp.Columns.Add(BlockComColumn.NameColumn.ColumnName, BlockComColumn.NameColumn.DataType);
			Temp.Columns.Add(BlockComColumn.TypeColumn.ColumnName, BlockComColumn.TypeColumn.DataType);
			Temp.Columns.Add(BlockComColumn.ValueColumn.ColumnName, BlockComColumn.ValueColumn.DataType);
			Temp.Columns.Add(BlockComColumn.DefaultValueColumn.ColumnName, BlockComColumn.DefaultValueColumn.DataType);
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

		public static OrganizationBlock FromBinary(byte[] binary)
		{
			MemoryStream memoryStream = new MemoryStream(binary);
			BinaryReader binaryReader = new BinaryReader(memoryStream, Encoding.UTF8, true);

			OrganizationBlock block = new OrganizationBlock
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

	public class CycleOrganizationBlock : OrganizationBlock
	{
		public new static CycleOrganizationBlock FromBinary(byte[] binary)
		{
			MemoryStream memoryStream = new MemoryStream(binary);
			BinaryReader binaryReader = new BinaryReader(memoryStream, Encoding.UTF8, true);

			CycleOrganizationBlock block = new CycleOrganizationBlock
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

	public class StartUpOrganizationBlock : OrganizationBlock
	{
		public new static StartUpOrganizationBlock FromBinary(byte[] binary)
		{
			MemoryStream memoryStream = new MemoryStream(binary);
			BinaryReader binaryReader = new BinaryReader(memoryStream, Encoding.UTF8, true);

			StartUpOrganizationBlock block = new StartUpOrganizationBlock
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

	public class CyclicInterruptOrganizationBlock : OrganizationBlock
	{
		public TimeSpan DelayTime;

		public override byte[] ToBinary()
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
			binaryWriter.Write(DelayTime.Ticks);

			binaryWriter.Close();
			return memoryStream.ToArray();
		}

		public new static CyclicInterruptOrganizationBlock FromBinary(byte[] binary)
		{
			MemoryStream memoryStream = new MemoryStream(binary);
			BinaryReader binaryReader = new BinaryReader(memoryStream, Encoding.UTF8, true);

			CyclicInterruptOrganizationBlock block = new CyclicInterruptOrganizationBlock
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
						row[column] = binaryReader.ReadString();
						continue;
					}
					table.Rows.Add(row);
				}
				block.BlockInterface.Tables.Add(table);
			}
			block.DelayTime = TimeSpan.FromTicks(binaryReader.ReadInt64());

			binaryReader.Close();
			return block;
		}
	}

	public class InterruptOrganizationBlock : OrganizationBlock
	{
		public string ConditionCode { get; set; }

		public override void Default()
		{
			base.Default();

			ConditionCode =
				$"#region {Name.Replace(' ', '_')}-ConditionMethodUsing\n" +
				$"//Write using code here\n" +
				$"\n" +
				"#endregion\n\n" +
				"public override bool Condition()\n" +
				"{\n" +
				"\t//Write interrupt condition code here\n" +
				"\t\n" +
				"\treturn false;\n" +
				"}\n";
		}

		public virtual bool Condition()
		{
			return false;
		}

		public override byte[] ToBinary()
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
			binaryWriter.Write(ConditionCode ?? string.Empty);

			binaryWriter.Close();
			return memoryStream.ToArray();
		}

		public new static InterruptOrganizationBlock FromBinary(byte[] binary)
		{
			MemoryStream memoryStream = new MemoryStream(binary);
			BinaryReader binaryReader = new BinaryReader(memoryStream, Encoding.UTF8, true);

			InterruptOrganizationBlock block = new InterruptOrganizationBlock
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
						row[column] = binaryReader.ReadString();
						continue;
					}
					table.Rows.Add(row);
				}
				block.BlockInterface.Tables.Add(table);
			}
			block.ConditionCode = binaryReader.ReadString();

			binaryReader.Close();
			return block;
		}
	}
}
