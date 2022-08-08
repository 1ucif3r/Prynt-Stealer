using System;
using System.Runtime.InteropServices;

namespace Stealer
{
	// Token: 0x02000005 RID: 5
	public static class cBCrypt
	{
		// Token: 0x06000012 RID: 18
		[DllImport("bcrypt.dll")]
		public static extern uint BCryptOpenAlgorithmProvider(out IntPtr phAlgorithm, [MarshalAs(UnmanagedType.LPWStr)] string pszAlgId, [MarshalAs(UnmanagedType.LPWStr)] string pszImplementation, uint dwFlags);

		// Token: 0x06000013 RID: 19
		[DllImport("bcrypt.dll")]
		public static extern uint BCryptCloseAlgorithmProvider(IntPtr hAlgorithm, uint flags);

		// Token: 0x06000014 RID: 20
		[DllImport("bcrypt.dll")]
		public static extern uint BCryptGetProperty(IntPtr hObject, [MarshalAs(UnmanagedType.LPWStr)] string pszProperty, byte[] pbOutput, int cbOutput, ref int pcbResult, uint flags);

		// Token: 0x06000015 RID: 21
		[DllImport("bcrypt.dll", EntryPoint = "BCryptSetProperty")]
		internal static extern uint BCryptSetAlgorithmProperty(IntPtr hObject, [MarshalAs(UnmanagedType.LPWStr)] string pszProperty, byte[] pbInput, int cbInput, int dwFlags);

		// Token: 0x06000016 RID: 22
		[DllImport("bcrypt.dll")]
		public static extern uint BCryptImportKey(IntPtr hAlgorithm, IntPtr hImportKey, [MarshalAs(UnmanagedType.LPWStr)] string pszBlobType, out IntPtr phKey, IntPtr pbKeyObject, int cbKeyObject, byte[] pbInput, int cbInput, uint dwFlags);

		// Token: 0x06000017 RID: 23
		[DllImport("bcrypt.dll")]
		public static extern uint BCryptDestroyKey(IntPtr hKey);

		// Token: 0x06000018 RID: 24
		[DllImport("bcrypt.dll")]
		public static extern uint BCryptEncrypt(IntPtr hKey, byte[] pbInput, int cbInput, ref cBCrypt.BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO pPaddingInfo, byte[] pbIV, int cbIV, byte[] pbOutput, int cbOutput, ref int pcbResult, uint dwFlags);

		// Token: 0x06000019 RID: 25
		[DllImport("bcrypt.dll")]
		internal static extern uint BCryptDecrypt(IntPtr hKey, byte[] pbInput, int cbInput, ref cBCrypt.BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO pPaddingInfo, byte[] pbIV, int cbIV, byte[] pbOutput, int cbOutput, ref int pcbResult, int dwFlags);

		// Token: 0x04000002 RID: 2
		public const uint ERROR_SUCCESS = 0U;

		// Token: 0x04000003 RID: 3
		public const uint BCRYPT_PAD_PSS = 8U;

		// Token: 0x04000004 RID: 4
		public const uint BCRYPT_PAD_OAEP = 4U;

		// Token: 0x04000005 RID: 5
		public static readonly byte[] BCRYPT_KEY_DATA_BLOB_MAGIC = BitConverter.GetBytes(1296188491);

		// Token: 0x04000006 RID: 6
		public static readonly string BCRYPT_OBJECT_LENGTH = "ObjectLength";

		// Token: 0x04000007 RID: 7
		public static readonly string BCRYPT_CHAIN_MODE_GCM = "ChainingModeGCM";

		// Token: 0x04000008 RID: 8
		public static readonly string BCRYPT_AUTH_TAG_LENGTH = "AuthTagLength";

		// Token: 0x04000009 RID: 9
		public static readonly string BCRYPT_CHAINING_MODE = "ChainingMode";

		// Token: 0x0400000A RID: 10
		public static readonly string BCRYPT_KEY_DATA_BLOB = "KeyDataBlob";

		// Token: 0x0400000B RID: 11
		public static readonly string BCRYPT_AES_ALGORITHM = "AES";

		// Token: 0x0400000C RID: 12
		public static readonly string MS_PRIMITIVE_PROVIDER = "Microsoft Primitive Provider";

		// Token: 0x0400000D RID: 13
		public static readonly int BCRYPT_AUTH_MODE_CHAIN_CALLS_FLAG = 1;

		// Token: 0x0400000E RID: 14
		public static readonly int BCRYPT_INIT_AUTH_MODE_INFO_VERSION = 1;

		// Token: 0x0400000F RID: 15
		public static readonly uint STATUS_AUTH_TAG_MISMATCH = 3221266434U;

