using System.IO;
using UnityEngine;

namespace NagaisoraFamework.DataFileSystem
{
	using static MainSystem;

	public static class ScoreDataSystem
	{
		/// <summary>
		/// 读取Score数据
		/// </summary>
		/// <param name="Path">读取地址，如果为Null或者无字符则在默认地址读取</param>
		/// <returns>返回ScoreData数据</returns>
		public static ScoreData ScoreDataLoad(string Path)
		{
			if (!File.Exists(Path))
			{
				Debug.LogWarning(new FileNotFoundException($"ScoreDataLoad({Path}) => 所在路径不存在ScoreData文件，将创建初始化的数据写入文件并装载"));
				ScoreDataSave(ScoreData.Default(), Path);
			}

			return ScoreData.FromBinary(File.ReadAllBytes(Path));
		}

		/// <summary>
		/// 保存Score数据
		/// </summary>
		/// <param name="scoreData">要保存的Score数据</param>
		/// <param name="Path">保存地址，如果为Null或者无字符则保存在默认地址</param>
		public static void ScoreDataSave(ScoreData scoreData, string Path)
		{
			FileStream fileStream = new FileStream(Path, FileMode.Create, FileAccess.ReadWrite);

			byte[] bytes = scoreData.ToBinady();

			fileStream.Write(bytes, 0, bytes.Length);
			fileStream.Close();
		}
	}
}