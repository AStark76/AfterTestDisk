using BildWiederhersteller.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BildWiederhersteller.Helper
{
    public class Cr2Extractor : IFileInfoExtractor
    {
        public IFileInfo Extract(string path)
        {
            // MetadataExtractor für CR2
            return new ImageFileInfo(...);
        }
    }
}
