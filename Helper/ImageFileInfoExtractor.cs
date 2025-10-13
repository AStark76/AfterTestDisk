using BildWiederhersteller.Model;
using MetadataExtractor;
using MetadataExtractor.Formats.Exif;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BildWiederhersteller.Helper
{
    public class ImageFileInfoExtractor : IFileInfoExtractor
    {
        public IFileInfo ExtractInfo(string path, ProcessorParam param)
        {
            DateTime? dateTaken = null;

            try
            {
                var directories = ImageMetadataReader.ReadMetadata(path);
                var subIfd = directories.OfType<ExifSubIfdDirectory>().FirstOrDefault();

                if (subIfd != null && subIfd.TryGetDateTime(ExifDirectoryBase.TagDateTimeOriginal, out DateTime dt))
                    dateTaken = dt;
            }
            catch (Exception ex)
            {
                Log.Warning($"EXIF konnte nicht gelesen werden für {path}: {ex.Message}");
            }

            dateTaken ??= File.GetCreationTime(path);

            // Du kannst param hier verwenden, z. B. für Logging, Zielpfad-Logik etc.
            return new ImageFileInfo(path, dateTaken);
        }
    }

}
