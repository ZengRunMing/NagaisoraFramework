using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;

using NagaisoraFramework.Underlyingsystem;
using System.Diagnostics;
using System.Collections;

namespace NagaisoraFramework
{
	public class NRFSInfo : IDiskInfo
	{
		public override ulong StartSector { get; }
		public override ulong EndSector { get; }
		public override ulong SectorSize { get; }
		public override ulong DataLength { get; }
		public override byte[] SaveID { get; }

		public DateTime CreateTime;
		public DateTime SaveTime;
		public long SaveIndex;
		public bool Encrypted;
		public bool CanList;
		public bool CanExport;
		public bool CanSet;

		public NRFSInfo(ulong startSector, ulong endSector, ulong sectorSize, ulong dataLength, byte[] saveID, DateTime createTime, DateTime saveTime, long saveIndex, bool encrypted, bool canList, bool canExport, bool canSet)
		{
			StartSector = startSector;
			EndSector = endSector;
			SectorSize = sectorSize;
			DataLength = dataLength;
			SaveID = saveID;
			CreateTime = createTime;
			SaveTime = saveTime;
			SaveIndex = saveIndex;
			Encrypted = encrypted;
			CanList = canList;
			CanExport = canExport;
			CanSet = canSet;
		}

		public new static NRFSInfo FormBinary(byte[] vs)
		{
			MemoryStream memoryStream = new MemoryStream(vs);
			BinaryReader br = new BinaryReader(memoryStream);

			br.BaseStream.Position = 4;

			return new NRFSInfo(br.ReadUInt64(), br.ReadUInt64(), br.ReadUInt64(), br.ReadUInt64(), br.ReadBytes(16), DateTime.FromBinary(br.ReadInt64()),
						DateTime.FromBinary(br.ReadInt64()), br.ReadInt64(), br.ReadBoolean(), br.ReadBoolean(), br.ReadBoolean(), br.ReadBoolean());
		}

		public byte[] ToBinary()
		{
			return ToBinary(this);
		}

		public static byte[] ToBinary(NRFSInfo Info)
		{
			if (Info == null)
			{
				return null;
			}

			MemoryStream memory = new MemoryStream();
			BinaryWriter writer = new BinaryWriter(memory);

			writer.Write("NRFS".ToCharArray());

			writer.Write(Info.StartSector);
			writer.Write(Info.EndSector);
			writer.Write(Info.SectorSize);

			writer.Write(Info.DataLength);

			writer.Write(Info.SaveID);

			writer.Write(Info.CreateTime.ToBinary());
			writer.Write(Info.SaveTime.ToBinary());

			writer.Write(Info.SaveIndex);

			writer.Write(Info.Encrypted);
			writer.Write(Info.CanList);
			writer.Write(Info.CanExport);
			writer.Write(Info.CanSet);

			writer.Close();

			byte[] data = new byte[512];

			memory.ToArray().CopyTo(data, 0);

			return data;
		}
	}

	// 存储信息接口
	public interface IStoreInformation
	{

	}

	// 单扇区存储信息
	public class SingleSectorStorageInformation : IStoreInformation
	{
		public long Sector { get; set; } // 扇区索引
		public int Length { get; set; } // 数据长度

		public SingleSectorStorageInformation(long sector, int length)
		{
			Sector = sector;
			Length = length;
		}
	}

	// 连续扇区存储信息
	public class ContinuousSectorStorageInformation : IStoreInformation
	{
		public long StartSector { get; set; } // 起始扇区索引
		public long EndSector { get; set; } // 结束扇区索引

		public ContinuousSectorStorageInformation(long startSector, long endSector)
		{
			StartSector = startSector;
			EndSector = endSector;
		}
	}

	// 成组链接空闲扇区信息
	public class IdleSectorInfomation
	{
		public const string Header = "ISDR";

		public int IdleSectorCount => IdleSectors.Count;

		public Stack<long> IdleSectors;

		public bool IsLast = true;

		public IdleSectorInfomation()
		{
			IdleSectors = new Stack<long>(63);
		}

