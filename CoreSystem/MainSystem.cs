using System;
using System.Collections;
using System.IO;

using UnityEngine;


namespace NagaisoraFramework
{
	using Miedia;
	using STGSystem;
	using DataFileSystem;
	using UnityEngine.SceneManagement;

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

		public static int GameModer;

		public static int Rank;
		public static int Player;

		public static int PlayerLength;
		public static int RankLength;

		public static float Music_Volume;
		public static float SE_Volume;

		public static int FPS;

		public static float[] zsin;
		public static float[] zcos;
		public static System.Random Random;

		public static int gameseed;

		public static byte[] key;

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

		public static event SelectControl SelMove;

		public static event EventHandler OnApplicationQuit;

		public static float Fps;

		static MainSystem()
		{
			zsin = new float[9000];
			zcos = new float[9000];

			for (int i = 0; i < 9000; i++)
			{
				zsin[i] = Mathf.Sin(i * 0.04f * ((float)Math.PI / 180f));
				zcos[i] = Mathf.Cos(i * 0.04f * ((float)Math.PI / 180f));
			}

			key = new byte[8] { 2, 7, 8, 6, 7, 2, 5, 5 };
		}

		public static void InitGolbalPoolManager()
		{
			GolbalPoolManager = new PoolManager();
		}

		public static void MainSystemInit(string name)
		{
			Name = name;
		}

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

		public static void CallSelMove(int i)
		{
			SelMove?.Invoke(i);
		}

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

		#region EulerAnglesMath

		public static float Sin(float In)
		{
			return zsin[(int)(In * 25f)];
		}

		public static float Cos(float In)
		{
			return zcos[(int)(In * 25f)];
		}

		public static float Distance(Transform a, Transform b)
		{
			float num = Mathf.Pow((a.position.x - b.position.x) * (a.position.x - b.position.x) + (a.position.y - b.position.y) * (a.position.y - b.position.y), 0.5f);
			if (num < 0f)
			{
				num = 0f - num;
			}
			return num;
		}

		public static float EulerAngles_ADS(float a)
		{
			a %= 360f;
			if (a < 0f)
			{
				a += 360f;
			}
			if (a == 360f)
			{
				a = 0f;
			}
			return a;
		}

		public static float EulerAngles_ADS2(float a)
		{
			a %= 360f;
			Debug.Log(a);
			if (a < 0f)
			{
				a += 360f;
			}
			if (a >= 180f)
			{
				a -= 360f;
			}
			if (a == 180f)
			{
				a = -180f;
			}
			return a;
		}

		public static Vector2 GetVectorFromDirection(float Direction)
		{
			return new Vector2(Mathf.Sin(Direction), Mathf.Cos(Direction));
		}

		#endregion

		#region Distances
		public static float Distance(Vector2 p1, Vector2 p2)
		{
			return Mathf.Sqrt((p1.x - p2.x) * (p1.x - p2.x) + (p1.y - p2.y) * (p1.y - p2.y));
		}

		public static float DistanceLine(Vector2 a, Vector2 b, Vector2 c)
		{
			Vector2 vector = b - a;
			Vector2 rhs = c - a;
			float num = Vector2.Dot(vector, rhs);
			if (num < 0f)
			{
				return Distance(c, a);
			}
			float num2 = Vector2.Dot(vector, vector);
			if (num > num2)
			{
				return Distance(c, b);
			}
			num /= num2;
			Vector2 p = a + num * vector;
			return Distance(c, p);
		}

		public static Vector2 Point2lineVerticalPointPosition(Vector2 m, Vector2 a, Vector2 b)
		{
			Vector2 vector = b - a;
			Vector2 rhs = m - a;
			float num = Vector2.Dot(vector, rhs);
			float num2 = Vector2.Dot(vector, vector);
			num /= num2;
			return a + num * vector;
		}
		#endregion

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

		#region Logic

		public static bool IF(float a, float orz, float b)
		{
			bool result = false;
			if (orz == 0f)
			{
				result = a >= b;
			}
			if (orz == 1f)
			{
				result = Mathf.Abs(a - b) <= 0.07f;
			}
			if (orz == 2f)
			{
				result = a < b;
			}
			return result;
		}

		public static float DO(float a, int orz, float b)
		{
			if (orz == 0)
			{
				a += b;
			}
			if (orz == 1)
			{
				a = b;
			}
			if (orz == 2)
			{
				a -= b;
			}
			return a;
		}

		public static bool OR(bool[] i, bool value)
		{
			foreach (bool rds in i)
			{
				if (rds == value)
				{
					return true;
				}
			}
			return false;
		}

