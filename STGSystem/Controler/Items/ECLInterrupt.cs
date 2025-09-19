using System.IO;

namespace NagaisoraFramework.STGSystem
{
	public class ECLInterrupt : IECLItem
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

public class {Path.GetFileNameWithoutExtension(Name)} : IECLInterrupt
{{
	// 中断类是ECL中特有的一个类, 继承自IECLInterrupt接口, 并实现接口中的Condition和Execute方法
	// Condition方法用于判断是否满足中断条件
	// 中断的优先级是最高的, 所以其判断条件的函数在OnUpdate前执行, 在满足条件后会停止运行STGControler, 并执行中断的Execute方法
	// 中断有标志控制, 不会重复执行, 但如果不清除标志, 则会一直阻止STGControler的运行

	public bool Flag {{ get; set; }}

	// 此处执行中断判断
	// 如果返回 true，则执行 Execute 方法
	// 默认返回 false
	public bool Condition()
	{{
		// 使用if语句或其他逻辑来判断是否满足中断条件
		// 例如：
		// if (someCondition)
		// {{
		//     return true; // 满足条件，执行中断
		// }}
		// 在此处编写中断判断逻辑
		
		return false; // 默认返回 false
	}}

	// 此处执行中断
	public void Execute()
	{{
		// 在此处编写中断执行代码
		// 例如：
		// Console.WriteLine(""{Name} Condition -> true"");
		return;
	}}
}}
";
		}
	}
}
