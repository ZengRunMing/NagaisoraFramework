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

public class {Path.GetFileNameWithoutExtension(Name)} : ECLInterrupt
{{
	// 此处执行中断判断
	// 如果返回 true，则执行 Execute 方法
	// 默认返回 false
	public override bool Condition()
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
	public override void Execute()
	{{
		// 在此处编写中断执行代码
		
		return;
	}}
}}
";
		}
	}
}
