using UnityEngine;

namespace NagaisoraFramework.STGSystem
{
	public class EnemyControl : STGComponment
	{
		EnemyInfo EnemyInfo;

		public int Type;
		public int Color;

		public bool Determing = true;
		public bool Delete_Effect = false;

		public Animator Animator;

		public override void Init()
		{
			base.Init();
			
			STGManager.Enemys.Add(this);

			InitSpriteRender();

			if (!TryGetComponent(out Animator))
			{
				Animator = gameObject.AddComponent<Animator>();
			}

			EnemyInfo = STGManager.STGSystemData.Enemy[Type];

			DetermineOffset = EnemyInfo.DetermineOffset;
			DetermineRadius = EnemyInfo.DetermineRadius;

			SpriteRender.drawMode = SpriteDrawMode.Sliced;
			SpriteRender.sortingLayerName = "StageMain";
			SpriteRender.sortingOrder = Order;
			SpriteRender.material = STGManager.BlendManager.Blends[BlendMode];       //设定SpriteRender材质

			EnemyObject enemyObject = EnemyInfo.Info[Color];

			Animator.runtimeAnimatorController = enemyObject.AnimatorController;

			SetAnimatorNormal();
		}

		public override void OnUpdate()
		{
			base.OnUpdate();
			Check(STGManager.Player);
		}

		public virtual void Check(STGComponment Target)
		{
			if (Target == null || Target.Disposed)
			{
				return;
			}

			if (ThisTime < 5 || !Determing)
			{
				return;
			}

			if (HitCheck(Target))
			{
				STGManager.LifeSub();
				BaseDelete();
				return;
			}
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

		public override void BaseDelete()
		{
			if (Delete_Effect)
			{
				STGManager.NewEffect<EffectControl>(Color, Order - 21, TransformPosition);
			}

			base.BaseDelete();

			STGManager.Enemys.Remove(this);
		}
	}
}
