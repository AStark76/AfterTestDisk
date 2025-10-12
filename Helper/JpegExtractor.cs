using BildWiederhersteller.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BildWiederhersteller.Helper
{
    public class JpegExtractor : IFileInfoExtractor
    {
        public IFileInfo Extract(string path)
        {
            // EXIF lesen, DateTimeOriginal holen
            // Zielpfad berechnen
            return new ImageFileInfo(...);
        }
    }
}
