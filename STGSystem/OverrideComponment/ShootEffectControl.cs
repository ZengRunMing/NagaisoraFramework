using UnityEngine;

namespace NagaisoraFramework.STGSystem
{
	//敌机发射效果控制系统
	public class EnemyShootEffectControl : EffectControl
	{
		public int Color;

		BulletObject EffectInfo;

		public override void Init()
		{
			EffectInfo = STGControler.STGSystemData.EnemyBulletEffect[Color];

			InitSpriteRender();                                                         //初始化SpriteRender组件

			SpriteRender.drawMode = SpriteDrawMode.Sliced;                              //设定SpriteRender渲染模式
			SpriteRender.sortingLayerName = "StageMain";                                //设定SpriteRender渲染图层
			SpriteRender.sortingOrder = Order;                                          //设定SpriteRender图层排序号
			SpriteRender.material = STGControler.BlendManager.Blends[BlendMode];        //设定SpriteRender材质

			SpriteRender.color = UnityEngine.Color.white;                               //初始化SpriteRender的RGBA值
			SpriteRender.sprite = EffectInfo.Sprite;                                    //通过颜色类型从设定缓存提取具体贴图至SpriteRender
			SpriteRender.size = EffectInfo.Sprite.rect.size;                            //设置SpriteRender的渲染大小

			base.Init();																//调用父类的初始化方法
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
