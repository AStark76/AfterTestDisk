using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BildWiederhersteller.Helper
{
    public static class FileInfoExtractorRegistry
    {
        private static readonly Dictionary<string, IFileInfoExtractor> _map = new()
        {
            [".jpg"] = new JpegExtractor(),
            [".jpeg"] = new JpegExtractor(),
            [".cr2"] = new Cr2Extractor(),
        };

        public static IFileInfoExtractor? GetExtractor(string extension)
        {
            return _map.TryGetValue(extension.ToLower(), out var extractor) ? extractor : null;
        }
    }
}
