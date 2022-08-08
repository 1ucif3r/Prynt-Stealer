using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Bunifu.UI.WinForms;
using Bunifu.UI.WinForms.BunifuButton;
using Siticone.UI.WinForms;
using Siticone.UI.WinForms.Enums;

namespace Loader
{
	// Token: 0x0200000B RID: 11
	public partial class Form2 : Form
	{
		// Token: 0x0600003F RID: 63 RVA: 0x0000539F File Offset: 0x0000359F
		public Form2()
		{
			this.InitializeComponent();
		}

		// Token: 0x06000040 RID: 64 RVA: 0x000053AD File Offset: 0x000035AD
		private void bunifuButton1_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		// Token: 0x06000041 RID: 65 RVA: 0x000053B5 File Offset: 0x000035B5
		private void siticoneCircleButton1_Click(object sender, EventArgs e)
		{
			Process.Start("https://t.me/officialpryntsoftware");
		}
	}
}
