using System;
using System.Windows.Forms;

namespace KeyAuth
{
	// Token: 0x02000007 RID: 7
	internal static class Program
	{
		// Token: 0x06000036 RID: 54 RVA: 0x00005318 File Offset: 0x00003518
		[STAThread]
		private static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new Login());
		}
	}
}