		public void PushIdleSector(long sectorIndex, NRFStream stream)
		{
			// 如果当前没有空闲扇区信息, 则需要判断压入的扇区是否保存了下一组的空闲扇区信息
			if (IdleSectorCount == 0)
			{
				stream.SetSector(sectorIndex);                      // 定位到该扇区
				byte[] binary = stream.DataReader.ReadBytes(512);   // 读取该扇区的二进制数据
				IdleSectorInfomation info = FromBinary(binary);     // 解析二进制数据为空闲扇区信息对象

				// 如果存在下一组空闲扇区信息，则将其设置为主空闲扇区信息
				if (!(info is null))
				{
					IsLast = false;
				}
			}

			// 如果空闲扇区数量未达到上限，则将当前信息压入栈中
			if (IdleSectorCount < 63)
			{
				IdleSectors.Push(sectorIndex);
				return;
			}

			// 如果空闲扇区数量达到上限，则将当前信息写入新加入的空闲扇区中，并新建空闲扇区信息到根信息
			stream.SetSector(sectorIndex);          // 定位到该扇区
			stream.DataWriter.Write(ToBinary());    // 将当前信息写入该扇区
			stream.DataWriter.Flush();              // 刷新写入器

			stream.RootIdleInfomation = new IdleSectorInfomation();         // 新建空闲扇区信息对象
			stream.RootIdleInfomation.PushIdleSector(sectorIndex, stream);  // 将该扇区索引压入新建的空闲扇区信息对象中
		}

		public long PopIdleSector(NRFStream stream)
		{
			// 如果有多个空闲扇区信息，直接弹出栈顶信息
			if (IdleSectorCount > 1)
			{
				return IdleSectors.Pop();
			}
			// 如果只有一个空闲扇区信息
			else if (IdleSectorCount == 1)
			{
				// 将当前信息读出
				long sector = IdleSectors.Pop();
				// 查询其是否保存了下一组的空闲扇区信息
				stream.SetSector(sector);                           // 定位到该扇区
				byte[] binary = stream.DataReader.ReadBytes(512);   // 读取该扇区的二进制数据
				IdleSectorInfomation info = FromBinary(binary);     // 解析二进制数据为空闲扇区信息对象

				// 如果存在下一组空闲扇区信息，则将其设置为主空闲扇区信息
				if (!(info is null))
				{
					stream.RootIdleInfomation = info;
				}

				return sector;
			}

			// 如果没有空闲扇区信息，则返回-1
			return -1;
		}

		public byte[] ToBinary()
		{
			MemoryStream memoryStream = new MemoryStream(new byte[512]);
			BinaryWriter binaryWriter = new BinaryWriter(memoryStream, Encoding.UTF8, true);

			binaryWriter.Write(Header.ToCharArray());	// 4字节标识
			binaryWriter.Write((short)IdleSectorCount);	// 2字节空闲扇区数量
			memoryStream.Seek(1, SeekOrigin.Current);   // 1字节保留
			binaryWriter.Write(IsLast);                 // 1字节是否为最后一组空闲扇区信息 (如果是最后一组，则后续没有空闲扇区信息)

			long[] sectors = IdleSectors.ToArray();     // 获取空闲扇区索引数组
			foreach (var sector in sectors)             // 写入每个空闲扇区索引
			{
				binaryWriter.Write(sector);             // 8字节空闲扇区索引
			}

			binaryWriter.Close();                       // 关闭二进制写入器

			return memoryStream.ToArray();	// 返回二进制数据
		}

		public static IdleSectorInfomation FromBinary(byte[] binary)
		{
			// 检查二进制数据长度是否为一个扇区的长度
			if (binary.Length != 512)
			{
				throw new ArgumentException("二进制数据的长度必须是一个扇区的长度");
			}

			// 新建空闲扇区信息对象
			IdleSectorInfomation infomation = new IdleSectorInfomation();

			MemoryStream memoryStream = new MemoryStream(binary);						// 新建内存流
			BinaryReader binaryReader = new BinaryReader(memoryStream, Encoding.UTF8);  // 新建二进制读取器

			string ReadedHeader = new string(binaryReader.ReadChars(4));                      // 读取4个字节的标识

			// 检查标识是否正确
			if (ReadedHeader != Header)
			{
				return null; // 标识不正确，返回空
			}

			int count = binaryReader.ReadInt16();						// 读取空闲扇区数量
			binaryReader.BaseStream.Seek(1, SeekOrigin.Current);        // 1字节保留
			infomation.IsLast = binaryReader.ReadBoolean();				// 读取是否为最后一组空闲扇区信息

			// 读取每个空闲扇区索引并压入栈中
			for (int i = 0; i < count; i++)
			{
				infomation.IdleSectors.Push(binaryReader.ReadInt64());  // 8字节空闲扇区索引
			}

			return infomation; // 返回空闲扇区信息对象
		}
	}

