using System;
using BildWiederhersteller.Model;

namespace BildWiederhersteller.Helper
{
    public class FileInfoBasic : IFileInfo
    {
        public string SourcePath { get; }
        public string TargetPath { get; set; }
        public DateTime? OriginalTimestamp { get; set; }

        // Optional: zusätzliche Felder für Metadaten
        public string? Title { get; set; }
        public string? Author { get; set; }
        public string? Error { get; set; }

        public FileInfoBasic(string sourcePath)
        {
            SourcePath = sourcePath;
            TargetPath = string.Empty;
        }
    }
}
