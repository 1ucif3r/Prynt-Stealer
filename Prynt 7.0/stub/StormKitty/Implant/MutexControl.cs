using System;
using System.Threading;

namespace StormKitty.Implant
{
	// Token: 0x02000055 RID: 85
	internal sealed class MutexControl
	{
		// Token: 0x06000183 RID: 387 RVA: 0x0000D4DC File Offset: 0x0000B6DC
		public static void Check()
		{
			bool flag = false;
			new Mutex(false, Config.Mutex, ref flag);
			if (!flag)
			{
				Environment.Exit(1);
			}
		}
	}
}
