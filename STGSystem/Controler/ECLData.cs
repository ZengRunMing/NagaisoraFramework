using System.IO;
using System.Text;
using System.Collections.Generic;
using System;

namespace NagaisoraFramework.STGSystem
{
	public class ECLData
	{
		public static readonly string EditorHeader = "EDIECL";
		public static readonly string ExecutableHander = "EXTECL";

		public uint SystemVersion;
		public string Name;
		public string Description;
		public uint FileVersion;
		public string Author;

		public List<VariableData> Variables;

		public List<IBlock> Blocks;

		public ECLData()
		{
			SystemVersion = BitConverter.ToUInt32(new byte[] { 1, 0, 0, 0 }, 0);
			FileVersion = BitConverter.ToUInt32(new byte[] { 1, 0, 0, 0 }, 0);
			Author = "Unknown";
			Variables = new List<VariableData>();
			Blocks = new List<IBlock>();
		}

		public static ECLData Create(string name = "Default", string description = "DefaultECL", string author = "Unknown")
		{
			ECLData data = new ECLData
			{
				Name = name,
				Description = description,
				Author = author,
			};

			// 默认添加一个启动组织块和一个循环组织块
			StartUpOrganizationBlock startUpOrganizationBlock = new StartUpOrganizationBlock { Name = "Start Up", Description = "程序启动时执行的块" };
			CycleOrganizationBlock cycleOrganizationBlock = new CycleOrganizationBlock { Name = "Main Cycle", Description = "程序主循环执行的块" };

			startUpOrganizationBlock.Default();
			cycleOrganizationBlock.Default();

			startUpOrganizationBlock.Init();
			cycleOrganizationBlock.Init();

			data.Blocks.Add(startUpOrganizationBlock);
			data.Blocks.Add(cycleOrganizationBlock);

			return data;
		}

		public byte[] ToBinary()
		{
			MemoryStream memoryStream = new MemoryStream();
			BinaryWriter binaryWriter = new BinaryWriter(memoryStream, Encoding.UTF8, true);

			binaryWriter.Write(EditorHeader.ToCharArray());		// 文件头标识
			binaryWriter.Write(SystemVersion);					// 系统版本

			binaryWriter.Write(Name ?? string.Empty);			// 名称
			binaryWriter.Write(Description ?? string.Empty);	// 描述
			binaryWriter.Write(FileVersion);					// 文件版本
			binaryWriter.Write(Author ?? string.Empty);			// 作者署名

			binaryWriter.Write(Variables?.Count ?? 0);          // 变量数量

			// 遍历所有变量并将它们的二进制数据写入文件流
			foreach (var variable in Variables)                 
			{
				byte[] binary = variable.ToBinary();

				binaryWriter.Write(binary.Length);
				binaryWriter.Write(binary);
			}

			binaryWriter.Write(Blocks?.Count ?? 0);				// 写入块的数量

			foreach (IBlock block in Blocks)                    // 遍历所有块并将它们的二进制数据写入文件流
			{
				if (block is OrganizationBlock organizationBlock)
				{
					binaryWriter.Write((byte)0x00); // 组织块类型标识

					byte[] blockBinary;

					// 根据组织块的具体类型进行处理
					if (organizationBlock is StartUpOrganizationBlock startUpOrganizationBlock)
					{
						binaryWriter.Write((byte)0x00); // 启动组织块类型标识
						blockBinary = startUpOrganizationBlock.ToBinary(); // 获取启动块的二进制数据
					}
					else if (organizationBlock is CycleOrganizationBlock cycleOrganizationBlock)
					{
						binaryWriter.Write((byte)0x01); // 循环组织块类型标识
						blockBinary = cycleOrganizationBlock.ToBinary(); // 获取块的二进制数据
					}
					else if (organizationBlock is InterruptOrganizationBlock interruptOrganizationBlock)
					{
						binaryWriter.Write((byte)0x02); // 中断组织块类型标识
						blockBinary = interruptOrganizationBlock.ToBinary(); // 获取块的二进制数据
					}
					else if (organizationBlock is CyclicInterruptOrganizationBlock cyclicInterruptOrganizationBlock)
					{
						binaryWriter.Write((byte)0x03); // 周期中断组织块类型标识
						blockBinary = cyclicInterruptOrganizationBlock.ToBinary(); // 获取块的二进制数据
					}
					else
					{
						throw new InvalidDataException($"Unknown organization block type: {block.GetType()}");
					}

					binaryWriter.Write(blockBinary.Length); // 写入块二进制长度
					binaryWriter.Write(blockBinary); // 写入块二进制数据
				}
				else if (block is FunctionBlock functionBlock)
				{
					binaryWriter.Write((byte)0x01); // 函数块类型标识
					byte[] blockBinary = functionBlock.ToBinary(); // 获取块的二进制数据
					binaryWriter.Write(blockBinary.Length); // 写入块二进制长度
					binaryWriter.Write(blockBinary); // 写入块二进制数据
				}
				else if (block is Function function)
				{
					binaryWriter.Write((byte)0x02); // 函数类型标识
					byte[] blockBinary = function.ToBinary(); // 获取块的二进制数据
					binaryWriter.Write(blockBinary.Length); // 写入块二进制长度
					binaryWriter.Write(blockBinary); // 写入块二进制数据
				}
				else if (block is DataBlock dataBlock)
				{
					binaryWriter.Write((byte)0x03); // 数据块类型标识
					byte[] blockBinary = dataBlock.ToBinary(); // 获取块的二进制数据
					binaryWriter.Write(blockBinary.Length); // 写入块二进制长度
					binaryWriter.Write(blockBinary); // 写入块二进制数据
				}
				else
				{
					throw new InvalidDataException($"Unknown block type: {block.GetType()}");
				}
			}

			binaryWriter.Close(); // 关闭二进制写入器

			return memoryStream.ToArray(); // 返回完整的二进制数据
		}