		// Token: 0x0200005B RID: 91
		public struct BCRYPT_PSS_PADDING_INFO
		{
			// Token: 0x06000197 RID: 407 RVA: 0x0000DA70 File Offset: 0x0000BC70
			public BCRYPT_PSS_PADDING_INFO(string pszAlgId, int cbSalt)
			{
				this.pszAlgId = pszAlgId;
				this.cbSalt = cbSalt;
			}

			// Token: 0x040000E5 RID: 229
			[MarshalAs(UnmanagedType.LPWStr)]
			public string pszAlgId;

			// Token: 0x040000E6 RID: 230
			public int cbSalt;
		}

		// Token: 0x0200005C RID: 92
		public struct BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO : IDisposable
		{
			// Token: 0x06000198 RID: 408 RVA: 0x0000DA80 File Offset: 0x0000BC80
			public BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO(byte[] iv, byte[] aad, byte[] tag)
			{
				this = default(cBCrypt.BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO);
				this.dwInfoVersion = cBCrypt.BCRYPT_INIT_AUTH_MODE_INFO_VERSION;
				this.cbSize = Marshal.SizeOf(typeof(cBCrypt.BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO));
				if (iv != null)
				{
					this.cbNonce = iv.Length;
					this.pbNonce = Marshal.AllocHGlobal(this.cbNonce);
					Marshal.Copy(iv, 0, this.pbNonce, this.cbNonce);
				}
				if (aad != null)
				{
					this.cbAuthData = aad.Length;
					this.pbAuthData = Marshal.AllocHGlobal(this.cbAuthData);
					Marshal.Copy(aad, 0, this.pbAuthData, this.cbAuthData);
				}
				if (tag != null)
				{
					this.cbTag = tag.Length;
					this.pbTag = Marshal.AllocHGlobal(this.cbTag);
					Marshal.Copy(tag, 0, this.pbTag, this.cbTag);
					this.cbMacContext = tag.Length;
					this.pbMacContext = Marshal.AllocHGlobal(this.cbMacContext);
				}
			}

			// Token: 0x06000199 RID: 409 RVA: 0x0000DB60 File Offset: 0x0000BD60
			public void Dispose()
			{
				if (this.pbNonce != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(this.pbNonce);
				}
				if (this.pbTag != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(this.pbTag);
				}
				if (this.pbAuthData != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(this.pbAuthData);
				}
				if (this.pbMacContext != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(this.pbMacContext);
				}
			}

			// Token: 0x040000E7 RID: 231
			public int cbSize;

			// Token: 0x040000E8 RID: 232
			public int dwInfoVersion;

			// Token: 0x040000E9 RID: 233
			public IntPtr pbNonce;

			// Token: 0x040000EA RID: 234
			public int cbNonce;

			// Token: 0x040000EB RID: 235
			public IntPtr pbAuthData;

			// Token: 0x040000EC RID: 236
			public int cbAuthData;

			// Token: 0x040000ED RID: 237
			public IntPtr pbTag;

			// Token: 0x040000EE RID: 238
			public int cbTag;

			// Token: 0x040000EF RID: 239
			public IntPtr pbMacContext;

			// Token: 0x040000F0 RID: 240
			public int cbMacContext;

			// Token: 0x040000F1 RID: 241
			public int cbAAD;

			// Token: 0x040000F2 RID: 242
			public long cbData;

			// Token: 0x040000F3 RID: 243
			public int dwFlags;
		}

		// Token: 0x0200005D RID: 93
		public struct BCRYPT_KEY_LENGTHS_STRUCT
		{
			// Token: 0x040000F4 RID: 244
			public int dwMinLength;

			// Token: 0x040000F5 RID: 245
			public int dwMaxLength;

			// Token: 0x040000F6 RID: 246
			public int dwIncrement;
		}

		// Token: 0x0200005E RID: 94
		public struct BCRYPT_OAEP_PADDING_INFO
		{
			// Token: 0x0600019A RID: 410 RVA: 0x0000DBE1 File Offset: 0x0000BDE1
			public BCRYPT_OAEP_PADDING_INFO(string alg)
			{
				this.pszAlgId = alg;
				this.pbLabel = IntPtr.Zero;
				this.cbLabel = 0;
			}

			// Token: 0x040000F7 RID: 247
			[MarshalAs(UnmanagedType.LPWStr)]
			public string pszAlgId;

			// Token: 0x040000F8 RID: 248
			public IntPtr pbLabel;

			// Token: 0x040000F9 RID: 249
			public int cbLabel;
		}
	}
}
