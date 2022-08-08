using System;

namespace Stealer
{
	// Token: 0x0200000A RID: 10
	internal struct Site
	{
		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000037 RID: 55 RVA: 0x00002996 File Offset: 0x00000B96
		// (set) Token: 0x06000038 RID: 56 RVA: 0x0000299E File Offset: 0x00000B9E
		public string sUrl { get; set; }

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000039 RID: 57 RVA: 0x000029A7 File Offset: 0x00000BA7
		// (set) Token: 0x0600003A RID: 58 RVA: 0x000029AF File Offset: 0x00000BAF
		public string sTitle { get; set; }

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600003B RID: 59 RVA: 0x000029B8 File Offset: 0x00000BB8
		// (set) Token: 0x0600003C RID: 60 RVA: 0x000029C0 File Offset: 0x00000BC0
		public int iCount { get; set; }
	}
}
