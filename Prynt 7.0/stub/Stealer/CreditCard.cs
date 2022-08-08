using System;

namespace Stealer
{
	// Token: 0x02000008 RID: 8
	internal struct CreditCard
	{
		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600002F RID: 47 RVA: 0x00002952 File Offset: 0x00000B52
		// (set) Token: 0x06000030 RID: 48 RVA: 0x0000295A File Offset: 0x00000B5A
		public string sNumber { get; set; }

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000031 RID: 49 RVA: 0x00002963 File Offset: 0x00000B63
		// (set) Token: 0x06000032 RID: 50 RVA: 0x0000296B File Offset: 0x00000B6B
		public string sExpYear { get; set; }

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000033 RID: 51 RVA: 0x00002974 File Offset: 0x00000B74
		// (set) Token: 0x06000034 RID: 52 RVA: 0x0000297C File Offset: 0x00000B7C
		public string sExpMonth { get; set; }

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000035 RID: 53 RVA: 0x00002985 File Offset: 0x00000B85
		// (set) Token: 0x06000036 RID: 54 RVA: 0x0000298D File Offset: 0x00000B8D
		public string sName { get; set; }
	}
}
