/**
 * Copyright Oualid Boutemine (c) 2009.
 **/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace FileSystem.Ntfs
{
	/// <summary>
	/// Maps the boot sector of an NTFS File System-based volume.
	/// </summary>
	[StructLayout(LayoutKind.Explicit)]
	public unsafe struct BootSector
	{
		public const int BootSectorSize = 512;

		#region Common Region With all FAT systems
		/// <summary>
		/// First 3 Bytes of the Jump insctructions.
		/// Offset 0x00 	
		/// </summary>
		//[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
		[FieldOffset(0x00)]
		public fixed byte JumpBootInstructions[3];

		/// <summary>
		/// 8 Bytes of the OemName
		/// Offset 0x03
		/// </summary>
		[FieldOffset(0x03)]
		public fixed byte _oemName[8];

		/// <summary>
		/// An ASCII representation of the <see cref="_oemName"/> data array.
		/// </summary>
		public string OemName
		{
			get
			{
				fixed (byte* p = _oemName)
				{
					byte* pointer = p;
					byte[] oemNameArray = new byte[8];

					for (int i = 0; i < oemNameArray.Length; i++)
					{
						oemNameArray[i] = *pointer;
						pointer++;
					}

					return Encoding
						.ASCII
						.GetString(oemNameArray)
						.Trim();
				}
			}
		}

		[MarshalAs(UnmanagedType.Struct)]
		[FieldOffset(0x00)]
		public BiosParameterBlock Bpb;
		#endregion

		/// <summary>
		/// Gets the boot sector of a given drive.
		/// <remarks>The drive letter must follow this pattern X:</remarks>
		/// </summary>
		/// <param name="driveLetter">The drive letter in this format X:</param>
		/// <returns>The boot sector of the specified drive.</returns>
		public static BootSector GetBootSector(string driveLetter)
		{
			byte[] bootSectorData = new byte[BootSectorSize];
			string drive = @"\\.\" + driveLetter;

			IntPtr hardDiskPointer = SystemIO.OpenFile(drive);

			// Seeks the start of the partition
			SystemIO.SeekAbsolute(hardDiskPointer, 0);

			// Read the first reserved sector of the drive data (Boot Sector)
			// The data should be read with a chunk of 512 X byte.
			SystemIO.ReadBytes(hardDiskPointer, bootSectorData, BootSectorSize);

			// Release IO handle
			SystemIO.CloseHandle(hardDiskPointer);

			// Prevent the GC from messing up with the position of the buffer in the heap.
			GCHandle pinnedBootSectorData = GCHandle.Alloc(bootSectorData, GCHandleType.Pinned);

			// Marshaling the buffer into a valid data structucture.
			var bootSector = (BootSector)Marshal.PtrToStructure(
				pinnedBootSectorData.AddrOfPinnedObject(),
				typeof(BootSector)
			);

			// Free up the pinned buffer.
			pinnedBootSectorData.Free();

			return bootSector;
		}
	}
}