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
			SpriteRender.sortingLayerName = "StageMain";
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

		public virtual void Check(STGComponment Target)
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
				STGControler.NewEnemyEndEffect(Order - 21, TransformPosition);
			}
			STGControler.Enemys.Remove(this);

			base.BaseDelete();
		}
	}
}