	// NRFS对象接口
	public interface INRFSObject : DiskSystemObject
	{
		string Name { get; set; }

		Guid ID { get; }

		DateTime CreateTime { get; }
		DateTime UpdateTime { get; }

		NRFolder Parent { get; }

		void NewUpdateTime();

		void SetParent(NRFolder parent);
	}

	// NRFS文件对象
	public class NRFile : INRFSObject
	{
		public string Name { get; set; }

		public Guid ID { get; }

		public DateTime CreateTime { get; private set; }
		public DateTime UpdateTime { get; private set; }

		public NRFolder Parent { get; private set; }

		public IStoreInformation[] StoreInformations { get; set; }

		public long Size
		{
			get
			{
				long size = 0;

				IStoreInformation[] storeInformations = StoreInformations;

				foreach (IStoreInformation storeInformation in storeInformations)
				{
					if (storeInformation == null)
					{
						continue;
					}

					if (storeInformation is SingleSectorStorageInformation singleSectorStorageInformation)
					{
						size += singleSectorStorageInformation.Length;
					}
					else if (storeInformation is ContinuousSectorStorageInformation continuousSectorStorageInformation)
					{
						long length = NRFStream.SectorSize * (continuousSectorStorageInformation.EndSector - continuousSectorStorageInformation.StartSector);

						size += length;
					}
				}

				return size;
			}
		}

		public NRFile(string name, IStoreInformation[] storeInformations) : this(name, Guid.NewGuid(), storeInformations)
		{

		}

		public NRFile(string name, Guid id, IStoreInformation[] storeInformations) : this(name, id, DateTime.Now, DateTime.Now, storeInformations)
		{

		}

		public NRFile(string name, Guid id, DateTime createTime, DateTime updateTime, IStoreInformation[] storeInformations)
		{
			Name = name;

			ID = id;

			CreateTime = createTime;
			UpdateTime = updateTime;

			StoreInformations = storeInformations;
		}

		public void NewUpdateTime()
		{
			UpdateTime = DateTime.Now;

			if (Parent is null)
			{
				return;
			}

			Parent.NewUpdateTime();
		}

		public void SetParent(NRFolder parent)
		{
			Parent = parent;
			Parent.NewUpdateTime();
		}

		public string GetFileSizeInfomation()
		{
			string FileSizeText = string.Empty;

			if (Size < 1024)
			{
				FileSizeText = $"{Size}B";
			}
			else if (Size < 1024 * 1024)
			{
				FileSizeText = $"{(Size / 1024f):0.00}KB";
			}
			else if (Size < 1024 * 1024 * 1024)
			{
				FileSizeText = $"{(Size / 1024f / 1024f):0.00}MB";
			}
			else if (Size < 1024 * 1024 * 1024 * 1024L)
			{
				FileSizeText = $"{(Size / 1024f / 1024f / 1024f):0.00}GB";
			}
			else
			{
				FileSizeText = $"{(Size / 1024f / 1024f / 1024f / 1024f):0.00}TB";
			}

			return FileSizeText;
		}
	}

	// NRFS文件夹
	public class NRFolder : INRFSObject
	{
		public string Name { get; set; }

		public Guid ID { get; }

		public DateTime CreateTime { get; private set; }
		public DateTime UpdateTime { get; private set; }

		public NRFolder Parent { get; private set; }

		public long SubItemCount
		{
			get
			{
				if (SubItems is null)
				{
					return 0;
				}

				return SubItems.Count;
			}
		}

		public List<INRFSObject> SubItems { get; set; }

		public NRFolder(string name, INRFSObject[] files = null) : this(name, Guid.NewGuid(), DateTime.Now, DateTime.Now, files)
		{

		}

