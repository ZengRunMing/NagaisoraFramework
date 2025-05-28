using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
using System.ComponentModel;
using System.Linq;

/*
 * 长空STG架构
 * 自定文件系统
 * By 万影 星梦
 * 魔晶自动化机电工程社团
 * 保留基本的著作权利
*/

namespace NagaisoraFamework
{

	public interface TRFileSystemObj : DiskSystemObj
	{
		string Name { get; set; }

		string FullName { get; set; }

		DateTime CreateTime { get; }
		DateTime SaveTime { get; set; }

		Guid Guid { get; }
	}

	public class TRFSInfo : IDiskInfo
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

		public TRFSInfo(ulong startSector, ulong endSector, ulong sectorSize, ulong dataLength, byte[] saveID, DateTime createTime, DateTime saveTime, long saveIndex, bool encrypted, bool canList, bool canExport, bool canSet)
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

		public new static TRFSInfo FormBinary(byte[] vs)
		{
			MemoryStream memoryStream = new MemoryStream(vs);
			BinaryReader br = new BinaryReader(memoryStream);

			br.BaseStream.Position = 4;

			return new TRFSInfo(br.ReadUInt64(), br.ReadUInt64(), br.ReadUInt64(), br.ReadUInt64(), br.ReadBytes(16), DateTime.FromBinary(br.ReadInt64()),
						DateTime.FromBinary(br.ReadInt64()), br.ReadInt64(), br.ReadBoolean(), br.ReadBoolean(), br.ReadBoolean(), br.ReadBoolean());
		}

		public byte[] ToBinary()
		{
			return ToBinary(this);
		}

