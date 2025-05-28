using System;
using System.IO;
using System.Text;

using UnityEngine;

namespace NagaisoraFamework
{
	[Serializable]
	public struct KeyConfig
	{
		public KeyCode Up;
		public KeyCode Down;
		public KeyCode Left;
		public KeyCode Right;
		public KeyCode ShootKey;
		public KeyCode BombKey;
		public KeyCode ESC;
		public KeyCode Slow;
		public KeyCode Change;
		public KeyCode Ctrl;
		public KeyCode J_ShootKey;
		public KeyCode J_BombKey;
		public KeyCode J_ESC;
		public KeyCode J_Slow;
		public KeyCode J_Change;
		public KeyCode J_Ctrl;


		public static KeyConfig Default = new KeyConfig()
		{
			Up = KeyCode.UpArrow,
			Down = KeyCode.DownArrow,
			Left = KeyCode.LeftArrow,
			Right = KeyCode.RightArrow,

			ShootKey = KeyCode.Z,
			J_ShootKey = KeyCode.JoystickButton0,

			BombKey = KeyCode.X,
			J_BombKey = KeyCode.JoystickButton6,

			ESC = KeyCode.Escape,
			J_ESC = KeyCode.JoystickButton1,

			Slow = KeyCode.LeftShift,
			J_Slow = KeyCode.JoystickButton7,

			Change = KeyCode.C,
			J_Change = KeyCode.JoystickButton3,

			Ctrl = KeyCode.LeftControl,
			J_Ctrl = KeyCode.JoystickButton2,
		};

		public byte[] ToBinary()
		{
			MemoryStream stream = new MemoryStream();
			BinaryWriter writer = new BinaryWriter(stream, Encoding.UTF8, true);

			writer.Write((ushort)Up);
			writer.Write((ushort)Down);
			writer.Write((ushort)Left);
			writer.Write((ushort)Right);

			writer.Write((ushort)ShootKey);
			writer.Write((ushort)J_ShootKey);

			writer.Write((ushort)BombKey);
			writer.Write((ushort)J_BombKey);

			writer.Write((ushort)ESC);
			writer.Write((ushort)J_ESC);

			writer.Write((ushort)Slow);
			writer.Write((ushort)J_Slow);

			writer.Write((ushort)Change);
			writer.Write((ushort)J_Change);

			writer.Write((ushort)Ctrl);
			writer.Write((ushort)J_Ctrl);

			writer.Close();

			return stream.ToArray();
		}

		public static KeyConfig FromBinary(byte[] binary)
		{
			MemoryStream stream = new MemoryStream(binary);
			BinaryReader binaryReader = new BinaryReader(stream, Encoding.UTF8, false);

			KeyConfig keyconfig = new KeyConfig()
			{
				Up = (KeyCode)binaryReader.ReadUInt16(),
				Down = (KeyCode)binaryReader.ReadUInt16(),
				Left = (KeyCode)binaryReader.ReadUInt16(),
				Right = (KeyCode)binaryReader.ReadUInt16(),

				ShootKey = (KeyCode)binaryReader.ReadUInt16(),
				J_ShootKey = (KeyCode)binaryReader.ReadUInt16(),

				BombKey = (KeyCode)binaryReader.ReadUInt16(),
				J_BombKey = (KeyCode)binaryReader.ReadUInt16(),

				ESC = (KeyCode)binaryReader.ReadUInt16(),
				J_ESC = (KeyCode)binaryReader.ReadUInt16(),

				Slow = (KeyCode)binaryReader.ReadUInt16(),
				J_Slow = (KeyCode)binaryReader.ReadUInt16(),

				Change = (KeyCode)binaryReader.ReadUInt16(),
				J_Change = (KeyCode)binaryReader.ReadUInt16(),

				Ctrl = (KeyCode)binaryReader.ReadUInt16(),
				J_Ctrl = (KeyCode)binaryReader.ReadUInt16(),
			};

			binaryReader.Close();
			stream.Close();

			return keyconfig;
		}
	}

}
