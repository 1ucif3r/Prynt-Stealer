using System;
using System.Threading;
using System.Windows.Forms;

namespace Clipper
{
	// Token: 0x0200003E RID: 62
	internal sealed class Clipboard
	{
		// Token: 0x0600010E RID: 270 RVA: 0x00008DC4 File Offset: 0x00006FC4
		public static string GetText()
		{
			string ReturnValue = string.Empty;
			try
			{
				Thread thread = new Thread(delegate()
				{
					ReturnValue = Clipboard.GetText();
				});
				thread.SetApartmentState(ApartmentState.STA);
				thread.Start();
				thread.Join();
			}
			catch
			{
			}
			return ReturnValue;
		}

		// Token: 0x0600010F RID: 271 RVA: 0x00008E20 File Offset: 0x00007020
		public static void SetText(string text)
		{
			Thread thread = new Thread(delegate()
			{
				try
				{
					Clipboard.SetText(text);
				}
				catch
				{
				}
			});
			thread.SetApartmentState(ApartmentState.STA);
			thread.Start();
			thread.Join();
		}
	}
}
