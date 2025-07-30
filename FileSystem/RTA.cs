using System;
using System.Collections.Generic;
using System.IO;

namespace NagaisoraFramework
{
	using Underlyingsystem;

	public class RTA : IDiskInfo, DiskSystemObj
	{
		public byte[] Version { get; }
		public override ulong StartSector { get; }
		public override ulong EndSector { get; }
		public override ulong SectorSize { get; }
		public override ulong DataLength { get; }
		public override byte[] SaveID { get; }

		public Guid Id { get; private set; }
		public Dictionary<string, Partition> Partitions;

		public int PartitionCount => Partitions.Count;

		public ulong BootPartitionCount;

		private ulong _PartitionCount;

		public RTA()
		{
			Id = Guid.NewGuid();
			Partitions = new Dictionary<string, Partition>();
		}

		public RTA(ulong startSector, ulong endSector, ulong sectorSize, ulong dataLength, ulong partitionCount, ulong bootPartitionCount, byte[] version, byte[] saveID, Guid guid) : this()
		{
			StartSector = startSector;
			EndSector = endSector;
			SectorSize = sectorSize;
			DataLength = dataLength;
			_PartitionCount = partitionCount;
			BootPartitionCount = bootPartitionCount;
			Version = version;
			SaveID = saveID;
			Id = guid;
		}

		public void LoadPartitions(DiskStream physicalDisk)
		{
			byte[][] PartitionDatas = physicalDisk.ReadSectorsArray(1, (int)_PartitionCount);

			foreach (byte[] PartitionData in PartitionDatas)
			{
				Partition partition = Partition.FromBinary(PartitionData);

				if (partition.IsBootPartition)
				{
					BootPartitionCount++;
				}

				AddPartitionRange(partition);
			}
		}

		public Partition[] GetAllBootPartitions()
		{
			List<Partition> partitions = new List<Partition>();

			foreach (Partition partition in Partitions.Values)
			{
				if (partition.IsBootPartition)
				{
					partitions.Add(partition);
				}
			}

			return partitions.ToArray();
		}

		public byte[] ToBinary()
		{
			MemoryStream Sector0Memory = new MemoryStream(new byte[512]);
			BinaryWriter Sector0Writer = new BinaryWriter(Sector0Memory);
			Sector0Writer.BaseStream.Position = 0;

			Sector0Writer.Write("RTA PART".ToCharArray());
			Sector0Writer.Write(Version);
			Sector0Writer.BaseStream.Position = 16;
			Sector0Writer.Write(SaveID);
			Sector0Writer.Write(Id.ToByteArray());

			Sector0Writer.Write(StartSector);
			Sector0Writer.Write(EndSector);
			Sector0Writer.Write(SectorSize);
			Sector0Writer.Write(DataLength);
			Sector0Writer.Write((long)Partitions.Count);
			Sector0Writer.Write(BootPartitionCount);

			Sector0Writer.BaseStream.Position = 510;
			Sector0Writer.Write(new byte[] { 0x55, 0xAA });

			MemoryStream memoryStream = new MemoryStream();
			BinaryWriter binaryWriter = new BinaryWriter(memoryStream);

			binaryWriter.Write(Sector0Memory.ToArray());

			foreach (Partition partition in Partitions.Values)
			{
				binaryWriter.Write(partition.ToBinary());
			}

			return memoryStream.ToArray();
		}

		public static RTA FromBinary(byte[] bytes)
		{
			if (bytes == null)
			{
				return null;
			}

			byte[] Version;
			ulong StartSector;
			ulong EndSector;
			ulong SectorSize;
			ulong DataLength;
			byte[] SaveID;
			Guid Id;
			ulong BootPartitionCount;
			ulong _PartitionCount;

			BinaryReader Reader = new BinaryReader(new MemoryStream(bytes));

			Reader.BaseStream.Position = 0;

			char[] hider = Reader.ReadChars(8);

			if (new string(hider) != "RTA PART")
			{
				throw new InvalidDataException("数据头不正确");
			}

			Version = Reader.ReadBytes(4);

			Reader.BaseStream.Position = 16;
			SaveID = Reader.ReadBytes(16);
			Id = new Guid(Reader.ReadBytes(16));

			StartSector = Reader.ReadUInt64();
			EndSector = Reader.ReadUInt64();
			SectorSize = Reader.ReadUInt64();
			DataLength = Reader.ReadUInt64();
			_PartitionCount = Reader.ReadUInt64();
			BootPartitionCount = Reader.ReadUInt64();

			RTA RTA = new RTA(StartSector, EndSector, SectorSize, DataLength, _PartitionCount, BootPartitionCount, Version, SaveID, Id);

			return RTA;
		}

		public static RTA FromDisk(DiskStream physicalDisk)
		{
			if (physicalDisk == null)
			{
				return null;
			}

			RTA RTA = FromBinary(physicalDisk.ReadSector(0));

			for (int i = 0; i < RTA.PartitionCount; i++)
			{
				RTA.AddPartitionRange(Partition.FromBinary(physicalDisk.ReadSector(i + 1)));
			}

			return RTA;
		}

		public void AddPartitionRange(params Partition[] partitions)
		{
			foreach (var partition in partitions)
			{
				Partitions?.Add(partition.Name, partition);
			}
		}

		public void DeletePartition(string partitionName)
		{
			Partitions.Remove(partitionName);
		}

		public new string ToString()
		{
			return "\nRTA分区表 信息\n" +
				  $"                    版本 : {BitConverter.ToString(Version).Replace('-', ' ')}\n" +
				  $"                起始扇区 : {StartSector}\n" +
				  $"                终止扇区 : {EndSector}\n" +
				  $"                扇区大小 : {SectorSize}\n" +
				  $"                数据长度 : {DataLength}\n" +
				  $"                分区数量 : {PartitionCount}\n" +
				  $"可执行启动引导的分区数量 : {BootPartitionCount}\n" +
				  $"              写盘设备ID : {BitConverter.ToString(SaveID).Replace('-', ' ')}\n";
		}
	}
}
