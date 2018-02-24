/**
 * Copyright Oualid Boutemine (c) 2009.
 **/

using System.Runtime.InteropServices;
using System.Text;

namespace FileSystem.Ntfs
{
	/// <summary>
	/// Maps the boot sector's bios parameter block of an NTFS File System-based volume.
	/// </summary>
	[StructLayout(LayoutKind.Explicit)]
	public unsafe struct BiosParameterBlock
	{
		/// <summary>
		/// 2 Bytes of the BytesPerSector parameter. The BIOS Paramter Block Starts here.
		/// Offset 0x0b 	
		/// </summary>
		[MarshalAs(UnmanagedType.U2)]
		[FieldOffset(0x0b)]
		public ushort BytesPerSector;

		/// <summary>
		/// 1 Byte containing the number of sectors per cluster. This must be a power of 2 from 1 to 128.
		/// Offset 0x0d
		/// </summary>
		[MarshalAs(UnmanagedType.U1)]
		[FieldOffset(0x0d)]
		public byte SectorsPerCluster;

		/// <summary>
		/// 2 Bytes for the Bpb reserved sectors count.
		/// Offset 0x0e
		/// </summary>
		[MarshalAs(UnmanagedType.U2)]
		[FieldOffset(0x0e)]
		public ushort ReservedSectorsCount;

		/// <summary>
		/// Value must be 0 or NTFS fails to mount the volume.
		/// Offset 0x10
		/// </summary>
		[FieldOffset(0x10)]
		public fixed byte Reserved1[3];

		/// <summary>
		/// Value must be 0 or NTFS fails to mount the volume.
		/// Offset 0x11
		/// </summary>
		[MarshalAs(UnmanagedType.U2)]
		[FieldOffset(0x13)]
		public ushort Reserved2;

		/// <summary>
		/// 1 Byte, the media descriptor.
		/// Offset 0x15
		/// </summary>
		[MarshalAs(UnmanagedType.U1)]
		[FieldOffset(0x15)]
		public MediaDescriptor MediaDescriptor;

		/// <summary>
		/// Value must be 0 or NTFS fails to mount the volume.
		/// Offset 0x16
		/// </summary>
		[MarshalAs(UnmanagedType.U2)]
		[FieldOffset(0x16)]
		public ushort Reserved3;

		/// <summary>
		/// 2 Bytes, Sectors per track. Not used or checked by NTFS.
		/// Offset 0x18
		/// </summary>
		[MarshalAs(UnmanagedType.U2)]
		[FieldOffset(0x18)]
		public ushort SectorsPerTrack;

		/// <summary>
		/// 2 Bytes Number of heads. Not used or checked by NTFS.
		/// Offset 0x1a
		/// </summary>
		[MarshalAs(UnmanagedType.U2)]
		[FieldOffset(0x1a)]
		public ushort NumberOfHeads;

		/// <summary>
		/// 4 Bytes Hidden sectors. Not used or checked by NTFS.
		/// Offset 0x1c
		/// </summary>
		[MarshalAs(UnmanagedType.U4)]
		[FieldOffset(0x1c)]
		public uint HiddenSectors;

		/// <summary>
		/// The value must be 0 or NTFS fails to mount the volume.
		/// Offset 0x20
		/// </summary>
		[MarshalAs(UnmanagedType.U4)]
		[FieldOffset(0x20)]
		public uint Reserved4;

		/// <summary>
		/// Not used or checked by NTFS.
		/// Offset 0x20
		/// </summary>
		[MarshalAs(UnmanagedType.U4)]
		[FieldOffset(0x24)]
		public uint Reserved5;

		/// <summary>
		/// 8 Bytes for the total number of sectors on the volume.
		/// Offset 0x28
		/// </summary>
		[MarshalAs(UnmanagedType.U8)]
		[FieldOffset(0x28)]
		public ulong TotalSectors;

		/// <summary>
		/// 8 Bytes for the Logical Cluster Number for of the Master File Table or MFT.
		/// Identifies the exact pointer location of the MFT by using its logical cluster number.
		/// Offset 0x30
		/// </summary>
		[MarshalAs(UnmanagedType.U8)]
		[FieldOffset(0x30)]
		public long MftClusterNumber;

		/// <summary>
		/// 8 Bytes for the Logical Cluster Number for the File $MFTMirr.
		/// Identifies the location of the mirrored copy of the MFT by using its logical cluster number. 
		/// Offset 0x38
		/// </summary>
		[MarshalAs(UnmanagedType.U8)]
		[FieldOffset(0x38)]
		public long MftMirrorClusterNumber;

		/// <summary>
		/// 1 Byte for the Clusters Per MFT Record. Used to calculate the size of each record.
		/// NTFS creates a file record for each file and a folder record for each folder that is created on an NTFS volume.
		/// Files and folders smaller than this size are contained within the MFT. 
		/// If this number is positive (up to 7F), then it represents clusters per MFT record.
		/// If the number is negative (80 to FF), then the size of the file record is 2 raised to the absolute value of this number.
		/// Offset 0x40
		/// </summary>
		[MarshalAs(UnmanagedType.U4)]
		[FieldOffset(0x40)]
		public uint ClustersPerMftRecord;

		/// <summary>
		/// 1 byte for the Clusters Per Index Buffer. The size of each index buffer, which is used to allocate space for directories.
		/// If this number is positive (up to 7F), then it represents clusters per MFT record.
		/// If the number is negative (80 to FF), then the size of the file record is 2 raised to the absolute value of this number.
		/// Offset 0x44
		/// </summary>
		[MarshalAs(UnmanagedType.U4)]
		[FieldOffset(0x44)]
		public uint ClustersPerIndexBuffer;

		/// <summary>
		/// 8 bytes for the volume serial number.
		/// Offset 0x48
		/// </summary>
		[FieldOffset(0x48)]
		public fixed byte _volumeSerialNumber[8];

		/// <summary>
		/// An ASCII representation of the <see cref="_volumeSerialNumber"/> data array.
		/// </summary>
		public string VolumeSerialNumber
		{
			get
			{
				fixed (byte* p = _volumeSerialNumber)
				{
					byte* pointer = p;
					byte[] volumeSerialArray = new byte[8];

					for (int i = 0; i < 8; i++)
					{
						volumeSerialArray[i] = *pointer;
						pointer++;
					}

					return Encoding
						.ASCII
						.GetString(volumeSerialArray)
						.Trim();
				}
			}
		}

		/// <summary>
		/// 4 Bytes Not used by NTFS.
		/// Offset 0x50
		/// </summary>
		[MarshalAs(UnmanagedType.U4)]
		[FieldOffset(0x50)]
		public uint Checksum;
	}
}