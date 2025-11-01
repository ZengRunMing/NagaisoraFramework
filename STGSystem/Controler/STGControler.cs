using System;
using System.Threading;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace NagaisoraFramework.STGSystem
{
	public class STGControler : IDisposable
	{
		public string Name;

		public STGControlerInterface Interface;

		public CoroutineManager CoroutineManager;
		public ReplaySystem ReplaySystem;
		public STGSystemData STGSystemData;
		public BlendManager BlendManager;

		public PoolManager PoolManager;

		public NFEMain NFE;

		public ClockSystem ClockSystem;

		public GameObject EnemyEndPrefab;

		public delegate void KeyDownEvent(InputKey inputKey);
		public event KeyDownEvent KeyDown;

		public Thread SubUpdateThread;

		public static List<Timer> Timers;

		public Camera Camera { get { return Interface.Camera; } set { Interface.Camera = value; } }
		public GameObject Parent { get { return Interface.Parent; } set { Interface.Parent = value; } }
		public RawImage BackgroundImage { get { return Interface.BackgroundImage; } set { Interface.BackgroundImage = value; }  }
		
		public Player Player { get { return Interface.Player; } set { Interface.Player = value; } }
		public GameObject PlayerDeterminePoint { get { return Interface.PlayerDeterminePoint; } set { Interface.PlayerDeterminePoint = value; } }

		public GameObject GameObject => Interface.gameObject;

		public RawImage RenderTarget;

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

		public int DefLife = 2;
		public int DefBomb = 3;
		public int MaxEnemyBulletCount = 1000000;
		public float DetermineVector = 3f;
		public float GrazeVector = 15f;
		public Vector2 MaxPosition = new Vector2(350, 350);
		public Vector2 PlayerMaxPosition = new Vector2(270, 310);
		public Vector2 DisablePosition = new Vector2(0, 800);
		public Vector2 PlayerDefaultPositon = new Vector2(0, -250);
		public Sprite DetermineObjectImage;
		public KeyConfig KeyConfig;
		public Vector2 AxisVector;
		public InputKey InputKey;
		public uint BulletEffectCount;
		public uint EnemyBulletCount;
		public uint GameTime = 0;
		public bool IsRunning;
		public bool IsReplaying;
		public bool TestStatus = false;

		public int Life;
		public int Bomb;
		public bool PlayerInvincible;

		[SerializeField]
		private float m_BackgroundTransparent;

		public List<Enemy> Enemys;
		public List<EnemyBullet> EnemyBullets;
		public List<Laser> EnemyLasers;
		public List<PlayerBullet> PlayerBullets;
		public List<Effect> Effects;

		public Enemy[] EnemysArray;
		public EnemyBullet[] EnemyBulletsArray;
		public Laser[] EnemyLasersArray;
		public PlayerBullet[] PlayerBulletsArray;
		public Effect[] EffectsArray;

		public STGControler(string name, STGControlerInterface @interface, ClockSystem clockSystem, KeyConfig keyConfig, RawImage renderTarget)
		{
			// 初始化所有变量
			Name = name;

			Interface = @interface;
			ClockSystem = clockSystem;
			KeyConfig = keyConfig;
			RenderTarget = renderTarget;

			// 创建渲染系统
			RenderTexture renderTexture = new RenderTexture(640, 640, 32, RenderTextureFormat.ARGB32)
			{
				name = $"{Name}_RenderTexture",
				antiAliasing = 0,
				filterMode = FilterMode.Bilinear,
				dimension = UnityEngine.Rendering.TextureDimension.Tex2D,
			};
			Interface.Camera.targetTexture = renderTexture;

			// 注册缓冲池管理器
			PoolManager = MainSystem.AddPoolManager($"{Name}_PoolManager");

			// 创建定时器列表
			Timers = new List<Timer>();

			// 创建对象栈列表
			Enemys = new List<Enemy>();
			EnemyBullets = new List<EnemyBullet>();
			EnemyLasers = new List<Laser>();
			PlayerBullets = new List<PlayerBullet>();
			Effects = new List<Effect>();

			// 创建携程管理器
			if (!Interface.TryGetComponent(out CoroutineManager))
			{
				CoroutineManager = Interface.gameObject.AddComponent<CoroutineManager>();
			}

			// 注册动作录像系统
			ReplaySystem = MainSystem.AddReplaySystem($"{Name}_ReplaySystem", this);

			// 注册时钟事件
			ClockSystem.FixedUpdateClockEvent += FixedUpdate;
			clockSystem.UpdateClockEvent += Update;

			// 注册按键事件
			KeyDown += ReplaySystem.KeyDown;

			// 复位玩家数据
			ResetPlayer();
		}

		public virtual void Dispose()
		{
			// 注销时钟事件
			ClockSystem.FixedUpdateClockEvent -= FixedUpdate;
			ClockSystem.UpdateClockEvent -= Update;

			// 注销按键事件
			KeyDown -= ReplaySystem.KeyDown;

			// 销毁动作录像系统
			ReplaySystem.Dispose();
		}

		public void Update(object sender)
		{
			// 处理按键输入
			InputCheck();

			// 更新数据
			EnemyBulletCount = (uint)EnemyBullets.Count;
			IsReplaying = ReplaySystem != null && ReplaySystem.IsReplaying;

			// 处理低速状态玩家判定点显示
			if (Player.IsSolt)
			{
				PlayerDeterminePoint.transform.localPosition = Player.TransformPosition;
				PlayerDeterminePoint.SetActive(true);
			}
			else if (PlayerDeterminePoint.activeSelf == true)
			{
				PlayerDeterminePoint.SetActive(false);
			}
		}

		public void FixedUpdate(object sender)
		{
			TimeClock();
		}

		public void TimeClock()
		{
			// 如果未运行则跳过
			if (!IsRunning)
			{
				return;
			}

			// 同步录像系统时间
			if (ReplaySystem != null)
			{
				ReplaySystem.GameTime = GameTime;
			}

			// 调用按键事件
			CallKeyDown(InputKey);

			// 更新游戏数据
			OnUpdate();

			// 增加游戏时间
			GameTime++;
		}

		public virtual void OnUpdate()
		{
			ReplaySystem?.OnUpdate();

			foreach (Timer timer in Timers.ToArray())
			{
				timer.OnUpdate();
			}

			NFE?.OnUpdate();

			EnemysArray = Enemys.ToArray();
			EnemyBulletsArray = EnemyBullets.ToArray();
			EnemyLasersArray = EnemyLasers.ToArray();
			PlayerBulletsArray = PlayerBullets.ToArray();
			EffectsArray = Effects.ToArray();

			Player?.OnUpdate();

			foreach (var enemy in EnemysArray)
			{
				enemy?.OnUpdate();
			}

			foreach (var enemyBullet in EnemyBulletsArray)
			{
				enemyBullet?.OnUpdate();
			}

			foreach (var enemyLaser in EnemyLasersArray)
			{
				enemyLaser?.OnUpdate();
			}

			foreach (var playerBullet in PlayerBulletsArray)
			{
				playerBullet?.OnUpdate();
			}

			foreach (var effect in EffectsArray)
			{
				effect?.OnUpdate();
			}
		}

		public virtual void CreateUpdateThread()
		{
			if (!(SubUpdateThread is null))
			{
				return;
			}

			SubUpdateThread = new Thread(new ThreadStart(() =>
			{
				while (true)
				{
					SubThreadUpdate();
				}
			}));
			SubUpdateThread.Start();
		}

		public virtual void SubThreadUpdate()
		{
			if (!IsRunning)
			{
				return;
			}

			Interface.Player?.OnSubThreadUpdate();

			foreach (var enemy in EnemysArray)
			{
				enemy?.OnSubThreadUpdate();
			}

			foreach (var enemyBullet in EnemyBulletsArray)
			{
				enemyBullet?.OnSubThreadUpdate();
			}

			foreach (var enemyLaser in EnemyLasersArray)
			{
				enemyLaser?.OnSubThreadUpdate();
			}

			foreach (var playerBullet in PlayerBulletsArray)
			{
				playerBullet?.OnSubThreadUpdate();
			}

			foreach (var effect in EffectsArray)
			{
				effect?.OnSubThreadUpdate();
			}
		}

		public void InputCheck()
		{
			if (IsReplaying)
			{
				return;
			}

			InputKey inputKey = CreateInputKey();
			BaseInputCheck(inputKey);

			InputKey = inputKey;
		}

		public virtual InputKey CreateInputKey()
		{
			return new InputKey();
		}

		public virtual void BaseInputCheck(InputKey inputKey)
		{
			AxisVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

			if (AxisVector.x != 0 || AxisVector.y != 0)
			{
				if (AxisVector.y > 0.5f)
				{
					inputKey.Up = true;
				}
				if (AxisVector.y < -0.5f)
				{
					inputKey.Down = true;
				}
				if (AxisVector.x < -0.5f)
				{
					inputKey.Left = true;
				}
				if (AxisVector.x > 0.5f)
				{
					inputKey.Right = true;
				}
			}

			if (Input.GetKey(KeyConfig.ShootKey) || Input.GetKey(KeyConfig.J_ShootKey))
			{
				inputKey.Shoot = true;
			}
			if (Input.GetKey(KeyConfig.BombKey) || Input.GetKey(KeyConfig.J_BombKey))
			{
				inputKey.Bomb = true;
			}

			if (Input.GetKey(KeyConfig.Slow) || Input.GetKey(KeyConfig.J_Slow))
			{
				inputKey.Slow = true;
			}
		}

		public void CallKeyDown(InputKey inputKey)
		{
			KeyDown?.Invoke(inputKey);
		}

		public virtual void Reset()
		{
			GameTime = 0;

			if (ReplaySystem != null)
			{
				ReplaySystem.GameTime = GameTime;
			}

			PoolManager.Clear();
		}

		public void ResetPlayer()
		{
			Life = DefLife;
			Bomb = DefBomb;
		}

		public void LifeSub()
		{
			if (!PlayerInvincible)
			{
				Life--;
				PlayerInvincible = true;
			}
		}

		public virtual void Run()
		{
			if (IsRunning)
			{
				return;
			}

			if (TestStatus)
			{
				Player?.gameObject.SetActive(false);
			}
			else
			{
				Player?.gameObject.SetActive(true);
			}

			Player?.Init();

			IsRunning = true;
		}

		public virtual void Stop()
		{
			if (!IsRunning)
			{
				return;
			}

			IsRunning = false;
		}

		public void RegisterTimer(Timer timer)
		{
			Timers.Add(timer);
		}

		public void UnRegisterTimer(Timer timer)
		{
			Timers.Remove(timer);
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

		public (GameObject, T) CreatePlayer<T>() where T : Player
		{
			if (!(Player is null))
			{
				return (Player.gameObject, Player as T);
			}

			GameObject Object = new GameObject();

			if (!Object.TryGetComponent(out T component))
			{
				component = Object.AddComponent<T>();
				component.STGControler = this;
			}

			Player = component;

			return (Object, component);
		}

		public (GameObject, T) NewEnemy<T>(int type, int color, string name, int order, Vector2 position, bool init = true, BlendMode blendMode = BlendMode.AlphaBlend) where T : Enemy
		{
			EnemyInfo EnemyInfo = STGSystemData.Enemy[type];

			GameObject Object = NewObject(typeof(T), name, Parent);

			if (!Object.TryGetComponent(out T component))
			{
				component = Object.AddComponent<T>();
				component.STGControler = this;
			}

			component.TransformPosition = position;
			component.Type = type;
			component.Color = color;
			component.DetermineOffset = EnemyInfo.DetermineOffset;
			component.DetermineRadius = EnemyInfo.DetermineRadius;
			component.Order = order;

			if (init)
			{
				component.Init();
			}

			return (Object, component);
		}

		public (GameObject, T) NewEnemyBullet<T>(int type, int color, string name, int order, Vector2 position, float angle, bool init = true, BlendMode blendMode = BlendMode.AlphaBlend) where T : EnemyBullet
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
			component.BulletData = STGSystemData.EnemyBullet[type];
			component.Color = color;
			component.Order = order;
			component.Direction = angle;


			if (init)
			{
				component.Init();
			}

			return (Object, component);
		}

		public (GameObject, T) NewEnemyLaser<T>(LaserType type, int color, int length, string name, int order, Vector2 position, float angle, bool init = true, BlendMode blendMode = BlendMode.AlphaBlend) where T : Laser
		{
			if (EnemyLasers.Count >= MaxEnemyBulletCount)
			{
				Debug.Log("LaserNumberOutMaxCount");
				return (null, null);
			}

			GameObject Object = NewObject(typeof(T), name, Parent);

			if (!Object.TryGetComponent(out T component))
			{
				component = Object.AddComponent<T>();
				component.STGControler = this;
			}

			component.TransformPosition = Vector2.zero;
			component.HeadPosition = position;
			component.Type = type;
			component.Color = color;
			component.LaserLength = length;
			component.Order = order;
			component.Direction = angle;

			if (init)
			{
				component.Init();
			}

			return (Object, component);
		}

		public (GameObject, T) NewPlayerBullet<T>(int type, string name, int order, Vector2 position, float angle, bool init = true, BlendMode blendMode = BlendMode.AlphaBlend) where T : PlayerBullet
		{
			GameObject Object = NewObject(typeof(T), name, Parent);

			if (!Object.TryGetComponent(out T component))
			{
				component = Object.AddComponent<T>();
				component.STGControler = this;
			}

			component.TransformPosition = position;
			component.BulletData = STGSystemData.PlayerBullet[type];
			component.Order = order;
			component.Direction = angle;

			if (init)
			{
				component.Init();
			}

			return (Object, component);
		}

		public (GameObject, EnemyShootEffect) NewEnemyShootEffect(int color, int order, Vector3 position, bool init = true, BlendMode blendMode = BlendMode.AlphaBlend)
		{
			GameObject Object = NewObject(typeof(EnemyShootEffect), "EnemyShootEffect", Parent);

			if (!Object.TryGetComponent(out EnemyShootEffect component))
			{
				component = Object.AddComponent<EnemyShootEffect>();
				component.STGControler = this;
			}

			component.TransformPosition = position;
			component.Color = color;
			component.Order = order;

			if (init)
			{
				component.Init();
			}

			return (Object, component);
		}

		public (GameObject, ParticleSystemEffect) NewEnemyEndEffect(int order, Vector3 position, bool init = true)
		{
			GameObject Object = NewObjectOfPrefab(typeof(ParticleSystemEffect), "EnemyEndEffect", Parent, EnemyEndPrefab);

			if (!Object.TryGetComponent(out ParticleSystemEffect component))
			{
				component = Object.AddComponent<ParticleSystemEffect>();
			}

			if (component.STGControler is null || component.STGControler != this)
			{
				component.STGControler = this;
			}

			component.TransformPosition = position;
			component.Order = order;

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