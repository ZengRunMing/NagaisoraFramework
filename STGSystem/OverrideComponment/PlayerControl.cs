using UnityEngine;

namespace NagaisoraFramework.STGSystem
{
	public class PlayerControl : STGComponment
	{
		[Header("系统引用 (PlayerControl)")]
		public GameObject Effect;
		public GameObject SlowEffectL;
		public GameObject SlowEffectR;

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

			if (!STGManager.IsReplaying)
			{
				BindKeyEvent();
			}

			base.Init();
		}

		public void Update()
		{
			if (IsSolt)
			{
				Effect.SetActive(true);
			}
			else
			{
				Effect.SetActive(false);
			}
		}

		public override void OnUpdate()
		{
			base.OnUpdate();

			SlowEffectL.transform.Rotate(new Vector3(0, 0, RD), Space.Self);
			SlowEffectR.transform.Rotate(new Vector3(0, 0, -RD), Space.Self);

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

			Vector2 position = Transform.localPosition;

			if (position.y >= STGManager.PlayerMaxPosition.y)
			{
				position.y = STGManager.PlayerMaxPosition.y;
			}
			if (position.y <= -STGManager.PlayerMaxPosition.y)
			{
				position.y = -STGManager.PlayerMaxPosition.y;
			}

			if (position.x >= STGManager.PlayerMaxPosition.x)
			{
				position.x = STGManager.PlayerMaxPosition.x;
			}
			if (position.x <= -STGManager.PlayerMaxPosition.x)
			{
				position.x = -STGManager.PlayerMaxPosition.x;
			}

			Transform.localPosition = position;
		}

		public override void KeyDown(bool[] keys)
		{
			if (!STGManager.IsRunning)
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

			if ((TransformPosition.y >= STGManager.PlayerMaxPosition.y && Vector.y > 0) || (TransformPosition.y <= -STGManager.PlayerMaxPosition.y && Vector.y < 0))
			{
				Vector.y = 0;
			}
			if ((TransformPosition.x >= STGManager.PlayerMaxPosition.x && Vector.x > 0) || (TransformPosition.x <= -STGManager.PlayerMaxPosition.x && Vector.x < 0))
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