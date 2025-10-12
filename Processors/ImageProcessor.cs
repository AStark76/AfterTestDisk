using BildWiederhersteller.Helper;
using BildWiederhersteller.Model;
using MetadataExtractor;
using MetadataExtractor.Formats.Exif;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BildWiederhersteller.Helper.ImageFileInfoExtractor;

namespace BildWiederhersteller.Processors
{
    internal class ImageProcessor : FileProcessor
    {
        HashSet<string> _rawExtensions = new(StringComparer.OrdinalIgnoreCase)
        {
            ".cr2", ".nef", ".arw", ".dng"
        };

        public ImageProcessor()
        {
            _fileExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "jpg"
                ,"jpeg"
                ,"png"
                ,"tiff"
                ,"bmp"
                ,"cr2"
                ,"nef"
                ,"arw"
            };
        }
    }
}
