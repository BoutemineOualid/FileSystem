using System.Runtime.InteropServices;

namespace FileSystem.Fat
{
	[StructLayout(LayoutKind.Explicit)]
	public struct BiosParameterBlock
	{
		/// <summary>
		/// 2 Bytes of the BytesPerSector parameter. The BIOS Paramter Block Starts here.
		/// Offset 0x0b 	
		/// </summary>
		[MarshalAs(UnmanagedType.U2)]
		[FieldOffset(0x0b)]
		public ushort BytesPerSector;

		/// <summary>
		/// 1 Byte containing the number of sectors per cluster. This must be a power of 2 from 1 to 128
		/// Offset 0x0d
		/// </summary>
		[MarshalAs(UnmanagedType.U1)]
		[FieldOffset(0x0d)]
		public byte SectorsPerCluster;

		/// <summary>
		/// 2 Bytes for the Bpb reserved sectors count, Usually 32 for FAT32.
		/// Offset 0x0e 	
		/// </summary>
		[MarshalAs(UnmanagedType.U2)]
		[FieldOffset(0x0e)]
		public ushort ReservedSectorsCount;

		/// <summary>
		/// 1 Byte Number of file allocation tables. Almost always 2.
		/// Offset 0x10
		/// </summary>
		[MarshalAs(UnmanagedType.U1)]
		[FieldOffset(0x10)]
		public byte FatCount;

		/// <summary>
		/// 2 Bytes, Maximum number of root directory entries. Only used on FAT12 and FAT16
		/// Offset 0x11
		/// </summary>
		[MarshalAs(UnmanagedType.U2)]
		[FieldOffset(0x11)]
		public ushort MaxRootDirectoriesCount;

		/// <summary>
		/// 2 Bytes, Total sectors (if zero, use 4 byte value at offset 0x20) used only for FAT12 AND FAT16 Systems
		/// Offset 0x13 	
		/// </summary>
		[MarshalAs(UnmanagedType.U2)]
		[FieldOffset(0x13)]
		public ushort TotalSectors16;

		/// <summary>
		/// 1 Byte, the media descriptor 
		/// Offset 0x15
		/// </summary>
		[MarshalAs(UnmanagedType.U1)]
		[FieldOffset(0x15)]
		public MediaDescriptor MediaDescriptor;

		/// <summary>
		/// 2 Bytes, Sectors per File Allocation Table for FAT12/FAT16
		/// Offset 0x16
		/// </summary>
		[MarshalAs(UnmanagedType.U2)]
		[FieldOffset(0x16)]
		public ushort FatSize16;

		/// <summary>
		/// 2 Bytes, Sectors per track
		/// Offset 0x18
		/// </summary>
		[MarshalAs(UnmanagedType.U2)]
		[FieldOffset(0x18)]
		public ushort SectorsPerTrack;

		/// <summary>
		/// 2 Bytes, Number of heads.
		/// Offset 0x1a
		/// </summary>
		[MarshalAs(UnmanagedType.U2)]
		[FieldOffset(0x1a)]
		public ushort NumberOfHeads;

		/// <summary>
		/// 4 Bytes, Hidden sectors.
		/// Offset 0x1c
		/// </summary>
		[MarshalAs(UnmanagedType.U4)]
		[FieldOffset(0x1c)]
		public uint HiddenSectors;

		/// <summary>
		/// 4 Bytes, Total sectors (if greater than 65535; otherwise, see offset 0x13)
		/// Offset 0x20
		/// </summary>
		[MarshalAs(UnmanagedType.U4)]
		[FieldOffset(0x20)]
		public uint TotalSectors32;
	}
}