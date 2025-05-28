using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace NagaisoraFamework
{
	using Cryptography;
	using DataFileSystem;
	using System.Collections;

	[Serializable]
	public struct ScoreData
	{
		public string CreaterName;
		public DateTime SaveTime;
		public TimeSpan TotalRunTime;
		public bool[] BGMUnlockData;
		public PlayerData[][] PlayerDatas;

		public ScoreData(string CreaterName ,DateTime SaveTime, TimeSpan TotalRunTime, bool[] BGMUnlockData, PlayerData[][] PlayerDatas)
		{
			this.CreaterName = CreaterName;
			this.SaveTime = SaveTime;
			this.TotalRunTime = TotalRunTime;
			this.PlayerDatas = PlayerDatas;
			this.BGMUnlockData = BGMUnlockData;
		}

		public static ScoreData Default()
		{
			bool[] MusicRoomGetData = new bool[21];

			for (int i = 0; i < MusicRoomGetData.Length; i++)
			{
				MusicRoomGetData[i] = false;

				if (i == 0)
				{
					MusicRoomGetData[i] = true;
				}
			}

			PlayerData[][] PlayerDatas = new PlayerData[MainSystem.PlayerLength][];

			for (int i = 0; i < PlayerDatas.Length; i++)
			{
				PlayerData[] keyValues = new PlayerData[MainSystem.RankLength];

				for (int a = 0; a < keyValues.Length; a++)
				{
					List<SpellCardScore> SpellData = new List<SpellCardScore>();

					for (int c = 0; c < 33; c++)
					{
						SpellData.Add(new SpellCardScore("Spell_" + c, 0, 0));
					}

					PlayerData playerData = new PlayerData(i, a, DateTime.Now, TimeSpan.Zero, 0, 0, null, SpellData);

					keyValues[a] = playerData;
				}
				PlayerDatas[i] = keyValues;
			}

			return new ScoreData("System", DateTime.Now, TimeSpan.Zero, MusicRoomGetData, PlayerDatas);
		}

		public static ScoreData AllDone()
		{
			bool[] MusicRoomGetData = new bool[21];

			for (int i = 0; i < MusicRoomGetData.Length; i++)
			{
				MusicRoomGetData[i] = true;
			}

			PlayerData[][] PlayerDatas = new PlayerData[MainSystem.PlayerLength][];

			for (int i = 0; i < PlayerDatas.Length; i++)
			{
				PlayerData[] keyValues = new PlayerData[MainSystem.RankLength];

				for (int a = 0; a < keyValues.Length; a++)
				{
					List<SpellCardScore> SpellData = new List<SpellCardScore>();

					for (int c = 0; c < 33; c++)
					{
						SpellData.Add(new SpellCardScore("Spell_" + c, 0, 0));
					}

					PlayerData playerData = new PlayerData(i, a, DateTime.Now, TimeSpan.Zero, 0, 0, null, SpellData);

					keyValues[a] = playerData;
				}
				PlayerDatas[i] = keyValues;
			}

			return new ScoreData("System", DateTime.Now, TimeSpan.Zero, MusicRoomGetData, PlayerDatas);
		}

		public static ScoreData FromBinary(byte[] binary)
		{
			MemoryStream MainMemory = new MemoryStream(binary);
			BinaryReader binaryReader = new BinaryReader(MainMemory, Encoding.UTF8);

			binaryReader.BaseStream.Seek(0, SeekOrigin.Begin);

			string Hider = Encoding.UTF8.GetString(binaryReader.ReadBytes(MainSystem.Name.Length + 10));
			if (Hider != $"{MainSystem.Name}\\SCOREDATA")
			{
				throw new InvalidDataException($"数据标识头不匹配");
			}

			byte[] MD5Data = binaryReader.ReadBytes(16);

			string createname = binaryReader.ReadString();
			DateTime savetime = DateTime.FromBinary(binaryReader.ReadInt64());
			TimeSpan totalruntime = TimeSpan.FromTicks(binaryReader.ReadInt64());

			int musicroom_getlength = binaryReader.ReadInt32();
			int datalength = binaryReader.ReadInt32();

			byte[] buffer = binaryReader.ReadBytes(Convert.ToInt32(binaryReader.BaseStream.Length));

			if (BitConverter.ToString(MD5.MD5Encrypt16Byte(buffer)) != BitConverter.ToString(MD5Data))
			{
				throw new InvalidDataException($"MD5校验不匹配");
			}

			MemoryStream memory = new MemoryStream(buffer);
			BinaryReader DBR = new BinaryReader(memory);

			bool[] bools = new bool[musicroom_getlength];

			for (int i = 0; i < bools.Length; i++)
			{
				bools[i] = DBR.ReadBoolean();
			}

			PlayerData[][] datas = PlayerDataSystem.ReadPlayerData(DBR.ReadBytes(datalength));

			return new ScoreData(createname, savetime, totalruntime, bools, datas);
		}

		public byte[] ToBinady()
		{
			MemoryStream TotalMemory = new MemoryStream();
			BinaryWriter TotalWriter = new BinaryWriter(TotalMemory, Encoding.UTF8);

			MemoryStream HeadMemory = new MemoryStream();
			BinaryWriter HeadWriter = new BinaryWriter(HeadMemory, Encoding.UTF8);

			MemoryStream DataMemory = new MemoryStream();
			BinaryWriter DataWriter = new BinaryWriter(DataMemory, Encoding.UTF8);

			for (int i = 0; i < BGMUnlockData.Length; i++)
			{
				DataWriter.Write(BGMUnlockData[i]);
			}

			byte[] bytes = PlayerDataSystem.WritePlayerData(PlayerDatas);
			if (bytes != null)
			{
				DataWriter.Write(bytes);
			}

			DataWriter.Close();

			byte[] data = DataMemory.ToArray();

			DataMemory.Close();

			HeadWriter.Write(Encoding.UTF8.GetBytes($"{MainSystem.Name}\\SCOREDATA"));

			HeadWriter.Write(MD5.MD5Encrypt16Byte(data));

			HeadWriter.Write(CreaterName);
			HeadWriter.Write(SaveTime.ToBinary());
			HeadWriter.Write(TotalRunTime.Ticks);

			HeadWriter.Write(BGMUnlockData.Length);

			if (bytes != null)
			{
				HeadWriter.Write(bytes.Length);
			}
			else
			{
				HeadWriter.Write(0);
			}

			HeadWriter.Close();

			TotalWriter.Write(HeadMemory.ToArray());
			TotalWriter.Write(DataMemory.ToArray());

			TotalWriter.Close();

			return TotalMemory.ToArray();
		}

		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public override string ToString()
		{
			return base.ToString();
		}
	}
}
