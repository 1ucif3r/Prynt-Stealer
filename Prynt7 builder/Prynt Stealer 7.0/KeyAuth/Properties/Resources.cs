using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace KeyAuth.Properties
{
	// Token: 0x02000008 RID: 8
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
	[DebuggerNonUserCode]
	[CompilerGenerated]
	internal class Resources
	{
		// Token: 0x06000037 RID: 55 RVA: 0x0000532F File Offset: 0x0000352F
		internal Resources()
		{
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000038 RID: 56 RVA: 0x00005337 File Offset: 0x00003537
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static ResourceManager ResourceManager
		{
			get
			{
				if (Resources.resourceMan == null)
				{
					Resources.resourceMan = new ResourceManager("KeyAuth.Properties.Resources", typeof(Resources).Assembly);
				}
				return Resources.resourceMan;
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000039 RID: 57 RVA: 0x00005363 File Offset: 0x00003563
		// (set) Token: 0x0600003A RID: 58 RVA: 0x0000536A File Offset: 0x0000356A
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static CultureInfo Culture
		{
			get
			{
				return Resources.resourceCulture;
			}
			set
			{
				Resources.resourceCulture = value;
			}
		}

		// Token: 0x04000020 RID: 32
		private static ResourceManager resourceMan;

		// Token: 0x04000021 RID: 33
		private static CultureInfo resourceCulture;
	}
}
