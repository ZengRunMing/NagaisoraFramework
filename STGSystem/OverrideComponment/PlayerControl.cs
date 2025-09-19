﻿using UnityEngine;

namespace NagaisoraFramework.STGSystem
{
	public class PlayerControl : STGComponment
	{
		[Header("系统参数 (PlayerControl)")]
		public Animator Animator;
		public float HighSpeed;
		public float SoltSpeed;
		
		public float RD = 10f;

		[Header("系统状态 (PlayerControl)")]
		public bool IsSolt = false;
		public bool IsShoot;

		public float MoveVectorX;
		public float MoveVectorY;

		public override void Init()
		{
			InitSpriteRender();

			if (!TryGetComponent(out Animator))
			{
				Animator = gameObject.AddComponent<Animator>();
			}

			base.Init();
		}

		public override void OnUpdate()
		{
			base.OnUpdate();

			Velocity = HighSpeed;

			if (IsSolt)
			{
				Velocity = SoltSpeed;
			}

			if (IsShoot)
			{
				Shoot();
			}

			AxisMove(MoveVectorX, MoveVectorY);
		}

		public override void Move()
		{
			base.Move();

			Vector2 position = TransformPosition;

			if (position.y >= STGControler.PlayerMaxPosition.y)
			{
				position.y = STGControler.PlayerMaxPosition.y;
			}
			if (position.y <= -STGControler.PlayerMaxPosition.y)
			{
				position.y = -STGControler.PlayerMaxPosition.y;
			}

			if (position.x >= STGControler.PlayerMaxPosition.x)
			{
				position.x = STGControler.PlayerMaxPosition.x;
			}
			if (position.x <= -STGControler.PlayerMaxPosition.x)
			{
				position.x = -STGControler.PlayerMaxPosition.x;
			}

			TransformPosition = position;
		}

		public override void KeyDown(bool[] keys)
		{
			if (!STGControler.IsRunning)
			{
				return;
			}

			float x;
			float y;

			if (keys[0])
			{
				y = 1;
			}
			else if (keys[1])
			{
				y = -1;
			}
			else
			{
				y = 0;
			}

			if (keys[2])
			{
				x = -1;
			}
			else if (keys[3])
			{
				x = 1;
			}
			else
			{
				x = 0;
			}

			if (keys[4])
			{
				IsShoot = true;
			}
			else
			{
				IsShoot = false;
			}

			if (keys[6])
			{
				IsSolt = true;
			}
			else
			{
				IsSolt = false;
			}

			MoveVectorX = x;
			MoveVectorY = y;
		}

		public void AxisMove(float x, float y)
		{
			Vector2 Vector = new Vector2(x, y);

			if (x < 0)
			{
				Animator.SetBool("MoveingL", true);
			}
			else
			{
				Animator.SetBool("MoveingL", false);
			}

			if (x > 0)
			{
				Animator.SetBool("MoveingR", true);
			}
			else
			{
				Animator.SetBool("MoveingR", false);
			}

			if ((TransformPosition.y >= STGControler.PlayerMaxPosition.y && Vector.y > 0) || (TransformPosition.y <= -STGControler.PlayerMaxPosition.y && Vector.y < 0))
			{
				Vector.y = 0;
			}
			if ((TransformPosition.x >= STGControler.PlayerMaxPosition.x && Vector.x > 0) || (TransformPosition.x <= -STGControler.PlayerMaxPosition.x && Vector.x < 0))
			{
				Vector.x = 0;
			}

			MoveVector = Vector;
		}

		public virtual void Shoot()
		{

		}
	}
}