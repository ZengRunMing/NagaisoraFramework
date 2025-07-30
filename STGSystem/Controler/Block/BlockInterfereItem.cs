using System.Collections.ObjectModel;

namespace NagaisoraFramework.STGSystem
{
	public enum ItemType
	{
		Unknown = 0,
		System_Boolean = 1,
		System_Int16 = 2,
		System_UInt16 = 3,
		System_Int32 = 4,
		System_UInt32 = 5,
		System_Int64 = 6,
		System_UInt64 = 7,
		System_Single = 8,
		System_Double = 9,
		System_Decimal = 10,
		System_Byte = 11,
		System_SByte = 12,
		System_String = 13,
		System_Char = 14,
		System_DateTime = 15,
		System_DateTimeOffset = 16,
		System_TimeSpan = 17,
		System_Guid = 18,
		System_GuidOffset = 19,
		Sytem_Object = 20,
		Struct = 21,
	}

	public class BlockInterfereItem
	{
		public string Name { get; set; }
		public ItemType Type { get; set; }
		public object Value { get; set; }

		public BlockInterfereItem(string name, ItemType type, object value)
		{
			Name = name;
			Type = type;
			Value = value;
		}
	}

	public class BlockInterfereItemEx : BlockInterfereItem
	{
		public object DefaultValue { get; set; }
		public bool Maintain { get; set; }

		public BlockInterfereItemEx(string name, ItemType type, object value, object defaultValue, bool maintain) : base(name, type, value)
		{
			DefaultValue = defaultValue;
			Maintain = maintain;
		}
	}

	public class BlockInterfereItemCollection : Collection<BlockInterfereItem>
	{

	}
}
