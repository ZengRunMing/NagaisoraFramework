using System.IO;

namespace NagaisoraFramework.STGSystem
{
	public class ECLInterface : IECLItem
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

public interface {Path.GetFileNameWithoutExtension(Name)} // 命名建议以I开头, 例如IMyInterface
{{
	// 接口可以定义函数，变量, 但不能定义具体的实现
	// 接口不能编写访问修饰符, 默认是public, 且不能编写静态的函数和变量
	// 接口的作用是给予一个规范, 从而可以通过接口来调用实现类的函数和变量
	// 如果一个类要实现这个接口, 不仅要继承这个接口, 还要实现接口中的所有函数和变量,
	// 具体请参考 https://learn.microsoft.com/zh-cn/dotnet/csharp/language-reference/keywords/interface
	// 接口编写示例:
	// 变量:
	// int MyVariable {{ get; set; }}
	// 函数:
	// void MyFunction(string i);
	// 在此处编写函数代码
	
}}
";
		}
	}
}