		public NRFolder(string name, Guid id, INRFSObject[] files = null) : this(name, id, DateTime.Now, DateTime.Now, files)
		{

		}

		public NRFolder(string name, DateTime createtime, DateTime updatetime, INRFSObject[] files = null) : this(name, Guid.NewGuid(), createtime, updatetime, files)
		{

		}

		public NRFolder(string name, Guid id, DateTime createtime, DateTime updatetime, INRFSObject[] files = null)
		{
			Name = name;

			ID = id;

			CreateTime = createtime;
			UpdateTime = updatetime;

			if (files is null)
			{
				SubItems = new List<INRFSObject>();
				return;
			}

			SubItems = files.ToList();
		}

		public void NewUpdateTime()
		{
			UpdateTime = DateTime.Now;
			if (Parent is null)
			{
				return;
			}
			Parent.NewUpdateTime();
		}

		public void SetParent(NRFolder parent)
		{
			Parent = parent;
			Parent.NewUpdateTime();
		}
	}

	// NRFS文件系统读写流
	public class NRFStream : Stream, IDisposable
	{
		public const int SectorSize = 512;
		public ulong Version;
		public DateTime CreateTime;
		public Guid Guid;

		public Stream Stream;

		public BinaryWriter DataWriter;
		public BinaryReader DataReader;

		public NRFolder Root;

		public override bool CanRead => Stream.CanRead;

		public override bool CanSeek => Stream.CanSeek;

		public override bool CanWrite => Stream.CanWrite;

		public override long Length => Stream.Length;

		public override long Position
		{
			get
			{
				return Stream.Position;
			}

			set
			{
				Stream.Position = value;
			}
		}

		public long UnusedSectorsCount
		{
			get
			{
				return (Stream.Length / SectorSize) - DataSectorLength;
			}
		}

		public long IdleSectorsCount => GetIdleSectorCount(RootIdleInfomation); // 空闲扇区数量

		public long DataSectorLength; // 数据扇区长度

		public IdleSectorInfomation RootIdleInfomation; // 根空闲扇区信息

		public NRFStream(Stream stream)
		{
			Stream = stream; // 设置底层流

			DataWriter = new BinaryWriter(Stream, Encoding.UTF8, true); // 初始化二进制写入器
			DataReader = new BinaryReader(Stream, Encoding.UTF8, true); // 初始化二进制读取器

			ReadHeaderSector();
		}

		/*MUFS结构
		 * +---+---+---+-------+------------------+
		 * | 0 | 1 | 2 | 3 - 9 | 10 20 30 40 More |
		 * +---+---+---+-------+------------------+
		 *
		 *          Sector0 : 标识以及基本数据扇区
		 *          Sector1 : 空闲扇区根数据扇区
		 *          Sector2 : 文件信息根数据扇区
		 *        Sector3-9 : 日志信息
		 * Sector10 To More : 数据&动态标识扇区
		 */

		public void ReadHeaderSector()
		{
			read:

			SetSector(0); // 定位到0号扇区
			
			string Header = new string(DataReader.ReadChars(4)); // 读取4字节标识

			if (Header != "NRFS") // 如果标识不正确，则需要新建数据
			{
				WriteHeaderSector(this);
				goto read;
			}

			Stream.Seek(4, SeekOrigin.Current); // 跳过4字节保留

			Version = DataReader.ReadUInt64(); // 读取版本号
			Guid = new Guid(DataReader.ReadBytes(16)); // 读取GUID
			CreateTime = DateTime.FromBinary(DataReader.ReadInt64()); // 读取创建时间
			DataSectorLength = DataReader.ReadInt64(); // 读取数据扇区长度



			SetSector(1); // 定位到1号扇区 (超级扇区)
			byte[] binary = DataReader.ReadBytes(512); // 读取超级扇区的二进制数据
			RootIdleInfomation = IdleSectorInfomation.FromBinary(binary); // 解析二进制数据为空闲扇区信息对象

		}

		public static void WriteHeaderSector(NRFStream stream)
		{

		}

