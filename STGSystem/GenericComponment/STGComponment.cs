using System;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using UnityEngine;

namespace NagaisoraFramework.STGSystem
{
	using static FrameworkMath;

	[Serializable]
	public class STGComponment : CommMonoScriptObject
	{
		public ECLControler ECLControler;

		[Header("系统引用 (必须)")]
		public STGControler STGControler;

		public Transform Transform;
		public SpriteRenderer SpriteRender;

		public STGComponment Parent;

		//显示设定
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
		public bool MoveVectorWithDirection
		{
			get
			{
				return m_MoveVectorWithDirection;
			}
			set
			{
				m_MoveVectorWithDirection = value;
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
		protected bool m_FlipXChanged;
		[SerializeField]
		protected bool m_MoveVectorWithDirection;

		public Vector2[] LastPositions;
		public readonly float MaxTransparent = 255f;

		public Vector2 VelocityVector => DirectionVector * Velocity;
		public Vector2 DirectionVector => new Vector2(Sin(ADSDitection), Cos(ADSDitection));
		public float ADSDitection => EulerAngles_ADS(Direction);
		public float SpriteRotation => !AngleWithDirection ? Angle : Direction + Angle;

		//系统引用
		public uint GameTime
		{
			get
			{
				return STGControler.GameTime;
			}
		}

		//
		Action MoveToPointAction;

		/// <summary>
		/// Unity的Awake函数，在创建实例时调用，不受对象状态影响
		/// 如果需要重写，必须在重写的函数加上base.Awake()或在重写的函数中实现base.Awake()的功能
		/// </summary>
		public virtual void Awake()
		{
			Transform = gameObject.transform;
		}

		/// <summary>
		/// Unity的OnEnable函数，在对象启用时调用
		/// </summary>
		public virtual void OnEnable()
		{

		}

		/// <summary>
		/// Unity的OnDisable函数，在对象禁用时调用
		/// </summary>
		public virtual void OnDisable()
		{

		}

		/// <summary>
		/// 初始化自身的函数
		/// </summary>
		public virtual void Init()
		{
			StartPosition = TransformPosition;

			ThisTime = 0;

			if (Transparent == 0)
			{
				Transparent = MaxTransparent;
			}

			Disposed = false;
		}

		/// <summary>
		/// 初始化自身的函数，带ECL程序数据传入
		/// </summary>
		/// <param name="ECLData">ECL控制程序的数据</param>
		public virtual void Init(ECLData ECLData)
		{
			if (!(ECLData is null))
			{
				ECLControler = new ECLControler(ECLData, STGControler, this);
			}

			Init();
		}

		/// <summary>
		/// 初始化自身的函数，带ECL控制器传入
		/// </summary>
		/// <param name="controler">ECL控制器</param>
		public virtual void Init(ECLControler controler)
		{
			ECLControler = controler;

			Init();
		}

		/// <summary>
		/// 初始化自身的函数，带ECL程序集传入
		/// </summary>
		/// <param name="assembly"></param>
		public virtual void Init(Assembly assembly)
		{
			if (!(assembly is null))
			{
				ECLControler = new ECLControler(assembly, STGControler, this);
			}

			Init();
		}

		/// <summary>
		/// 初始化自身SpriteRender引用的函数
		/// </summary>
		public virtual void InitSpriteRender()
		{
			if (!TryGetComponent(out SpriteRender))
			{
				SpriteRender = gameObject.AddComponent<SpriteRenderer>();
			}
		}

		/// <summary>
		/// 更新Unity对象属性的函数
		/// </summary>
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
			if (m_FlipXChanged)
			{
				SpriteRender.flipX = m_FlipX;
			}
		}

		/// <summary>
		/// 自身的更新函数，用于重写
		/// </summary>
		public virtual void OnUpdate()
		{
			if (Disposed)
			{
				return;
			}

			TransformPosition = TransformPosition;

			if (MoveVectorWithDirection)
			{
				GetMoveVectorWithDirection();
			}

			Move();
			Velocity += Accelerate;

			ECLControler?.OnUpdate();

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

		public virtual void OnSubThreadUpdate()
		{
			if (Disposed)
			{
				return;
			}

			// 子线程更新内容
			// 在此处的执行不涉及Unity API的计算

		}

		/// <summary>
		/// 清除Unity属性更新的Flag
		/// </summary>
		public virtual void ClearUnityPropertyUpdateFlags()
		{
			m_RotationChanged = false;
			m_ScaleChanged = false;
			m_PositionChanged = false;
			m_SpriteChanged = false;
			m_ColorChanged = false;
			m_FlipXChanged = false;
		}

		/// <summary>
		/// 按键事件，用于重写函数，不需要调用
		/// </summary>
		/// <param name="keys">采集的按键</param>
		public virtual void KeyDown(bool[] keys)
		{

		}

		/// <summary>
		/// 移动自身
		/// </summary>
		public virtual void Move()
		{
			if (MoveVector == Vector2.zero || Velocity == 0)
			{
				return;
			}

			if (OnMoveToPoint)
			{
				MovePoint();
			}

			TransformPosition += MoveVector * Velocity;
		}

		/// <summary>
		/// 根据设定的目标点计算移动速度和方向
		/// </summary>
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

				OnMoveToPoint = false;

				MoveToPointAction?.Invoke();

				MoveToPointAction = null;
			}
		}

