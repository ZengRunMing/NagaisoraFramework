using System.Data;

#if UNITY
using Mono.Data.Sqlite;
#else
using System.Data.SQLite;
#endif

namespace NagaisoraFamework
{
	public class SQLite
	{
#if UNITY
		public SqliteConnection m_dbConnection;
#else
		public SQLiteConnection m_dbConnection;
#endif
		public string DBFilePath;
		public int DBVersion;

		public void ConntentDB()
		{
			//没有数据库则自动创建
#if UNITY
			m_dbConnection = new SqliteConnection($"Data Source={DBFilePath};Version={DBVersion};");
#else
			m_dbConnection = new SQLiteConnection($"Data Source={DBFilePath};Version={DBVersion};");
#endif
			m_dbConnection.Open();
		}


		public DataTable DelectTable(string DataTable)
		{
			string SQLCommand = $"Select * from {DataTable}";
#if UNITY
			SqliteDataAdapter command = new SqliteDataAdapter(SQLCommand, m_dbConnection);
#else
			SQLiteDataAdapter command = new SQLiteDataAdapter(SQLCommand, m_dbConnection);
#endif
			DataTable DTable = new DataTable();
			command.Fill(DTable);

			return DTable;
		}

		public DataTable ListALL(string DataTable)
		{
			string SQLCommand = $"Select * from {DataTable}";
#if UNITY
			SqliteDataAdapter command = new SqliteDataAdapter(SQLCommand, m_dbConnection);
#else
			SQLiteDataAdapter command = new SQLiteDataAdapter(SQLCommand, m_dbConnection);
#endif
			DataTable DTable = new DataTable();
			command.Fill(DTable);

			return DTable;
		}

		/// <summary> 
		/// 添加数据记录
		/// </summary>
		/// <param name="DataTable">要执行的数据表</param>
		/// <param name="Command">后端SQL命令 格式：(--,--,--) Values (--,'--','--')</param>
		/// <returns>null</returns>
		public void InsertInto(string DataTable, string Command)
		{
			string SQLCommand = $"insert into {DataTable} {Command}";
#if UNITY
			SqliteCommand command = new SqliteCommand(SQLCommand, m_dbConnection);
#else
			SQLiteCommand command = new SQLiteCommand(SQLCommand, m_dbConnection);
#endif
			command.ExecuteNonQuery();
		}

		public DataTable SelectFrom(string DataTable, string Command)
		{
			string SQLCommand = $"Select * from {DataTable} {Command}";
#if UNITY
			SqliteDataAdapter command = new SqliteDataAdapter(SQLCommand, m_dbConnection);
#else
			SQLiteDataAdapter command = new SQLiteDataAdapter(SQLCommand, m_dbConnection);
#endif
			DataTable DTable = new DataTable();
			command.Fill(DTable);

			return DTable;
		}

		public void InsertFrom(string DataTable, string Command)
		{
			string SQLCommand = $"insert from {DataTable} {Command}";
#if UNITY
			SqliteCommand command = new SqliteCommand(SQLCommand, m_dbConnection);
#else
			SQLiteCommand command = new SQLiteCommand(SQLCommand, m_dbConnection);
#endif
			command.ExecuteNonQuery();
		}

		public void Update(string DataTable, string Command)
		{
			string SQLCommand = $"Update {DataTable} {Command}";
#if UNITY
			SqliteCommand command = new SqliteCommand(SQLCommand, m_dbConnection);
#else
			SQLiteCommand command = new SQLiteCommand(SQLCommand, m_dbConnection);
#endif
			command.ExecuteNonQuery();
		}
	}
}