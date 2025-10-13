using BildWiederhersteller.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BildWiederhersteller.Processors
{
    internal class AudioProcessor : FileProcessor
    {

        public AudioProcessor()
        {
            _fileExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                ".mp3", ".wav", ".flac"
            };
        }
        public override void Run()
        {
            throw new NotImplementedException();
        }

        protected override IFileInfo ExtractFileInfo(string path)
        {
            throw new NotImplementedException();
        }
    }
}
