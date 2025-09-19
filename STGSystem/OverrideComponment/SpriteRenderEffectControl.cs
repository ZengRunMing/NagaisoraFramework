using UnityEngine;

namespace NagaisoraFramework.STGSystem
{
	//敌机发射效果控制系统
	public class SpriteRenderEffectControl : EffectControl
	{
		public Vector2 Size;

		public override void Init()
		{
			InitSpriteRender();

			SpriteRender.drawMode = SpriteDrawMode.Sliced;
			SpriteRender.sortingLayerName = "StageMain";
			SpriteRender.sortingOrder = Order;

			SpriteRender.color = Color.white;
			SpriteRender.sprite = Sprite;
			SpriteRender.size = Size;

			base.Init();
		}
	}
}
