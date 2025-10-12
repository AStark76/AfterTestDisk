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

        private string GetTargetPath()
        {
            var extension = Path.GetExtension(SourcePath).ToLowerInvariant();

            return extension switch
            {
                ".cr2" => GetCr2TargetPath(),
                _ => Path.Combine("Images", OriginalTimestamp?.ToString("yyyy-MM-dd") ?? "Unknown", Path.GetFileName(SourcePath))
            };
        }

        private string GetCr2TargetPath()
        {
            var sourceDir = Path.GetDirectoryName(SourcePath)!;
            var jpgName = Path.GetFileNameWithoutExtension(SourcePath) + ".jpg";
            var jpgPath = Path.Combine(sourceDir, jpgName);

            if (File.Exists(jpgPath))
            {
                var jpgDir = Path.GetDirectoryName(jpgPath)!;
                var cr2Dir = Path.Combine(jpgDir, "cr2");
                Directory.CreateDirectory(cr2Dir); // optional: falls du sicherstellen willst, dass es existiert
                return Path.Combine(cr2Dir, Path.GetFileName(SourcePath));
            }

            // Fallback, falls kein JPG-Zwilling existiert
            return Path.Combine("Images", "cr2", Path.GetFileName(SourcePath));
        }
    }


}
