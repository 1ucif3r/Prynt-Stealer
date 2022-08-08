using System;
using System.CodeDom.Compiler;
using System.Configuration;
using System.Runtime.CompilerServices;

namespace KeyAuth.Properties
{
	// Token: 0x02000009 RID: 9
	[CompilerGenerated]
	[GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "11.0.0.0")]
	internal sealed partial class Settings : ApplicationSettingsBase
	{
		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600003B RID: 59 RVA: 0x00005372 File Offset: 0x00003572
		public static Settings Default
		{
			get
			{
				return Settings.defaultInstance;
			}
		}

		// Token: 0x04000022 RID: 34
		private static Settings defaultInstance = (Settings)SettingsBase.Synchronized(new Settings());
	}
}
