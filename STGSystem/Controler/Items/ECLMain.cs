using System.IO;

namespace NagaisoraFramework.STGSystem
{
	public class ECLMain : IECLItem
	{
		public string Name
		{ 
			get
			{
				return "Main";
			}
			set
			{
				// 这里不需要设置Name，因为它是固定的
				return;
			}
		}

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

public static class {Path.GetFileNameWithoutExtension(Name)}
{{
	// ECL运行的入口点, 在ECL启动时调用, 可用于初始化
	public static void OnStart()
	{{
		// ECL启动时的初始化逻辑
		// 例如：加载资源、设置初始状态等
		// 在此处编写初始化代码
		
		return;
	}}

	// ECL运行的更新点, 在每帧调用
	public static void OnUpdate()
	{{
		// 每帧更新逻辑
		// 例如：处理输入、更新游戏状态等
		// 在此处编写每帧更新代码
		
		return;
	}}
}}
";
		}
	}
}
