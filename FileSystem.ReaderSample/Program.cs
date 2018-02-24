using System;
using FileSystem;

namespace FileSystem.ReaderSample
{
	class Program
	{
		static void Main(string[] args)
		{
			var bootSector = Ntfs.BootSector.GetBootSector("C:");

			Console.WriteLine($"Oem Name : {bootSector.OemName}");
			Console.WriteLine($"Bytes per sector : {bootSector.BiosParameterBlock.BytesPerSector}");
			Console.WriteLine($"Checksum : {bootSector.BiosParameterBlock.Checksum}");
			Console.WriteLine($"Number Of Heads : {bootSector.BiosParameterBlock.NumberOfHeads}");
			Console.WriteLine($"Volume Serial Number : {bootSector.BiosParameterBlock.VolumeSerialNumber}");
			Console.WriteLine($"Sectors Per Cluster: {bootSector.BiosParameterBlock.SectorsPerCluster}");

			Console.ReadLine();
		}
	}
}