		public NRFile AddFile(NRFolder folder, FileStream data)
		{
			string name = Path.GetFileName(data.Name);
			data.Position = 0;

			BinaryReader binaryReader = new BinaryReader(data, Encoding.UTF8);

			IStoreInformation[] storeInformations = AllocateNewStorageAddress(data.Length);

			NRFile file = new NRFile(name, storeInformations);

			folder.SubItems.Add(file);

			foreach (IStoreInformation storeInfo in file.StoreInformations)
			{
				if (storeInfo is SingleSectorStorageInformation singleSectorStorageInformation)
				{
					SetSector(singleSectorStorageInformation.Sector);

					byte[] binary = binaryReader.ReadBytes(singleSectorStorageInformation.Length);

					DataWriter.Write(binary);
				}
				else if (storeInfo is ContinuousSectorStorageInformation continuousSectorStorageInformation)
				{
					SetSector(continuousSectorStorageInformation.StartSector);

					long SectorCount = continuousSectorStorageInformation.EndSector - continuousSectorStorageInformation.StartSector;
					long Length = SectorCount * SectorSize;

					int buffersize = 1048576;

					long index = 0;
					long count = Length / buffersize;
					int remain = (int)(Length % buffersize);

					while (index < count)
					{
						DataWriter.Write(binaryReader.ReadBytes(buffersize));
						index++;
					}

					DataWriter.Write(binaryReader.ReadBytes(remain));

				}
				else
				{
					throw new InvalidDataException("不支持的存储信息类型");
				}
			}

			DataWriter.Flush();
			Stream.Flush();

			binaryReader.Close();

			return file;
		}

		public NRFile[] AddFileRange(NRFolder folder, params FileStream[] datas)
		{
			List<NRFile> files = new List<NRFile>();

			foreach (FileStream data in datas)
			{
				files.Add(AddFile(folder, data));
			}

			return files.ToArray();
		}

		public void RemoveFile(NRFolder folder, params NRFile[] files)
		{
			foreach (NRFile file in files)
			{
				foreach (IStoreInformation infomation in file.StoreInformations)
				{
					if (infomation is SingleSectorStorageInformation singleSectorStorageInformation)
					{
						RootIdleInfomation.PushIdleSector(singleSectorStorageInformation.Sector, this);
					}
					else if (infomation is ContinuousSectorStorageInformation continuousSectorStorageInformation)
					{
						for (long i = continuousSectorStorageInformation.StartSector; i < continuousSectorStorageInformation.EndSector + 1; i++)
						{
							RootIdleInfomation.PushIdleSector(i, this);
						}
					}
					else
					{
						throw new InvalidDataException("不支持的存储信息类型");
					}
				}

				folder.SubItems.Remove(file);
			}
		}

		public NRFolder AddFolder(NRFolder folder, string name)
		{
			NRFolder Folder = new NRFolder(name);
			
			folder.SubItems.Add(Folder);

			return Folder;
		}

		public NRFolder[] AddFolderRange(NRFolder folder, params string[] names)
		{
			List<NRFolder> folders = new List<NRFolder>();

			foreach (string foldername in names)
			{
				folders.Add(AddFolder(folder, foldername));
			}

			return folders.ToArray();
		}

		public void RemoveFolder(NRFolder folder, params NRFolder[] folders)
		{
			foreach (NRFolder folder1 in folders)
			{
				folder.SubItems.Remove(folder1);
			}
		}

		//public MemoryMappedFile GetFileMapped(NRFile file, string name)
		//{

		//}

		public override void Close()
		{
			DataWriter.Close();
			DataReader.Close();
			Stream.Close();
			base.Close();

			Dispose();
		}

		public new void Dispose()
		{
			Stream.Dispose();
		}

		public override void Flush()
		{
			Stream.Flush();
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			return Stream.Seek(offset, origin);
		}

		public override void SetLength(long value)
		{
			Stream.SetLength(value);
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			return Stream.Read(buffer, offset, count);
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			Stream.Write(buffer, offset, count);
		}

