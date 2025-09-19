using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;

namespace NagaisoraFramework
{
	public static class WindowsAPI
	{
		[DllImport("Kernel32.dll", SetLastError = true)]
		public static extern SafeFileHandle CreateFile(string FileName, AccessFlag accessFlag, ShareMode shareMode, IntPtr securityAttr, CreateMode createMode, uint flagsAndAttributes, IntPtr templateFile);
		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern bool ReadFile(IntPtr hFile, byte[] lpBuffer, uint nNumberOfBytesToRead, out uint lpNumberOfBytesRead, IntPtr lpOverlapped);
		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern bool WriteFile(IntPtr hFile, byte[] lpBuffer, uint nNumberOfBytesToWrite, out uint lpNumberOfBytesWritten, IntPtr lpOverlapped);
		[DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		public static extern uint SetFilePointer([In] SafeFileHandle hFile, [In] long lDistanceToMove, IntPtr lpDistanceToMoveHigh, [In] EMoveMethod dwMoveMethod);
		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern bool SetFilePointerEx([In] SafeFileHandle hFile, [In] long lDistanceToMove, IntPtr lpDistanceToMoveHigh, [In] EMoveMethod dwMoveMethod);
		[DllImport("kernel32.dll")]
		public static extern SafeFileHandle OpenProcess(uint flag, bool ihh, int processid);
		[DllImport("kernel32.dll")]
		public static extern bool ReadProcessMemory(SafeFileHandle handle, int address, int[] buffer, int size, int[] nor);
		[DllImport("kernel32.dll")]
		public static extern SafeFileHandle WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [In, Out] byte[] buffer, int size, out IntPtr lpNumberOfBytesWritten);
		[DllImport("kernel32", EntryPoint = "CreateRemoteThread")]
		public static extern int CreateRemoteThread(int hProcess, int lpThreadAttributes, int dwStackSize, int lpStartAddress, int lpParameter, int dwCreationFlags, ref int lpThreadId);
		[DllImport("Kernel32.dll")]
		public static extern SafeFileHandle VirtualAllocEx(IntPtr hProcess, int lpAddress, int dwSize, short flAllocationType, short flProtect);
		[DllImport("kernel32.dll", EntryPoint = "CloseHandle")]
		public static extern int CloseHandle(int hObject);
		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern bool CloseHandle(IntPtr hObject);

		public const uint GENERIC_READ = 0x80000000;
		public const uint GENERIC_WRITE = 0x40000000;
		public const uint FILE_SHARE_READ = 0x00000001;
		public const uint FILE_SHARE_WRITE = 0x00000002;
		public const uint OPEN_EXISTING = 3;
		public const uint FILE_ATNRIBUTE_NORMAL = 0x00000080;

		public static List<ManagementObject> GetManagementObjects(WMIPath path)
		{
			string mappedName = $"ManagedObjectResult{path}.mmf";

			MemoryMappedFile file = MemoryMappedFile.CreateNew(mappedName, 1024 * 1024);
			MemoryMappedViewStream stream = file.CreateViewStream();

			ProcessStartInfo processStartInfo = new ProcessStartInfo()
			{
				WorkingDirectory = MainSystem.DataPath,
				FileName = $"{MainSystem.DataPath}/GetManagentObject.exe",
				UseShellExecute = false,
				RedirectStandardInput = false,
				RedirectStandardOutput = false,
				RedirectStandardError = false,
				CreateNoWindow = true,

				Arguments = $"{path} \"{mappedName}\""
			};

			Process process = new Process()
			{
				StartInfo = processStartInfo
			};

			process.Start();
			process.WaitForExit();
			process.Close();

			List<ManagementObject> objects = SerializeSystem.DeserializeBinary<List<ManagementObject>>(stream);

			stream.Close();
			file.Dispose();

			return objects;
		}
	}
}
