using UnityEngine;

namespace NagaisoraFamework.STGSystem
{
    public class BulletControl : STGComponment
	{
		public bool Determing = true;
		public bool Delete_Effect = false;

		public override void Awake()
		{
			base.Awake();
			InitSpriteRender();
		}

		public override void Init()
		{
			base.Init();

			SpriteRender.drawMode = SpriteDrawMode.Sliced;
			SpriteRender.sortingLayerName = "StageMain";
			SpriteRender.sortingOrder = Order;
			SpriteRender.material = STGManager.BlendManager.Blends[BlendMode];       //设定SpriteRender材质
		}

		public virtual void Check(STGComponment Target)
		{

		}
	}
}