		public static ECLData FromBinary(byte[] binary)
		{
			MemoryStream memoryStream = new MemoryStream(binary); // 创建内存流
			BinaryReader binaryReader = new BinaryReader(memoryStream, Encoding.UTF8, true); // 创建二进制读取器

			char[] header = binaryReader.ReadChars(EditorHeader.Length); // 读取文件头标识

			string headerString = new string(header);

			if (headerString != EditorHeader)
			{
				throw new InvalidDataException($"Invalid file header: {headerString}. Expected: {EditorHeader}"); // 检查文件头标识是否正确
			}

			ECLData data = new ECLData // 创建 ECLData 实例
			{
				SystemVersion = binaryReader.ReadUInt32(), // 读取系统版本
				Name = binaryReader.ReadString(), // 读取名称
				Description = binaryReader.ReadString(), // 读取描述
				FileVersion = binaryReader.ReadUInt32(), // 读取版本
				Author = binaryReader.ReadString(), // 读取作者署名
			};

			int variableCount = binaryReader.ReadInt32(); // 读取变量数量

			for (int i = 0; i < variableCount; i++)
			{
				int length = binaryReader.ReadInt32(); // 读取变量二进制长度
				byte[] variableBinary = binaryReader.ReadBytes(length); // 读取变量二进制数据
				VariableData variableData = new VariableData().FromBinary(variableBinary); // 从二进制数据创建变量数据
				data.Variables.Add(variableData); // 将变量数据添加到列表中
			}

			int blockCount = binaryReader.ReadInt32(); // 读取块的数量
			
			for(int i = 0; i < blockCount; i++)
			{
				IBlock block;

				byte blockType = binaryReader.ReadByte(); // 读取块类型
				
				int Length;

				switch (blockType)
				{
					case 0x00: // OrganizationBlock
						byte OrganizationBlockblockType = binaryReader.ReadByte(); // 读取组织块类型
						
						Length = binaryReader.ReadInt32(); // 读取块二进制长度
						byte[] OrganizationBlockBinary = binaryReader.ReadBytes(Length); // 读取组织块二进制数据

						switch (OrganizationBlockblockType)
						{
							case 0x00: // StartUpOrganizationBlock
								block = StartUpOrganizationBlock.FromBinary(OrganizationBlockBinary); // 从二进制数据创建启动组织块
								break;
							case 0x01: // CycleOrganizationBlock
								block = CycleOrganizationBlock.FromBinary(OrganizationBlockBinary); // 从二进制数据创建循环组织块
								break;
							case 0x02: // InterruptOrganizationBlock
								block = InterruptOrganizationBlock.FromBinary(OrganizationBlockBinary); // 从二进制数据创建中断组织块
								break;
							case 0x03: // CyclicInterruptOrganizationBlock
								block = CyclicInterruptOrganizationBlock.FromBinary(OrganizationBlockBinary); // 从二进制数据创建周期中断组织块
								break;
							default:
								throw new InvalidDataException($"Unknown organization block type: 0x{OrganizationBlockblockType:X2}");
						}
						break;
					case 0x01: // FunctionBlock
						Length = binaryReader.ReadInt32(); // 读取块二进制长度
						byte[] FunctionBlockBinary = binaryReader.ReadBytes(Length); // 读取函数块二进制数据

						block = FunctionBlock.FromBinary(FunctionBlockBinary); // 从二进制数据创建函数块
						break;
					case 0x02: // Function
						Length = binaryReader.ReadInt32(); // 读取块二进制长度
						byte[] FunctionBinary = binaryReader.ReadBytes(Length); // 读取函数二进制数据

						block = Function.FromBinary(FunctionBinary); // 从二进制数据创建函数
						break;
					case 0x03: // DataBlock
						Length = binaryReader.ReadInt32(); // 读取块二进制长度
						byte[] DataBlockBinary = binaryReader.ReadBytes(Length); // 读取数据块二进制数据

						block = DataBlock.FromBinary(DataBlockBinary); // 从二进制数据创建数据块
						break;
					default:
						throw new InvalidDataException($"Unknown block type: 0x{blockType:X2}");
				}

				data.Blocks.Add(block); // 将块添加到数据块列表中
			}

			binaryReader.Close();
			memoryStream.Close();

			return data;
		}
	}

