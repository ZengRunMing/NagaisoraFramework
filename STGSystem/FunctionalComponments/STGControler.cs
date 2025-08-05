using System;
using System.Collections;
using System.Collections.Generic;
using Diagnostics = System.Diagnostics;

using UnityEngine;
using System.Linq;
using UnityEngine.UI;

namespace NagaisoraFramework.STGSystem
{
	using static MainSystem;

	public class STGControler : CommMonoScriptObject
	{
		public PoolManager PoolManager;

		[Header("系统引用 (必须)")]
		public CoroutineManager CoroutineManager;
		public ReplaySystem ReplaySystem;
		public STGSystemData STGSystemData;
		public BlendManager BlendManager;

		public PlayerControl Player;

		public ECLControler ECLControler;

		public RawImage BackgroundImage;

		public GameObject Parent;

		[Header("系统引用 (选用)")]
		public ClockSystem ClockSystem;
		public STGControler Master;

		[Header("预制体设定")]
		public GameObject EnemyEndPrefab;

		//系统参数
		public ClockMode ClockMode
		{
			get
			{
				return m_ClockMode;
			}
			set
			{
				m_ClockMode = value;
			}
		}
		public bool IsMaster
		{
			get
			{
				return m_IsMaster;
			}
			set
			{
				m_IsMaster = value;
			}
		}
		public bool IsGolbal
		{
			get
			{
				return m_IsGolbal;
			}
			set
			{
				m_IsGolbal = value;
			}
		}
		public int DefLife
		{
			get
			{
				return m_DefLife;
			}
			set
			{
				m_DefLife = value;
			}
		}
		public int DefBomb
		{
			get
			{
				return m_DefBomb;
			}
			set
			{
				m_DefBomb = value;
			}
		}
		public int MaxEnemyBulletCount
		{
			get
			{
				return m_MaxEnemyBulletCount;
			}
			set
			{
				m_MaxEnemyBulletCount = value;
			}
		}
		public float DetermineVector
		{
			get
			{
				return m_DetermineVector;
			}
			set
			{
				m_DetermineVector = value;
			}
		}
		public float GrazeVector
		{
			get
			{
				return m_GrazeVector;
			}
			set
			{
				m_GrazeVector = value;
			}
		}
		public Vector2 MaxPosition
		{
			get
			{
				return m_MaxPosition;
			}
			set
			{
				m_MaxPosition = value;
			}
		}
		public Vector2 PlayerMaxPosition
		{
			get
			{
				return m_PlayerMaxPosition;
			}
			set
			{
				m_PlayerMaxPosition = value;
			}
		}
		public Vector2 DisablePosition
		{
			get
			{
				return m_DisablePosition;
			}
			set
			{
				m_DisablePosition = value;
			}
		}
		public Sprite DetermineObjectImage
		{
			get
			{
				return m_DetermineObjectImage;
			}
			set
			{
				m_DetermineObjectImage = value;
			}
		}
		public KeyConfig KeyConfig
		{
			get 
			{
				return m_KeyConfig;
			}
			set
			{
				m_KeyConfig = value;
			}
		}

		//Status
		public Vector2 AxisVector
		{
			get
			{
				return m_AxisVector;
			}
			set
			{
				m_AxisVector = value;
			}
		}
		public int Life
		{
			get
			{
				return m_Life;
			}
			set
			{
				m_Life = value;
			}
		}
		public int Bomb
		{
			get
			{
				return m_Bomb;
			}
			set
			{
				m_Bomb = value;
			}
		}
		public uint GameTime
		{
			get
			{
				return m_GameTime;
			}
			set
			{
				m_GameTime = value;
			}
		}
		public ushort DownKey
		{
			get
			{
				return m_DownKey;
			}
			set
			{
				m_DownKey = value;
			}
		}
		public float BackgroundTransparent
		{
			get
			{
				return m_BackgroundTransparent;
			}
			set
			{
				m_BackgroundTransparent = value;

				if (BackgroundImage != null)
				{
					BackgroundImage.color = new Color(BackgroundImage.color.r, BackgroundImage.color.g, BackgroundImage.color.b, m_BackgroundTransparent / 255f);
				}
			}
		}
		public uint BulletEffectCount
		{
			get
			{
				return m_BulletEffectCount;
			}
			set
			{
				m_BulletEffectCount = value;
			}
		}
		public uint EnemyBulletCount
		{
			get
			{
				return m_EnemyBulletCount;
			}
			set
			{
				m_EnemyBulletCount = value;
			}
		}
		public bool StatusLock
		{
			get
			{
				return m_StatusLock;
			}
			set
			{
				m_StatusLock = value;
			}
		}
		public bool IsRunning
		{
			get
			{
				return m_IsRunning;
			}
			set
			{
				m_IsRunning = value;
			}
		}
		public bool IsReplaying
		{
			get
			{
				return m_IsReplaying;
			}
			set
			{
				m_IsReplaying = value;
			}
		}
		public bool TestStatus
		{
			get
			{
				return m_TestStatus;
			}
			set
			{
				m_TestStatus = value;
			}
		}
		
