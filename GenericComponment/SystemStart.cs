using System;
using System.Reflection;

using UnityEngine;

namespace NagaisoraFramework
{
	public class SystemStart : CommMonoScriptObject
	{
		public string Name;

		public int PlayerLength;
		public int RankLength;

		public string[] arguments;

		public string ServerPort;
		public string ThisPort;

		public long MaxBullet;

		public virtual void Awake()
		{
			MainSystem.Name = Name;
			
			MainSystem.InitLogSystem($"{MainSystem.DataPath}/engine.log");

			Assembly FrameworkBaseAssembly = typeof(MainSystem).Assembly;
			AssemblyName FrameworkAssemblyName = FrameworkBaseAssembly.GetName();

			Debug.Log($"{FrameworkAssemblyName.Name} [Version {FrameworkAssemblyName.Version}]");
			Debug.Log($"[Framework Kernel] [{SystemInfo.deviceName}] {SystemInfo.operatingSystem}");

			ManagementObject[] cpus = WindowsAPI.GetManagementObjects(WMIPath.Win32_Processor).ToArray();
			Debug.Log($"[Framework Kernel] CPU数量 {cpus.Length}");

			int i = 0;
			foreach (ManagementObject cpu in cpus)
			{
				Debug.Log($"[Framework Kernel] CPU{i} - {cpu.GetValue("Name")} CPUID:{cpu.GetValue("ProcessorId")}");
				i++;
			}

			Debug.Log($"[Framework Kernel] SYSTEM MEMORY: {SystemInfo.systemMemorySize / 1024} GiB");

			ManagementObject[] objects = WindowsAPI.GetManagementObjects(WMIPath.Win32_DiskDrive).ToArray();

			Debug.Log($"[Framework Kernel] 物理磁盘数量 {objects.Length}");

			foreach (ManagementObject drive in objects)
			{
				string interfaceType = (string)drive.GetValue("InterfaceType") ?? "NULL";
				string size = CommonFunctions.GetSzieString(Convert.ToUInt64(drive.GetValue("Size")));

				Debug.Log($"[Framework Kernel] {drive.GetValue("Name")} - Model:{drive.GetValue("Model")} SID:{drive.GetValue("SerialNumber").ToString().TrimEnd('.')} InterfaceType:{interfaceType} Size:{size}");
			}

			Debug.Log($"[Framework Kernel] 初始化随机数系统");
			MainSystem.InitRandom();

			OnStart();

			MainSystem.SetResolution();
		}

		public virtual void OnStart()
		{
			MainSystem.LoadConfigData(DataPathDefine.ConfigData);

			MainSystem.LoadScoreData(DataPathDefine.ScoreData);

			MainSystem.LoadWaveBGMData(DataPathDefine.WaveBGMData);
			MainSystem.LoadMidiBGMData(DataPathDefine.MidiBGMData);
		}
	}
}