	public class VariableData
	{
		public string Name;
		public string Type;
		public object Value;
		public string DefaultValue;
		public Accessibility Accessibility;

		public byte[] ToBinary()
		{
			MemoryStream memoryStream = new MemoryStream();
			BinaryWriter binaryWriter = new BinaryWriter(memoryStream, Encoding.UTF8, true);
			binaryWriter.Write(Name ?? string.Empty);
			binaryWriter.Write(Type ?? string.Empty);
			binaryWriter.Write(DefaultValue ?? string.Empty);
			binaryWriter.Write((byte)Accessibility); // 将Accessibility枚举转换为字节
			binaryWriter.Close();
			return memoryStream.ToArray();
		}

		public VariableData FromBinary(byte[] binary)
		{
			MemoryStream memoryStream = new MemoryStream(binary);
			BinaryReader binaryReader = new BinaryReader(memoryStream, Encoding.UTF8, true);
			VariableData variableData = new VariableData
			{
				Name = binaryReader.ReadString(),
				Type = binaryReader.ReadString(),
				DefaultValue = binaryReader.ReadString(),
				Accessibility = (Accessibility)binaryReader.ReadByte() // 将字节转换回Accessibility枚举
			};
			binaryReader.Close();
			memoryStream.Close();
			return variableData;
		}
	}

	public enum Accessibility
	{
		Public,
		Private,
		Protected,
		Internal,
		ProtectedInternal,
		PrivateProtected
	}
}
