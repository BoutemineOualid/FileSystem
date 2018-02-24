using System;
using System.Runtime.InteropServices;
using System.Text;

namespace FileSystem.Fat
{
	/// <summary>
	/// Maps the boot sector of a FAT32, FAT16 or FAT12 File System-based volume.
	/// </summary>
	[StructLayout(LayoutKind.Explicit, CharSet = CharSet.Ansi, Pack = 1)]
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
		/// 8 Bytes of the OemName.
		/// Offset 0x03
		/// </summary>
		[FieldOffset(0x03)]
		public fixed byte _oemName[8];

		/// <summary>
		/// Gets an ASCII representation of the <see cref="_oemName"/> data array.
		/// </summary>
		public string OemName
		{
			get
			{
				fixed (byte* p = _oemName)
				{
					byte* pointer = p;
					var oemNameArray = new byte[8];

					for (int i = 0; i < 8; i++)
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

		#region Extended BIOS Parameter Block: FAT32 Specific

		/// <summary>
		/// 4 Bytes for the number of sectors occupied by a single File Allocation Table.
		/// Offset 0x24
		/// </summary>
		[MarshalAs(UnmanagedType.U4)]
		[FieldOffset(0x24)]
		public uint FatSize32;

		/// <summary>
		/// 2 Bytes
		/// This field is only defined for FAT32 media and does not exist on FAT12 nor FAT16.
		/// Bits 0-3	-- Zero-based number of active FAT. Only valid if mirroring is disabled.
		/// Bits 4-6	-- Reserved.
		/// Bit  7	    -- 0 means the FAT is mirrored at runtime into all FATs.
		///             -- 1 means only one FAT is active; it is the one referenced in bits 0-3.
		/// Bits 8-15 	-- Reserved.
		/// Offset 0x28
		/// </summary>
		[MarshalAs(UnmanagedType.U2)]
		[FieldOffset(0x28)]
		public ushort ExtendedFlags;

		/// <summary>
		/// 2 Bytes for the file system version.
		/// The high byte is major revision number.
		/// Low byte is minor revision number. 
		/// Offset 0x2a
		/// </summary>
		[FieldOffset(0x2a)]
		public fixed byte FileSystemVersion[2];

		/// <summary>
		/// 4 Bytes for the first cluster number of the root directory.
		/// Offset 0x2c
		/// </summary>
		[MarshalAs(UnmanagedType.U4)]
		[FieldOffset(0x2c)]
		public uint RootDirFirstClusterNumber;

		/// <summary>
		/// 2 Bytes for the Sector number of FS Information Sector.
		/// Offset 0x30
		/// </summary>
		[MarshalAs(UnmanagedType.U2)]
		[FieldOffset(0x30)]
		public ushort FSInfoSectorNumber;

		/// <summary>
		/// 2 Bytes. If non-zero, indicates the sector number in the reserved area of the volume of a copy of the boot record. Usually 6.
		/// Offset 0x32
		/// </summary>
		[MarshalAs(UnmanagedType.U2)]
		[FieldOffset(0x32)]
		public ushort BackupBootSectorNumber;

		/// <summary>
		/// 12 Reserved Bytes.
		/// Offset 0x34
		/// </summary>
		[FieldOffset(0x34)]
		public fixed byte Reserved[12];

		/// <summary>
		/// 1 Byte for the physical drive number.
		/// Offset 0x40
		/// </summary>
		[MarshalAs(UnmanagedType.U1)]
		[FieldOffset(0x40)]
		public byte PhysicalDriveNumber;

		/// <summary>
		/// 1 Reserved byte.
		/// Offset 0x41
		/// </summary>
		[MarshalAs(UnmanagedType.U1)]
		[FieldOffset(0x41)]
		public byte Reserved1;

		/// <summary>
		/// 1 Byte. The boot signature.
		/// Offset 0x42
		/// </summary>
		[MarshalAs(UnmanagedType.U1)]
		[FieldOffset(0x42)]
		public byte ExtendedBootSignature;

		/// <summary>
		/// 4 Bytes for the volume serial number.
		/// Offset 0x43
		/// </summary>
		[MarshalAs(UnmanagedType.U4)]
		[FieldOffset(0x43)]
		public uint VolumeID;

		/// <summary>
		/// 11 Byte for the volume label.
		/// Offset 0x47
		/// </summary>
		[FieldOffset(0x47)]
		public fixed byte _volumeLabel[11];

		/// <summary>
		/// Gets an ASCII representation of the <see cref="_volumeLabel"/> data array.
		/// </summary>
		public string VolumeLabel
		{
			get
			{
				fixed (byte* p = _volumeLabel)
				{
					byte* pointer = p;
					var volumeLabelArray = new byte[11];

					for (int i = 0; i < 11; i++)
					{
						volumeLabelArray[i] = *pointer;
						pointer++;
					}

					return Encoding
						.ASCII
						.GetString(volumeLabelArray)
						.Trim();
				}
			}
		}

		/// <summary>
		/// 8 Bytes for the file system type string. 
		/// Offset 0x52
		/// </summary>
		[FieldOffset(0x52)]
		public fixed byte _fileSystemType[8];

		/// <summary>
		/// Gets an ASCI representation of the <see cref="_fileSystemType"/> data array.
		/// </summary>
		public string FileSystemType
		{
			get
			{
				fixed (byte* p = _fileSystemType)
				{
					byte* pointer = p;
					var fileSystemTypeArray = new byte[8];

					for (int i = 0; i < 8; i++)
					{
						fileSystemTypeArray[i] = *pointer;
						pointer++;
					}

					return Encoding
						.ASCII
						.GetString(fileSystemTypeArray)
						.Trim();
				}
			}
		}
		#endregion

		#region Methods
		/// <summary>
		/// Gets the boot sector of the specified drive.
		/// <remarks>The drive letter must follow this pattern X: </remarks>
		/// </summary>
		/// <param name="driveLetter">The drive letter in this format X:</param>
		/// <returns>The boot sector of the specified drive.</returns>
		public static BootSector GetBootSector(string driveLetter)
		{
			var bootSectorData = new byte[BootSectorSize];
			var drive = @"\\.\" + driveLetter;

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

			pinnedBootSectorData.Free();

			return bootSector;
		}

		/// <summary>
		/// Gets the offset of the first data sector located in the 2nd cluster
		/// </summary>
		/// <returns>The offset of the sector</returns>
		public uint GetFirstDataSectorNumber()
		{
			try
			{
				uint fatSize = Bpb.FatSize16 == 0
					? FatSize32
					: Bpb.FatSize16;

				return Bpb.ReservedSectorsCount + Bpb.FatCount * fatSize;
			}
			catch (Exception)
			{

				return 0;
			}
		}

		/// <summary>
		/// Gets the absolute offset of first sector of a given FAT cluster.
		/// </summary>
		/// <param name="clusterNumber"></param>
		/// <returns></returns>
		public uint GetFirstSectorNumber(uint clusterNumber)
		{
			return ((clusterNumber - 2) * Bpb.SectorsPerCluster) + GetFirstDataSectorNumber();
		}

		/// <summary>
		/// Gets File Allocation Table's type for the specified drive based on its clusters count.
		/// </summary>
		/// <param name="driveLetter">X:</param>
		/// <returns>
		/// 12 : Fat12
		/// 16 : Fat16
		/// 32 : Fat32
		/// </returns>
		public static int GetFatType(string driveLetter)
		{
			BootSector driveBootSector = GetBootSector(driveLetter);

			return driveBootSector.GetFatType();
		}

		/// <summary>
		/// Gets File Allocation Table type based on its clusters count.
		/// </summary>
		/// <returns>
		/// 12 : Fat12
		/// 16 : Fat16
		/// 32 : Fat32
		/// </returns>
		public int GetFatType()
		{
			uint clustersCount = GetClustersCount();
			if (clustersCount < 4085)
			{
				/* Volume is FAT12 */
				return 12;
			}
			else if (clustersCount < 65525)
			{
				return 16;
				/* Volume is FAT16 */
			}
			else
			{
				/* Volume is FAT32 */
				return 32;
			}
		}

		/// <summary>
		/// Returns the root directory offset.
		/// </summary>
		public ulong GetRootDirOffset()
		{
			return GetFirstSectorNumber(RootDirFirstClusterNumber) * Bpb.BytesPerSector;
		}

		/// <summary>
		/// Returns the clusters count of the mounted volume represented by this Boot sector (Excluding the 2 reserved clusters).
		/// </summary>
		public uint GetClustersCount()
		{
			uint dataSectorsCount = Bpb.TotalSectors32 - (Bpb.ReservedSectorsCount + (Bpb.FatCount * FatSize32));

			uint clustersCount = dataSectorsCount / Bpb.SectorsPerCluster;

			return clustersCount;
		}
		#endregion
	}
}