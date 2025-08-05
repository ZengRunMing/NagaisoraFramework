using UnityEngine;

namespace NagaisoraFramework.STGSystem
{
	// EffectControl类用于控制游戏中的特效，此类作为效果类的基类仅用于分类作用
	public class EffectControl : STGComponment
	{
		public override void Init()
		{
			base.Init(); //调用父类的初始化方法
		}

		public override void OnUpdate()
		{
			base.OnUpdate();
		}
	}
}