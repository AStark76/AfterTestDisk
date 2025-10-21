using BildWiederhersteller.Helper;
using BildWiederhersteller.Klassifikation;
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

        public AudioProcessor(ProcessorParam param)
        {
            _parameters = param ?? throw new ArgumentNullException(nameof(param));

            _fileTypeGroup = FileTypeGroup.Get(_parameters.Category);
            _fileExtensions = _fileTypeGroup.GetAll();
        }

        protected override IFileInfo ExtractFileInfo(string path)
        {
            var extractor = new AudioFileInfoExtractor();

            return extractor.ExtractInfo(path, _parameters);
        }
    }
}
