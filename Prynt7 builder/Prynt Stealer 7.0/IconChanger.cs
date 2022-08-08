using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using Microsoft.VisualBasic.CompilerServices;

// Token: 0x02000002 RID: 2
public class IconChanger
{
	// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
	public static void InjectIcon(string exeFileName, string iconFileName)
	{
		IconChanger.InjectIcon(exeFileName, iconFileName, 1U, 1U);
	}

	// Token: 0x06000002 RID: 2 RVA: 0x0000205C File Offset: 0x0000025C
	public static void InjectIcon(string exeFileName, string iconFileName, uint iconGroupID, uint iconBaseID)
	{
		IconChanger.IconFile iconFile = IconChanger.IconFile.FromFile(iconFileName);
		IntPtr hUpdate = IconChanger.NativeMethods.BeginUpdateResource(exeFileName, false);
		byte[] array = iconFile.CreateIconGroupData(iconBaseID);
		IconChanger.NativeMethods.UpdateResource(hUpdate, new IntPtr(14L), new IntPtr((long)((ulong)iconGroupID)), 0, array, array.Length);
		int i = 0;
		int num = iconFile.ImageCount - 1;
		while (i <= num)
		{
			byte[] array2 = iconFile.get_ImageData(i);
			IconChanger.NativeMethods.UpdateResource(hUpdate, new IntPtr(3L), new IntPtr((long)((ulong)iconBaseID + (ulong)((long)i))), 0, array2, array2.Length);
			i++;
		}
		IconChanger.NativeMethods.EndUpdateResource(hUpdate, false);
	}

	// Token: 0x02000011 RID: 17
	[SuppressUnmanagedCodeSecurity]
	private class NativeMethods
	{
		// Token: 0x0600006C RID: 108
		[DllImport("kernel32")]
		public static extern IntPtr BeginUpdateResource(string fileName, [MarshalAs(UnmanagedType.Bool)] bool deleteExistingResources);

