using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace Services.Utilities
{
    public static class ZipFileHelper
    {
        private static void ZipFiles(string filePath, Dictionary<string, string> files)
        {
            using var memoryStream = new MemoryStream();
            using (var zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Update, true))
            {
                foreach (var file in files)
                {
                    Console.WriteLine(file.Key);
                    Console.WriteLine(file.Value.Length);
                    var zipArchiveEntry = zipArchive.CreateEntry(file.Key);
                    using var writer = new BinaryWriter(zipArchiveEntry.Open());
                    writer.Write(
                        file.Key.StartsWith("Technology")
                            ? Convert.FromBase64String(file.Value)
                            : Encoding.UTF8.GetBytes(file.Value));
                    Console.WriteLine(zipArchive.Entries.Count);
                }
            }

            memoryStream.Seek(0, SeekOrigin.Begin);
            Console.WriteLine(memoryStream.Length);
            File.WriteAllBytes(
                filePath,
                memoryStream.ToArray());
        }
    }
}