using System.Data;

namespace NagaisoraFramework.STGSystem
{
	public static class BlockComColumn
	{
		public static DataColumn NameColumn = new DataColumn("Name", typeof(string));
		public static DataColumn TypeColumn = new DataColumn("Type", typeof(ItemType));
		public static DataColumn ValueColumn = new DataColumn("Value", typeof(object));
		public static DataColumn DefaultValueColumn = new DataColumn("DefaultValue", typeof(object));
		public static DataColumn MaintainColumn = new DataColumn("Maintain", typeof(bool));
		public static DataColumn NotesColumn = new DataColumn("Notes", typeof(string));
	}
}
