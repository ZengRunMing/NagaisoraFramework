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

		public List<IECLItem> ECLItems;

		public ECLData()
		{
			SystemVersion = BitConverter.ToUInt32(new byte[] { 1, 0, 0, 0 }, 0);
			FileVersion = BitConverter.ToUInt32(new byte[] { 1, 0, 0, 0 }, 0);
			Author = "Unknown";
			ECLItems = new List<IECLItem>();
		}

		public static ECLData Create(string name = "Default", string description = "DefaultECL", string author = "Unknown")
		{
			ECLData data = new ECLData
			{
				Name = name,
				Description = description,
				Author = author,
				ECLItems = new List<IECLItem>(),
			};

			IECLItem item = new ECLMain
			{
				Name = "Main",
				Description = "ECL的程序入口",
			};
			item.Init();

			data.ECLItems.Add(item);

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

			binaryWriter.Write(ECLItems.Count); // 写入 ECLItems 的数量

			foreach (var item in ECLItems)
			{
				if (item is ECLMain)
				{
					binaryWriter.Write((byte)0); // 写入 ECLMain 类型标识
				}
				else if (item is ECLFunction)
				{
					binaryWriter.Write((byte)1); // 写入 ECLMain 类型标识
				}
				else if (item is ECLInterrupt)
				{
					binaryWriter.Write((byte)2); // 写入 ECLMain 类型标识
				}
				else
				{
					throw new InvalidDataException($"Unknown ECLItem type: {item.GetType().Name}");
				}

				binaryWriter.Write(item.Name ?? string.Empty); // 写入每个 ECLItem 的名称
				binaryWriter.Write(item.Description ?? string.Empty); // 写入每个 ECLItem 的描述
				binaryWriter.Write(item.ExecuteCode ?? string.Empty); // 写入每个 ECLItem 的执行代码
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

			int itemCount = binaryReader.ReadInt32(); // 读取 ECLItems 的数量

			data.ECLItems = new List<IECLItem>(itemCount); // 初始化 ECLItems 列表

			for (int i = 0; i < itemCount; i++)
			{
				byte itemType = binaryReader.ReadByte(); // 读取每个 ECLItem 的类型标识

				string itemName = binaryReader.ReadString(); // 读取每个 ECLItem 的名称
				string itemDescription = binaryReader.ReadString(); // 读取每个 ECLItem 的描述
				string itemExecuteCode = binaryReader.ReadString(); // 读取每个 ECLItem 的执行代码
				IECLItem item;

				switch (itemType)
				{
					case 0: // ECLMain
						item = new ECLMain
						{
							Name = itemName,
							Description = itemDescription,
							ExecuteCode = itemExecuteCode
						};
						break;
					case 1: // ECLFunction
						item = new ECLFunction
						{
							Name = itemName,
							Description = itemDescription,
							ExecuteCode = itemExecuteCode
						};
						break;
					case 2: // ECLInterrupt
						item = new ECLInterrupt
						{
							Name = itemName,
							Description = itemDescription,
							ExecuteCode = itemExecuteCode
						};
						break;
					default:
						throw new InvalidDataException($"Unknown ECLItem type: {itemType}");
				}

				data.ECLItems.Add(item); // 添加到 ECLItems 列表中
			}

			binaryReader.Close();
			memoryStream.Close();

			return data;
		}
	}
}
