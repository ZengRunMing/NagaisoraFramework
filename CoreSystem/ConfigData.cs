using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using System;

namespace NagaisoraFramework
{
	[Serializable]
	public struct ConfigData
	{
		public byte MusicVolume;
		public byte SEVolume;
		public byte BGMode;
		public string OutputDeviceName;
		public uint ResolutionX;
		public uint ResolutionY;
		public uint ResolutionDenominator;
		public uint ResolutionNumerator;
		public byte DrawMode;
		public byte Frame;


		public Dictionary<string, KeyConfig> KeyConfigs;

		public static ConfigData Default = new ConfigData()
		{
			MusicVolume = 10,
			SEVolume = 8,
			BGMode = 0,
			OutputDeviceName = "Microsoft GS Wavetable Synth",
			ResolutionX = 1440,
			ResolutionY = 1080,
			ResolutionDenominator = 0,
			ResolutionNumerator = 0,
			DrawMode = 0,
			Frame = 2,
			KeyConfigs = new Dictionary<string, KeyConfig>()
			{
				{"default", KeyConfig.Default },
			},
		};

		public byte[] ToBinary()
		{
			MemoryStream memoryStream = new MemoryStream();
			BinaryWriter writer = new BinaryWriter(memoryStream, Encoding.UTF8, true);
			writer.BaseStream.Seek(0, SeekOrigin.Begin);

			writer.Write(MusicVolume);
			writer.Write(SEVolume);
			writer.Write(BGMode);
			writer.Write(OutputDeviceName);
			writer.Write(ResolutionX);
			writer.Write(ResolutionY);
			writer.Write(ResolutionDenominator);
			writer.Write(ResolutionNumerator);
			writer.Write(DrawMode);
			writer.Write(Frame);

			string[] keys = KeyConfigs.Keys.ToArray();
			KeyConfig[] values = KeyConfigs.Values.ToArray();

			writer.Write((uint)KeyConfigs.Count);

			for (uint i = 0; i < KeyConfigs.Count; i++)
			{
				writer.Write(keys[i]);

				byte[] bytes = values[i].ToBinary();
				
				writer.Write((uint)bytes.Length);
				writer.Write(bytes);
			}

			writer.Close();

			return memoryStream.ToArray();
		}

		public static ConfigData FromBinary(byte[] binary)
		{
			MemoryStream memoryStream = new MemoryStream(binary);
			BinaryReader binaryReader = new BinaryReader(memoryStream, Encoding.UTF8);

			ConfigData configData = new ConfigData()
			{
				MusicVolume = binaryReader.ReadByte(),
				SEVolume = binaryReader.ReadByte(),
				BGMode = binaryReader.ReadByte(),
				OutputDeviceName = binaryReader.ReadString(),
				ResolutionX = binaryReader.ReadUInt32(),
				ResolutionY = binaryReader.ReadUInt32(),
				ResolutionDenominator = binaryReader.ReadUInt32(),
				ResolutionNumerator = binaryReader.ReadUInt32(),
				DrawMode = binaryReader.ReadByte(),
				Frame = binaryReader.ReadByte(),
			};

			uint count = binaryReader.ReadUInt32();

			configData.KeyConfigs = new Dictionary<string, KeyConfig>();

			for (uint i = 0; i < count; i++)
			{
				string name = binaryReader.ReadString();

				uint length = binaryReader.ReadUInt32();
				byte[] buffer = binaryReader.ReadBytes((int)length);
				KeyConfig keyConfig = KeyConfig.FromBinary(buffer);

				configData.KeyConfigs.Add(name, keyConfig);
			}

			binaryReader.Close();
			memoryStream.Close();

			return configData;
		}

		public ConfigData Copy()
		{
			byte[] bytes = ToBinary();
			return FromBinary(bytes);
		}
	}

}