		[Header("系统参数")]
		[SerializeField]
		private ClockMode m_ClockMode;
		[SerializeField]
		private bool m_IsMaster = true;
		[SerializeField]
		private bool m_IsGolbal = true;
		[SerializeField]
		private int m_DefLife;
		[SerializeField]
		private int m_DefBomb;
		[SerializeField]
		private int m_MaxEnemyBulletCount;
		[SerializeField]
		private float m_DetermineVector;
		[SerializeField]
		private float m_GrazeVector;
		[SerializeField]
		private Vector2 m_MaxPosition;
		[SerializeField]
		private Vector2 m_PlayerMaxPosition;
		[SerializeField]
		private Vector2 m_DisablePosition;
		[SerializeField]
		private Sprite m_DetermineObjectImage;
		[SerializeField]
		private KeyConfig m_KeyConfig;
		[Header("系统状态")]
		[SerializeField]
		private Vector2 m_AxisVector;
		[SerializeField]
		private int m_Life;
		[SerializeField]
		private int m_Bomb;
		[SerializeField]
		private ushort m_DownKey;
		[SerializeField]
		private float m_BackgroundTransparent;
		[SerializeField]
		private uint m_BulletEffectCount;
		[SerializeField]
		private uint m_EnemyBulletCount;
		[SerializeField]
		private uint m_GameTime = 0;
		[SerializeField]
		private bool m_StatusLock;
		[SerializeField]
		public bool m_IsRunning;
		[SerializeField]
		public bool m_IsReplaying;
		[SerializeField]
		private bool m_TestStatus = false;

		public List<STGComponment> EnemyBullets;
		public List<EnemyControl> Enemys;
		public List<PlayerBulletControl> PlayerBullets;

		//Flags
		public Dictionary<string, ISTGControlerFlag> ConditionFlags;
		public List<ISTGControlerFlag> RemovedConditionFlags;
		public Dictionary<string, ISTGControlerFlag> RunningFlags;
		public List<ISTGControlerFlag> RemovedRunningFlags;

		//Events
		public delegate void ComponentUpdate();
		public event ComponentUpdate OnUpdate;

		public delegate void KeyDownEvent(bool[] bools);
		public event KeyDownEvent KeyDown;

		public void Awake()
		{
			ECLControler = new ECLControler()
			{
				STGControler = this
			};

			ConditionFlags = new Dictionary<string, ISTGControlerFlag>();
			RemovedConditionFlags = new List<ISTGControlerFlag>();
			RunningFlags = new Dictionary<string, ISTGControlerFlag>();
			RemovedRunningFlags = new List<ISTGControlerFlag>();

			if (ClockSystem != null)
			{
				ClockSystem.ClockEvent += TimeClockEvent;
			}

			PoolManager = new PoolManager();

			if (!gameObject.TryGetComponent(out CoroutineManager))
			{
				CoroutineManager = gameObject.AddComponent<CoroutineManager>();
			}

			if (!gameObject.TryGetComponent(out ReplaySystem))
			{
				ReplaySystem = gameObject.AddComponent<ReplaySystem>();
			}

			ReplaySystem.STGControler = this;

			DefLife = 2;
			DefBomb = 2;

			DetermineVector = 3f;
			GrazeVector = 15f;

			PlayerReset();
		}