		public static byte[] NRFSHeaderToBinary(INRFSObject obj)
		{
			MemoryStream memoryStream = new MemoryStream();
			BinaryWriter bw = new BinaryWriter(memoryStream, Encoding.UTF8, true);

			bw.Write(obj.Name);
			bw.Write(obj.ID.ToByteArray());
			bw.Write(obj.CreateTime.ToBinary());
			bw.Write(obj.UpdateTime.ToBinary());

			if (obj is NRFolder)
			{
				NRFolder folder = obj as NRFolder;

				bw.Write((byte)0);
				bw.Write(folder.SubItemCount);

				foreach (INRFSObject objt in folder.SubItems)
				{
					bw.Write(NRFSHeaderToBinary(objt));
				}
			}
			else
			{
				NRFile file = obj as NRFile;

				bw.Write((byte)1);
				bw.Write(file.StoreInformations.Length);
				foreach (IStoreInformation information in file.StoreInformations)
				{
					if (information is SingleSectorStorageInformation singleSectorStorageInformation)
					{
						bw.Write((byte)0); // 标记为单扇区存储信息
						bw.Write(singleSectorStorageInformation.Sector);
						bw.Write(singleSectorStorageInformation.Length);
					}
					else if (information is ContinuousSectorStorageInformation continuousSectorStorageInformation)
					{
						bw.Write((byte)1); // 标记为连续扇区存储信息
						bw.Write(continuousSectorStorageInformation.StartSector);
						bw.Write(continuousSectorStorageInformation.EndSector);
					}
					else
					{
						throw new InvalidDataException("不支持的存储信息类型");
					}
				}
			}

			bw.Close();

			return memoryStream.ToArray();
		}

		public static INRFSObject BinaryToNRFSHeader(byte[] binary)
		{
			MemoryStream stream = new MemoryStream(binary);
			INRFSObject @object = BinaryToNRFSHeader(stream);
			stream.Close();

			return @object;
		}

		public static INRFSObject BinaryToNRFSHeader(Stream stream)
		{
			BinaryReader reader = new BinaryReader(stream, Encoding.UTF8, true);
			INRFSObject @object = BinaryToNRFSHeader(reader);
			reader.Close();

			return @object;
		}

		public static INRFSObject BinaryToNRFSHeader(BinaryReader br)
		{
			INRFSObject obj = null;

			string name = br.ReadString();
			Guid id = new Guid(br.ReadBytes(16));
			DateTime createtime = DateTime.FromBinary(br.ReadInt64());
			DateTime updatetime = DateTime.FromBinary(br.ReadInt64());

			byte type = br.ReadByte();

			if (type == 0)
			{
				List<INRFSObject> objs = new List<INRFSObject>();

				long count = br.ReadInt64();

				long index = 0;

				while (index < count)
				{
					objs.Add(BinaryToNRFSHeader(br));
					index++;
				}

				obj = new NRFolder(name, id, createtime, updatetime, objs.ToArray());
			}
			else if (type == 1)
			{
				int storeCount = br.ReadInt32();
				IStoreInformation[] storeInformations = new IStoreInformation[storeCount];
				for (int i = 0; i < storeCount; i++)
				{
					byte storeType = br.ReadByte();
					if (storeType == 0)
					{
						long sector = br.ReadInt64();
						int length = br.ReadInt32();

						storeInformations[i] = new SingleSectorStorageInformation(sector, length);
					}
					else if (storeType == 1)
					{
						long startSector = br.ReadInt64();
						long endSector = br.ReadInt64();

						storeInformations[i] = new ContinuousSectorStorageInformation(startSector, endSector);
					}
					else
					{
						throw new InvalidDataException("不支持的存储信息类型");
					}
				}

				obj = new NRFile(name, id, storeInformations);
			}
			else
			{
				throw new InvalidDataException("不正确的数据类型");
			}

			return obj;
		}

		public static (long TotalLength, long FileLength, long FolderLength) GetNRFileTree(INRFSObject[] files)
		{
			if (files == null || files.Length == 0)
			{
				return (0, 0, 0);
			}

			long Totallength = 0;
			long FileLength = 0;
			long FolderLength = 0;

			for (int i = 0; i < files.Length; i++)
			{
				if (files[i] is NRFolder)
				{
					NRFolder folder = files[i] as NRFolder;

					if (folder.SubItems != null && folder.SubItemCount != 0)
					{
						(long, long, long) IMT = GetNRFileTree(folder.SubItems.ToArray());

						Totallength += IMT.Item1;
						FileLength += IMT.Item2;
						FolderLength += IMT.Item3;
					}

					FolderLength++;
				}
				else
				{
					FileLength++;
				}

				Totallength++;
			}

			return (Totallength, FileLength, FolderLength);
		}

