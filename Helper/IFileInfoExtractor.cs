using BildWiederhersteller.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BildWiederhersteller.Helper
{
        public interface IFileInfoExtractor
        {
            IFileInfo ExtractInfo(string path, ProcessorParam param);
        }
}
