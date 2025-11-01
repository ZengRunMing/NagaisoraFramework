using UnityEngine;

namespace NagaisoraFramework.STGSystem
{
	public class Enemy : STGComponent
	{
		EnemyInfo EnemyInfo;

		public int Type;
		public int Color;

		public bool Determing = true;
		public bool Delete_Effect = false;

		public Animator Animator;

		public float HealthValue
		{
			get
			{
				return m_HealthValue;
			}
			set
			{
				m_HealthValue = value;

				if (HealthValue < 0f)
				{
					Delete_Effect = true;
					BaseDelete();
				}
			}
		}

		[SerializeField]
		protected float m_HealthValue = 1f;

		[ContextMenu("销毁机体 (仅限编辑器测试)", false)]
		public void InstDelete()
		{
			BaseDelete();
		}

		public override void Init()
		{
			base.Init();
			
			STGControler.Enemys.Add(this);

			InitSpriteRender();

			if (!TryGetComponent(out Animator))
			{
				Animator = gameObject.AddComponent<Animator>();
			}

			EnemyInfo = STGControler.STGSystemData.Enemy[Type];

			DetermineOffset = EnemyInfo.DetermineOffset;
			DetermineRadius = EnemyInfo.DetermineRadius;

			SpriteRender.drawMode = SpriteDrawMode.Sliced;
			SpriteRender.sortingLayerName = "Enemy";
			SpriteRender.sortingOrder = Order;

			EnemyObject enemyObject = EnemyInfo.Info[Color];

			Animator.runtimeAnimatorController = enemyObject.AnimatorController;

			SetAnimatorNormal();
		}

		public override void OnUpdate()
		{
			base.OnUpdate();

			Check(STGControler.Player);
		}

		public virtual void Check(STGComponent Target)
		{
			if (Target == null || Target.Disposed)
			{
				return;
			}

			if (STGControler.TestStatus)
			{
				return;
			}

			if (ThisTime < 5 || !Determing)
			{
				return;
			}

			if (HitCheck(Target))
			{
				STGControler.LifeSub();
				BaseDelete();
				return;
			}
		}

		public void OnDamage(float damageValue)
		{
			HealthValue -= damageValue;
		}

		public override void BaseDelete()
		{
			if (Delete_Effect)
			{
				STGControler.NewEnemyEndEffect(Order, TransformPosition);
			}
			STGControler.Enemys.Remove(this);

			base.BaseDelete();
		}

		public virtual void SetAnimatorNormal()
		{
			if (Animator == null)
			{
				return;
			}
			Animator.SetBool("MoveingL", false);
			Animator.SetBool("MoveingR", false);
		}

		public virtual void SetAnimatorMoveLeft()
		{
			if (Animator == null)
			{
				return;
			}
			Animator.SetBool("MoveingL", true);
			Animator.SetBool("MoveingR", false);
		}

		public virtual void SetAnimatorMoveRight()
		{
			if (Animator == null)
			{
				return;
			}
			Animator.SetBool("MoveingL", false);
			Animator.SetBool("MoveingR", true);
		}
	}
}
