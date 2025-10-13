using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BildWiederhersteller.Model
{
    public readonly record struct ImageFileInfo(string SourcePath, DateTime? OriginalTimestamp) : IFileInfo
    {
        public string TargetPath => GetTargetPath();

        /// <summary>
        /// Gets the target path.
        /// </summary>
        /// <returns></returns>
        private string GetTargetPath()
        {
            var extension = Path.GetExtension(SourcePath).ToLowerInvariant();

            return extension switch
            {
                ".cr2" => GetCr2TargetPath(),
                _ => Path.Combine("Images", OriginalTimestamp?.ToString("yyyy-MM-dd") ?? "Unknown", Path.GetFileName(SourcePath))
            };
        }

        /// <summary>
        /// Gets the CR2 target path.
        /// </summary>
        /// <returns></returns>
        string GetCr2TargetPath()
        {
            var dateFolder = OriginalTimestamp?.ToString("yyyy-MM-dd") ?? "Unknown";
            var baseFolder = Path.Combine("Images", dateFolder);
            var subtypeFolder = Path.Combine(baseFolder, "cr2");

            Directory.CreateDirectory(subtypeFolder);

            return Path.Combine(subtypeFolder, Path.GetFileName(SourcePath));
        }

    }
}
