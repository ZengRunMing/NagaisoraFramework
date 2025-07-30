using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace NagaisoraFramework.STGSystem
{
	using static MainSystem;

	[Serializable]
	public class STGComponment : CommMonoScriptObject
	{
		[Header("系统引用 (必须)")]
		public STGManager STGManager;

		public Transform Transform;
		public SpriteRenderer SpriteRender;

		//显示设定
		public BlendMode BlendMode
		{
			get
			{
				return m_BlendMode;
			}
			set
			{
				m_BlendMode = value;
				m_BlendChanged = true;
			}
		}
		public Sprite Sprite
		{
			get
			{
				return m_Sprite;
			}
			set
			{
				m_Sprite = value;
				m_SpriteChanged = true;
			}
		}
		public bool FlipX
		{
			get
			{
				return m_FlipX;
			}
			set
			{
				m_FlipX = value;
				m_FlipXChanged = true;
			}
		}
		public int Order
		{
			get
			{
				return m_Order;
			}
			set
			{
				m_Order = value;
				if (SpriteRender != null)
				{
					SpriteRender.sortingOrder = m_Order;
				}
			}
		}
		public float Angle
		{
			get
			{
				return m_Angle;
			}
			set
			{
				m_Angle = value;
				m_RotationChanged = true;
			}
		}
		public float AngleOffsetCompensation
		{
			get
			{
				return m_AngleOffsetCompensation;
			}
			set
			{
				m_AngleOffsetCompensation = value;
				m_RotationChanged = true;
			}
		}
		public float Transparent
		{
			get
			{
				return m_Transparent;
			}
			set
			{
				m_Transparent = value;
				m_ColorChanged = true;
			}
		}
		public Color SpriteColor
		{
			get
			{
				return m_SpriteColor;
			}
			set
			{
				m_SpriteColor = value;
				m_ColorChanged = true;
			}
		}
		public Vector2 Scale
		{
			get
			{
				return m_Scale;
			}
			set
			{
				m_Scale = value;
				m_ScaleChanged = true;
			}
		}
		public float RenderScale
		{
			get
			{
				return m_RenderScale;
			}
			set
			{
				m_RenderScale = value;
				m_ScaleChanged = true;
			}
		}

		//程序设定
		public float DetermineRadius
		{
			get
			{
				return m_DetermineRadius;
			}
			set
			{
				m_DetermineRadius = value;
			}
		}
		public Vector2 DetermineOffset
		{
			get
			{
				return m_DetermineOffset;
			}
			set
			{
				m_DetermineOffset = value;
			}
		}
		public float Direction
		{
			get
			{
				return m_Direction;
			}
			set
			{
				m_Direction = value;
				m_RotationChanged = true;
			}
		}

		private float Accelerate;

		public Vector2 MoveVector
		{
			get
			{
				return m_MoveVector;
			}
			set
			{
				m_MoveVector = value;
			}
		}
		public float Velocity
		{
			get
			{
				return m_velocity;
			}
			set
			{
				if (value > MaxVelocity)
				{
					m_velocity = MaxVelocity;
				}
				else if (value < MinVelocity)
				{
					m_velocity = MinVelocity;
				}
				else
				{
					m_velocity = value;
				}
			}
		}
		public float MaxVelocity
		{
			get
			{
				return m_MaxVelocity;
			}
			set
			{
				m_MaxVelocity = value;
			}
		}
		public float MinVelocity
		{
			get
			{
				return m_MinVelocity;
			}
			set
			{
				m_MinVelocity = value;
			}
		}
		public bool AngleWithDirection
		{
			get
			{
				return m_AngleWithDirection;
			}
			set
			{
				m_AngleWithDirection = value;
			}
		}
		public bool CheckOutSize
		{
			get
			{
				return m_CheckOutSize;
			}
			set
			{
				m_CheckOutSize = value;
			}
		}
		public bool OnMoveToPoint
		{
			get
			{
				return m_OnMoveToPoint;
			}
			set
			{
				m_OnMoveToPoint = value;
			}
		}
		public Vector2 DestPoint
		{
			get
			{
				return m_DestPoint;
			}
			set
			{
				m_DestPoint = value;
			}
		}

		//当前状态
		public uint ThisTime
		{
			get
			{
				return m_ThisTime;
			}
			set
			{
				m_ThisTime = value;
			}
		}
		public bool Disposed
		{
			get
			{
				return m_Disposed;
			}
			set
			{
				m_Disposed = value;
			}
		}
		public Vector2 StartPosition
		{
			get
			{
				return m_StartPosition;
			}
			internal set
			{
				m_StartPosition = value;
			}
		}
		public Vector2 TransformPosition
		{
			get
			{
				return m_TransformPosition;
			}
			set
			{
				m_TransformPosition = value;
				m_PositionChanged = true;
			}
		}

		[Header("系统状态")]
		[SerializeField]
		protected BlendMode m_BlendMode;
		[SerializeField]
		protected Sprite m_Sprite;
		[SerializeField]
		protected bool m_FlipX = false;
		[SerializeField]
		protected int m_Order;
		[SerializeField]
		protected float m_Angle;
		[SerializeField]
		protected float m_AngleOffsetCompensation;
		[SerializeField]
		protected float m_Transparent = 255f;
		[SerializeField]
		protected Color m_SpriteColor = Color.white;
		[SerializeField]
		protected Vector2 m_Scale = Vector2.one;
		[SerializeField]
		protected float m_RenderScale = 1f;
		[SerializeField]
		protected float m_DetermineRadius;
		[SerializeField]
		protected Vector2 m_DetermineOffset;
		[SerializeField]
		protected float m_Direction;
		[SerializeField]
		protected Vector2 m_MoveVector;
		[SerializeField]
		protected float m_velocity;
		[SerializeField]
		protected float m_MaxVelocity = 1000f;
		[SerializeField]
		protected float m_MinVelocity = 0f;
		[SerializeField]
		protected Vector2 m_DestPoint;
		[SerializeField]
		protected Vector2 m_StartPosition;
		[SerializeField]
		protected Vector2 m_TransformPosition;
		[SerializeField]
		protected uint m_ThisTime;
		[SerializeField]
		protected bool m_AngleWithDirection;
		[SerializeField]
		protected bool m_CheckOutSize;
		[SerializeField]
		protected bool m_OnMoveToPoint;
		[SerializeField]
		protected bool m_Disposed = false;
		[SerializeField]
		protected bool m_PositionChanged;
		[SerializeField]
		protected bool m_RotationChanged;
		[SerializeField]
		protected bool m_ScaleChanged;
		[SerializeField]
		protected bool m_SpriteChanged;
		[SerializeField]
		protected bool m_ColorChanged;
		[SerializeField]
		protected bool m_BlendChanged;
		[SerializeField]
		protected bool m_FlipXChanged;

		public Vector2[] LastPositions;
		public readonly float MaxTransparent = 255f;

		public Dictionary<string, ISTGComponmentFlag> ConditionFlags;
		public List<ISTGComponmentFlag> RemovedConditionFlags;
		public Dictionary<string, ISTGComponmentFlag> RunningFlags;
		public List<ISTGComponmentFlag> RemovedRunningFlags;

		public Vector2 VelocityVector => DirectionVector * Velocity;
		public Vector2 DirectionVector => new Vector2(Sin(ADSDitection), Cos(ADSDitection));
		public float ADSDitection => EulerAngles_ADS(Direction);
		public float SpriteRotation => !AngleWithDirection ? Angle : Direction + Angle;

		//系统引用
		public uint GameTime
		{
			get
			{
				return STGManager.GameTime;
			}
		}

		public List<STGComponment> EnemyBullets
		{
			get
			{
				return STGManager.EnemyBullets;
			}
		}

		public List<PlayerBulletControl> PlayerBullets
		{
			get
			{
				return STGManager.PlayerBullets;
			}
		}

		public List<EnemyControl> Enemys
		{
			get
			{
				return STGManager.Enemys;
			}
		}

		public virtual void Awake()
		{
			Transform = gameObject.transform;
		}

		public virtual void OnEnable()
		{

		}

		public virtual void OnDisable()
		{

		}

		public virtual void Init()
		{
			StartPosition = TransformPosition;

			ConditionFlags = new Dictionary<string, ISTGComponmentFlag>();
			RemovedConditionFlags = new List<ISTGComponmentFlag>();
			RunningFlags = new Dictionary<string, ISTGComponmentFlag>();
			RemovedRunningFlags = new List<ISTGComponmentFlag>();

			ThisTime = 0;

			if (Transparent == 0)
			{
				Transparent = MaxTransparent;
			}

			Disposed = false;

			BindUpdateEvent();
		}

		public virtual void InitSpriteRender()
		{
			if (!TryGetComponent(out SpriteRender))
			{
				SpriteRender = gameObject.AddComponent<SpriteRenderer>();
			}
		}

		public virtual void UpdateUnityProperty()
		{
			if (Disposed)
			{
				return;
			}

			if (m_RotationChanged)
			{
				Transform.localEulerAngles = new Vector3(0f, 0f, -SpriteRotation + AngleOffsetCompensation);
			}
			if (m_ScaleChanged)
			{
				Transform.localScale = new Vector3(Scale.x, Scale.y, 1f) * RenderScale;
			}
			if (m_PositionChanged)
			{
				Transform.localPosition = TransformPosition;
			}

			if (SpriteRender == null)
			{
				return;
			}
			if (m_SpriteChanged)
			{
				SpriteRender.sprite = Sprite;
			}
			if (m_ColorChanged)
			{
				SpriteRender.color = new Color(SpriteColor.r, SpriteColor.g, SpriteColor.b, Transparent / 255f);
			}
			if (m_BlendChanged)
			{
				if (BlendMode == BlendMode.Additive)
				{
					SpriteRender.material = STGManager.BlendManager.Blends[BlendMode.Additive];
				}
				else
				{
					SpriteRender.material = STGManager.BlendManager.Blends[BlendMode.AlphaBlend];
				}
			}
			if (m_FlipXChanged)
			{
				SpriteRender.flipX = m_FlipX;
			}
		}

		public virtual void OnUpdate()
		{
			if (Disposed)
			{
				return;
			}

			TransformPosition = TransformPosition;

			Move();
			Velocity += Accelerate;

			FlagCheck();

			if (ThisTime < 5)
			{
				goto Skip;
			}

			if (CheckOutSize && OutSizeCheck())
			{
				BaseDelete();
				return;
			}

			Skip:
			UpdateUnityProperty();
			ClearUnityPropertyUpdateFlags();
			ThisTime++;
		}

		public virtual void ClearUnityPropertyUpdateFlags()
		{
			m_RotationChanged = false;
			m_ScaleChanged = false;
			m_PositionChanged = false;
			m_SpriteChanged = false;
			m_ColorChanged = false;
			m_BlendChanged = false;
			m_FlipXChanged = false;
		}

		public virtual void KeyDown(bool[] keys)
		{

		}

		public virtual void FlagCheck()
		{
			if ((ConditionFlags == null || ConditionFlags.Count == 0) && (RunningFlags == null || RunningFlags.Count == 0))
			{
				return;
			}

			ISTGComponmentFlag[] ConditionFlagsArray = ConditionFlags.Values.ToArray();
			RemovedConditionFlags.Clear();
			foreach (var flag in ConditionFlagsArray)
			{
				if (!flag.Condition())
				{
					continue;
				}

				AddRunningFlags(flag);
				RemovedConditionFlags.Add(flag);
			}

			ISTGComponmentFlag[] RemovedConditionFlagsArray = RemovedConditionFlags.ToArray();
			foreach (var flag in RemovedConditionFlagsArray)
			{
				RemoveFlags(flag);
			}

			ISTGComponmentFlag[] RunningFlagsArray = RunningFlags.Values.ToArray();
			RemovedRunningFlags.Clear();
			foreach (var flag in RunningFlagsArray)
			{
				flag.Action();

				if (!flag.MultipleExecutions)
				{
					RemovedRunningFlags.Add(flag);
				}
			}

			ISTGComponmentFlag[] RemovedRunningFlagsArray = RemovedRunningFlags.ToArray();
			foreach (var flag in RemovedRunningFlagsArray)
			{
				RemoveRunningFlags(flag);
			}
		}

		public virtual void Move()
		{
			if (MoveVector == Vector2.zero)
			{
				return;
			}

			if (OnMoveToPoint)
			{
				MovePoint();
			}

			TransformPosition += MoveVector * Velocity;
		}

		public void MovePoint()
		{
			float distance = GetDistance(DestPoint);
			if (distance > Mathf.Abs(Velocity))
			{
				Direction = GetDirection(DestPoint) * Mathf.Rad2Deg;
				Accelerate = (0f - Velocity) * Velocity / (2f * distance);
				if (Velocity < 0f)
				{
					Velocity = 0f;
				}
			}
			else
			{
				TransformPosition = DestPoint;
				Accelerate = 0f;
				Velocity = 0f;
			}
		}

		public void MoveVectorWithDirection()
		{
			MoveVector = DirectionVector;
		}

		public virtual bool HitCheck(STGComponment Target)
		{
			return HitCheck(Target, DetermineRadius);
		}

		public virtual bool HitCheck(STGComponment Target, float DetermineRadius)
		{
			if (Target == null || Target.m_Disposed)
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

			float py = fy + STGManager.DetermineVector;
			float px = fx + STGManager.DetermineVector;

			float ady = Mathf.Abs(dy);
			float adx = Mathf.Abs(dx);

			if (Scale.x != Scale.y)
			{
				float EADS_Angle = Mathf.Atan2(dy, dx) * 57.29578f - transform.rotation.eulerAngles.z - 90f;
				float EADS = EulerAngles_ADS(EADS_Angle);
				float num1 = Mathf.Pow(dx * dx + dy * dy, 0.5f) * Cos(EADS);
				float num2 = Mathf.Pow(dx * dx + dy * dy, 0.5f) * Sin(EADS);
				if (num1 / px * (num1 / px) + num2 / py * (num2 / py) < 1f)
				{
					return true;
				}
			}
			else
			{
				if (ady < py && adx < px && py * px > dy * dy + dx * dx)
				{
					return true;
				}
			}

			return false;
		}

		public virtual bool OutSizeCheck()
		{
			if (TransformPosition.x > STGManager.MaxPosition.x || TransformPosition.x < -STGManager.MaxPosition.x || TransformPosition.y > STGManager.MaxPosition.y || TransformPosition.y < -STGManager.MaxPosition.y)
			{
				return true;
			}

			return false;
		}

		public virtual void GuidanceControl(STGComponment Target)
		{
			if (Target == null || Target.m_Disposed)
			{
				return;
			}

			float num3 = GetDirection(Target);
			float num4 = Direction * Mathf.Deg2Rad;
			float num5 = num3 - num4;
			if (num5 > (float)Math.PI)
			{
				num5 -= (float)Math.PI * 2f;
			}
			else if (num5 < -(float)Math.PI)
			{
				num5 += (float)Math.PI * 2f;
			}
			if (Mathf.Abs(num5) > 0.02f && GetDistance(Target) > 50f)
			{
				Direction += num5 / 5f * Mathf.Rad2Deg;
			}
			else
			{
				Direction += num5 * Mathf.Rad2Deg;
			}
		}

		public float GetDirection(STGComponment Target)
		{
			return GetDirection(Target.TransformPosition);
		}

		public float GetDirection(Vector2 TargetGamePosition)
		{
			return (float)Math.PI + Mathf.Atan2(TransformPosition.x - TargetGamePosition.x, TransformPosition.y - TargetGamePosition.y);
		}

		public float GetDistance(STGComponment Target)
		{
			return GetDistance(Target.TransformPosition);
		}

		public float GetDistance(Vector2 TargetGamePosition)
		{
			return (TargetGamePosition - TransformPosition).magnitude;
		}

		public void BindUpdateEvent()
		{
			STGManager.OnUpdate += OnUpdate;
		}

		public void UnBindUpdateEvent()
		{
			STGManager.OnUpdate -= OnUpdate;
		}

		public void BindKeyEvent()
		{
			STGManager.KeyDown += KeyDown;
		}

		public void UnBindKeyEvent()
		{
			STGManager.KeyDown -= KeyDown;
		}

		public virtual void AddFlags(params ISTGComponmentFlag[] flags)
		{
			foreach(var flag in flags)
			{
				if (ConditionFlags.ContainsKey(flag.FlagName))
				{
					throw new Exception($"ConditionFlag {flag.FlagName} already exists in {name}.");
				}
				flag.Componment = this;
				ConditionFlags.Add(flag.FlagName, flag);
			}
		}

		public virtual void RemoveFlags(params ISTGComponmentFlag[] flags)
		{
			foreach (var flag in flags)
			{
				ConditionFlags.Remove(flag.FlagName);
			}
		}

		public virtual void AddRunningFlags(params ISTGComponmentFlag[] flags)
		{
			foreach (var flag in flags)
			{
				if (RunningFlags.ContainsKey(flag.FlagName))
				{
					throw new Exception($"RunningFlag {flag.FlagName} already exists in {name}.");
				}
				flag.Componment = this;
				RunningFlags.Add(flag.FlagName, flag);
			}
		}

		public virtual void RemoveRunningFlags(params ISTGComponmentFlag[] flags)
		{
			foreach (var flag in flags)
			{
				RunningFlags.Remove(flag.FlagName);
			}
		}

		public virtual void BaseDelete()
		{
			UnBindUpdateEvent();

			TransformPosition = STGManager.DisablePosition;

			UpdateUnityProperty();
			ClearUnityPropertyUpdateFlags();

			ALLReset();
			Disposed = true;

			STGManager.PoolManager.DeleteObject(GetType(), gameObject);
		}

		public virtual void ALLReset()
		{
			OnMoveToPoint = false;
			CheckOutSize = false;
			
			DestPoint = Vector2.zero;

			MoveVector = Vector2.zero;
			
			DetermineRadius = 0f;
			DetermineOffset = Vector2.zero;

			Order = 0;
			Angle = 0f;
			AngleOffsetCompensation = 0f;
			Transparent = MaxTransparent;

			Scale = Vector2.one;
			Transform.localEulerAngles = Vector3.zero;
			
			Direction = 0f;
			Velocity = 0f;
			MaxVelocity = 0f;
			MinVelocity = 0f;
		}
	}
}
