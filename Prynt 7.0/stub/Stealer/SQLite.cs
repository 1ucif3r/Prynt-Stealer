using System;
using System.IO;
using System.Text;

namespace Stealer
{
	// Token: 0x0200000F RID: 15
	internal class SQLite
	{
		// Token: 0x0600004C RID: 76 RVA: 0x00003324 File Offset: 0x00001524
		public SQLite(string fileName)
		{
			this._fileBytes = File.ReadAllBytes(fileName);
			this._pageSize = this.ConvertToULong(16, 2);
			this._dbEncoding = this.ConvertToULong(56, 4);
			this.ReadMasterTable(100L);
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00003384 File Offset: 0x00001584
		public string GetValue(int rowNum, int field)
		{
			string result;
			try
			{
				if (rowNum >= this._tableEntries.Length)
				{
					result = null;
				}
				else
				{
					result = ((field >= this._tableEntries[rowNum].Content.Length) ? null : this._tableEntries[rowNum].Content[field]);
				}
			}
			catch
			{
				result = "";
			}
			return result;
		}

		// Token: 0x0600004E RID: 78 RVA: 0x000033EC File Offset: 0x000015EC
		public int GetRowCount()
		{
			return this._tableEntries.Length;
		}

		// Token: 0x0600004F RID: 79 RVA: 0x000033F8 File Offset: 0x000015F8
		private bool ReadTableFromOffset(ulong offset)
		{
			bool result;
			try
			{
				if (this._fileBytes[(int)(checked((IntPtr)offset))] == 13)
				{
					uint num = (uint)(this.ConvertToULong((int)offset + 3, 2) - 1UL);
					int num2 = 0;
					if (this._tableEntries != null)
					{
						num2 = this._tableEntries.Length;
						Array.Resize<SQLite.TableEntry>(ref this._tableEntries, this._tableEntries.Length + (int)num + 1);
					}
					else
					{
						this._tableEntries = new SQLite.TableEntry[num + 1U];
					}
					for (uint num3 = 0U; num3 <= num; num3 += 1U)
					{
						ulong num4 = this.ConvertToULong((int)offset + 8 + (int)(num3 * 2U), 2);
						if (offset != 100UL)
						{
							num4 += offset;
						}
						int num5 = this.Gvl((int)num4);
						this.Cvl((int)num4, num5);
						int num6 = this.Gvl((int)(num4 + (ulong)((long)num5 - (long)num4) + 1UL));
						this.Cvl((int)(num4 + (ulong)((long)num5 - (long)num4) + 1UL), num6);
						ulong num7 = num4 + (ulong)((long)num6 - (long)num4 + 1L);
						int num8 = this.Gvl((int)num7);
						int num9 = num8;
						long num10 = this.Cvl((int)num7, num8);
						SQLite.RecordHeaderField[] array = null;
						long num11 = (long)(num7 - (ulong)((long)num8) + 1UL);
						int num12 = 0;
						while (num11 < num10)
						{
							Array.Resize<SQLite.RecordHeaderField>(ref array, num12 + 1);
							int num13 = num9 + 1;
							num9 = this.Gvl(num13);
							array[num12].Type = this.Cvl(num13, num9);
							array[num12].Size = (long)((array[num12].Type <= 9L) ? ((ulong)this._sqlDataTypeSize[(int)(checked((IntPtr)array[num12].Type))]) : ((ulong)((!SQLite.IsOdd(array[num12].Type)) ? ((array[num12].Type - 12L) / 2L) : ((array[num12].Type - 13L) / 2L))));
							num11 = num11 + (long)(num9 - num13) + 1L;
							num12++;
						}
						if (array != null)
						{
							this._tableEntries[num2 + (int)num3].Content = new string[array.Length];
							int num14 = 0;
							for (int i = 0; i <= array.Length - 1; i++)
							{
								if (array[i].Type > 9L)
								{
									if (!SQLite.IsOdd(array[i].Type))
									{
										if (this._dbEncoding == 1UL)
										{
											this._tableEntries[num2 + (int)num3].Content[i] = Encoding.Default.GetString(this._fileBytes, (int)(num7 + (ulong)num10 + (ulong)((long)num14)), (int)array[i].Size);
										}
										else if (this._dbEncoding == 2UL)
										{
											this._tableEntries[num2 + (int)num3].Content[i] = Encoding.Unicode.GetString(this._fileBytes, (int)(num7 + (ulong)num10 + (ulong)((long)num14)), (int)array[i].Size);
										}
										else if (this._dbEncoding == 3UL)
										{
											this._tableEntries[num2 + (int)num3].Content[i] = Encoding.BigEndianUnicode.GetString(this._fileBytes, (int)(num7 + (ulong)num10 + (ulong)((long)num14)), (int)array[i].Size);
										}
									}
									else
									{
										this._tableEntries[num2 + (int)num3].Content[i] = Encoding.Default.GetString(this._fileBytes, (int)(num7 + (ulong)num10 + (ulong)((long)num14)), (int)array[i].Size);
									}
								}
								else
								{
									this._tableEntries[num2 + (int)num3].Content[i] = Convert.ToString(this.ConvertToULong((int)(num7 + (ulong)num10 + (ulong)((long)num14)), (int)array[i].Size));
								}
								num14 += (int)array[i].Size;
							}
						}
					}
				}
				else if (this._fileBytes[(int)(checked((IntPtr)offset))] == 5)
				{
					uint num15 = (uint)(this.ConvertToULong((int)(offset + 3UL), 2) - 1UL);
					for (uint num16 = 0U; num16 <= num15; num16 += 1U)
					{
						uint num17 = (uint)this.ConvertToULong((int)offset + 12 + (int)(num16 * 2U), 2);
						this.ReadTableFromOffset((this.ConvertToULong((int)(offset + (ulong)num17), 4) - 1UL) * this._pageSize);
					}
					this.ReadTableFromOffset((this.ConvertToULong((int)(offset + 8UL), 4) - 1UL) * this._pageSize);
				}
				result = true;
			}
			catch
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00003864 File Offset: 0x00001A64
		private void ReadMasterTable(long offset)
		{
			try
			{
				byte b = this._fileBytes[(int)(checked((IntPtr)offset))];
				if (b != 5)
				{
					if (b == 13)
					{
						ulong num = this.ConvertToULong((int)offset + 3, 2) - 1UL;
						int num2 = 0;
						if (this._masterTableEntries != null)
						{
							num2 = this._masterTableEntries.Length;
							Array.Resize<SQLite.SqliteMasterEntry>(ref this._masterTableEntries, this._masterTableEntries.Length + (int)num + 1);
						}
						else
						{
							this._masterTableEntries = new SQLite.SqliteMasterEntry[num + 1UL];
						}
						for (ulong num3 = 0UL; num3 <= num; num3 += 1UL)
						{
							ulong num4 = this.ConvertToULong((int)offset + 8 + (int)num3 * 2, 2);
							if (offset != 100L)
							{
								num4 += (ulong)offset;
							}
							int num5 = this.Gvl((int)num4);
							this.Cvl((int)num4, num5);
							int num6 = this.Gvl((int)(num4 + (ulong)((long)num5 - (long)num4) + 1UL));
							this.Cvl((int)(num4 + (ulong)((long)num5 - (long)num4) + 1UL), num6);
							ulong num7 = num4 + (ulong)((long)num6 - (long)num4 + 1L);
							int num8 = this.Gvl((int)num7);
							int num9 = num8;
							long num10 = this.Cvl((int)num7, num8);
							long[] array = new long[5];
							for (int i = 0; i <= 4; i++)
							{
								int startIdx = num9 + 1;
								num9 = this.Gvl(startIdx);
								array[i] = this.Cvl(startIdx, num9);
								array[i] = (long)((array[i] <= 9L) ? ((ulong)this._sqlDataTypeSize[(int)(checked((IntPtr)array[i]))]) : ((ulong)((!SQLite.IsOdd(array[i])) ? ((array[i] - 12L) / 2L) : ((array[i] - 13L) / 2L))));
							}
							if (this._dbEncoding == 1UL || this._dbEncoding == 2UL)
							{
								if (this._dbEncoding == 1UL)
								{
									this._masterTableEntries[num2 + (int)num3].ItemName = Encoding.Default.GetString(this._fileBytes, (int)(num7 + (ulong)num10 + (ulong)array[0]), (int)array[1]);
								}
								else if (this._dbEncoding == 2UL)
								{
									this._masterTableEntries[num2 + (int)num3].ItemName = Encoding.Unicode.GetString(this._fileBytes, (int)(num7 + (ulong)num10 + (ulong)array[0]), (int)array[1]);
								}
								else if (this._dbEncoding == 3UL)
								{
									this._masterTableEntries[num2 + (int)num3].ItemName = Encoding.BigEndianUnicode.GetString(this._fileBytes, (int)(num7 + (ulong)num10 + (ulong)array[0]), (int)array[1]);
								}
							}
							this._masterTableEntries[num2 + (int)num3].RootNum = (long)this.ConvertToULong((int)(num7 + (ulong)num10 + (ulong)array[0] + (ulong)array[1] + (ulong)array[2]), (int)array[3]);
							if (this._dbEncoding == 1UL)
							{
								this._masterTableEntries[num2 + (int)num3].SqlStatement = Encoding.Default.GetString(this._fileBytes, (int)(num7 + (ulong)num10 + (ulong)array[0] + (ulong)array[1] + (ulong)array[2] + (ulong)array[3]), (int)array[4]);
							}
							else if (this._dbEncoding == 2UL)
							{
								this._masterTableEntries[num2 + (int)num3].SqlStatement = Encoding.Unicode.GetString(this._fileBytes, (int)(num7 + (ulong)num10 + (ulong)array[0] + (ulong)array[1] + (ulong)array[2] + (ulong)array[3]), (int)array[4]);
							}
							else if (this._dbEncoding == 3UL)
							{
								this._masterTableEntries[num2 + (int)num3].SqlStatement = Encoding.BigEndianUnicode.GetString(this._fileBytes, (int)(num7 + (ulong)num10 + (ulong)array[0] + (ulong)array[1] + (ulong)array[2] + (ulong)array[3]), (int)array[4]);
							}
						}
					}
				}
				else
				{
					uint num11 = (uint)(this.ConvertToULong((int)offset + 3, 2) - 1UL);
					for (int j = 0; j <= (int)num11; j++)
					{
						uint num12 = (uint)this.ConvertToULong((int)offset + 12 + j * 2, 2);
						if (offset == 100L)
						{
							this.ReadMasterTable((long)((this.ConvertToULong((int)num12, 4) - 1UL) * this._pageSize));
						}
						else
						{
							this.ReadMasterTable((long)((this.ConvertToULong((int)(offset + (long)((ulong)num12)), 4) - 1UL) * this._pageSize));
						}
					}
					this.ReadMasterTable((long)((this.ConvertToULong((int)offset + 8, 4) - 1UL) * this._pageSize));
				}
			}
			catch
			{
			}
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00003CAC File Offset: 0x00001EAC
		public bool ReadTable(string tableName)
		{
			bool result;
			try
			{
				int num = -1;
				for (int i = 0; i <= this._masterTableEntries.Length; i++)
				{
					if (string.Compare(this._masterTableEntries[i].ItemName.ToLower(), tableName.ToLower(), StringComparison.Ordinal) == 0)
					{
						num = i;
						break;
					}
				}
				if (num == -1)
				{
					result = false;
				}
				else
				{
					string[] array = this._masterTableEntries[num].SqlStatement.Substring(this._masterTableEntries[num].SqlStatement.IndexOf("(", StringComparison.Ordinal) + 1).Split(new char[]
					{
						','
					});
					for (int j = 0; j <= array.Length - 1; j++)
					{
						array[j] = array[j].TrimStart(new char[0]);
						int num2 = array[j].IndexOf(' ');
						if (num2 > 0)
						{
							array[j] = array[j].Substring(0, num2);
						}
						if (array[j].IndexOf("UNIQUE", StringComparison.Ordinal) != 0)
						{
							Array.Resize<string>(ref this._fieldNames, j + 1);
							this._fieldNames[j] = array[j];
						}
					}
					result = this.ReadTableFromOffset((ulong)((this._masterTableEntries[num].RootNum - 1L) * (long)this._pageSize));
				}
			}
			catch
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00003E04 File Offset: 0x00002004
		private ulong ConvertToULong(int startIndex, int size)
		{
			ulong result;
			try
			{
				if (size > 8 | size == 0)
				{
					result = 0UL;
				}
				else
				{
					ulong num = 0UL;
					for (int i = 0; i <= size - 1; i++)
					{
						num = (num << 8 | (ulong)this._fileBytes[startIndex + i]);
					}
					result = num;
				}
			}
			catch
			{
				result = 0UL;
			}
			return result;
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00003E60 File Offset: 0x00002060
		private int Gvl(int startIdx)
		{
			int result;
			try
			{
				if (startIdx > this._fileBytes.Length)
				{
					result = 0;
				}
				else
				{
					for (int i = startIdx; i <= startIdx + 8; i++)
					{
						if (i > this._fileBytes.Length - 1)
						{
							return 0;
						}
						if ((this._fileBytes[i] & 128) != 128)
						{
							return i;
						}
					}
					result = startIdx + 8;
				}
			}
			catch
			{
				result = 0;
			}
			return result;
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00003ED0 File Offset: 0x000020D0
		private long Cvl(int startIdx, int endIdx)
		{
			long result;
			try
			{
				endIdx++;
				byte[] array = new byte[8];
				int num = endIdx - startIdx;
				bool flag = false;
				if (num == 0 | num > 9)
				{
					result = 0L;
				}
				else if (num == 1)
				{
					array[0] = (this._fileBytes[startIdx] & 127);
					result = BitConverter.ToInt64(array, 0);
				}
				else
				{
					if (num == 9)
					{
						flag = true;
					}
					int num2 = 1;
					int num3 = 7;
					int num4 = 0;
					if (flag)
					{
						array[0] = this._fileBytes[endIdx - 1];
						endIdx--;
						num4 = 1;
					}
					for (int i = endIdx - 1; i >= startIdx; i += -1)
					{
						if (i - 1 >= startIdx)
						{
							array[num4] = (byte)((this._fileBytes[i] >> num2 - 1 & 255 >> num2) | (int)this._fileBytes[i - 1] << num3);
							num2++;
							num4++;
							num3--;
						}
						else if (!flag)
						{
							array[num4] = (byte)(this._fileBytes[i] >> num2 - 1 & 255 >> num2);
						}
					}
					result = BitConverter.ToInt64(array, 0);
				}
			}
			catch
			{
				result = 0L;
			}
			return result;
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00003FF0 File Offset: 0x000021F0
		private static bool IsOdd(long value)
		{
			return (value & 1L) == 1L;
		}

		// Token: 0x04000047 RID: 71
		private readonly byte[] _sqlDataTypeSize = new byte[]
		{
			0,
			1,
			2,
			3,
			4,
			6,
			8,
			8,
			0,
			0
		};

		// Token: 0x04000048 RID: 72
		private readonly ulong _dbEncoding;

		// Token: 0x04000049 RID: 73
		private readonly byte[] _fileBytes;

		// Token: 0x0400004A RID: 74
		private readonly ulong _pageSize;

		// Token: 0x0400004B RID: 75
		private string[] _fieldNames;

		// Token: 0x0400004C RID: 76
		private SQLite.SqliteMasterEntry[] _masterTableEntries;

		// Token: 0x0400004D RID: 77
		private SQLite.TableEntry[] _tableEntries;

		// Token: 0x02000060 RID: 96
		private struct RecordHeaderField
		{
			// Token: 0x040000FB RID: 251
			public long Size;

			// Token: 0x040000FC RID: 252
			public long Type;
		}

		// Token: 0x02000061 RID: 97
		private struct TableEntry
		{
			// Token: 0x040000FD RID: 253
			public string[] Content;
		}

		// Token: 0x02000062 RID: 98
		private struct SqliteMasterEntry
		{
			// Token: 0x040000FE RID: 254
			public string ItemName;

			// Token: 0x040000FF RID: 255
			public long RootNum;

			// Token: 0x04000100 RID: 256
			public string SqlStatement;
		}
	}
}