		public void FlagReset()
		{
			ConditionFlags.Clear();
			RemovedConditionFlags.Clear();
			RunningFlags.Clear();
			RemovedRunningFlags.Clear();
		}

		public void Start()
		{
			if (IsGolbal)
			{
				GolbalSTGControler = this;
			}

			STGSystemData = MainSystem.STGSystemData;
			KeyConfig = KeyConfig.Default;
		}

		public void OnEnable()
		{
			if (!IsMaster && Master == null)
			{
				Debug.LogWarning(new NullReferenceException($"STGControler::[ID = {GUIDMD5String}]设定为从动体模式，但没有为该从动体指定必要的驱动体, 从动体无驱动体将无法正常工作"));
			}

			KeyDown += ReplaySystem.KeyDown;
			OnUpdate += ReplaySystem.OnUpdate;
		}

		public void OnDisable()
		{
			KeyDown -= ReplaySystem.KeyDown;
			OnUpdate -= ReplaySystem.OnUpdate;
		}

		public void Update()
		{
			InputCheck();

			EnemyBulletCount = (uint)EnemyBullets.Count;
			IsReplaying = ReplaySystem != null && ReplaySystem.IsReplaying;
		}

		public void FixedUpdate()
		{
			if (ClockMode == ClockMode.Internal)
			{
				TimeClock();
			}
		}

		public void TimeClockEvent(object sender, Diagnostics.Stopwatch stopwatch)
		{
			if (ClockMode == ClockMode.External)
			{
				TimeClock();
			}
		}

		public void TimeClock()
		{
			if (!IsRunning)
			{
				return;
			}

			if (ReplaySystem != null)
			{
				ReplaySystem.GameTime = GameTime;
			}

			FlagCheck();

			CallKeyDown(DownKey);

			ECLControler?.OnUpdate();

			OnUpdate?.Invoke();

			if (IsMaster)
			{
				GameTime++;
			}
			else if (Master != null)
			{
				Gametime = Master.GameTime;
			}
		}

		public virtual void FlagCheck()
		{
			if ((ConditionFlags == null || ConditionFlags.Count == 0) && (RunningFlags == null || RunningFlags.Count == 0))
			{
				return;
			}

			ISTGControlerFlag[] ConditionFlagsArray = ConditionFlags.Values.ToArray();
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

			ISTGControlerFlag[] RemovedConditionFlagsArray = RemovedConditionFlags.ToArray();
			foreach (var flag in RemovedConditionFlagsArray)
			{
				RemoveFlags(flag);
			}

			ISTGControlerFlag[] RunningFlagsArray = RunningFlags.Values.ToArray();
			RemovedRunningFlags.Clear();
			foreach (var flag in RunningFlagsArray)
			{
				flag.Action();

				if (!flag.MultipleExecutions)
				{
					RemovedRunningFlags.Add(flag);
				}
			}

			ISTGControlerFlag[] RemovedRunningFlagsArray = RemovedRunningFlags.ToArray();
			foreach (var flag in RemovedRunningFlagsArray)
			{
				RemoveRunningFlags(flag);
			}
		}

		public virtual void InputCheck()
		{
			if (IsReplaying)
			{
				return;
			}

			BitArray bitArray = new BitArray(16, false);

			AxisVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

			if (AxisVector.x != 0 || AxisVector.y != 0)
			{
				if (AxisVector.y > 0.5f)
				{
					bitArray[0] = true;
				}
				if (AxisVector.y < -0.5f)
				{
					bitArray[1] = true;
				}
				if (AxisVector.x < -0.5f)
				{
					bitArray[2] = true;
				}
				if (AxisVector.x > 0.5f)
				{
					bitArray[3] = true;
				}
			}

			if (Input.GetKey(KeyConfig.ShootKey) || Input.GetKey(KeyConfig.J_ShootKey))
			{
				bitArray[4] = true;
			}
			if (Input.GetKey(KeyConfig.ShootKey) || Input.GetKey(KeyConfig.J_BombKey))
			{
				bitArray[5] = true;
			}

			if (Input.GetKey(KeyConfig.Slow) || Input.GetKey(KeyConfig.J_Slow))
			{
				bitArray[6] = true;
			}

			byte[] data = new byte[bitArray.Count / 8];
			bitArray.CopyTo(data, 0);
			DownKey = BitConverter.ToUInt16(data, 0);
		}

