using System;
using System.Data;
using System.IO;
using System.Text;

namespace NagaisoraFramework.STGSystem
{
	[Serializable]
	public class FunctionBlock : Function
	{
		public string DataBlockName;
		public DataBlock DataBlock;

		public virtual void Init(string dataBlockName)
		{
			Init(Controler.FindDataBlock(DataBlockName));
		}

		public virtual void Init(DataBlock dataBlock)
		{
			if (dataBlock is null)
			{
				throw new Exception($"{Name} => DataBlock is null.");
			}

			DataBlock = dataBlock;
			DataBlockName = dataBlock.Name;
			BlockInterface = DataBlock.BlockInterface;

			if (!BlockInterface.Tables.Contains("Input"))
			{
				DataTable Input = new DataTable("Input");
				Input.Columns.Add(BlockComColumn.NameColumn.ColumnName, BlockComColumn.NameColumn.DataType);
				Input.Columns.Add(BlockComColumn.TypeColumn.ColumnName, BlockComColumn.TypeColumn.DataType);
				Input.Columns.Add(BlockComColumn.ValueColumn.ColumnName, BlockComColumn.ValueColumn.DataType);
				Input.Columns.Add(BlockComColumn.DefaultValueColumn.ColumnName, BlockComColumn.DefaultValueColumn.DataType);
				Input.Columns.Add(BlockComColumn.MaintainColumn.ColumnName, BlockComColumn.MaintainColumn.DataType);
				Input.Columns.Add(BlockComColumn.NotesColumn.ColumnName, BlockComColumn.NotesColumn.DataType);
				Input.PrimaryKey = new DataColumn[] { Input.Columns[0] };
				DataBlock.BlockInterface.Tables.Add(Input);
			}
			if (!BlockInterface.Tables.Contains("Output"))
			{
				DataTable Output = new DataTable("Output");
				Output.Columns.Add(BlockComColumn.NameColumn.ColumnName, BlockComColumn.NameColumn.DataType);
				Output.Columns.Add(BlockComColumn.TypeColumn.ColumnName, BlockComColumn.TypeColumn.DataType);
				Output.Columns.Add(BlockComColumn.ValueColumn.ColumnName, BlockComColumn.ValueColumn.DataType);
				Output.Columns.Add(BlockComColumn.DefaultValueColumn.ColumnName, BlockComColumn.DefaultValueColumn.DataType);
				Output.Columns.Add(BlockComColumn.MaintainColumn.ColumnName, BlockComColumn.MaintainColumn.DataType);
				Output.Columns.Add(BlockComColumn.NotesColumn.ColumnName, BlockComColumn.NotesColumn.DataType);
				Output.PrimaryKey = new DataColumn[] { Output.Columns[0] };
				DataBlock.BlockInterface.Tables.Add(Output);
			}
			if (!BlockInterface.Tables.Contains("InOut"))
			{
				DataTable InOut = new DataTable("InOut");
				InOut.Columns.Add(BlockComColumn.NameColumn.ColumnName, BlockComColumn.NameColumn.DataType);
				InOut.Columns.Add(BlockComColumn.TypeColumn.ColumnName, BlockComColumn.TypeColumn.DataType);
				InOut.Columns.Add(BlockComColumn.ValueColumn.ColumnName, BlockComColumn.ValueColumn.DataType);
				InOut.Columns.Add(BlockComColumn.DefaultValueColumn.ColumnName, BlockComColumn.DefaultValueColumn.DataType);
				InOut.Columns.Add(BlockComColumn.MaintainColumn.ColumnName, BlockComColumn.MaintainColumn.DataType);
				InOut.Columns.Add(BlockComColumn.NotesColumn.ColumnName, BlockComColumn.NotesColumn.DataType);
				InOut.PrimaryKey = new DataColumn[] { InOut.Columns[0] };
				DataBlock.BlockInterface.Tables.Add(InOut);
			}
			if (!BlockInterface.Tables.Contains("Temp"))
			{
				DataTable Temp = new DataTable("Temp");
				Temp.Columns.Add(BlockComColumn.NameColumn.ColumnName, BlockComColumn.NameColumn.DataType);
				Temp.Columns.Add(BlockComColumn.TypeColumn.ColumnName, BlockComColumn.TypeColumn.DataType);
				Temp.Columns.Add(BlockComColumn.ValueColumn.ColumnName, BlockComColumn.ValueColumn.DataType);
				Temp.Columns.Add(BlockComColumn.DefaultValueColumn.ColumnName, BlockComColumn.DefaultValueColumn.DataType);
				Temp.Columns.Add(BlockComColumn.MaintainColumn.ColumnName, BlockComColumn.MaintainColumn.DataType);
				Temp.Columns.Add(BlockComColumn.NotesColumn.ColumnName, BlockComColumn.NotesColumn.DataType);
				Temp.PrimaryKey = new DataColumn[] { Temp.Columns[0] };
				DataBlock.BlockInterface.Tables.Add(Temp);
			}
			if (!BlockInterface.Tables.Contains("Constant"))
			{
				DataTable Constant = new DataTable("Constant");
				Constant.Columns.Add(BlockComColumn.NameColumn.ColumnName, BlockComColumn.NameColumn.DataType);
				Constant.Columns.Add(BlockComColumn.TypeColumn.ColumnName, BlockComColumn.TypeColumn.DataType);
				Constant.Columns.Add(BlockComColumn.ValueColumn.ColumnName, BlockComColumn.ValueColumn.DataType);
				Constant.Columns.Add(BlockComColumn.NotesColumn.ColumnName, BlockComColumn.NotesColumn.DataType);
				// Constant does not need DefaultValueColumn
				// Constant does not need MaintainColumn
				Constant.PrimaryKey = new DataColumn[] { Constant.Columns[0] };
				DataBlock.BlockInterface.Tables.Add(Constant);
			}
		}

		public override byte[] ToBinary()
		{
			MemoryStream memoryStream = new MemoryStream();
			BinaryWriter binaryWriter = new BinaryWriter(memoryStream, Encoding.UTF8, true);

			binaryWriter.Write(Name ?? string.Empty);
			binaryWriter.Write(Description ?? string.Empty);
			binaryWriter.Write(ExecuteCode ?? string.Empty);
			binaryWriter.Write(DataBlockName);

			binaryWriter.Close();
			return memoryStream.ToArray();
		}

		public new static FunctionBlock FromBinary(byte[] binary)
		{
			MemoryStream memoryStream = new MemoryStream(binary);
			BinaryReader binaryReader = new BinaryReader(memoryStream, Encoding.UTF8, true);

			FunctionBlock block = new FunctionBlock
			{
				Name = binaryReader.ReadString(),
				Description = binaryReader.ReadString(),
				ExecuteCode = binaryReader.ReadString(),
				DataBlockName = binaryReader.ReadString()
			};

			binaryReader.Close();
			return block;
		}
	}
}
