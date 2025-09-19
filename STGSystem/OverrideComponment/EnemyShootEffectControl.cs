namespace NagaisoraFramework.STGSystem
{
	//敌机发射效果控制系统
	public class EnemyShootEffectControl : SpriteRenderEffectControl
	{
		public int Color;

		BulletObject EffectInfo;

		public override void Init()
		{
			EffectInfo = STGControler.STGSystemData.EnemyBulletEffect[Color];

			Sprite = EffectInfo.Sprite;
			Size = EffectInfo.Sprite.rect.size;
		}

		public override void OnUpdate()
		{
			//if (ThisTime < 4)
			//{
			//	return;
			//}

			Transparent -= MaxTransparent / 8;

			if (Transparent <= 0)
			{
				BaseDelete();
			}

			base.OnUpdate();
		}
	}
}