		public NRFile[] ListALLFiles()
		{
			return TraverseFiles(Root);
		}

		public NRFile[] TraverseFiles(NRFolder folder)
		{
			List<NRFile> files = new List<NRFile>();

			INRFSObject[] objects = folder.SubItems.ToArray();
			foreach (INRFSObject obj in objects)
			{
				if (obj is NRFolder)
				{
					files.AddRange(TraverseFiles((NRFolder)obj));
					continue;
				}

				files.Add(obj as NRFile);
			}

			return files.ToArray();
		}

		/// <summary>
		///	查找指定数量的使用过但已空闲的扇区
		/// </summary> 
		/// <returns>使用过但已空闲的扇区索引数组</returns>
		public (long[], long) AllocateIdleSectors(long SectorCount)
		{
			List<long> idleSectors = new List<long>(); // 新建空闲扇区列表

			for (long i = SectorCount; i > 0 ; i--)
			{
				long sector = RootIdleInfomation.PopIdleSector(this); // 弹出一个空闲扇区索引
				
				if (sector == -1)
				{
					return (idleSectors.ToArray(), i); // 返回空闲扇区索引数组以及是否分配完
				}

				idleSectors.Add(sector); // 将空闲扇区索引添加到列表中
			}

			return (idleSectors.ToArray(), 0); // 返回空闲扇区索引数组以及是否分配完
		}

		public long GetIdleSectorCount(IdleSectorInfomation infomation)
		{
			long count = infomation.IdleSectorCount; // 获取当前组空闲扇区数量

			// 如果不是最后一组空闲扇区信息，则递归计算下一组空闲扇区数量
			if (!infomation.IsLast)
			{
				long[] sectors = infomation.IdleSectors.ToArray();                          // 获取当前空闲扇区索引数组
				SetSector(sectors[sectors.Length - 1]);                                     // 定位到堆栈中最底下的空闲扇区
				byte[] binary = DataReader.ReadBytes(512);                                  // 读取该扇区的二进制数据
				IdleSectorInfomation nextinfo = IdleSectorInfomation.FromBinary(binary);    // 解析二进制数据为空闲扇区信息对象

				count += GetIdleSectorCount(nextinfo);                                      // 递归计算下一组空闲扇区数量
			}

			return count; // 返回空闲扇区数量
		}

		public long GetRequiredSectorsCount(long size)
		{
			long sectorsCount = size / SectorSize; // 计算数据存储所需的扇区数量
			long requiredSectorsCount = (size % SectorSize == 0) ? sectorsCount : sectorsCount + 1; // 如果数据整数分配扇区后有剩余数据，则需要额外的一个扇区存储剩余数据

			return requiredSectorsCount; // 返回所需的扇区数量
		}

