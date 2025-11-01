namespace NagaisoraFramework.STGSystem
{
	// EffectControl类用于控制游戏中的特效，此类作为效果类的基类仅用于分类作用
	public class Effect : STGComponent
	{
		public string RenderLayerName = "BulletEffect"; //渲染层级名称

		public override void Init()
		{
			base.Init(); //调用父类的初始化方法

			STGControler.Effects.Add(this);
		}

		public override void OnUpdate()
		{
			base.OnUpdate();
		}

		public override void BaseDelete()
		{
			STGControler.Effects.Remove(this);

			base.BaseDelete();
		}
	}
}