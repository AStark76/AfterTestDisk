using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BildWiederhersteller.Processors
{
    internal class Mp3Processor : FileProcessor
    {

        public Mp3Processor()
        {
            _fileExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "mp3"
            };
        }
        public override void Run()
        {
            throw new NotImplementedException();
        }
    }
}