		// Token: 0x0600006D RID: 109
		[DllImport("kernel32")]
		public static extern bool UpdateResource(IntPtr hUpdate, IntPtr type, IntPtr name, short language, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 5)] byte[] data, int dataSize);

		// Token: 0x0600006E RID: 110
		[DllImport("kernel32")]
		public static extern bool EndUpdateResource(IntPtr hUpdate, [MarshalAs(UnmanagedType.Bool)] bool discard);
	}

	// Token: 0x02000012 RID: 18
	private struct ICONDIR
	{
		// Token: 0x04000056 RID: 86
		public ushort Reserved;

		// Token: 0x04000057 RID: 87
		public ushort Type;

		// Token: 0x04000058 RID: 88
		public ushort Count;
	}

	// Token: 0x02000013 RID: 19
	private struct ICONDIRENTRY
	{
		// Token: 0x04000059 RID: 89
		public byte Width;

		// Token: 0x0400005A RID: 90
		public byte Height;

		// Token: 0x0400005B RID: 91
		public byte ColorCount;

		// Token: 0x0400005C RID: 92
		public byte Reserved;

		// Token: 0x0400005D RID: 93
		public ushort Planes;

		// Token: 0x0400005E RID: 94
		public ushort BitCount;

		// Token: 0x0400005F RID: 95
		public int BytesInRes;

		// Token: 0x04000060 RID: 96
		public int ImageOffset;
	}

	// Token: 0x02000014 RID: 20
	private struct BITMAPINFOHEADER
	{
		// Token: 0x04000061 RID: 97
		public uint Size;

		// Token: 0x04000062 RID: 98
		public int Width;

		// Token: 0x04000063 RID: 99
		public int Height;

		// Token: 0x04000064 RID: 100
		public ushort Planes;

		// Token: 0x04000065 RID: 101
		public ushort BitCount;

		// Token: 0x04000066 RID: 102
		public uint Compression;

		// Token: 0x04000067 RID: 103
		public uint SizeImage;

		// Token: 0x04000068 RID: 104
		public int XPelsPerMeter;

		// Token: 0x04000069 RID: 105
		public int YPelsPerMeter;

		// Token: 0x0400006A RID: 106
		public uint ClrUsed;

		// Token: 0x0400006B RID: 107
		public uint ClrImportant;
	}

	// Token: 0x02000015 RID: 21
	[StructLayout(LayoutKind.Sequential, Pack = 2)]
	private struct GRPICONDIRENTRY
	{
		// Token: 0x0400006C RID: 108
		public byte Width;

		// Token: 0x0400006D RID: 109
		public byte Height;

		// Token: 0x0400006E RID: 110
		public byte ColorCount;

		// Token: 0x0400006F RID: 111
		public byte Reserved;

		// Token: 0x04000070 RID: 112
		public ushort Planes;

		// Token: 0x04000071 RID: 113
		public ushort BitCount;

		// Token: 0x04000072 RID: 114
		public int BytesInRes;

		// Token: 0x04000073 RID: 115
		public ushort ID;
	}

	// Token: 0x02000016 RID: 22
	private class IconFile
	{
		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000070 RID: 112 RVA: 0x0000AA10 File Offset: 0x00008C10
		public int ImageCount
		{
			get
			{
				return (int)this.iconDir.Count;
			}
		}

		// Token: 0x06000071 RID: 113 RVA: 0x0000AA1D File Offset: 0x00008C1D
		public byte[] get_ImageData(int index)
		{
			return this.iconImage[index];
		}

		// Token: 0x06000072 RID: 114 RVA: 0x0000AA27 File Offset: 0x00008C27
		private IconFile()
		{
		}

		// Token: 0x06000073 RID: 115 RVA: 0x0000AA30 File Offset: 0x00008C30
		public static IconChanger.IconFile FromFile(string filename)
		{
			IconChanger.IconFile iconFile = new IconChanger.IconFile();
			byte[] array = File.ReadAllBytes(filename);
			GCHandle gchandle = GCHandle.Alloc(array, GCHandleType.Pinned);
			iconFile.iconDir = (IconChanger.ICONDIR)Marshal.PtrToStructure(gchandle.AddrOfPinnedObject(), typeof(IconChanger.ICONDIR));
			iconFile.iconEntry = new IconChanger.ICONDIRENTRY[(int)iconFile.iconDir.Count];
			iconFile.iconImage = new byte[(int)iconFile.iconDir.Count][];
			int num = Marshal.SizeOf<IconChanger.ICONDIR>(iconFile.iconDir);
			Type typeFromHandle = typeof(IconChanger.ICONDIRENTRY);
			int num2 = Marshal.SizeOf(typeFromHandle);
			int i = 0;
			int num3 = (int)(iconFile.iconDir.Count - 1);
			while (i <= num3)
			{
				IconChanger.ICONDIRENTRY icondirentry = (IconChanger.ICONDIRENTRY)Marshal.PtrToStructure(new IntPtr(gchandle.AddrOfPinnedObject().ToInt64() + (long)num), typeFromHandle);
				iconFile.iconEntry[i] = icondirentry;
				iconFile.iconImage[i] = new byte[icondirentry.BytesInRes];
				Buffer.BlockCopy(array, icondirentry.ImageOffset, iconFile.iconImage[i], 0, icondirentry.BytesInRes);
				num += num2;
				i++;
			}
			gchandle.Free();
			return iconFile;
		}

		// Token: 0x06000074 RID: 116 RVA: 0x0000AB58 File Offset: 0x00008D58
		public byte[] CreateIconGroupData(uint iconBaseID)
		{
			byte[] array = new byte[Marshal.SizeOf(typeof(IconChanger.ICONDIR)) + Marshal.SizeOf(typeof(IconChanger.GRPICONDIRENTRY)) * this.ImageCount];
			GCHandle gchandle = GCHandle.Alloc(array, GCHandleType.Pinned);
			Marshal.StructureToPtr<IconChanger.ICONDIR>(this.iconDir, gchandle.AddrOfPinnedObject(), false);
			int num = Marshal.SizeOf<IconChanger.ICONDIR>(this.iconDir);
			int i = 0;
			int num2 = this.ImageCount - 1;
			while (i <= num2)
			{
				IconChanger.GRPICONDIRENTRY structure = default(IconChanger.GRPICONDIRENTRY);
				IconChanger.BITMAPINFOHEADER bitmapinfoheader = default(IconChanger.BITMAPINFOHEADER);
				GCHandle gchandle2 = GCHandle.Alloc(bitmapinfoheader, GCHandleType.Pinned);
				Marshal.Copy(this.get_ImageData(i), 0, gchandle2.AddrOfPinnedObject(), Marshal.SizeOf(typeof(IconChanger.BITMAPINFOHEADER)));
				gchandle2.Free();
				structure.Width = this.iconEntry[i].Width;
				structure.Height = this.iconEntry[i].Height;
				structure.ColorCount = this.iconEntry[i].ColorCount;
				structure.Reserved = this.iconEntry[i].Reserved;
				structure.Planes = bitmapinfoheader.Planes;
				structure.BitCount = bitmapinfoheader.BitCount;
				structure.BytesInRes = this.iconEntry[i].BytesInRes;
				structure.ID = Conversions.ToUShort((long)((ulong)iconBaseID + (ulong)((long)i)));
				Marshal.StructureToPtr<IconChanger.GRPICONDIRENTRY>(structure, new IntPtr(gchandle.AddrOfPinnedObject().ToInt64() + (long)num), false);
				num += Marshal.SizeOf(typeof(IconChanger.GRPICONDIRENTRY));
				i++;
			}
			gchandle.Free();
			return array;
		}

		// Token: 0x04000074 RID: 116
		private IconChanger.ICONDIR iconDir;

		// Token: 0x04000075 RID: 117
		private IconChanger.ICONDIRENTRY[] iconEntry;

		// Token: 0x04000076 RID: 118
		private byte[][] iconImage;
	}
}
