using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace KeyAuth
{
	// Token: 0x02000004 RID: 4
	public static class encryption
	{
		// Token: 0x0600001E RID: 30 RVA: 0x00003618 File Offset: 0x00001818
		public static string byte_arr_to_str(byte[] ba)
		{
			StringBuilder stringBuilder = new StringBuilder(ba.Length * 2);
			foreach (byte b in ba)
			{
				stringBuilder.AppendFormat("{0:x2}", b);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600001F RID: 31 RVA: 0x0000365C File Offset: 0x0000185C
		public static byte[] str_to_byte_arr(string hex)
		{
			byte[] result;
			try
			{
				int length = hex.Length;
				byte[] array = new byte[length / 2];
				for (int i = 0; i < length; i += 2)
				{
					array[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
				}
				result = array;
			}
			catch
			{
				Console.WriteLine("\n\n  The session has ended, open program again.");
				Thread.Sleep(3500);
				Environment.Exit(0);
				result = null;
			}
			return result;
		}

		// Token: 0x06000020 RID: 32 RVA: 0x000036D0 File Offset: 0x000018D0
		public static string encrypt_string(string plain_text, byte[] key, byte[] iv)
		{
			Aes aes = Aes.Create();
			aes.Mode = CipherMode.CBC;
			aes.Key = key;
			aes.IV = iv;
			string result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (ICryptoTransform cryptoTransform = aes.CreateEncryptor())
				{
					using (CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write))
					{
						byte[] bytes = Encoding.Default.GetBytes(plain_text);
						cryptoStream.Write(bytes, 0, bytes.Length);
						cryptoStream.FlushFinalBlock();
						result = encryption.byte_arr_to_str(memoryStream.ToArray());
					}
				}
			}
			return result;
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00003788 File Offset: 0x00001988
		public static string decrypt_string(string cipher_text, byte[] key, byte[] iv)
		{
			Aes aes = Aes.Create();
			aes.Mode = CipherMode.CBC;
			aes.Key = key;
			aes.IV = iv;
			string @string;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (ICryptoTransform cryptoTransform = aes.CreateDecryptor())
				{
					using (CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write))
					{
						byte[] array = encryption.str_to_byte_arr(cipher_text);
						cryptoStream.Write(array, 0, array.Length);
						cryptoStream.FlushFinalBlock();
						byte[] array2 = memoryStream.ToArray();
						@string = Encoding.Default.GetString(array2, 0, array2.Length);
					}
				}
			}
			return @string;
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00003848 File Offset: 0x00001A48
		public static string iv_key()
		{
			return Guid.NewGuid().ToString().Substring(0, Guid.NewGuid().ToString().IndexOf("-", StringComparison.Ordinal));
		}

		// Token: 0x06000023 RID: 35 RVA: 0x0000388C File Offset: 0x00001A8C
		public static string sha256(string r)
		{
			return encryption.byte_arr_to_str(new SHA256Managed().ComputeHash(Encoding.Default.GetBytes(r)));
		}

		// Token: 0x06000024 RID: 36 RVA: 0x000038A8 File Offset: 0x00001AA8
		public static string encrypt(string message, string enc_key, string iv)
		{
			byte[] bytes = Encoding.Default.GetBytes(encryption.sha256(enc_key).Substring(0, 32));
			byte[] bytes2 = Encoding.Default.GetBytes(encryption.sha256(iv).Substring(0, 16));
			return encryption.encrypt_string(message, bytes, bytes2);
		}

		// Token: 0x06000025 RID: 37 RVA: 0x000038F0 File Offset: 0x00001AF0
		public static string decrypt(string message, string enc_key, string iv)
		{
			byte[] bytes = Encoding.Default.GetBytes(encryption.sha256(enc_key).Substring(0, 32));
			byte[] bytes2 = Encoding.Default.GetBytes(encryption.sha256(iv).Substring(0, 16));
			return encryption.decrypt_string(message, bytes, bytes2);
		}
	}
}
