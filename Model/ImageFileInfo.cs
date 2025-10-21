using BildWiederhersteller.Klassifikation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BildWiederhersteller.Helper.Literals;

namespace BildWiederhersteller.Model
{
    public readonly record struct ImageFileInfo(string SourcePath, string destination, DateTime? OriginalTimestamp) : IFileInfo
    {
        public string TargetPath => GetTargetPath();

        readonly FileTypeGroup _group = FileTypeGroup.Get(IMAGE);


        string GetTargetPath()
        {
            var extension = Path.GetExtension(SourcePath).ToLowerInvariant();

            if (_group.Subtypes.Contains(extension))
            {
                return GetSubtypeTargetPath(extension);
            }

            return Path.Combine(destination, OriginalTimestamp?.ToString("yyyy"), OriginalTimestamp?.ToString("MM-dd") ?? "Unknown", Path.GetFileName(SourcePath));
        }

        string GetSubtypeTargetPath(string extension)
        {
            var dateFolder = OriginalTimestamp?.ToString("MM-dd") ?? "Unknown";
            var subtypeFolder = Path.Combine(IMAGES, OriginalTimestamp?.ToString("yyyy"), dateFolder, extension.TrimStart('.'));

            Directory.CreateDirectory(subtypeFolder);

            return Path.Combine(subtypeFolder, Path.GetFileName(SourcePath));
        }
    }
}
