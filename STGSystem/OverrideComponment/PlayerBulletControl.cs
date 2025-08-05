using UnityEngine;

namespace NagaisoraFramework.STGSystem
{
	using static MainSystem;

	//玩家子弹控制系统
	public class PlayerBulletControl : BulletControl
	{
		public PlayerBulletInfo BulletData
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

		[SerializeField]
		protected PlayerBulletInfo m_BulletData;

		[SerializeField]
		protected bool m_BulletDataChanged;

		public override void Init()
		{
			base.Init();
			STGControler.PlayerBullets.Add(this);
		}

		public override void OnUpdate()
		{
			base.OnUpdate();

			EnemyControl[] enemys = STGControler.Enemys.ToArray();
			foreach (var enemy in enemys)
			{
				Check(enemy);
			}
		}

		public override void UpdateUnityProperty()
		{
			if (m_BulletDataChanged)
			{
				DetermineOffset = BulletData.DetermineOffset;
				DetermineRadius = BulletData.DetermineRadius;
				AngleOffsetCompensation = BulletData.AngleOffsetCompensation;   //设定显示角度偏移补偿

				Sprite = BulletData.Sprite;
			}

			base.UpdateUnityProperty();
			
			if (m_BulletDataChanged)
			{
				SpriteRender.size = BulletData.Normoal_Size;
			}
		}

		public override void ClearUnityPropertyUpdateFlags()
		{
			base.ClearUnityPropertyUpdateFlags();

			m_BulletDataChanged = false;
		}

		public override void Check(STGComponment Target)
		{
			if (Target == null || Target.Disposed)
			{
				return;
			}

			if (HitCheck(Target))
			{
				BaseDelete();
				return;
			}
		}

		public override void BaseDelete()
		{
			STGControler.PlayerBullets.Remove(this);

			base.BaseDelete();
		}
	}
}