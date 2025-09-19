using System.IO;

namespace NagaisoraFramework.STGSystem
{
	public class ECLCondition : IECLItem
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

public class {Path.GetFileNameWithoutExtension(Name)} : IECLCondition
{{
	// 判断类是ECL中特有的一个类, 继承自IECLCondition接口, 并实现接口中的Condition和Execute方法
	// 对比IECLInterrupt, 它不会影响STGControler的运行, 有两个模式, 分别是循环控制模式和标志控制模式
	// 循环控制模式下, 每次扫描都会执行Condition方法判断是否满足条件, 如果满足条件则执行Execute方法
	// 标志控制模式下, 会在满足条件时执行Execute方法, 但在这之后标志设为1, 标志设为1后将不再参与到执行中, 直到标志被清除后加入执行
	// Condition方法用于判断是否满足执行条件, 如果满足则执行Execute方法
	// 判断会在每次扫描的OnUpdate后执行, 条件满足后执行Execute方法
	// 如果一直满足条件则会每次扫描后都会执行, 但是可以通过设置标志控制模式来避免重复执行

	public bool LoopExecution {{ get; set; }}
	public bool Flag {{ get; set; }}

	// 此处执行条件判断
	// 如果返回 true，则执行 Execute 方法
	// 默认返回 false
	public bool Condition()
	{{
		// 使用if语句或其他逻辑来判断是否满足条件
		// 例如：
		// if (someCondition)
		// {{
		//     return true; // 满足条件，返回true
		// }}
		// 在此处编写判断逻辑
		
		return false; // 默认返回 false
	}}

	// 此处执行满足条件后的操作
	public void Execute()
	{{
		// 在此处编写执行代码
		// 例如：
		// Console.WriteLine(""{Name} Condition -> true"");
		return;
	}}
}}
";
		}
	}
}
