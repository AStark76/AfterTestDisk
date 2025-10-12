using BildWiederhersteller.Helper;
using MetadataExtractor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BildWiederhersteller.Processors
{
    internal class ImageProcessor : FileProcessor
    {
        public ImageProcessor()
        {
            _fileExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "jpg"
                ,"jpeg"
                ,"png"
                ,"tiff"
                ,"tif"
            };
        }

        protected override FileInfo ExtractFileInfo(string path)
        {
            var ext = Path.GetExtension(path);
            var extractor = FileInfoExtractorRegistry.GetExtractor(ext);

            if (extractor is null)
                throw new NotSupportedException($"Kein Extraktor für {ext}");

            return extractor.Extract(path);
        }
    }
}
