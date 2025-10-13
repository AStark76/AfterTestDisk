using BildWiederhersteller.Helper;
using BildWiederhersteller.Klassifikation;
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

        public ImageProcessor()
        {
            _fileTypeGroup = FileTypeGroup.Get(_parameters.Category);
            _fileExtensions = _fileTypeGroup.GetAll();
        }
    }
}