		public static byte[] ToBinary(TRFSInfo Info)
		{
			if (Info == null)
			{
				return null;
			}

			MemoryStream memory = new MemoryStream();
			BinaryWriter writer = new BinaryWriter(memory);

			writer.Write(Encoding.UTF8.GetBytes("TRFS"));

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

	public class TRFile : TRFileSystemObj
	{
		[CategoryAttribute("基本信息"), DescriptionAttribute("名称")]
		public string Name { get; set; }

		[Browsable(false)]
		public string FullName { get; set; }

		[CategoryAttribute("基本信息"), DescriptionAttribute("创建时间")]
		public DateTime CreateTime { get; }
		[CategoryAttribute("基本信息"), DescriptionAttribute("最后修改时间")]
		public DateTime SaveTime { get; set; }

		[CategoryAttribute("底层信息"), DescriptionAttribute("自动分配底层信息")]
		public bool Auto { get; set; }

		[CategoryAttribute("底层信息"), DescriptionAttribute("文件映像地址")]
		public ulong Position { get; set; }
		[CategoryAttribute("底层信息"), DescriptionAttribute("文件大小 (byte)")]
		public ulong Size { get; }

		public Guid Guid { get; private set; }

		public TRFile(string name, string fullname, DateTime createtime, DateTime savetime, ulong position = 0, ulong size = 0, bool auto = true) : this(name, fullname, createtime, savetime, Guid.Empty, position, size, auto)
		{

		}

		public TRFile(string name, string fullname, DateTime createtime, DateTime savetime, Guid guid, ulong position = 0, ulong size = 0, bool auto = true)
		{
			Name = name;
			FullName = fullname;
			CreateTime = createtime;
			SaveTime = savetime;

			Guid = guid;

			if (Guid == Guid.Empty)
			{
				Guid = Guid.NewGuid();
			}

			Position = position;
			Size = size;

			Auto = auto;
		}

		public void SetSaveTime(DateTime dateTime)
		{
			SaveTime = dateTime;
		}
	}

	public class TRFolder : TRFileSystemObj
	{
		[CategoryAttribute("基本信息"), DescriptionAttribute("名称")]
		public string Name { get; set; }

		[Browsable(false)]
		public string FullName { get; set; }

		[CategoryAttribute("基本信息"), DescriptionAttribute("创建时间")]
		public DateTime CreateTime { get; }
		[CategoryAttribute("基本信息"), DescriptionAttribute("最后修改时间")]
		public DateTime SaveTime { get; set; }

		[CategoryAttribute("基本信息"), DescriptionAttribute("包含项目")]
		public long SubItemCount
		{
			get
			{
				if (SubFiles is null)
				{
					return 0;
				}

				return SubFiles.Count;
			}
		}

		public List<TRFileSystemObj> SubFiles { get; set; }

		public Guid Guid { get; private set; }

		public TRFolder(string name, string fullname, DateTime createtime, DateTime savetime, TRFileSystemObj[] files = null) : this(name, fullname, createtime, savetime, Guid.Empty, files)
		{

		}

		public TRFolder(string name, string fullname, DateTime createtime, DateTime savetime, Guid guid, TRFileSystemObj[] files = null)
		{
			Name = name;
			FullName = fullname;
			FullName = fullname;
			CreateTime = createtime;
			SaveTime = savetime;

			Guid = guid;

			if (Guid == Guid.Empty)
			{
				Guid = Guid.NewGuid();
			}

			if (files is null)
			{
				SubFiles = new List<TRFileSystemObj>();
				return;
			}

			SubFiles = files.ToList();
		}

		public void SetSaveTime(DateTime dateTime)
		{
			SaveTime = dateTime;
		}
	}

	public class TRFileStream : Stream, IDisposable
	{
		public string TempFile;

		public FileStream datamemory;
		public BinaryWriter dw;
		public BinaryReader dr;

		public TRFolder MainFolder;

		public override bool CanRead => datamemory.CanRead;

		public override bool CanSeek => datamemory.CanSeek;

		public override bool CanWrite => datamemory.CanWrite;

		public override long Length => datamemory.Length;

		public override long Position { get => datamemory.Position; set => datamemory.Position = value; }

		public TRFileStream() : this(Path.GetTempFileName())
		{

		}

		public TRFileStream(string tempfile)
		{
			TempFile = tempfile;

			datamemory = new FileStream(TempFile, FileMode.OpenOrCreate, FileAccess.ReadWrite);

			dw = new BinaryWriter(datamemory, Encoding.UTF8, true);
			dr = new BinaryReader(datamemory, Encoding.UTF8, true);

			if (MainFolder is null)
			{
				MainFolder = new TRFolder("Disk", ".\\Disk", DateTime.Now, DateTime.Now);
			}
		}

		public TRFile[] AddFile(TRFolder folder, params FileStream[] datas)
		{
			List<TRFile> files = new List<TRFile>();

			foreach (FileStream stream in datas)
			{
				string name = Path.GetFileName(stream.Name);

				files.Add(new TRFile(name, $"{folder.FullName}\\{name}", DateTime.Now, DateTime.Now, (ulong)datamemory.Position, (ulong)stream.Length));

				BinaryReader binaryReader = new BinaryReader(stream, Encoding.UTF8);

				stream.Position = 0;

				int buffersize = int.MaxValue - 56;

				ulong index = 0;
				ulong count = (ulong)(stream.Length / buffersize);
				int remain = (int)(stream.Length % buffersize);

				while (index < count)
				{
					dw.Write(binaryReader.ReadBytes(buffersize));
					index++;
				}

				dw.Write(binaryReader.ReadBytes(remain));

				binaryReader.Close();
			}
			dw.Flush();
			datamemory.Flush();

			folder.SubFiles.AddRange(files);

			return files.ToArray();
		}
		public void RemoveFile(TRFolder folder, params TRFile[] files)
		{
			foreach (TRFile file in files)
			{
				folder.SubFiles.Remove(file);

				datamemory.Position = (long)file.Position;

				int buffersize = int.MaxValue - 56;

				ulong index = 0;
				ulong count = file.Size / (ulong)buffersize;
				int remain = (int)(file.Size % (ulong)buffersize);

				while (index < count)
				{
					dw.Write(new byte[buffersize]);
					index++;
				}

				dw.Write(new byte[remain]);
			}

			dw.Flush();
			datamemory.Flush();
		}

		public TRFolder[] AddFolder(TRFolder folder, params string[] names)
		{
			List<TRFolder> folders = new List<TRFolder>();

			foreach (string foldername in names)
			{
				folders.Add(new TRFolder(foldername, $"{folder.FullName}\\{foldername}", DateTime.Now, DateTime.Now, null));
			}

			folder.SubFiles.AddRange(folders);

			return folders.ToArray();
		}

		public void RemoveFolder(TRFolder folder, params TRFolder[] folders)
		{
			foreach (TRFolder folder1 in folders)
			{
				folder.SubFiles.Remove(folder1);
			}
		}

		public void ToStream(Stream stream)
		{
			if (MainFolder is null || stream is null)
			{
				return;
			}

			BinaryWriter odw = new BinaryWriter(stream, Encoding.UTF8, true);

			odw.Write("TRFS".ToCharArray());
			odw.Write(TRFSHeaderToBinary(MainFolder));

			datamemory.Position = 0;

			int buffersize = int.MaxValue - 56;

			ulong index = 0;
			ulong count = (ulong)(datamemory.Length / buffersize);
			int remain = (int)(datamemory.Length % buffersize);

			while (index < count)
			{
				odw.Write(dr.ReadBytes(buffersize));
				index++;
			}

			odw.Write(dr.ReadBytes(remain));

			odw.Close();

			stream.Flush();
		}

		public static TRFileStream FromStream(Stream stream)
		{
			return FromStream(stream, Path.GetTempFileName());
		}

		public static TRFileStream FromStream(Stream stream, string datapath)
		{
			if (stream == null)
			{
				return null;
			}

			byte[] hander = new byte[4];

			stream.Read(hander, 0, hander.Length);

			string handerstring = Encoding.UTF8.GetString(hander);

			if (handerstring != "TRFS")
			{
				throw new InvalidDataException("文件头不正确");
			}

			TRFolder folder = BinaryToTRFSHeader(stream) as TRFolder;

			TRFileStream ts = new TRFileStream(datapath)
			{
				MainFolder = folder,
			};

			BinaryReader dr = new BinaryReader(stream, Encoding.UTF8, true);

			int buffersize = int.MaxValue - 56;

			ulong index = 0;
			ulong count = (ulong)(stream.Length / buffersize);
			int remain = (int)(stream.Length % buffersize);

			while (index < count)
			{
				ts.dw.Write(dr.ReadBytes(buffersize));
				index++;
			}

			ts.dw.Write(dr.ReadBytes(remain));

			ts.dw.Flush();

			return ts;
		}

		public override void Close()
		{
			dw.Close();
			dr.Close();
			datamemory.Close();
			base.Close();
		}

		public new void Dispose()
		{
			File.Delete(TempFile);
		}

		public override void Flush()
		{
			datamemory.Flush();
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			return datamemory.Seek(offset, origin);
		}

		public override void SetLength(long value)
		{
			datamemory.SetLength(value);
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			return datamemory.Read(buffer, offset, count);
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			datamemory.Write(buffer, offset, count);
		}

		public static byte[] TRFSHeaderToBinary(TRFileSystemObj obj)
		{
			MemoryStream memoryStream = new MemoryStream();
			BinaryWriter bw = new BinaryWriter(memoryStream, Encoding.UTF8, true);

			bw.Write(obj.Name);
			bw.Write(obj.FullName);
			bw.Write(obj.CreateTime.ToBinary());
			bw.Write(obj.SaveTime.ToBinary());
			bw.Write(obj.Guid.ToByteArray());

			if (obj is TRFolder)
			{
				TRFolder folder = obj as TRFolder;

				bw.Write((byte)0);

				bw.Write(folder.SubItemCount);

				foreach (TRFileSystemObj objt in folder.SubFiles)
				{
					bw.Write(TRFSHeaderToBinary(objt));
				}
			}
			else
			{
				TRFile file = obj as TRFile;

				bw.Write((byte)1);
				bw.Write(file.Position);
				bw.Write(file.Size);
			}

			bw.Close();

			return memoryStream.ToArray();
		}

		public static TRFileStream Default = new TRFileStream()
		{

		};

		public static TRFileSystemObj BinaryToTRFSHeader(byte[] binary)
		{
			return BinaryToTRFSHeader(new MemoryStream(binary));
		}

		public static TRFileSystemObj BinaryToTRFSHeader(Stream stream)
		{
			return BinaryToTRFSHeader(new BinaryReader(stream, Encoding.UTF8));
		}

		public static TRFileSystemObj BinaryToTRFSHeader(BinaryReader br)
		{
			TRFileSystemObj obj = null;

			string name = br.ReadString();
			string fullname = br.ReadString();
			DateTime createtime = DateTime.FromBinary(br.ReadInt64());
			DateTime savetime = DateTime.FromBinary(br.ReadInt64());
			Guid guid = new Guid(br.ReadBytes(16));

			byte type = br.ReadByte();

			if (type == 0)
			{
				List<TRFileSystemObj> objs = new List<TRFileSystemObj>();

				long count = br.ReadInt64();

				long index = 0;

				while (index < count)
				{
					objs.Add(BinaryToTRFSHeader(br));
					index++;
				}

				obj = new TRFolder(name, fullname, createtime, savetime, guid, objs.ToArray());
			}
			else if (type == 1)
			{
				ulong position = br.ReadUInt64();
				ulong size = br.ReadUInt64();

				obj = new TRFile(name, fullname, createtime, savetime, guid, position, size, false);
			}
			else
			{
				throw new InvalidDataException("不正确的数据类型");
			}

			return obj;
		}

		public static (long TotalLength, long FileLength, long FolderLength) GetTRFileTree(TRFileSystemObj[] files)
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
				if (files[i] is TRFolder)
				{
					TRFolder folder = files[i] as TRFolder;

					if (folder.SubFiles != null && folder.SubItemCount != 0)
					{
						(long, long, long) IMT = GetTRFileTree(folder.SubFiles.ToArray());

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

		public static (long TotalLength, long FileLength, long FolderLength) GetTRFileLength(TRFileSystemObj[] files)
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
				if (files[i].GetType() == typeof(TRFolder))
				{
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

		public static TRFileSystemObj GetForPath(TRFileSystemObj obj, string path)
		{
			foreach (string str in path.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries))
			{
				obj = GetForName(obj, str);
			}

			return obj;
		}

		public static TRFileSystemObj GetForName(TRFileSystemObj obj, string name)
		{
			if (!(obj is TRFolder))
			{
				return null;
			}

			TRFolder folder = obj as TRFolder;

			foreach (TRFileSystemObj obj1 in folder.SubFiles)
			{
				if (obj1.Name == name)
				{
					return obj1;
				}
			}

			return null;
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
