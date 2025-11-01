namespace NagaisoraFramework.STGSystem
{
	//敌机发射效果控制系统
	public class EnemyShootEffect : SpriteRenderEffect
	{
		public int Color;

		public int Segment = 10;

		BulletObject EffectInfo;

		public override void Init()
		{
			EffectInfo = STGControler.STGSystemData.EnemyBulletEffect[Color];

			Sprite = EffectInfo.Sprite;
			Size = EffectInfo.Sprite.rect.size;
		}

		public override void OnUpdate()
		{
			Transparent -= MaxTransparent / Segment;

			if (Transparent <= 0)
			{
				BaseDelete();
			}

			base.OnUpdate();
		}
	}
}
