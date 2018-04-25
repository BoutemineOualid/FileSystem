# FileSystem
Helper classes that map low level NTFS/FAT32 File System data structures in C#.

# Features
Currently, the library only supports read-only mode and is limited to reading boot sector data. Support for more advanced features might be shared later.

# Requirements
To consume the library, make sure you have the highest User Account control UAC level by adding the following tag in your app's manifest.
<requestedExecutionLevel  level="highestAvailable" uiAccess="false" />
