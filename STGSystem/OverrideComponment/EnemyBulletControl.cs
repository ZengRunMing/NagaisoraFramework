using UnityEngine;

namespace NagaisoraFramework.STGSystem
{
	using static MainSystem;

	//敌机子弹控制系统
	public class EnemyBulletControl : BulletControl
	{
		public EnemyBulletInfo BulletData                                   //子弹设定缓存
		{
			get
			{
				return m_BulletData;
			}
			set
			{
				m_BulletData = value;
				m_BulletDataChanged = true;
			}
		}

		public int Color
		{
			get
			{
				return m_Color;
			}
			set
			{
				m_Color = value;
				m_BulletDataChanged = true;
			}
		}

		[SerializeField]
		protected EnemyBulletInfo m_BulletData;
		[SerializeField]
		protected int m_Color;

		[SerializeField]
		protected bool m_BulletDataChanged;

		public override void Init()                                         //基于父类派生重写的初始化方法，此处编写初始化程序
		{
			base.Init();                                                    //调用父类的初始化方法

			STGManager.EnemyBullets.Add(this);
		}

		public override void OnUpdate()                                     //基于父类派生重写的逻辑更新方法
		{
			base.OnUpdate();                                                //调用父类的逻辑更新方法
			Check(STGManager.Player);                                       //调用判定检查方法
		}

		public override void UpdateUnityProperty()
		{
			if (m_BulletDataChanged)
			{
				DetermineOffset = BulletData.DetermineOffset;                   //设定判定偏移
				DetermineRadius = BulletData.DetermineRadius;                   //设定判定半径
				AngleOffsetCompensation = BulletData.AngleOffsetCompensation;   //设定显示角度偏移补偿

				Sprite = BulletData.Info[Color].Sprite;         //通过颜色类型从设定缓存提取具体贴图至SpriteRender
			}
			
			base.UpdateUnityProperty();
			
			if (m_BulletDataChanged)
			{
				SpriteRender.size = BulletData.Normoal_Size;                    //设置SpriteRender的渲染大小
			}
		}

		public override void ClearUnityPropertyUpdateFlags()
		{
			base.ClearUnityPropertyUpdateFlags();

			m_BulletDataChanged = false;
		}

		public override void Check(STGComponment Target)                      //基于父类派生重写的判定检查方法
		{
			if (Target == null || Target.Disposed)
			{
				return;
			}

			if (GrazeCheck(Target))                                          //判断指定对象(玩家)是否在Graze判定范围内
			{
				if (ThisTime % 2 == 0)                                      //每间隔一次更新执行一次
				{
					SEManager.PlaySE("Graze");								//调用播放音效方法
				}
			}

			if (ThisTime < 5 || !Determing)									//判断本地逻辑更新时间是否小于5以及是否未开启判定
			{
				return;														//执行无条件返回
			}

			if (HitCheck(Target))											//判断指定对象(玩家)是否在判定范围内
			{																
				STGManager.LifeSub();                                       //调用STG管理器玩家残机减一函数
				BaseDelete();												//销毁自身 (入列对象池)
				return;														//执行无条件返回
			}
		}

		public virtual bool GrazeCheck(STGComponment Target)                 //基于父类派生重写的Graze判定方法，原理与标准判定类似
		{
			if (Target == null || Target.Disposed)
			{
				return false;
			}

			if (DetermineRadius <= 0f)
			{
				return false;
			}

			float fy = DetermineRadius * Scale.y;
			float fx = DetermineRadius * Scale.x;

			float dy = TransformPosition.y - Target.TransformPosition.y - DetermineOffset.y;
			float dx = TransformPosition.x - Target.TransformPosition.x - DetermineOffset.x;

			float ey = fy + STGManager.GrazeVector;						//在此添加了Graze判定半径向量
			float ex = fx + STGManager.GrazeVector;                        //在此添加了Graze判定半径向量

			float ady = Mathf.Abs(dy);
			float adx = Mathf.Abs(dx);

			if (ady < ey && adx < ex && ey * ex > dy * dy + dx * dx)
			{
				return true;
			}

			return false;
		}

		public override void BaseDelete()                                   //基于父类派生重写的销毁自身方法
		{
			if (Delete_Effect)
			{
				STGManager.NewEffect<EffectControl>(Color, Order - 21, TransformPosition);
			}
			STGManager.EnemyBullets.Remove(this);

			base.BaseDelete();
		}
	}
}
