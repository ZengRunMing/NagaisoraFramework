using UnityEngine;

namespace NagaisoraFramework.STGSystem
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
		}

		/// <summary>
		/// 子弹组件的全判定函数，可以通过重写添加需要执行的判定程序，不需要回调父类的函数
		/// </summary>
		/// <param name="Target">针对的STGComponment</param>
		public virtual void Check(STGComponment Target)
		{

		}
	}
}
