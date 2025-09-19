using System.IO;

namespace NagaisoraFramework.STGSystem
{
	public class ECLClass : IECLItem
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

public class {Path.GetFileNameWithoutExtension(Name)} // 默认为动态类, 如果需要静态, 请在public和class之间添加static修饰符, 继承类或者接口需要在类名后面添加:符号并写下继承对象, 例如public static class MyClass : MyBaseClass, IMyInterface
{{
	// 类可以定义函数，变量, 构造函数, 并实现具体的功能
	// 类可以继承其他类, 也可以继承接口
	// 类可以编写访问修饰符, 例如public, private, protected, internal等
	// 具体请参考 https://learn.microsoft.com/zh-cn/dotnet/csharp/language-reference/keywords/class
	// 类编写示例:
	// 变量:
	// public string MyVariable0 = ""Hello World!"";
	// public static string MyVariable1 = ""Hello World!"";
	// public string MyVariable2 {{ get; set; }} = ""Hello World!"";
	// 动态类的构造函数:
	// public MyClass() // 构造函数与类名相同, 没有返回值
	// {{
	// 		// 在此处编写构造函数代码
	// }}
	// 静态类的静态构造函数:
	// static MyClass() // 静态构造函数与类名相同, 没有返回值, 且必须是静态的
	// {{
	// 		// 在此处编写静态构造函数代码
	// }}
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
