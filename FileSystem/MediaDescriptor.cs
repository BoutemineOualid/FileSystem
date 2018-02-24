/**
 * Copyright Oualid Boutemine (c) 2009.
 **/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FileSystem
{
	/// <summary>
	/// 0xF0 	3.5" Double Sided, 80 tracks per side, 18 or 36 sectors per track (1.44MB or 2.88MB). 5.25" Double Sided, 15 sectors per track (1.2MB). Used also for other media types.
	/// 0xF8 	Fixed disk (i.e. Hard disk).[28]
	/// 0xF9 	3.5" Double sided, 80 tracks per side, 9 sectors per track (720K). 5.25" Double sided, 40 tracks per side, 15 sectors per track (1.2MB)
	/// 0xFA 	5.25" Single sided, 80 tracks per side, 8 sectors per track (320K)
	/// 0xFB 	3.5" Double sided, 80 tracks per side, 8 sectors per track (640K)
	/// 0xFC 	5.25" Single sided, 40 tracks per side, 9 sectors per track (180K)
	/// 0xFD 	5.25" Double sided, 40 tracks per side, 9 sectors per track (360K). Also used for 8".
	/// 0xFE 	5.25" Single sided, 40 tracks per side, 8 sectors per track (160K). Also used for 8".
	/// 0xFF 	5.25" Double sided, 40 tracks per side, 8 sectors per track (320K)
	/// </summary>
	public enum MediaDescriptor : byte
	{
		/// <summary>
		/// 3.5" Double Sided, 80 tracks per side, 18 or 36 sectors per track (1.44MB or 2.88MB). 5.25" Double Sided, 15 sectors per track (1.2MB). Used also for other media types.
		/// </summary>
		_0xF0 = 0xF0,

		/// <summary>
		/// Fixed disk (i.e. Hard disk).[28]
		/// </summary>
		_0xF8 = 0xF8,

		/// <summary>
		/// 3.5" Double sided, 80 tracks per side, 9 sectors per track (720K). 5.25" Double sided, 40 tracks per side, 15 sectors per track (1.2MB)
		/// </summary>
		_0xF9 = 0xF9,

		/// <summary>
		/// 5.25" Single sided, 80 tracks per side, 8 sectors per track (320K)
		/// </summary>
		_0xFA = 0xFA,

		/// <summary>
		/// 3.5" Double sided, 80 tracks per side, 8 sectors per track (640K)
		/// </summary>

		_0xFB = 0xFB,
		/// <summary>
		/// 5.25" Single sided, 40 tracks per side, 9 sectors per track (180K)
		/// </summary>

		_0xFC = 0xFC,
		/// <summary>
		/// 5.25" Double sided, 40 tracks per side, 9 sectors per track (360K). Also used for 8".
		/// </summary>

		_0xFD = 0xFD,

		/// <summary>
		/// 5.25" Single sided, 40 tracks per side, 8 sectors per track (160K). Also used for 8".
		/// </summary>
		_0xFE = 0xFE,

		/// <summary>
		/// 5.25" Double sided, 40 tracks per side, 8 sectors per track (320K)
		/// </summary>
		_0xFF = 0xFF
	}
}