		public static bool AND(bool[] i, bool value)
		{
			foreach (bool rds in i)
			{
				if (rds != value)
				{
					return false;
				}
			}
			return true;
		}

		#endregion

		public static float DO_time(float a, int orz, float b, int fangshi, int time_now, float time_fen, float fendu = 0f)
		{
			if (orz != 2)
			{
				if (fangshi <= 0)
				{
					a += fendu;
				}
				if (fangshi == 1)
				{
					a += (Mathf.Sin((float)(time_now + 1) * time_fen * ((float)Math.PI / 180f)) - Mathf.Sin((float)time_now * time_fen * ((float)Math.PI / 180f))) * b;
				}
				if (fangshi == 2)
				{
					a += 0f - (Mathf.Cos((float)(time_now + 1) * time_fen * ((float)Math.PI / 180f)) - Mathf.Cos((float)time_now * time_fen * ((float)Math.PI / 180f))) * b;
				}
			}
			if (orz == 2)
			{
				if (fangshi <= 0)
				{
					a -= fendu;
				}
				if (fangshi == 1)
				{
					a -= (Mathf.Sin((float)(time_now + 1) * time_fen * ((float)Math.PI / 180f)) - Mathf.Sin((float)time_now * time_fen * ((float)Math.PI / 180f))) * b;
				}
				if (fangshi == 2)
				{
					a += (Mathf.Cos((float)(time_now + 1) * time_fen * ((float)Math.PI / 180f)) - Mathf.Cos((float)time_now * time_fen * ((float)Math.PI / 180f))) * b;
				}
			}
			return a;
		}

		#region Scale

		public static float Scale(float IN, float IN_Min, float IN_Max, float OUT_Min, float OUT_Max)
		{
			float Std = IN - IN_Min / IN_Max - IN_Min;
			float SAD = Std * (OUT_Max - OUT_Min) + OUT_Min;
			return SAD;
		}

		public static int Scale(int IN, int IN_Min, int IN_Max, int OUT_Min, int OUT_Max)
		{
			int Std = IN - IN_Min / IN_Max - IN_Min;
			int SAD = Std * (OUT_Max - OUT_Min) + OUT_Min;
			return SAD;
		}

		public static long Scale(long IN, long IN_Min, long IN_Max, long OUT_Min, long OUT_Max)
		{
			long Std = IN - IN_Min / IN_Max - IN_Min;
			long SAD = Std * (OUT_Max - OUT_Min) + OUT_Min;
			return SAD;
		}

		public static double Scale(double IN, double IN_Min, double IN_Max, double OUT_Min, double OUT_Max)
		{
			double Std = IN - IN_Min / IN_Max - IN_Min;
			double SAD = Std * (OUT_Max - OUT_Min) + OUT_Min;
			return SAD;
		}

		#endregion

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

		public static void Quit()
		{
			OnApplicationQuit?.Invoke(null, null);

			ScoreData.TotalRunTime = TotalRunTime;
			ScoreDataSystem.ScoreDataSave(ScoreData, DataPathDefine.ScoreData);
			
			Application.Quit();
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

			Screen.SetResolution((int)ConfigData.ResolutionX, (int)ConfigData.ResolutionY, fullScreenMode, refreshRate);
		}

		public static void LoadConfigData(string path)
		{
			Debug.Log($"装载配置文件");

			ConfigData = ConfigData.Default;

			FileStream ConfigDataStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite);

			if (ConfigDataStream.Length == 0)
			{
				Debug.Log($"配置文件不存在, 装载默认设置并保存");

				ConfigFileSystem.SaveConfig(ConfigDataStream, ConfigData);
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
			Debug.Log("装载ScoreData");
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
				Debug.Log("指定的路径中不存在Wave BGM数据，将跳过装载");
				return;
			}

			Debug.Log($"装载Wave BGM数据");
			WaveBGMData[] WaveBGMData = WaveBGMDataPack.ReadWaveBGMData(path);
			MainSystem.WaveBGMData = WaveBGMData;
		}

		public static void LoadMidiBGMData(string path)
		{
			if (!File.Exists(path))
			{
				Debug.Log("指定的路径中不存在Midi BGM数据，将跳过装载");
				return;
			}

			Debug.Log($"装载Midi BGM数据");
			MidiBGMData[] MidiBGMData = MidiBGMDataPack.ReadMidiBGMData(path);
			MainSystem.MidiBGMData = MidiBGMData;
		}
	}
}