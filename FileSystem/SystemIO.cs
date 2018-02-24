/**
 * Copyright Oualid Boutemine (c) 2009.
 **/

using System;
using System.Runtime.InteropServices;

namespace FileSystem
{
	/// <summary>
	/// Offers low level OS IO operations.
	/// </summary>
	public static class SystemIO
	{
		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern bool ReadFile(
			IntPtr hFile,
			byte[] lpBuffer,
			uint nNumberOfBytesToRead,
			out uint lpNumberOfBytesRead,
			IntPtr lpOverlapped
		);


		[DllImport("kernel32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool CloseHandle(IntPtr hObject);

		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern IntPtr CreateFile(
			string lpFileName,
			EFileAccess dwDesiredAccess,
			EFileShare dwShareMode,
			IntPtr lpSecurityAttributes,
			ECreationDisposition dwCreationDisposition,
			EFileAttributes dwFlagsAndAttributes,
			IntPtr hTemplateFile
		);

		[DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		public unsafe static extern int SetFilePointer(
			IntPtr hFile,
			int lDistanceToMove,
			int* lpDistanceToMoveHigh,
			EMoveMethod dwMoveMethod
		);

		/// <summary>
		/// Gets a pointer to the persisted data of a given file in the drive.
		/// </summary>
		/// <param name="fileName">Absolute location of the target file in the file system.</param>
		/// <returns>An IntPtr holding the address of the file.</returns>
		public static IntPtr OpenFile(string fileName)
		{
			IntPtr ret = CreateFile(
				fileName,
				EFileAccess.GenericRead | EFileAccess.GenericWrite,
				EFileShare.Read | EFileShare.Write,
				IntPtr.Zero,
				ECreationDisposition.OpenExisting,
				EFileAttributes.Normal,
				IntPtr.Zero
			);

			if (ret.ToInt32().Equals(-1))
			{
				string msg = GetErrorMessage(Marshal.GetLastWin32Error());

				throw new InvalidOperationException(msg);
			}

			return ret;
		}

		/// <summary>
		/// Read a chunk of data from a file in the drive.
		/// </summary>
		/// <param name="file">Pointer to the file on the drive.</param>
		/// <param name="buffer">Buffer that will hold the read data.</param>
		/// <param name="size">Size of the chunk to be read.</param>
		/// <returns>True if successfull, false otherwise.</returns>
		public static bool ReadBytes(IntPtr file, byte[] buffer, uint size)
		{
			uint readBytesCount;

			var result = ReadFile(file, buffer, size, out readBytesCount, IntPtr.Zero);

			if (!result)
			{
				string msg = GetErrorMessage(Marshal.GetLastWin32Error());

				throw new InvalidOperationException(msg);
			}

			return result;
		}

		/// <summary>
		/// Moves the reading pointer to the specified absolute position.
		/// </summary>
		/// <param name="file">Pointer to the file in the drive</param>
		/// <param name="offset">Position offset from the header of the file.</param>
		/// <returns>Real obtained offset.</returns>
		public static unsafe long SeekAbsolute(IntPtr file, long offset)
		{
			if (file != new IntPtr(-1))
			{
				var hi = (int)offset;
				var lo = (int)(offset >> 32);

				int hr;

				lo = SetFilePointer(file, hi, &lo, EMoveMethod.Begin);

				if (lo == -1 && ((hr = Marshal.GetLastWin32Error()) != 0))
				{
					string msg = GetErrorMessage(Marshal.GetLastWin32Error());

					throw new InvalidOperationException(msg);
				}

				return (long)((((ulong)(uint)hi) << 32) | (ulong)lo);
			}

			return 0;
		}


		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern int FormatMessage(
			int dwFlags,
			ref IntPtr lpSource,
			int dwMessageId,
			int dwLanguageId,
			ref String lpBuffer,
			int nSize,
			ref IntPtr Arguments
		);

		private static string GetErrorMessage(int errorCode)
		{
			int FORMAT_MESSAGE_ALLOCATE_BUFFER = 0x100;
			int FORMAT_MESSAGE_IGNORE_INSERTS = 0x200;
			int FORMAT_MESSAGE_FROM_SYSTEM = 0x1000;

			int msgSize = 255;
			string lpMsgBuf = null;
			int dwFlags = FORMAT_MESSAGE_ALLOCATE_BUFFER | FORMAT_MESSAGE_FROM_SYSTEM | FORMAT_MESSAGE_IGNORE_INSERTS;

			IntPtr lpSource = IntPtr.Zero;
			IntPtr lpArguments = IntPtr.Zero;

			int returnVal = FormatMessage(dwFlags, ref lpSource, errorCode, 0, ref lpMsgBuf, msgSize, ref lpArguments);
			if (returnVal == 0)
			{
				throw new Exception("Failed to format message for error code " + errorCode.ToString() + ". ");
			}

			return lpMsgBuf;
		}
	}

	public enum EMoveMethod : uint
	{
		Begin = 0,
		Current = 1,
		End = 2
	}

	[Flags]
	public enum EFileAccess : uint
	{
		/// <summary>
		/// Blah blah
		/// </summary>
		GenericRead = 0x80000000,

		/// <summary>
		/// Blah blaah
		/// </summary>
		GenericWrite = 0x40000000,

		/// <summary>
		/// Blah blaaaah
		/// </summary>
		GenericExecute = 0x20000000,

		/// <summary>
		/// Blah blaaaaaaaah
		/// </summary>
		GenericAll = 0x10000000
	}

	[Flags]
	public enum EFileShare : uint
	{
		/// <summary>
		/// Nada
		/// </summary>
		None = 0x00000000,
		
		/// <summary>
		/// Enables subsequent open operations on an object to request read access.
		/// Otherwise, other processes cannot open the object if they request read access.
		/// If this flag is not specified, but the object has been opened for read access, the function fails.
		/// </summary>
		Read = 0x00000001,
		
		/// <summary>
		/// Enables subsequent open operations on an object to request write access.
		/// Otherwise, other processes cannot open the object if they request write access.
		/// If this flag is not specified, but the object has been opened for write access, the function fails.
		/// </summary>
		Write = 0x00000002,
		
		/// <summary>
		/// Enables subsequent open operations on an object to request delete access.
		/// Otherwise, other processes cannot open the object if they request delete access.
		/// If this flag is not specified, but the object has been opened for delete access, the function fails.
		/// </summary>
		Delete = 0x00000004
	}

	public enum ECreationDisposition : uint
	{
		/// <summary>
		/// Creates a new file. The function fails if a specified file exists.
		/// </summary>
		New = 1,
		
		/// <summary>
		/// Creates a new file, always.
		/// If a file exists, the function overwrites the file, clears the existing attributes, combines the specified file attributes,
		/// and flags with FILE_ATTRIBUTE_ARCHIVE, but does not set the security descriptor that the SECURITY_ATTRIBUTES structure specifies.
		/// </summary>
		CreateAlways = 2,
		
		/// <summary>
		/// Opens a file. The function fails if the file does not exist.
		/// </summary>
		OpenExisting = 3,

		/// <summary>
		/// Opens a file, always.
		/// If a file does not exist, the function creates a file as if dwCreationDisposition is CREATE_NEW.
		/// </summary>
		OpenAlways = 4,
		
		/// <summary>
		/// Opens a file and truncates it so that its size is 0 (zero) bytes. The function fails if the file does not exist.
		/// The calling process must open the file with the GENERIC_WRITE access right.
		/// </summary>
		TruncateExisting = 5
	}

	[Flags]
	public enum EFileAttributes : uint
	{
		None = 0,
		Readonly = 0x00000001,
		Hidden = 0x00000002,
		System = 0x00000004,
		Directory = 0x00000010,
		Archive = 0x00000020,
		Device = 0x00000040,
		Normal = 0x00000080,
		Temporary = 0x00000100,
		SparseFile = 0x00000200,
		ReparsePoint = 0x00000400,
		Compressed = 0x00000800,
		Offline = 0x00001000,
		NotContentIndexed = 0x00002000,
		Encrypted = 0x00004000,
		WriteThrough = 0x80000000,
		Overlapped = 0x40000000,
		NoBuffering = 0x20000000,
		RandomAccess = 0x10000000,
		SequentialScan = 0x08000000,
		DeleteOnClose = 0x04000000,
		BackupSemantics = 0x02000000,
		PosixSemantics = 0x01000000,
		OpenReparsePoint = 0x00200000,
		OpenNoRecall = 0x00100000,
		FirstPipeInstance = 0x00080000
	}
}