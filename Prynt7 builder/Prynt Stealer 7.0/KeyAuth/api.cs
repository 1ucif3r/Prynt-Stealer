using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading;

namespace KeyAuth
{
	// Token: 0x02000003 RID: 3
	public class api
	{
		// Token: 0x06000004 RID: 4 RVA: 0x000020EC File Offset: 0x000002EC
		public api(string name, string ownerid, string secret, string version)
		{
			if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(ownerid) || string.IsNullOrWhiteSpace(secret) || string.IsNullOrWhiteSpace(version))
			{
				api.error("Application not setup correctly. Please watch video link found in Program.cs");
				Environment.Exit(0);
			}
			this.name = name;
			this.ownerid = ownerid;
			this.secret = secret;
			this.version = version;
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00002180 File Offset: 0x00000380
		public void init()
		{
			this.enckey = encryption.sha256(encryption.iv_key());
			string text = encryption.sha256(encryption.iv_key());
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection["type"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes("init"));
			nameValueCollection["ver"] = encryption.encrypt(this.version, this.secret, text);
			nameValueCollection["hash"] = api.checksum(Process.GetCurrentProcess().MainModule.FileName);
			nameValueCollection["enckey"] = encryption.encrypt(this.enckey, this.secret, text);
			nameValueCollection["name"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(this.name));
			nameValueCollection["ownerid"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(this.ownerid));
			nameValueCollection["init_iv"] = text;
			string text2 = api.req(nameValueCollection);
			if (text2 == "KeyAuth_Invalid")
			{
				api.error("Application not found");
				Environment.Exit(0);
			}
			text2 = encryption.decrypt(text2, this.secret, text);
			api.response_structure response_structure = this.response_decoder.string_to_generic<api.response_structure>(text2);
			this.load_response_struct(response_structure);
			if (response_structure.success)
			{
				this.load_app_data(response_structure.appinfo);
				this.sessionid = response_structure.sessionid;
				this.initzalized = true;
				return;
			}
			if (response_structure.message == "invalidver")
			{
				this.app_data.downloadLink = response_structure.download;
			}
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000006 RID: 6 RVA: 0x00002301 File Offset: 0x00000501
		public static bool IsDebugRelease
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002304 File Offset: 0x00000504
		public void Checkinit()
		{
			if (!this.initzalized)
			{
				if (api.IsDebugRelease)
				{
					api.error("Not initialized Check if KeyAuthApp.init() does exist");
					return;
				}
				api.error("Please initialize first");
			}
		}

		// Token: 0x06000008 RID: 8 RVA: 0x0000232C File Offset: 0x0000052C
		public void register(string username, string pass, string key)
		{
			this.Checkinit();
			string value = WindowsIdentity.GetCurrent().User.Value;
			string text = encryption.sha256(encryption.iv_key());
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection["type"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes("register"));
			nameValueCollection["username"] = encryption.encrypt(username, this.enckey, text);
			nameValueCollection["pass"] = encryption.encrypt(pass, this.enckey, text);
			nameValueCollection["key"] = encryption.encrypt(key, this.enckey, text);
			nameValueCollection["hwid"] = encryption.encrypt(value, this.enckey, text);
			nameValueCollection["sessionid"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(this.sessionid));
			nameValueCollection["name"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(this.name));
			nameValueCollection["ownerid"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(this.ownerid));
			nameValueCollection["init_iv"] = text;
			string text2 = api.req(nameValueCollection);
			text2 = encryption.decrypt(text2, this.enckey, text);
			api.response_structure response_structure = this.response_decoder.string_to_generic<api.response_structure>(text2);
			this.load_response_struct(response_structure);
			if (response_structure.success)
			{
				this.load_user_data(response_structure.info);
			}
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002488 File Offset: 0x00000688
		public void login(string username, string pass)
		{
			this.Checkinit();
			string value = WindowsIdentity.GetCurrent().User.Value;
			string text = encryption.sha256(encryption.iv_key());
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection["type"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes("login"));
			nameValueCollection["username"] = encryption.encrypt(username, this.enckey, text);
			nameValueCollection["pass"] = encryption.encrypt(pass, this.enckey, text);
			nameValueCollection["hwid"] = encryption.encrypt(value, this.enckey, text);
			nameValueCollection["sessionid"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(this.sessionid));
			nameValueCollection["name"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(this.name));
			nameValueCollection["ownerid"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(this.ownerid));
			nameValueCollection["init_iv"] = text;
			string text2 = api.req(nameValueCollection);
			text2 = encryption.decrypt(text2, this.enckey, text);
			api.response_structure response_structure = this.response_decoder.string_to_generic<api.response_structure>(text2);
			this.load_response_struct(response_structure);
			if (response_structure.success)
			{
				this.load_user_data(response_structure.info);
			}
		}

		// Token: 0x0600000A RID: 10 RVA: 0x000025CC File Offset: 0x000007CC
		public void upgrade(string username, string key)
		{
			this.Checkinit();
			string value = WindowsIdentity.GetCurrent().User.Value;
			string text = encryption.sha256(encryption.iv_key());
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection["type"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes("upgrade"));
			nameValueCollection["username"] = encryption.encrypt(username, this.enckey, text);
			nameValueCollection["key"] = encryption.encrypt(key, this.enckey, text);
			nameValueCollection["sessionid"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(this.sessionid));
			nameValueCollection["name"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(this.name));
			nameValueCollection["ownerid"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(this.ownerid));
			nameValueCollection["init_iv"] = text;
			string text2 = api.req(nameValueCollection);
			text2 = encryption.decrypt(text2, this.enckey, text);
			api.response_structure response_structure = this.response_decoder.string_to_generic<api.response_structure>(text2);
			response_structure.success = false;
			this.load_response_struct(response_structure);
		}

		// Token: 0x0600000B RID: 11 RVA: 0x000026EC File Offset: 0x000008EC
		public void license(string key)
		{
			this.Checkinit();
			string value = WindowsIdentity.GetCurrent().User.Value;
			string text = encryption.sha256(encryption.iv_key());
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection["type"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes("license"));
			nameValueCollection["key"] = encryption.encrypt(key, this.enckey, text);
			nameValueCollection["hwid"] = encryption.encrypt(value, this.enckey, text);
			nameValueCollection["sessionid"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(this.sessionid));
			nameValueCollection["name"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(this.name));
			nameValueCollection["ownerid"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(this.ownerid));
			nameValueCollection["init_iv"] = text;
			string text2 = api.req(nameValueCollection);
			text2 = encryption.decrypt(text2, this.enckey, text);
			api.response_structure response_structure = this.response_decoder.string_to_generic<api.response_structure>(text2);
			this.load_response_struct(response_structure);
			if (response_structure.success)
			{
				this.load_user_data(response_structure.info);
			}
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002818 File Offset: 0x00000A18
		public void check()
		{
			this.Checkinit();
			string text = encryption.sha256(encryption.iv_key());
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection["type"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes("check"));
			nameValueCollection["sessionid"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(this.sessionid));
			nameValueCollection["name"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(this.name));
			nameValueCollection["ownerid"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(this.ownerid));
			nameValueCollection["init_iv"] = text;
			string text2 = api.req(nameValueCollection);
			text2 = encryption.decrypt(text2, this.enckey, text);
			api.response_structure data = this.response_decoder.string_to_generic<api.response_structure>(text2);
			this.load_response_struct(data);
		}

		// Token: 0x0600000D RID: 13 RVA: 0x000028F0 File Offset: 0x00000AF0
		public void setvar(string var, string data)
		{
			this.Checkinit();
			string text = encryption.sha256(encryption.iv_key());
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection["type"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes("setvar"));
			nameValueCollection["var"] = encryption.encrypt(var, this.enckey, text);
			nameValueCollection["data"] = encryption.encrypt(data, this.enckey, text);
			nameValueCollection["sessionid"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(this.sessionid));
			nameValueCollection["name"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(this.name));
			nameValueCollection["ownerid"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(this.ownerid));
			nameValueCollection["init_iv"] = text;
			string text2 = api.req(nameValueCollection);
			text2 = encryption.decrypt(text2, this.enckey, text);
			api.response_structure data2 = this.response_decoder.string_to_generic<api.response_structure>(text2);
			this.load_response_struct(data2);
		}

		// Token: 0x0600000E RID: 14 RVA: 0x000029F8 File Offset: 0x00000BF8
		public string getvar(string var)
		{
			this.Checkinit();
			string text = encryption.sha256(encryption.iv_key());
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection["type"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes("getvar"));
			nameValueCollection["var"] = encryption.encrypt(var, this.enckey, text);
			nameValueCollection["sessionid"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(this.sessionid));
			nameValueCollection["name"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(this.name));
			nameValueCollection["ownerid"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(this.ownerid));
			nameValueCollection["init_iv"] = text;
			string text2 = api.req(nameValueCollection);
			text2 = encryption.decrypt(text2, this.enckey, text);
			api.response_structure response_structure = this.response_decoder.string_to_generic<api.response_structure>(text2);
			this.load_response_struct(response_structure);
			if (response_structure.success)
			{
				return response_structure.response;
			}
			return null;
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002AF8 File Offset: 0x00000CF8
		public void ban()
		{
			this.Checkinit();
			string text = encryption.sha256(encryption.iv_key());
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection["type"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes("ban"));
			nameValueCollection["sessionid"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(this.sessionid));
			nameValueCollection["name"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(this.name));
			nameValueCollection["ownerid"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(this.ownerid));
			nameValueCollection["init_iv"] = text;
			string text2 = api.req(nameValueCollection);
			text2 = encryption.decrypt(text2, this.enckey, text);
			api.response_structure data = this.response_decoder.string_to_generic<api.response_structure>(text2);
			this.load_response_struct(data);
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00002BD0 File Offset: 0x00000DD0
		public string var(string varid)
		{
			this.Checkinit();
			string value = WindowsIdentity.GetCurrent().User.Value;
			string text = encryption.sha256(encryption.iv_key());
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection["type"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes("var"));
			nameValueCollection["varid"] = encryption.encrypt(varid, this.enckey, text);
			nameValueCollection["sessionid"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(this.sessionid));
			nameValueCollection["name"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(this.name));
			nameValueCollection["ownerid"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(this.ownerid));
			nameValueCollection["init_iv"] = text;
			string text2 = api.req(nameValueCollection);
			text2 = encryption.decrypt(text2, this.enckey, text);
			api.response_structure response_structure = this.response_decoder.string_to_generic<api.response_structure>(text2);
			this.load_response_struct(response_structure);
			if (response_structure.success)
			{
				return response_structure.message;
			}
			return null;
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002CE0 File Offset: 0x00000EE0
		public List<api.msg> chatget(string channelname)
		{
			this.Checkinit();
			string text = encryption.sha256(encryption.iv_key());
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection["type"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes("chatget"));
			nameValueCollection["channel"] = encryption.encrypt(channelname, this.enckey, text);
			nameValueCollection["sessionid"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(this.sessionid));
			nameValueCollection["name"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(this.name));
			nameValueCollection["ownerid"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(this.ownerid));
			nameValueCollection["init_iv"] = text;
			string text2 = api.req(nameValueCollection);
			text2 = encryption.decrypt(text2, this.enckey, text);
			api.response_structure response_structure = this.response_decoder.string_to_generic<api.response_structure>(text2);
			this.load_response_struct(response_structure);
			if (response_structure.success)
			{
				return response_structure.messages;
			}
			return null;
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002DE0 File Offset: 0x00000FE0
		public bool chatsend(string msg, string channelname)
		{
			this.Checkinit();
			string text = encryption.sha256(encryption.iv_key());
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection["type"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes("chatsend"));
			nameValueCollection["message"] = encryption.encrypt(msg, this.enckey, text);
			nameValueCollection["channel"] = encryption.encrypt(channelname, this.enckey, text);
			nameValueCollection["sessionid"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(this.sessionid));
			nameValueCollection["name"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(this.name));
			nameValueCollection["ownerid"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(this.ownerid));
			nameValueCollection["init_iv"] = text;
			string text2 = api.req(nameValueCollection);
			text2 = encryption.decrypt(text2, this.enckey, text);
			api.response_structure response_structure = this.response_decoder.string_to_generic<api.response_structure>(text2);
			this.load_response_struct(response_structure);
			return response_structure.success;
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002EF4 File Offset: 0x000010F4
		public bool checkblack()
		{
			this.Checkinit();
			string value = WindowsIdentity.GetCurrent().User.Value;
			string text = encryption.sha256(encryption.iv_key());
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection["type"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes("checkblacklist"));
			nameValueCollection["hwid"] = encryption.encrypt(value, this.enckey, text);
			nameValueCollection["sessionid"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(this.sessionid));
			nameValueCollection["name"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(this.name));
			nameValueCollection["ownerid"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(this.ownerid));
			nameValueCollection["init_iv"] = text;
			string text2 = api.req(nameValueCollection);
			text2 = encryption.decrypt(text2, this.enckey, text);
			api.response_structure response_structure = this.response_decoder.string_to_generic<api.response_structure>(text2);
			this.load_response_struct(response_structure);
			return response_structure.success;
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00003000 File Offset: 0x00001200
		public string webhook(string webid, string param, string body = "", string conttype = "")
		{
			this.Checkinit();
			string text = encryption.sha256(encryption.iv_key());
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection["type"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes("webhook"));
			nameValueCollection["webid"] = encryption.encrypt(webid, this.enckey, text);
			nameValueCollection["params"] = encryption.encrypt(param, this.enckey, text);
			nameValueCollection["body"] = encryption.encrypt(body, this.enckey, text);
			nameValueCollection["conttype"] = encryption.encrypt(conttype, this.enckey, text);
			nameValueCollection["sessionid"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(this.sessionid));
			nameValueCollection["name"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(this.name));
			nameValueCollection["ownerid"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(this.ownerid));
			nameValueCollection["init_iv"] = text;
			string text2 = api.req(nameValueCollection);
			text2 = encryption.decrypt(text2, this.enckey, text);
			api.response_structure response_structure = this.response_decoder.string_to_generic<api.response_structure>(text2);
			this.load_response_struct(response_structure);
			if (response_structure.success)
			{
				return response_structure.response;
			}
			return null;
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00003148 File Offset: 0x00001348
		public byte[] download(string fileid)
		{
			this.Checkinit();
			string text = encryption.sha256(encryption.iv_key());
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection["type"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes("file"));
			nameValueCollection["fileid"] = encryption.encrypt(fileid, this.enckey, text);
			nameValueCollection["sessionid"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(this.sessionid));
			nameValueCollection["name"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(this.name));
			nameValueCollection["ownerid"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(this.ownerid));
			nameValueCollection["init_iv"] = text;
			string text2 = api.req(nameValueCollection);
			text2 = encryption.decrypt(text2, this.enckey, text);
			api.response_structure response_structure = this.response_decoder.string_to_generic<api.response_structure>(text2);
			this.load_response_struct(response_structure);
			if (response_structure.success)
			{
				return encryption.str_to_byte_arr(response_structure.contents);
			}
			return null;
		}

		// Token: 0x06000016 RID: 22 RVA: 0x0000324C File Offset: 0x0000144C
		public void log(string message)
		{
			this.Checkinit();
			string text = encryption.sha256(encryption.iv_key());
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection["type"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes("log"));
			nameValueCollection["pcuser"] = encryption.encrypt(Environment.UserName, this.enckey, text);
			nameValueCollection["message"] = encryption.encrypt(message, this.enckey, text);
			nameValueCollection["sessionid"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(this.sessionid));
			nameValueCollection["name"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(this.name));
			nameValueCollection["ownerid"] = encryption.byte_arr_to_str(Encoding.Default.GetBytes(this.ownerid));
			nameValueCollection["init_iv"] = text;
			api.req(nameValueCollection);
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00003334 File Offset: 0x00001534
		public static string checksum(string filename)
		{
			string result;
			using (MD5 md = MD5.Create())
			{
				using (FileStream fileStream = File.OpenRead(filename))
				{
					result = BitConverter.ToString(md.ComputeHash(fileStream)).Replace("-", "").ToLowerInvariant();
				}
			}
			return result;
		}

		// Token: 0x06000018 RID: 24 RVA: 0x000033A4 File Offset: 0x000015A4
		public static void error(string message)
		{
			Process.Start(new ProcessStartInfo("cmd.exe", "/c start cmd /C \"color b && title Error && echo " + message + " && timeout /t 5\"")
			{
				CreateNoWindow = true,
				RedirectStandardOutput = true,
				RedirectStandardError = true,
				UseShellExecute = false
			});
			Environment.Exit(0);
		}

		// Token: 0x06000019 RID: 25 RVA: 0x000033F4 File Offset: 0x000015F4
		private static string req(NameValueCollection post_data)
		{
			string result;
			try
			{
				using (WebClient webClient = new WebClient())
				{
					byte[] bytes = webClient.UploadValues("https://keyauth.win/api/1.0/", post_data);
					result = Encoding.Default.GetString(bytes);
				}
			}
			catch (WebException ex)
			{
				if (((HttpWebResponse)ex.Response).StatusCode == (HttpStatusCode)429)
				{
					Thread.Sleep(1000);
					result = api.req(post_data);
				}
				else
				{
					api.error("Connection failure. Please try again, or contact us for help.");
					Environment.Exit(0);
					result = "";
				}
			}
			return result;
		}

		// Token: 0x0600001A RID: 26 RVA: 0x0000348C File Offset: 0x0000168C
		private void load_app_data(api.app_data_structure data)
		{
			this.app_data.numUsers = data.numUsers;
			this.app_data.numOnlineUsers = data.numOnlineUsers;
			this.app_data.numKeys = data.numKeys;
			this.app_data.version = data.version;
			this.app_data.customerPanelLink = data.customerPanelLink;
		}

		// Token: 0x0600001B RID: 27 RVA: 0x000034F0 File Offset: 0x000016F0
		private void load_user_data(api.user_data_structure data)
		{
			this.user_data.username = data.username;
			this.user_data.ip = data.ip;
			this.user_data.hwid = data.hwid;
			this.user_data.createdate = data.createdate;
			this.user_data.lastlogin = data.lastlogin;
			this.user_data.subscriptions = data.subscriptions;
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00003564 File Offset: 0x00001764
		public string expirydaysleft()
		{
			this.Checkinit();
			DateTime d = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Local);
			d = d.AddSeconds((double)long.Parse(this.user_data.subscriptions[0].expiry)).ToLocalTime();
			TimeSpan timeSpan = d - DateTime.Now;
			return Convert.ToString(timeSpan.Days.ToString() + " Days " + timeSpan.Hours.ToString() + " Hours Left");
		}

		// Token: 0x0600001D RID: 29 RVA: 0x000035F4 File Offset: 0x000017F4
		private void load_response_struct(api.response_structure data)
		{
			this.response.success = data.success;
			this.response.message = data.message;
		}

		// Token: 0x04000001 RID: 1
		public string name;

		// Token: 0x04000002 RID: 2
		public string ownerid;

		// Token: 0x04000003 RID: 3
		public string secret;

		// Token: 0x04000004 RID: 4
		public string version;

		// Token: 0x04000005 RID: 5
		private string sessionid;

		// Token: 0x04000006 RID: 6
		private string enckey;

		// Token: 0x04000007 RID: 7
		private bool initzalized;

		// Token: 0x04000008 RID: 8
		public api.app_data_class app_data = new api.app_data_class();

		// Token: 0x04000009 RID: 9
		public api.user_data_class user_data = new api.user_data_class();

		// Token: 0x0400000A RID: 10
		public api.response_class response = new api.response_class();

		// Token: 0x0400000B RID: 11
		private json_wrapper response_decoder = new json_wrapper(new api.response_structure());

		// Token: 0x02000017 RID: 23
		[DataContract]
		private class response_structure
		{
			// Token: 0x17000006 RID: 6
			// (get) Token: 0x06000075 RID: 117 RVA: 0x0000AD02 File Offset: 0x00008F02
			// (set) Token: 0x06000076 RID: 118 RVA: 0x0000AD0A File Offset: 0x00008F0A
			[DataMember]
			public bool success { get; set; }

			// Token: 0x17000007 RID: 7
			// (get) Token: 0x06000077 RID: 119 RVA: 0x0000AD13 File Offset: 0x00008F13
			// (set) Token: 0x06000078 RID: 120 RVA: 0x0000AD1B File Offset: 0x00008F1B
			[DataMember]
			public string sessionid { get; set; }

			// Token: 0x17000008 RID: 8
			// (get) Token: 0x06000079 RID: 121 RVA: 0x0000AD24 File Offset: 0x00008F24
			// (set) Token: 0x0600007A RID: 122 RVA: 0x0000AD2C File Offset: 0x00008F2C
			[DataMember]
			public string contents { get; set; }

			// Token: 0x17000009 RID: 9
			// (get) Token: 0x0600007B RID: 123 RVA: 0x0000AD35 File Offset: 0x00008F35
			// (set) Token: 0x0600007C RID: 124 RVA: 0x0000AD3D File Offset: 0x00008F3D
			[DataMember]
			public string response { get; set; }

			// Token: 0x1700000A RID: 10
			// (get) Token: 0x0600007D RID: 125 RVA: 0x0000AD46 File Offset: 0x00008F46
			// (set) Token: 0x0600007E RID: 126 RVA: 0x0000AD4E File Offset: 0x00008F4E
			[DataMember]
			public string message { get; set; }

			// Token: 0x1700000B RID: 11
			// (get) Token: 0x0600007F RID: 127 RVA: 0x0000AD57 File Offset: 0x00008F57
			// (set) Token: 0x06000080 RID: 128 RVA: 0x0000AD5F File Offset: 0x00008F5F
			[DataMember]
			public string download { get; set; }

			// Token: 0x1700000C RID: 12
			// (get) Token: 0x06000081 RID: 129 RVA: 0x0000AD68 File Offset: 0x00008F68
			// (set) Token: 0x06000082 RID: 130 RVA: 0x0000AD70 File Offset: 0x00008F70
			[DataMember(IsRequired = false, EmitDefaultValue = false)]
			public api.user_data_structure info { get; set; }

			// Token: 0x1700000D RID: 13
			// (get) Token: 0x06000083 RID: 131 RVA: 0x0000AD79 File Offset: 0x00008F79
			// (set) Token: 0x06000084 RID: 132 RVA: 0x0000AD81 File Offset: 0x00008F81
			[DataMember(IsRequired = false, EmitDefaultValue = false)]
			public api.app_data_structure appinfo { get; set; }

			// Token: 0x1700000E RID: 14
			// (get) Token: 0x06000085 RID: 133 RVA: 0x0000AD8A File Offset: 0x00008F8A
			// (set) Token: 0x06000086 RID: 134 RVA: 0x0000AD92 File Offset: 0x00008F92
			[DataMember]
			public List<api.msg> messages { get; set; }
		}

		// Token: 0x02000018 RID: 24
		public class msg
		{
			// Token: 0x1700000F RID: 15
			// (get) Token: 0x06000088 RID: 136 RVA: 0x0000ADA3 File Offset: 0x00008FA3
			// (set) Token: 0x06000089 RID: 137 RVA: 0x0000ADAB File Offset: 0x00008FAB
			public string message { get; set; }

			// Token: 0x17000010 RID: 16
			// (get) Token: 0x0600008A RID: 138 RVA: 0x0000ADB4 File Offset: 0x00008FB4
			// (set) Token: 0x0600008B RID: 139 RVA: 0x0000ADBC File Offset: 0x00008FBC
			public string author { get; set; }

			// Token: 0x17000011 RID: 17
			// (get) Token: 0x0600008C RID: 140 RVA: 0x0000ADC5 File Offset: 0x00008FC5
			// (set) Token: 0x0600008D RID: 141 RVA: 0x0000ADCD File Offset: 0x00008FCD
			public string timestamp { get; set; }
		}

		// Token: 0x02000019 RID: 25
		[DataContract]
		private class user_data_structure
		{
			// Token: 0x17000012 RID: 18
			// (get) Token: 0x0600008F RID: 143 RVA: 0x0000ADDE File Offset: 0x00008FDE
			// (set) Token: 0x06000090 RID: 144 RVA: 0x0000ADE6 File Offset: 0x00008FE6
			[DataMember]
			public string username { get; set; }

			// Token: 0x17000013 RID: 19
			// (get) Token: 0x06000091 RID: 145 RVA: 0x0000ADEF File Offset: 0x00008FEF
			// (set) Token: 0x06000092 RID: 146 RVA: 0x0000ADF7 File Offset: 0x00008FF7
			[DataMember]
			public string ip { get; set; }

			// Token: 0x17000014 RID: 20
			// (get) Token: 0x06000093 RID: 147 RVA: 0x0000AE00 File Offset: 0x00009000
			// (set) Token: 0x06000094 RID: 148 RVA: 0x0000AE08 File Offset: 0x00009008
			[DataMember]
			public string hwid { get; set; }

			// Token: 0x17000015 RID: 21
			// (get) Token: 0x06000095 RID: 149 RVA: 0x0000AE11 File Offset: 0x00009011
			// (set) Token: 0x06000096 RID: 150 RVA: 0x0000AE19 File Offset: 0x00009019
			[DataMember]
			public string createdate { get; set; }

			// Token: 0x17000016 RID: 22
			// (get) Token: 0x06000097 RID: 151 RVA: 0x0000AE22 File Offset: 0x00009022
			// (set) Token: 0x06000098 RID: 152 RVA: 0x0000AE2A File Offset: 0x0000902A
			[DataMember]
			public string lastlogin { get; set; }

			// Token: 0x17000017 RID: 23
			// (get) Token: 0x06000099 RID: 153 RVA: 0x0000AE33 File Offset: 0x00009033
			// (set) Token: 0x0600009A RID: 154 RVA: 0x0000AE3B File Offset: 0x0000903B
			[DataMember]
			public List<api.Data> subscriptions { get; set; }
		}

		// Token: 0x0200001A RID: 26
		[DataContract]
		private class app_data_structure
		{
			// Token: 0x17000018 RID: 24
			// (get) Token: 0x0600009C RID: 156 RVA: 0x0000AE4C File Offset: 0x0000904C
			// (set) Token: 0x0600009D RID: 157 RVA: 0x0000AE54 File Offset: 0x00009054
			[DataMember]
			public string numUsers { get; set; }

			// Token: 0x17000019 RID: 25
			// (get) Token: 0x0600009E RID: 158 RVA: 0x0000AE5D File Offset: 0x0000905D
			// (set) Token: 0x0600009F RID: 159 RVA: 0x0000AE65 File Offset: 0x00009065
			[DataMember]
			public string numOnlineUsers { get; set; }

			// Token: 0x1700001A RID: 26
			// (get) Token: 0x060000A0 RID: 160 RVA: 0x0000AE6E File Offset: 0x0000906E
			// (set) Token: 0x060000A1 RID: 161 RVA: 0x0000AE76 File Offset: 0x00009076
			[DataMember]
			public string numKeys { get; set; }

			// Token: 0x1700001B RID: 27
			// (get) Token: 0x060000A2 RID: 162 RVA: 0x0000AE7F File Offset: 0x0000907F
			// (set) Token: 0x060000A3 RID: 163 RVA: 0x0000AE87 File Offset: 0x00009087
			[DataMember]
			public string version { get; set; }

			// Token: 0x1700001C RID: 28
			// (get) Token: 0x060000A4 RID: 164 RVA: 0x0000AE90 File Offset: 0x00009090
			// (set) Token: 0x060000A5 RID: 165 RVA: 0x0000AE98 File Offset: 0x00009098
			[DataMember]
			public string customerPanelLink { get; set; }

			// Token: 0x1700001D RID: 29
			// (get) Token: 0x060000A6 RID: 166 RVA: 0x0000AEA1 File Offset: 0x000090A1
			// (set) Token: 0x060000A7 RID: 167 RVA: 0x0000AEA9 File Offset: 0x000090A9
			[DataMember]
			public string downloadLink { get; set; }
		}

		// Token: 0x0200001B RID: 27
		public class app_data_class
		{
			// Token: 0x1700001E RID: 30
			// (get) Token: 0x060000A9 RID: 169 RVA: 0x0000AEBA File Offset: 0x000090BA
			// (set) Token: 0x060000AA RID: 170 RVA: 0x0000AEC2 File Offset: 0x000090C2
			public string numUsers { get; set; }

			// Token: 0x1700001F RID: 31
			// (get) Token: 0x060000AB RID: 171 RVA: 0x0000AECB File Offset: 0x000090CB
			// (set) Token: 0x060000AC RID: 172 RVA: 0x0000AED3 File Offset: 0x000090D3
			public string numOnlineUsers { get; set; }

			// Token: 0x17000020 RID: 32
			// (get) Token: 0x060000AD RID: 173 RVA: 0x0000AEDC File Offset: 0x000090DC
			// (set) Token: 0x060000AE RID: 174 RVA: 0x0000AEE4 File Offset: 0x000090E4
			public string numKeys { get; set; }

			// Token: 0x17000021 RID: 33
			// (get) Token: 0x060000AF RID: 175 RVA: 0x0000AEED File Offset: 0x000090ED
			// (set) Token: 0x060000B0 RID: 176 RVA: 0x0000AEF5 File Offset: 0x000090F5
			public string version { get; set; }

			// Token: 0x17000022 RID: 34
			// (get) Token: 0x060000B1 RID: 177 RVA: 0x0000AEFE File Offset: 0x000090FE
			// (set) Token: 0x060000B2 RID: 178 RVA: 0x0000AF06 File Offset: 0x00009106
			public string customerPanelLink { get; set; }

			// Token: 0x17000023 RID: 35
			// (get) Token: 0x060000B3 RID: 179 RVA: 0x0000AF0F File Offset: 0x0000910F
			// (set) Token: 0x060000B4 RID: 180 RVA: 0x0000AF17 File Offset: 0x00009117
			public string downloadLink { get; set; }
		}

		// Token: 0x0200001C RID: 28
		public class user_data_class
		{
			// Token: 0x17000024 RID: 36
			// (get) Token: 0x060000B6 RID: 182 RVA: 0x0000AF28 File Offset: 0x00009128
			// (set) Token: 0x060000B7 RID: 183 RVA: 0x0000AF30 File Offset: 0x00009130
			public string username { get; set; }

			// Token: 0x17000025 RID: 37
			// (get) Token: 0x060000B8 RID: 184 RVA: 0x0000AF39 File Offset: 0x00009139
			// (set) Token: 0x060000B9 RID: 185 RVA: 0x0000AF41 File Offset: 0x00009141
			public string ip { get; set; }

			// Token: 0x17000026 RID: 38
			// (get) Token: 0x060000BA RID: 186 RVA: 0x0000AF4A File Offset: 0x0000914A
			// (set) Token: 0x060000BB RID: 187 RVA: 0x0000AF52 File Offset: 0x00009152
			public string hwid { get; set; }

			// Token: 0x17000027 RID: 39
			// (get) Token: 0x060000BC RID: 188 RVA: 0x0000AF5B File Offset: 0x0000915B
			// (set) Token: 0x060000BD RID: 189 RVA: 0x0000AF63 File Offset: 0x00009163
			public string createdate { get; set; }

			// Token: 0x17000028 RID: 40
			// (get) Token: 0x060000BE RID: 190 RVA: 0x0000AF6C File Offset: 0x0000916C
			// (set) Token: 0x060000BF RID: 191 RVA: 0x0000AF74 File Offset: 0x00009174
			public string lastlogin { get; set; }

			// Token: 0x17000029 RID: 41
			// (get) Token: 0x060000C0 RID: 192 RVA: 0x0000AF7D File Offset: 0x0000917D
			// (set) Token: 0x060000C1 RID: 193 RVA: 0x0000AF85 File Offset: 0x00009185
			public List<api.Data> subscriptions { get; set; }
		}

		// Token: 0x0200001D RID: 29
		public class Data
		{
			// Token: 0x1700002A RID: 42
			// (get) Token: 0x060000C3 RID: 195 RVA: 0x0000AF96 File Offset: 0x00009196
			// (set) Token: 0x060000C4 RID: 196 RVA: 0x0000AF9E File Offset: 0x0000919E
			public string subscription { get; set; }

			// Token: 0x1700002B RID: 43
			// (get) Token: 0x060000C5 RID: 197 RVA: 0x0000AFA7 File Offset: 0x000091A7
			// (set) Token: 0x060000C6 RID: 198 RVA: 0x0000AFAF File Offset: 0x000091AF
			public string expiry { get; set; }

			// Token: 0x1700002C RID: 44
			// (get) Token: 0x060000C7 RID: 199 RVA: 0x0000AFB8 File Offset: 0x000091B8
			// (set) Token: 0x060000C8 RID: 200 RVA: 0x0000AFC0 File Offset: 0x000091C0
			public string timeleft { get; set; }
		}

		// Token: 0x0200001E RID: 30
		public class response_class
		{
			// Token: 0x1700002D RID: 45
			// (get) Token: 0x060000CA RID: 202 RVA: 0x0000AFD1 File Offset: 0x000091D1
			// (set) Token: 0x060000CB RID: 203 RVA: 0x0000AFD9 File Offset: 0x000091D9
			public bool success { get; set; }

			// Token: 0x1700002E RID: 46
			// (get) Token: 0x060000CC RID: 204 RVA: 0x0000AFE2 File Offset: 0x000091E2
			// (set) Token: 0x060000CD RID: 205 RVA: 0x0000AFEA File Offset: 0x000091EA
			public string message { get; set; }
		}
	}
}