		public void RegisterMoveToPointAction(Vector2 destPoint, float maxVelocity, float defaultVelovity, Action action = null)
		{
			if (action is null)
			{
				MoveToPointAction = null;
			}
			else
			{
				MoveToPointAction = new Action(action);
			}

			DestPoint = destPoint;
			OnMoveToPoint = true;

			MaxVelocity = maxVelocity;
			MinVelocity = 0f;
			Velocity = defaultVelovity;
		}

		/// <summary>
		/// 将移动向量设置为面向方向
		/// </summary>
		public void GetMoveVectorWithDirection()
		{
			MoveVector = DirectionVector;
		}

		/// <summary>
		/// 检查是否与指定的STGComponment产生碰撞，使用自身参数指定的判定半径
		/// </summary>
		/// <param name="Target">指定的STGComponment</param>
		/// <returns>返回一个布尔值，True则为满足碰撞</returns>
		public virtual bool HitCheck(STGComponment Target)
		{
			return HitCheck(Target, DetermineRadius);
		}

		/// <summary>
		/// 检查是否与指定的STGComponment产生碰撞，指定判定半径
		/// </summary>
		/// <param name="Target">指定的STGComponment</param>
		/// <param name="DetermineRadius">判定的半径</param>
		/// <returns>返回一个布尔值，True则为满足碰撞</returns>
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

			float py = fy + STGControler.DetermineVector;
			float px = fx + STGControler.DetermineVector;

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

		/// <summary>
		/// 检查是否出界
		/// </summary>
		/// <returns>返回一个布尔值，True则为出界</returns>
		public virtual bool OutSizeCheck()
		{
			if (TransformPosition.x > STGControler.MaxPosition.x || TransformPosition.x < -STGControler.MaxPosition.x || TransformPosition.y > STGControler.MaxPosition.y || TransformPosition.y < -STGControler.MaxPosition.y)
			{
				return true;
			}

			return false;
		}

		/// <summary>
		/// 诱导系统
		/// </summary>
		/// <param name="Target">指定的STGComponment</param>
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

		/// <summary>
		/// 获取指定的STGComponment在自身的方向
		/// </summary>
		/// <param name="Target"></param>
		/// <returns>返回方向角度</returns>
		public float GetDirection(STGComponment Target)
		{
			return GetDirection(Target.TransformPosition);
		}

		/// <summary>
		/// 获取指定的坐标在自身的方向
		/// </summary>
		/// <param name="TargetGamePosition">指定的坐标</param>
		/// <returns>返回方向角度</returns>
		public float GetDirection(Vector2 TargetGamePosition)
		{
			return (float)Math.PI + Mathf.Atan2(TransformPosition.x - TargetGamePosition.x, TransformPosition.y - TargetGamePosition.y);
		}

		/// <summary>
		/// 获取与另一个STGComponment的距离
		/// </summary>
		/// <param name="Target">指定的STGComponment</param>
		/// <returns>返回距离</returns>
		public float GetDistance(STGComponment Target)
		{
			return GetDistance(Target.TransformPosition);
		}

		/// <summary>
		/// 获取与指定坐标的距离
		/// </summary>
		/// <param name="TargetGamePosition">指定的坐标</param>
		/// <returns>返回距离</returns>
		public float GetDistance(Vector2 TargetGamePosition)
		{
			return (TargetGamePosition - TransformPosition).magnitude;
		}

		/// <summary>
		/// 向STGControler注册KeyDown事件
		/// </summary>
		public void RegisterKeyEvent()
		{
			STGControler.KeyDown += KeyDown;
		}

		/// <summary>
		/// 向STGControler注销KeyDown事件
		/// </summary>
		public void UnRegisterKeyEvent()
		{
			STGControler.KeyDown -= KeyDown;
		}

		/// <summary>
		/// 删除自身
		/// </summary>
		public virtual void BaseDelete()
		{
			TransformPosition = STGControler.DisablePosition;

			ECLControler?.Stop();
			ECLControler?.Dispose();

			UpdateUnityProperty();
			ClearUnityPropertyUpdateFlags();

			ALLReset();
			Disposed = true;

			STGControler.PoolManager.DeleteObject(GetType(), gameObject);
		}

		/// <summary>
		/// 将所有属性复位
		/// </summary>
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

		public override bool Equals(object obj)
		{
			if (obj == null || GetType() != obj.GetType())
			{
				return false;
			}

			var other = obj as STGComponment;
			return GUIDMD5String == other.GUIDMD5String;
		}

		public override int GetHashCode()
		{
			return GUID.GetHashCode();
		}
	}
}
