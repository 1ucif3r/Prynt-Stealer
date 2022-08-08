using System;

namespace Stealer
{
	// Token: 0x02000007 RID: 7
	internal struct Cookie
	{
		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000021 RID: 33 RVA: 0x000028DB File Offset: 0x00000ADB
		// (set) Token: 0x06000022 RID: 34 RVA: 0x000028E3 File Offset: 0x00000AE3
		public string sHostKey { get; set; }

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000023 RID: 35 RVA: 0x000028EC File Offset: 0x00000AEC
		// (set) Token: 0x06000024 RID: 36 RVA: 0x000028F4 File Offset: 0x00000AF4
		public string sName { get; set; }

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000025 RID: 37 RVA: 0x000028FD File Offset: 0x00000AFD
		// (set) Token: 0x06000026 RID: 38 RVA: 0x00002905 File Offset: 0x00000B05
		public string sPath { get; set; }

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000027 RID: 39 RVA: 0x0000290E File Offset: 0x00000B0E
		// (set) Token: 0x06000028 RID: 40 RVA: 0x00002916 File Offset: 0x00000B16
		public string sExpiresUtc { get; set; }

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000029 RID: 41 RVA: 0x0000291F File Offset: 0x00000B1F
		// (set) Token: 0x0600002A RID: 42 RVA: 0x00002927 File Offset: 0x00000B27
		public string sKey { get; set; }

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600002B RID: 43 RVA: 0x00002930 File Offset: 0x00000B30
		// (set) Token: 0x0600002C RID: 44 RVA: 0x00002938 File Offset: 0x00000B38
		public string sValue { get; set; }

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600002D RID: 45 RVA: 0x00002941 File Offset: 0x00000B41
		// (set) Token: 0x0600002E RID: 46 RVA: 0x00002949 File Offset: 0x00000B49
		public string sIsSecure { get; set; }
	}
}