		public void CallKeyDown(ushort keys)
		{
			DownKey = keys;

			BitArray bitArray = new BitArray(BitConverter.GetBytes(keys));

			bool[] bools = new bool[bitArray.Count];
			bitArray.CopyTo(bools, 0);

			KeyDown?.Invoke(bools);
		}

		public virtual void SystemReset()
		{
			GameTime = 0;

			if (ReplaySystem != null)
			{
				ReplaySystem.GameTime = GameTime;
			}
		}

		public void PlayerReset()
		{
			Life = DefLife;
			Bomb = DefBomb;
		}

		public void LifeSub()
		{
			if (!StatusLock)
			{
				Life--;
				StatusLock = true;
			}
		}

		public virtual void Run()
		{
			if (IsRunning)
			{
				return;
			}

			IsRunning = true;

			ECLControler.Run();
		}

		public virtual void Stop()
		{
			if (!IsRunning)
			{
				return;
			}

			IsRunning = false;

			ECLControler.Stop();
		}

		public virtual void AddFlags(params ISTGControlerFlag[] flags)
		{
			foreach (var flag in flags)
			{
				if (ConditionFlags.ContainsKey(flag.FlagName))
				{
					throw new Exception($"ConditionFlag {flag.FlagName} already exists in {name}.");
				}
				flag.STGControler = this;
				ConditionFlags.Add(flag.FlagName, flag);
			}
		}

		public virtual void RemoveFlags(params ISTGControlerFlag[] flags)
		{
			foreach (var flag in flags)
			{
				ConditionFlags.Remove(flag.FlagName);
			}
		}

		public virtual void AddRunningFlags(params ISTGControlerFlag[] flags)
		{
			foreach (var flag in flags)
			{
				if (RunningFlags.ContainsKey(flag.FlagName))
				{
					throw new Exception($"RunningFlag {flag.FlagName} already exists in {name}.");
				}
				flag.STGControler = this;
				RunningFlags.Add(flag.FlagName, flag);
			}
		}

		public virtual void RemoveRunningFlags(params ISTGControlerFlag[] flags)
		{
			foreach (var flag in flags)
			{
				RunningFlags.Remove(flag.FlagName);
			}
		}

		public GameObject NewObject(Type type, string name, GameObject parent)
		{
			GameObject Object = PoolManager.NewObject(type);

			if (Object.transform.parent != parent.transform)
			{
				Object.transform.SetParent(parent.transform);
			}

			Object.name = name;
			Object.layer = parent.layer;
			Object.transform.localScale = new Vector3(1, 1, 1);

			return Object;
		}

		public GameObject NewObjectOfPrefab(Type type, string name, GameObject parent, GameObject prefab)
		{
			GameObject Object = PoolManager.NewObjectOfPrefab(type, prefab);

			if (Object.transform.parent != parent.transform)
			{
				Object.transform.SetParent(parent.transform);
			}

			Object.name = name;
			Object.layer = parent.layer;
			Object.transform.localScale = new Vector3(1, 1, 1);

			return Object;
		}

		public (GameObject, T) NewEnemy<T>(int type, int color, string name, int order, Vector2 position, bool init = true, BlendMode blendMode = BlendMode.AlphaBlend) where T : EnemyControl
		{
			EnemyInfo EnemyInfo = STGSystemData.Enemy[type];

			GameObject Object = NewObject(typeof(T), name, Parent);

			if (!Object.TryGetComponent(out T component))
			{
				component = Object.AddComponent<T>();
				component.STGControler = this;
			}

			component.TransformPosition = position;
			component.BlendMode = blendMode;
			component.Type = type;
			component.Color = color;
			component.DetermineOffset = EnemyInfo.DetermineOffset;
			component.DetermineRadius = EnemyInfo.DetermineRadius;
			component.Order = 21 + order;

			if (init)
			{
				component.Init();
			}

			return (Object, component);
		}

