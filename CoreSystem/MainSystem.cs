using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Reflection;
using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace NagaisoraFramework
{
	using DataFileSystem;
	using LogSystem;
	using Miedia;
	using STGSystem;

	public static class MainSystem
	{
		public static string Name;

		public static string DataPath = GetDataPath();

		public static STGControler GolbalSTGControler;

		public static PoolManager GolbalPoolManager;

		public static ReplaySystem GolbalReplaySystem;

		public static BGMControl BGMControl;
		public static AudioControl AudioControl;
		public static MidiControl MidiControl;
		
		public static SEManager SEManager;
		public static InputModule InputModule;
		public static STGSystemData STGSystemData;

		public static ScoreData ScoreData;
		public static ConfigData ConfigData;

		public static WaveBGMData[] WaveBGMData;
		public static MidiBGMData[] MidiBGMData;

		public static int PlayerLength;
		public static int RankLength;

		public static float FPS;

		public static System.Random Random;

		public static int gameseed;

		public static long Gametime;
		public static long FixedGameTime;

		public static DateTime DateTime => DateTime.Now;
		public static TimeSpan SystemTime;
		public static TimeSpan RunTime;
		public static TimeSpan TotalRunTime;
		public static TimeSpan PlayerTime;

		public delegate void SelectControl(int STMA);

		public delegate void KeyDownEvent(bool[] bools);
		public static event KeyDownEvent KeyDown;

		//public static event SelectControl SelMove;

		public static event EventHandler OnApplicationQuit;

		public static LogSystem LogSystem;

		static MainSystem()
		{
			AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
		}

		public static void InitLogSystem(string path)
		{
			OutLivel outLivel = OutLivel.Infomation;

			if (Debug.isDebugBuild)
			{
				outLivel = OutLivel.Debug;
			}

			LogSystem = new LogSystem(outLivel, path);

			Application.logMessageReceived += OnLogCallBack;
		}

		public static void InitGolbalPoolManager()
		{
			GolbalPoolManager = new PoolManager();
		}

		public static void MainSystemInit(string name)
		{
			Name = name;
		}

		#region Random
#if UNITY
		public static int InitRandom()
		{
			return InitRandom(UnityEngine.Random.Range(int.MinValue, int.MaxValue));
		}
#else
		public static int InitRandom()
		{
			System.Random random = new System.Random();
			return InitRandom(random.Next(int.MinValue, int.MaxValue));
		}
#endif

		public static int InitRandom(int seed)
		{
			gameseed = seed;
			Random = new System.Random(gameseed);

			return gameseed;
		}

		public static float Seedrandom(float a, float b)
		{
			return (float)Random.NextDouble() * (b - a) + a;
		}

		public static float Seedrandom2(float a)
		{
			if (a == 0f)
			{
				return 0f;
			}
			return ((float)Random.NextDouble() * 2f - 1f) * a;
		}

		public static int RandomInt()
		{
			return Random.Next();
		}

		public static float RandomFloat()
		{
			return (float)Random.NextDouble();
		}

		public static byte RandomByte()
		{
			return (byte)Random.Next(byte.MinValue, byte.MaxValue);
		}

		public static int RandomInt(int min, int max)
		{
			return Random.Next(min, max);
		}

		public static byte RandomByte(int min, int max)
		{
			return (byte)Random.Next(min, max);
		}
		#endregion

		# region Event
		public static void CallKeyDown(ushort keys)
		{
			BitArray bitArray = new BitArray(BitConverter.GetBytes(keys));

			bool[] bools = new bool[bitArray.Count];
			bitArray.CopyTo(bools, 0);

			CallKeyDown(bools);
		}

		public static void CallKeyDown(bool[] keys)
		{
			if (KeyDown == null)
			{
				return;
			}

			KeyDown(keys);
		}

		//public static void CallSelMove(int i)
		//{
		//	SelMove?.Invoke(i);
		//}
		#endregion

		#region Convert
		public static string IntToString(int i)
		{
			return i.ToString();
		}

		public static string FloatToString(float i)
		{
			return i.ToString();
		}

		public static float StringToFloat(string str)
		{
			return float.Parse(str);
		}

		public static int StringToInt(string str)
		{
			return int.Parse(str);
		}
		#endregion

		public static string Readtxt(string[] strs, int linenumber)
		{
			if (linenumber == 0 || linenumber > strs.Length)
			{
				return string.Empty;
			}
			return strs[linenumber].Trim();
		}

		public static string GetDataPath()
		{
			return Path.GetDirectoryName(Application.dataPath);
		}

		public static bool DataPathFileExists(string path)
		{
			if (File.Exists($"{DataPath}\\{path}"))
			{
				return true;
			}
			return false;
		}

		public static bool DataPathDirectoryExists(string path)
		{
			if (Directory.Exists($"{DataPath}\\{path}"))
			{
				return true;
			}
			return false;
		}

		public static AsyncOperation LoadScenseAsync(int i, LoadSceneMode mode)
		{
			return SceneManager.LoadSceneAsync(i, mode);
		}

		public static void SetTimeScale(float i)
		{
			Time.timeScale = i;
		}

		public static void SetAudioSourceScale(AudioSource source, float i)
		{
			source.pitch = i;
		}

		public static IBGMPackData BGMPlay(int index)
		{
			BGMControl.WaveStop();
			BGMControl.MidiStop();

			IBGMPackData packData = null;

			switch (ConfigData.BGMode)
			{
				case 0:
					if (BGMControl.WaveBGMData == null || BGMControl.WaveBGMData.Length == 0)
					{
						throw new Exception("设定了Wave播放模式，但没有Wave文件可播放");
					}
					packData = BGMControl.SelectWaveBGMDataOfIndex(index);
					BGMControl.WavePlay();
					break;
				case 1:
					if (BGMControl.MidiBGMData == null || BGMControl.MidiBGMData.Length == 0)
					{
						throw new Exception("设定了Midi播放模式，但没有Midi文件可播放");
					}
					packData = BGMControl.SelectMidiBGMDataOfIndex(index);
					BGMControl.MidiControl.SetOutputDevice(ConfigData.OutputDeviceName);
					BGMControl.MidiPlay();
					break;
				case 2:
					if (BGMControl.WaveBGMData != null && BGMControl.WaveBGMData.Length != 0)
					{
						packData = BGMControl.SelectWaveBGMDataOfIndex(index);
					}
					else if (BGMControl.MidiBGMData != null && BGMControl.MidiBGMData.Length != 0)
					{
						packData = BGMControl.SelectMidiBGMDataOfIndex(index);
					}
					else
					{
						packData = null;
					}
					return packData;
			}

			return packData;
		}

		public static void UnlockBGM(int i)
		{
			if (i >= ScoreData.BGMUnlockData.Length)
			{
				return;
			}

			ScoreData.BGMUnlockData[i] = true;
		}

		public static EventTriggerListener AddEventTriggerListener(GameObject Object)
		{
			return EventTriggerListener.Get(Object);
		}

		public static IEnumerator MoveToPosition(Transform transform, Vector3 position, float Speed)
		{
			while (transform.localPosition != position)
			{
				transform.localPosition = Vector3.MoveTowards(transform.localPosition, position, Speed * Time.deltaTime);
				yield return 0;
			}

			yield break;
		}

		public static void SetResolution()
		{
			FullScreenMode fullScreenMode = FullScreenMode.Windowed;

			if (ConfigData.DrawMode == 1)
			{
				fullScreenMode = FullScreenMode.FullScreenWindow;
			}

			RefreshRate refreshRate = new RefreshRate()
			{
				denominator = ConfigData.ResolutionDenominator,
				numerator = ConfigData.ResolutionNumerator,
			};

			Debug.Log($"[Framework Kernel] 设定显示分辨率 目标分辨率[{(int)ConfigData.ResolutionX}, {(int)ConfigData.ResolutionY}] {fullScreenMode} {refreshRate.value}Hz");
			Screen.SetResolution((int)ConfigData.ResolutionX, (int)ConfigData.ResolutionY, fullScreenMode, refreshRate);
		}

		public static void LoadConfigData(string path)
		{
			ConfigData = ConfigData.Default;

			FileStream ConfigDataStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite);

			if (ConfigDataStream.Length == 0)
			{
				Debug.Log($"[Framework Kernel] 配置文件不存在, 装载默认设置并保存");

				ConfigFileSystem.SaveConfig(ConfigDataStream, ConfigData);
			}
			else
			{
				Debug.Log($"[Framework Kernel] 装载配置文件");
			}

			try
			{
				ConfigData = ConfigFileSystem.LoadConfig(ConfigDataStream);
			}
			catch (Exception e)
			{
				Debug.LogException(e);
			}

			ConfigDataStream.Close();
		}

		public static void LoadScoreData(string path)
		{
			ScoreData = ScoreDataSystem.ScoreDataLoad(path);
		}

		public static void FlushALLDoneScoreData(string path)
		{
			ScoreData = ScoreData.AllDone();
		}

		public static void LoadWaveBGMData(string path)
		{
			if (!File.Exists(path))
			{
				Debug.Log("[Framework Kernel] 指定的路径中不存在Wave BGM数据，将跳过装载");
				return;
			}

			Debug.Log($"[Framework Kernel] 装载Wave BGM数据");
			WaveBGMData[] WaveBGMData = WaveBGMDataPack.ReadWaveBGMData(path);
			MainSystem.WaveBGMData = WaveBGMData;
		}

		public static void LoadMidiBGMData(string path)
		{
			if (!File.Exists(path))
			{
				Debug.Log("[Framework Kernel] 指定的路径中不存在Midi BGM数据，将跳过装载");
				return;
			}

			Debug.Log($"[Framework Kernel] 装载Midi BGM数据");
			MidiBGMData[] MidiBGMData = MidiBGMDataPack.ReadMidiBGMData(path);
			MainSystem.MidiBGMData = MidiBGMData;
		}

		public static Assembly LoadAssembly(byte[] binary)
		{
			return AppDomain.CurrentDomain.Load(binary);
		}

		public static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
		{
			return AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.GetName().FullName.Split(',')[0] == args.Name.Split(',')[0]);
		}

		public static void OnLogCallBack(string condition, string stackTrace, UnityEngine.LogType type)
		{
			if (LogSystem is null)
			{
				return;
			}

			LogType logType = LogType.Debug;

			switch (type)
			{
				case UnityEngine.LogType.Log:
					logType = LogType.Infomation;
					break;
				case UnityEngine.LogType.Warning:
					logType = LogType.Warning;
					break;
				case UnityEngine.LogType.Error:
					logType = LogType.Error;
					break;
				case UnityEngine.LogType.Exception:
					logType = LogType.Stop;
					break;
				case UnityEngine.LogType.Assert:
					logType = LogType.Fatal;
					break;
			}

			LogSystem.LogOut(condition, logType, $"0x{Thread.CurrentThread.ManagedThreadId:X4}");
		}

		public static void Quit()
		{
			OnApplicationQuit?.Invoke(null, null);

			ScoreData.TotalRunTime = TotalRunTime;
			ScoreDataSystem.ScoreDataSave(ScoreData, DataPathDefine.ScoreData);

			Application.Quit();
		}

	}
}