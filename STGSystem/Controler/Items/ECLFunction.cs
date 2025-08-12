using System.IO;

namespace NagaisoraFramework.STGSystem
{
	public class ECLFunction : IECLItem
	{
		public string Name { get; set; }
		public string Description { get; set; }
		
		public string ExecuteCode { get; set; }

		public void Init()
		{
			ExecuteCode =
$@"#region {Path.GetFileNameWithoutExtension(Name)}_Usings
// 此处用于添加所需的引用
// 例如：
// using System;
// 在此处编写引用

#endregion

public class {Path.GetFileNameWithoutExtension(Name)} // 默认为动态类, 如果需要静态, 请在public和class之间添加static修饰符
{{
	// ECL定义函数
	// 例如:
	// 动态函数:
	// public void MyFunction(string i)
	// {{
	// 		Console.WriteLine(i);
	// }}
	// 静态函数:
	// public static void MyFunction(string i)
	// {{
	//		Console.WriteLine(i);
	// }}
	// 注意: 静态类中的函数必须是静态
	// 在此处编写函数代码
	
}}
";
		}
	}
}