		public (GameObject, T) NewEnemyBullet<T>(int type, int color, string name, int order, Vector2 position, float angle, bool init = true, BlendMode blendMode = BlendMode.AlphaBlend) where T : EnemyBulletControl
		{
			if (EnemyBullets.Count >= MaxEnemyBulletCount)
			{
				Debug.Log("BulletNumberOutMaxCount");
				return (null, null);
			}

			GameObject Object = NewObject(typeof(T), name, Parent);

			if (!Object.TryGetComponent(out T component))
			{
				component = Object.AddComponent<T>();
				component.STGControler = this;
			}

			component.TransformPosition = position;
			component.BlendMode = blendMode;
			component.BulletData = STGSystemData.EnemyBullet[type];
			component.Color = color;
			component.Order = 21 + order;
			component.Direction = angle;

			if (init)
			{
				component.Init();
			}

			return (Object, component);
		}

		public (GameObject, T) NewEnemyLaser<T>(LaserType type, int color, int length, string name, int order, Vector2 position, float angle, bool init = true, BlendMode blendMode = BlendMode.AlphaBlend) where T : LaserControl
		{
			if (EnemyBullets.Count >= MaxEnemyBulletCount)
			{
				Debug.Log("BulletNumberOutMaxCount");
				return (null, null);
			}

			GameObject Object = NewObject(typeof(T), name, Parent);

			if (!Object.TryGetComponent(out T component))
			{
				component = Object.AddComponent<T>();
				component.STGControler = this;
			}

			component.TransformPosition = Vector2.zero;
			component.BlendMode = blendMode;
			component.HeadPosition = position;
			component.Type = type;
			component.Color = color;
			component.LaserLength = length;
			component.Order = 21 + order;
			component.Direction = angle;

			if (init)
			{
				component.Init();
			}

			return (Object, component);
		}

		public (GameObject, T) NewPlayerBullet<T>(int type, string name, int order, Vector2 position, float angle, bool init = true, BlendMode blendMode = BlendMode.AlphaBlend) where T : PlayerBulletControl
		{
			GameObject Object = NewObject(typeof(T), name, Parent);

			if (!Object.TryGetComponent(out T component))
			{
				component = Object.AddComponent<T>();
				component.STGControler = this;
			}

			component.TransformPosition = position;
			component.BlendMode = blendMode;
			component.BulletData = STGSystemData.PlayerBullet[type];
			component.Order = 21 + order;
			component.Direction = angle;

			if (init)
			{
				component.Init();
			}

			return (Object, component);
		}

		public (GameObject, T) NewEnemyShootEffect<T>(int color, int order, Vector3 position, bool init = true, BlendMode blendMode = BlendMode.AlphaBlend) where T : EnemyShootEffectControl
		{
			GameObject Object = NewObject(typeof(T), "EnemyShootEffect", Parent);

			if (!Object.TryGetComponent(out T component))
			{
				component = Object.AddComponent<T>();
				component.STGControler = this;
			}

			component.TransformPosition = position;
			component.BlendMode = blendMode;
			component.Color = color;
			component.Order = 21 + order;

			BulletEffectCount++;

			if (init)
			{
				component.Init();
			}

			return (Object, component);
		}

		public (GameObject, EnemyEndEffectControl) NewEnemyEndEffect(int order, Vector3 position, bool init = true)
		{
			GameObject Object = NewObjectOfPrefab(typeof(EnemyEndEffectControl), "EnemyEndEffect", Parent, EnemyEndPrefab);

			if (!Object.TryGetComponent(out EnemyEndEffectControl component))
			{
				component = Object.AddComponent<EnemyEndEffectControl>();
			}

			if (component.STGControler is null || component.STGControler != this)
			{
				component.STGControler = this;
			}

			component.TransformPosition = position;
			component.Order = 21 + order;

			BulletEffectCount++;

			if (init)
			{
				component.Init();
			}

			return (Object, component);
		}

		public void SpellCardAttack(string name, uint score)
		{

		}
	}
}