		/// <summary>
		/// 分配新的存储地址
		/// 使用未使用或使用过但已空闲的扇区根据数据大小进行分配
		/// </summary>
		/// <param name="size">需要分配的数据大小</param>
		/// <returns>存储分配数据</returns>
		public IStoreInformation[] AllocateNewStorageAddress(long size)
		{
			List<IStoreInformation> storeInformations = new List<IStoreInformation>(); // 新建存储分配信息列表

			long filesize = size; // 内部设定文件大小

			long RequiredSectorsCount = GetRequiredSectorsCount(filesize); // 计算所需的扇区数量
			(long[] IdleSectors, long ExpandSectorCount) = AllocateIdleSectors(RequiredSectorsCount); // 分配空闲的扇区和需要扩展的扇区数量

			Array.Sort(IdleSectors); // 对空闲扇区进行升序排序，优先使用前面的扇区

			// 如果需要拓展扇区，则根据实际情况处理
			if (ExpandSectorCount > 0)
			{
				if (Stream is FileStream)
				{
					// 如果是文件流，则扩展文件长度
					Stream.SetLength(Stream.Length + (SectorSize * ExpandSectorCount)); // 扩展文件长度
					Stream.Flush(); // 刷新文件流
				}
				else if (Stream is DiskStream diskStream)
				{
					// 如果是物理硬盘流，则不支持扩展，计算还未使用过的扇区数量决定是否可以存储数据
					long UnusedSectorsCount = (diskStream.Length / SectorSize) - DataSectorLength; // 获取未使用的扇区数量

					if (UnusedSectorsCount < ExpandSectorCount)
					{
						throw new NotSupportedException($"磁盘\"{diskStream.DiskInfo.Name}\"的剩余空间不足以存放该文件");
					}
				}
			}

			// 使用空闲扇区分配存储信息
			long ContinueSectoCount = 0; // 连续扇区起始索引
			long LastSector = 0; // 上一个扇区索引

			foreach (long sector in IdleSectors)
			{
				res:
				// 如果文件大小已经分配完毕
				if (filesize <= 0)
				{
					// 如果前面有连续扇区，则先分配连续扇区存储信息
					if (ContinueSectoCount > 0)
					{
						storeInformations.Add(new ContinuousSectorStorageInformation(LastSector - ContinueSectoCount, LastSector)); // 分配连续扇区存储信息
						ContinueSectoCount = 0; // 重置连续扇区数量
					}

					break; // 跳出循环
				}

				if (filesize < SectorSize)
				{
					// 如果前面有连续扇区，则先分配连续扇区存储信息
					if (ContinueSectoCount > 0)
					{
						storeInformations.Add(new ContinuousSectorStorageInformation(LastSector - ContinueSectoCount, LastSector)); // 分配连续扇区存储信息
						ContinueSectoCount = 0; // 重置连续扇区数量
					}

					// 分配单扇区存储信息
					storeInformations.Add(new SingleSectorStorageInformation(sector, (int)filesize));
					filesize = 0; // 文件大小分配完毕
				}

				if (sector == 0)
				{
					LastSector = sector; // 存入扇区索引
					filesize -= SectorSize; // 减少文件大小
					continue; // 跳过0号扇区
				}

				if (sector - LastSector == 1)
				{
					ContinueSectoCount++; // 增加连续扇区数量
					LastSector = sector; // 存入扇区索引
					filesize -= SectorSize; // 减少文件大小
					continue; // 如果是连续扇区，则跳过分配
				}

				// 如果不是连续扇区
				// 如果前面有连续扇区，则先分配连续扇区存储信息
				if (ContinueSectoCount > 0)
				{
					storeInformations.Add(new ContinuousSectorStorageInformation(LastSector - ContinueSectoCount, LastSector)); // 分配连续扇区存储信息
					ContinueSectoCount = 0; // 重置连续扇区数量
				}

				goto res; // 重新判断当前扇区
			}

			// 如果分配已经完成
			if (filesize <= 0)
			{
				goto ret; // 跳转到返回
			}

			// 如果还需要分配信息
			long sectorcount = filesize / SectorSize; // 计算还需要分配的扇区数量
			int remian = (int)(filesize % SectorSize); // 计算还需要分配的剩余数据大小

			// 如果需要分配连续存储信息
			if (sectorcount > 0)
			{
				storeInformations.Add(new ContinuousSectorStorageInformation(DataSectorLength, DataSectorLength + sectorcount - 1)); // 分配连续扇区存储信息
				DataSectorLength += sectorcount; // 增加数据扇区长度
			}

			// 如果需要分配单扇区存储信息
			if (remian > 0)
			{
				storeInformations.Add(new SingleSectorStorageInformation(DataSectorLength, remian)); // 分配单扇区存储信息
				DataSectorLength++; // 增加数据扇区长度
			}

			ret:
			return storeInformations.ToArray();
		}

		public void SetSector(long SectorIndex)
		{
			long position = SectorIndex * SectorSize;

			if (position < 0 || position + SectorSize > Stream.Length)
			{
				throw new ArgumentOutOfRangeException(nameof(SectorIndex), "扇区索引超出范围");
			}

			Stream.Position = position;
		}
	}

	public static class FileCom
	{
		public static (string Path, string Name, string Extension) getFileName(string path)
		{
			string FilePath = Path.GetDirectoryName(path); //文件路径
			string FileName = Path.GetFileName(path); //全文件名
			string LSRCFileName = Path.GetExtension(path); //拓展名

			return (FilePath, FileName, LSRCFileName);
		}
	}
}
